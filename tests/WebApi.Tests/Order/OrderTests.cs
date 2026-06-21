using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommonTestUtilities.Requests;
using MarketAPI.Communication.Requests;
using MarketAPI.Communication.Responses;
using Shouldly;

namespace WebApi.Tests.Order;

public class OrderTests : IClassFixture<MarketApiApplicationFactory>
{
    private const string REQUEST_URI = "/orders";
    private readonly MarketApiApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public OrderTests(MarketApiApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Create_Success()
    {
        var adminToken = _factory.GetAdminToken();
        var customerToken = _factory.GetCustomerToken(); 

        Guid realCategoryId = await CreateCategoryAndGetIdAsync(adminToken);
        Guid realProductId = await CreateProductAndGetIdAsync(realCategoryId, adminToken);

        await AddItemToCartAsync(realProductId, customerToken);

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, REQUEST_URI)
        {
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", customerToken) }
        };

        var response = await _httpClient.SendAsync(httpRequestMessage);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadFromJsonAsync<ResponseOrderJson>();
        responseBody.ShouldNotBeNull();
    }

    [Fact]
    public async Task Create_Error_Forbidden_With_AdminToken()
    {
        var adminToken = _factory.GetAdminToken();

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, REQUEST_URI)
        {
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", adminToken) }
        };

        var response = await _httpClient.SendAsync(httpRequestMessage);

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }


    private async Task<Guid> CreateCategoryAndGetIdAsync(string token)
    {
        var categoryRequest = new RequestCategoryJson { Name = "Mercado" };
        var request = new HttpRequestMessage(HttpMethod.Post, "/categories")
        {
            Content = JsonContent.Create(categoryRequest),
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<ResponseCategoryJson>();
        return body!.Id;
    }

    private async Task<Guid> CreateProductAndGetIdAsync(Guid categoryId, string token)
    {
        var productRequest = RequestProductJsonBuilder.Build(categoryId);
        var request = new HttpRequestMessage(HttpMethod.Post, "/products")
        {
            Content = JsonContent.Create(productRequest),
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<ResponseProductJson>();
        return body!.Id;
    }

    private async Task AddItemToCartAsync(Guid productId, string token)
    {
        var cartRequest = RequestAddItemToCartJsonBuilder.Build(productId);
        var request = new HttpRequestMessage(HttpMethod.Post, "/cart")
        {
            Content = JsonContent.Create(cartRequest),
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
