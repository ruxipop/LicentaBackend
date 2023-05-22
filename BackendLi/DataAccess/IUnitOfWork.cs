namespace BackendLi.DataAccess;

public interface IUnitOfWork
{
    void Add<T>(T entity) where T : class;
    void AddRange<T>(IEnumerable<T> entities) where T : class;
    void Update<T>(T entity) where T : class;
    void UpdateRange<T>(IEnumerable<T> entities) where T : class;
    void Delete<T>(T entity) where T : class;
    void DeleteRange<T>(IEnumerable<T> entities) where T : class;
    void SaveChanges(bool disableInterceptors = false);
    Task SaveChangesAsync(bool disableInterceptors = false);
    void BeginTransaction();
    void BulkInsert<T>(IEnumerable<T> entities) where T : class;
    void ExecuteSqlCommand(string query, params object[] parameters);
    Task<int> ExecuteSqlCommandAsync(string query, params object[] parameters);
    void DisableDetectChanges(); //TODO MUST remove this method in the future
    int? GetCurrentUserId();
}