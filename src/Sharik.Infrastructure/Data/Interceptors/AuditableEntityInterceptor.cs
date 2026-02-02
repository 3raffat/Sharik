using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common;

namespace Sharik.Infrastructure.Data.Interceptors
{
    public sealed class AuditableEntityInterceptor(ILogger<AuditableEntityInterceptor> logger, TimeProvider time, IUser user) : SaveChangesInterceptor
    {
        private readonly ILogger<AuditableEntityInterceptor> _logger = logger;
        private readonly TimeProvider _time = time;
        private readonly IUser _user = user;


        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context is null)
            {
                _logger.LogWarning("DbContext is null in SavedChanges of AuditableEntityInterceptor");
                return base.SavingChanges(eventData, result);
            }

            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
                                                                              InterceptionResult<int> result,
                                                                              CancellationToken ct = default)
        {
            if (eventData.Context is null)
            {
                _logger.LogWarning("DbContext is null in SavedChangesAsync of AuditableEntityInterceptor");
                return base.SavingChangesAsync(eventData, result, ct);
            }

            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, ct);
        }
        private void UpdateEntities(DbContext context)
        {
            var utcNow = _time.GetUtcNow();
            var userId = _user.Id;

            if (userId is null)
                _logger.LogWarning("User ID is null - auditable fields will not be set");

            foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.CreatedAtUtc = utcNow;
                        entry.Entity.LastModifiedBy = userId;
                        entry.Entity.LastModifiedUtc = utcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = userId;
                        entry.Entity.LastModifiedUtc = utcNow;
                        break;
                }
            }
        }
    }
}
