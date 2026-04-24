using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace eShop.Infrastructure.EntityFramework;

public class DatabaseInitializationTask : IHostedService
{
    private readonly ILogger<DatabaseInitializationTask> _logger;
    private readonly IServiceScopeFactory _serviceFactory;

    public DatabaseInitializationTask(
        ILogger<DatabaseInitializationTask> logger,
        IServiceScopeFactory serviceFactory)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        using var scope = _serviceFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<EShopDbContext>();
        
        await MigrateAsync(dbContext, ct);
        await SeedAsync(dbContext);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    
    private async Task MigrateAsync(EShopDbContext dbContext, CancellationToken ct)
    {
        IEnumerable<string> pending = await dbContext.Database.GetPendingMigrationsAsync(ct);

        _logger.LogInformation($"Pending migrations: {pending.Count()}");

        if (!pending.Any())
        {
            return;
        }

        await dbContext.Database.MigrateAsync(ct);
        _logger.LogInformation("Migrations completed");
    }

    private async Task SeedAsync(EShopDbContext dbContext)
    {
        await dbContext.SeedDataAsync();
    }
}