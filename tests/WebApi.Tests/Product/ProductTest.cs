using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommonTestUtilities.Requests;
using MarketAPI.Communication.Requests;
using MarketAPI.Communication.Responses;
using Shouldly;

namespace WebApi.Tests.Product;

public class ProductTest : IClassFixture<MarketApiApplicationFactory>
{
    private const string REQUEST_URI = "/products";
    private readonly MarketApiApplicationFactory _factory;
    private readonly HttpClient _httpClient; 

    public ProductTest(MarketApiApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient(); 
    }

    [Fact]
    public async Task Create_Success()
    {
        var token = _factory.GetAdminToken();

        Guid realCategoryId = await CreateCategoryAndGetIdAsync(token);

        var request = RequestProductJsonBuilder.Build(realCategoryId);

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, REQUEST_URI)
        {
            Content = JsonContent.Create(request),
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };

        var response = await _httpClient.SendAsync(httpRequestMessage);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task List_Success()
    {
        var token = _factory.GetAdminToken();

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, REQUEST_URI)
        {
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };

        var response = await _httpClient.SendAsync(httpRequestMessage);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_Error_Unauthorized()
    {
        var request = RequestProductJsonBuilder.Build();

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    private async Task<Guid> CreateCategoryAndGetIdAsync(string token)
    {
        var categoryRequest = new RequestCategoryJson { Name = "Eletrônicos" };

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
}