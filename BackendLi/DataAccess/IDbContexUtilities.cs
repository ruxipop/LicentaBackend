using Microsoft.EntityFrameworkCore;

namespace BackendLi.DataAccess;

internal interface IDbContextUtilities
{
    IEnumerable<IInterceptorEntityEntry> GetChangedEntities(DbContext context,
        Predicate<EntityState> statePredicate);

    IInterceptorEntityEntry GetEntry(object entity, DbContext context);
    IInterceptorEntityEntry<T> GetEntry<T>(T entity, DbContext context) where T : class;
    IEnumerable<IInterceptorEntityEntry> GetEntries(DbContext context);
}