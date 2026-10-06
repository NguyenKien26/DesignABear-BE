using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DesignABear.Services.Services.Interfaces;
using DesignABear.Repositories.UnitOfWork;

namespace DesignABear.WebAPI.Workers
{
    public class ViewCountSyncWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ViewCountSyncWorker> _logger;
        private readonly TimeSpan _syncInterval = TimeSpan.FromMinutes(1);
        private const string Green = "\x1b[32m";
        private const string Reset = "\x1b[0m";

        public ViewCountSyncWorker(IServiceProvider serviceProvider, ILogger<ViewCountSyncWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ViewCountSyncWorker is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation($"{Green}[Redis Sync] Starting view count sync to database.{Reset}");
                    await SyncViewCountsAsync();
                    _logger.LogInformation($"{Green}[Redis Sync] View count sync completed successfully.{Reset}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while syncing view counts.");
                }

                await Task.Delay(_syncInterval, stoppingToken);
            }

            _logger.LogInformation("ViewCountSyncWorker is stopping.");
        }

        private async Task SyncViewCountsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var viewCounts = await cacheService.GetAllViewCountsAsync();
                if (!viewCounts.Any())
                {
                    _logger.LogInformation("No view counts to sync.");
                    return;
                }

                if (viewCounts.Any())
                {
                    // Update DB in Bulk - Extremely efficient (1 connection, 1 round-trip)
                    await unitOfWork.Products.BulkIncrementViewCountsAsync(viewCounts);
                    
                    // Clear Redis counts after successful bulk sync
                    foreach (var productId in viewCounts.Keys)
                    {
                        await cacheService.ClearViewCountAsync(productId);
                    }

                    _logger.LogInformation($"{Green}[Redis Sync] Bulk synced {{Count}} products' view counts.{Reset}", viewCounts.Count);
                }
                
                // After syncing views, invalidate popular products cache
                await cacheService.RemoveByPrefixAsync("products:page:");
            }
        }
    }
}
