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
                    ["Jwt:SigningKey"] = "test-signing-key-minimum-32-characters",
                    ["Jwt:ExpirationMinutes"] = "60",
                    ["Admin:Email"] = "admin@test.com",
                    ["Admin:Password"] = "Admin@123",
                    ["RabbitMQ:Host"] = "localhost",
                    ["Redis:Connection"] = "localhost:6379"
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
}
