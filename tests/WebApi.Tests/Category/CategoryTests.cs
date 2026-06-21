using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Shouldly;

namespace WebApi.Tests.Category;

public class CategoryTests : IClassFixture<MarketApiApplicationFactory>
{
    private const string REQUEST_URI = "/categories";
    private readonly MarketApiApplicationFactory _factory;

    public CategoryTests(MarketApiApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Create_Success()
    {
        var httpClient = _factory.CreateClient();
        var token = _factory.GetAdminToken();

        var request = RequestCategoryJsonBuilder.Build();

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, REQUEST_URI)
        {
            Content = JsonContent.Create(request),
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };

        var response = await httpClient.SendAsync(httpRequestMessage);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_Error_Unauthorized()
    {
        var httpClient = _factory.CreateClient();
        var request = RequestCategoryJsonBuilder.Build();

        var response = await httpClient.PostAsJsonAsync(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task List_Success()
    {
        var httpClient = _factory.CreateClient();
        var token = _factory.GetAdminToken();

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, REQUEST_URI)
        {
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };

        var response = await httpClient.SendAsync(httpRequestMessage);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
