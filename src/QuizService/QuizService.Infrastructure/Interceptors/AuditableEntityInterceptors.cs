using BuildingBlocks.Security.ClientToService.CurrentUser;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using QuizService.Domain.Abstraction;

namespace QuizService.Infrastructure.Interceptors;

public class AuditableEntityInterceptors(ICurrentUser currentUser) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;
        var user = currentUser.IsAuthenticated ? currentUser.UserName : "anonymous";
        var time = DateTime.UtcNow;
        foreach (var entry in context.ChangeTracker.Entries<IEntity>())
        {
            
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = user;
                entry.Entity.CreateTime = time;
            }
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified ||
                entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = user;
                entry.Entity.LastModified = time;
            }
            if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Property(nameof(IEntity.CreatedBy)).IsModified = false;
                entry.Property(nameof(IEntity.CreateTime)).IsModified = false;
            }
        }
    }
}


public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => r.TargetEntry != null &&
                                  r.TargetEntry.Metadata.IsOwned() &&
                                  (r.TargetEntry.State == EntityState.Added ||
                                   r.TargetEntry.State == EntityState.Modified));
}