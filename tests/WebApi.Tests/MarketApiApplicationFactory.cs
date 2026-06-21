using System.Net.Http.Json;
using CommonTestUtilities.Requests;
using MarketAPI.Communication.Requests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RabbitMQ.Client;
using Testcontainers.MsSql;

namespace WebApi.Tests;

public class MarketApiApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer;

    private string? _adminToken;

    public MarketApiApplicationFactory()
    {
        _msSqlContainer = new MsSqlBuilder(
       "mcr.microsoft.com/mssql/server:2022-CU10-ubuntu-22.04")
       .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing")
            .ConfigureAppConfiguration((_, configuration) =>
            {
                var parameters = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = _msSqlContainer.GetConnectionString(),
                    ["Admin:Email"] = "admin@test.com",
                    ["Admin:Password"] = "Admin@123"
                };

                configuration.AddInMemoryCollection(parameters);
            })
            .ConfigureServices(services =>
            {
                var rabbitDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IConnection));
                if (rabbitDescriptor != null)
                    services.Remove(rabbitDescriptor);

                var mockConnection = new Mock<IConnection>();
                var mockChannel = new Mock<IChannel>();
                mockConnection.Setup(c => c.CreateChannelAsync(It.IsAny<CreateChannelOptions?>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(mockChannel.Object);
                services.AddSingleton(mockConnection.Object);

                var redisDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDistributedCache));
                if (redisDescriptor != null)
                    services.Remove(redisDescriptor);

                services.AddDistributedMemoryCache();
            });
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
    }

    Task IAsyncLifetime.DisposeAsync() => _msSqlContainer.DisposeAsync().AsTask();

    public string GetAdminToken()
    {
        if (_adminToken is not null)
            return _adminToken;

        var loginRequest = new MarketAPI.Communication.Requests.RequestLoginJson
        {
            Email = "admin@test.com",
            Password = "Admin@123"
        };

        var client = CreateClient();
        var response = client.PostAsJsonAsync("/authentication", loginRequest).GetAwaiter().GetResult();
        var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        var json = System.Text.Json.JsonDocument.Parse(content);

        _adminToken = json.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString()!;

        return _adminToken;
    }

    public string GetCustomerToken()
    {
        var httpClient = CreateClient();

        var registerRequest = RequestRegisterUserAccountJsonBuilder.Build();

        string passwordUsed = registerRequest.Password;

        var registerResponse = httpClient.PostAsJsonAsync("/user", registerRequest).GetAwaiter().GetResult();
        registerResponse.EnsureSuccessStatusCode();

        var loginRequest = new RequestLoginJson
        {
            Email = registerRequest.Email,
            Password = passwordUsed
        };

        var loginResponse = httpClient.PostAsJsonAsync("/authentication", loginRequest).GetAwaiter().GetResult();
        loginResponse.EnsureSuccessStatusCode();

        var content = loginResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        var json = System.Text.Json.JsonDocument.Parse(content);

        return json.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString()!;
    }
}
