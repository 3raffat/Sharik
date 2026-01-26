using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Sharik.Application.Common.Interfaces;
using Sharik.Domain.Common;
using System;

namespace Sharik.Infrastructure.Data.Interceptors
{
    public sealed class AuditableEntityInterceptor(ILogger<AuditableEntityInterceptor> logger, TimeProvider time, IUser user) : SaveChangesInterceptor
    {
        private readonly ILogger<AuditableEntityInterceptor> _logger = logger;
        private readonly TimeProvider _time = time;
        private readonly IUser _user = user;

        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            if (eventData.Context is null)
            {
                _logger.LogWarning("DbContext is null in SavedChanges of AuditableEntityInterceptor");
                return base.SavedChanges(eventData, result);
            }

            UpdateEntities(eventData.Context);

            return base.SavedChanges(eventData, result);
        }
        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData,
                                                         int result,
                                                         CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null)
            {
                _logger.LogWarning("DbContext is null in SavedChangesAsync of AuditableEntityInterceptor");
                return base.SavedChangesAsync(eventData, result, cancellationToken);
            }

            UpdateEntities(eventData.Context);

            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }
        public void UpdateEntities(DbContext context)
        {

            _logger.LogInformation("user id {userid}", _user.Id);

            foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
            {
                var utcNow = _time.GetUtcNow();

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedBy = _user.Id;
                        entry.Entity.CreatedAtUtc = utcNow;
                    }

                    entry.Entity.LastModifiedBy = _user.Id;
                    entry.Entity.LastModifiedUtc = utcNow;

                    _logger.LogInformation("Updated auditable entity {entity} with user id {userid} at {time}",
                                           entry.Entity.GetType().Name,
                                           _user.Id,
                                           utcNow);
                }
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.LastModifiedBy = _user.Id;
                    entry.Entity.LastModifiedUtc = utcNow;

                    _logger.LogInformation("Soft deleted auditable entity {entity} with user id {userid} at {time}",
                                           entry.Entity.GetType().Name,
                                           _user.Id,
                                           utcNow);
                }
            }
        }
    }
}
