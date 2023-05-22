using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BackendLi.DataAccess;

public interface IRepository
{
    T GetById<T>(params object[] keyValues) where T : class;
    Task<T> GetByIdAsync<T>(params object[] keyValues) where T : class;
    IQueryable<T> GetEntities<T>() where T : class;
    EntityEntry<T> Entry<T>(T entity) where T : class;
    IQueryable<T> Query<T>(string sql) where T : class;
    IQueryable<T> ExecuteFunction<T>(string functionName, params object[] parameters) where T : class;
    IUnitOfWork CreateUnitOfWork();
    void SetCommandTimeout(int seconds);
    void SetUserId(int? userId);
    string GetTableName<T>() where T : class;
}