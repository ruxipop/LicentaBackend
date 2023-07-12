using Microsoft.EntityFrameworkCore;

namespace BackendLi.DataAccess;

internal class DbContextUtilities : IDbContextUtilities
{
    public IEnumerable<IInterceptorEntityEntry> GetChangedEntities(DbContext context,
        Predicate<EntityState> statePredicate)
    {
        context.ChangeTracker.DetectChanges();

        return context.ChangeTracker.Entries().Where(x => statePredicate(x.State))
            .Select(x => new InterceptorEntityEntry(x));
    }

    public IInterceptorEntityEntry GetEntry(object entity, DbContext context)
    {
        var entityEntry = context.Entry(entity);

        return new InterceptorEntityEntry(entityEntry);
    }

    public IInterceptorEntityEntry<T> GetEntry<T>(T entity, DbContext context) where T : class
    {
        var entityEntry = context.Entry(entity);

        return new InterceptorEntityEntry<T>(entityEntry);
    }

    public IEnumerable<IInterceptorEntityEntry> GetEntries(DbContext context)
    {
        return context.ChangeTracker.Entries().Select(x => new InterceptorEntityEntry(x));
    }
}