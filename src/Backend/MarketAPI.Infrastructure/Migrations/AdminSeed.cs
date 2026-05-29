using MarketAPI.Domain.Entities;
using MarketAPI.Domain.Enums;
using MarketAPI.Domain.Repositories;
using MarketAPI.Domain.Security.PasswordHashing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketAPI.Infrastructure.Migrations;

public class AdminSeed
{
    public static async Task Seed(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var userRepository = serviceProvider.GetRequiredService<IUserReadOnlyRepository>();
        var userWriteRepository = serviceProvider.GetRequiredService<IUserWriteOnlyRepository>();
        var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        if (await userRepository.ExistsAnyAdmin())
            return;

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            Email = configuration["Admin:Email"]!,
            Password = passwordHasher.HashPassword(configuration["Admin:Password"]!),
            Role = UserRole.Admin,
            Active = true,
            CreatedAt = DateTime.UtcNow
        };

        await userWriteRepository.Add(admin);
        await unitOfWork.Commit();
    }
}