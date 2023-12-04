using eShop.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Infrastructure;

public static class InfrastructureModuleExtensions
{
    public static IServiceCollection AddInfrastructureModule(this IServiceCollection services,
        ConfigurationManager configuration) =>
        services
            .AddDbContext<EShopDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("eShop")))
            .AddHostedService<DatabaseInitializationTask>();
}