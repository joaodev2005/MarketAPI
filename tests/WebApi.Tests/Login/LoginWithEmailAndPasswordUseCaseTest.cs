using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Shouldly;

namespace WebApi.Tests.Login;

public class LoginWithEmailAndPasswordUseCaseTest : IClassFixture<MarketApiApplicationFactory>
{
    private const string REQUEST_URI = "/authentication";

    private readonly HttpClient _httpClient;

    public LoginWithEmailAndPasswordUseCaseTest(MarketApiApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Success()
    {
        var registerRequest = RequestRegisterUserAccountJsonBuilder.Build();
        await _httpClient.PostAsJsonAsync("/user", registerRequest);

        var loginRequest = RequestLoginJsonBuilder.Build();
        loginRequest.Email = registerRequest.Email;
        loginRequest.Password = registerRequest.Password;

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, loginRequest);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldNotBeNullOrEmpty();
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_Invalid_Credentials()
    {
        var request = RequestLoginJsonBuilder.Build();

        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
