using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common;

namespace Sharik.Infrastructure.Data.Interceptors
{
    public sealed class SoftDeleteInterceptor(ILogger<SoftDeleteInterceptor> logger, TimeProvider time, IUser user) : SaveChangesInterceptor
    {
        private readonly ILogger<SoftDeleteInterceptor> _logger = logger;
        private readonly TimeProvider _time = time;
        private readonly IUser _user = user;

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context is null)
            {
                _logger.LogWarning("DbContext is null in SavingChanges");
                return base.SavingChanges(eventData, result);
            }

            ProcessSoftDeletes(eventData.Context);

            return base.SavingChanges(eventData, result);
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
                                                                              InterceptionResult<int> result,
                                                                              CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null)
            {
                _logger.LogWarning("DbContext is null in SavingChangesAsync");
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            ProcessSoftDeletes(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ProcessSoftDeletes(DbContext context)
        {

            var utcNow = _time.GetUtcNow();
            var userId = _user.Id;

            if (userId is null)
                _logger.LogWarning("User ID is null - auditable fields will not be set");

            foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModifiedUtc = utcNow;

                    _logger.LogInformation("Soft deleted auditable entity {entity} with user id {userid} at {time}",
                                           entry.Entity.GetType().Name,
                                           userId,
                                           utcNow);
                }
            }
        }
    }
}
