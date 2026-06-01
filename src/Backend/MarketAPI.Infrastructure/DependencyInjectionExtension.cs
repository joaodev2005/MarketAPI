using System.Reflection;
using FluentMigrator.Runner;
using MarketAPI.Domain.Repositories;
using MarketAPI.Domain.Repositories.Cart;
using MarketAPI.Domain.Repositories.Product;
using MarketAPI.Domain.Security;
using MarketAPI.Domain.Security.PasswordHashing;
using MarketAPI.Domain.Security.Tokens;
using MarketAPI.Infrastructure.DataAccess;
using MarketAPI.Infrastructure.DataAccess.Repositories;
using MarketAPI.Infrastructure.Security;
using MarketAPI.Infrastructure.Security.PasswordHashing;
using MarketAPI.Infrastructure.Security.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketAPI.Infrastructure;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructure(IConfiguration configuration)
        {
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddHttpContextAccessor();
            services.AddScoped<ILoggedUser, LoggedUser>();
            
            var signingKey = configuration["Jwt:SigningKey"]!;
            var expirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"]!);

            services.AddScoped<IAccessTokenGenerator>(_ =>
                new JwtTokenGenerator(signingKey, expirationMinutes));
            
            services.AddDbContext<MarketApiDbContext>(config =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                
                config.UseSqlServer(connectionString);
            });

            services.AddFluentMigratorCore().ConfigureRunner(config =>
            {
                config
                    .AddSqlServer()
                    .WithGlobalConnectionString(_ =>
                    {
                        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

                        return connectionString;
                    })
                    .ScanIn(Assembly.Load("MarketAPI.Infrastructure"))
                    .For.All();
            });
        }
    }
}