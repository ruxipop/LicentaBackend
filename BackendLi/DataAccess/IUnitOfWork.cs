namespace BackendLi.DataAccess;

public interface IUnitOfWork : IRepository, IDisposable
{
    void Add<T>(T entity) where T : class;
    void AddRange<T>(IEnumerable<T> entities) where T : class;
    void Update<T>(T entity) where T : class;
    void UpdateRange<T>(IEnumerable<T> entities) where T : class;
    void Delete<T>(T entity) where T : class;
    void DeleteRange<T>(IEnumerable<T> entities) where T : class;
    void SaveChanges(bool disableInterceptors = false);
}