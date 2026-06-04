using MarketAPI.Worker;
using MarketAPI.Worker.Services;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IConnection>(_ =>
{
    var factory = new ConnectionFactory
    {
        HostName = builder.Configuration["RabbitMQ:Host"] ?? "localhost"
    };
    return factory.CreateConnectionAsync().GetAwaiter().GetResult();
});

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IEmailService>(sp =>
{
    var apiKey = builder.Configuration["Resend:ApiKey"]!;
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    var logger = sp.GetRequiredService<ILogger<EmailService>>();
    return new EmailService(logger, httpClient, apiKey);
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();