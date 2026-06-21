using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommonTestUtilities.Requests;
using MarketAPI.Communication.Requests;
using MarketAPI.Communication.Responses;
using Shouldly;

namespace WebApi.Tests.Cart;

public class CartTests : IClassFixture<MarketApiApplicationFactory>
{
    private const string REQUEST_URI = "/cart";
    private readonly MarketApiApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public CartTests(MarketApiApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task AddItem_Success()
    {
        var token = _factory.GetAdminToken();

        Guid realCategoryId = await CreateCategoryAndGetIdAsync(token);
        Guid realProductId = await CreateProductAndGetIdAsync(realCategoryId, token);

        var request = RequestAddItemToCartJsonBuilder.Build(realProductId);

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, REQUEST_URI)
        {
            Content = JsonContent.Create(request),
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };

        var response = await _httpClient.SendAsync(httpRequestMessage);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task AddItem_Error_Unauthorized()
    {
        var request = RequestAddItemToCartJsonBuilder.Build();

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    private async Task<Guid> CreateCategoryAndGetIdAsync(string token)
    {
        var categoryRequest = new RequestCategoryJson { Name = "Bebidas" };

        var categoryHttpRequest = new HttpRequestMessage(HttpMethod.Post, "/categories")
        {
            Content = JsonContent.Create(categoryRequest),
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };

        var categoryResponse = await _httpClient.SendAsync(categoryHttpRequest);
        categoryResponse.EnsureSuccessStatusCode();

        var categoryResponseBody = await categoryResponse.Content.ReadFromJsonAsync<ResponseCategoryJson>();
        return categoryResponseBody!.Id;
    }

    private async Task<Guid> CreateProductAndGetIdAsync(Guid categoryId, string token)
    {
        var productRequest = RequestProductJsonBuilder.Build(categoryId);

        var productHttpRequest = new HttpRequestMessage(HttpMethod.Post, "/products")
        {
            Content = JsonContent.Create(productRequest),
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };

        var productResponse = await _httpClient.SendAsync(productHttpRequest);
        productResponse.EnsureSuccessStatusCode();

        var productResponseBody = await productResponse.Content.ReadFromJsonAsync<ResponseProductJson>();
        return productResponseBody!.Id;
    }
}
