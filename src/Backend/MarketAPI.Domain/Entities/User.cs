using MarketAPI.Domain.Enums;

namespace MarketAPI.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public bool Active { get; set; } = true;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Customer;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}