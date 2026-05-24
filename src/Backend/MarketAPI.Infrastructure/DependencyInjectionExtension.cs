using System.Reflection;
using FluentMigrator.Runner;
using MarketAPI.Infrastructure.DataAccess;
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