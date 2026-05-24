using MarketAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Infrastructure.DataAccess;

internal class MarketApiDbContext : DbContext
{
    public MarketApiDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}