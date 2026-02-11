using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Interfaces;

namespace Sharik.Infrastructure.BackgroundJobs
{
    public sealed class CleanupUnVerifiedUsers(
        ILogger<CleanupUnVerifiedUsers> _logger ,
        IServiceScopeFactory _scopeFactory ,
        TimeProvider _timeProvider
        ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromHours(1));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("Starting cleanup of unverified users at {Time}." , _timeProvider.GetLocalNow());

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var _context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
                    var now = _timeProvider.GetUtcNow();


                    var cutoffTime = now.AddDays(-1);

                    var unverifiedUsers = await _context.Users
                        .Where(u => !u.EmailConfirmed && u.CreatedAtUtc < cutoffTime)
                        .ToListAsync(stoppingToken);
                        

                    if(unverifiedUsers.Any())
                    {
                        _context.Users.RemoveRange(unverifiedUsers);
                        await _context.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("Cleaned up {Count} unverified users at {Time}." , unverifiedUsers.Count , _timeProvider.GetLocalNow());
                    }
                    else
                    {
                        _logger.LogInformation("No unverified users found for cleanup at {Time}." , _timeProvider.GetLocalNow());
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex , "An error occurred while cleaning up unverified users.");
                }
            }
        }
    }
}
