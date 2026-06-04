using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MarketAPI.Domain.Messaging.Messages;

namespace MarketAPI.Worker.Services;

public class EmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger, HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
        _logger = logger;
    }

    public async Task SendOrderCreatedEmailAsync(OrderCreatedMessage message)
    {
        var itemsHtml = string.Join("", message.Items.Select(i =>
            $"<tr><td>{i.ProductName}</td><td>{i.Quantity}</td><td>R$ {i.Price:F2}</td></tr>"));

        var payload = new
        {
            from = "onboarding@resend.dev",
            to = new[] { "joao.contatos49@gmail.com" },
            subject = $"Pedido #{message.OrderId} confirmado!",
            html = $"""
                        <h1>Olá, {message.CustomerName}!</h1>
                        <p>Seu pedido foi confirmado com sucesso!</p>
                        <table>
                            <tr><th>Produto</th><th>Quantidade</th><th>Preço</th></tr>
                            {itemsHtml}
                        </table>
                        <p><strong>Total: R$ {message.Total:F2}</strong></p>
                    """
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);

        var response = await _httpClient.PostAsync("https://api.resend.com/emails", content);

        _logger.LogInformation("Email sent to {Email} - Status: {Status}",
            message.CustomerEmail, response.StatusCode);
    }
}