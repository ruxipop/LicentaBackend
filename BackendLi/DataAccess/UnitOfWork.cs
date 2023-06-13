using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using BackendLi.DataAccess.Enums;
using EFBulkInsert;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace BackendLi.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly int? _currentUserId;
    private readonly DbContext _dbContext;
    private IDbContextTransaction _dbContextTransaction;

    private readonly IDbContextUtilities _contextUtilities;
    private readonly IEnumerable<IEntityInterceptor> _globalInterceptors;


    internal UnitOfWork(INewDbContextFactory<DbContext> newDbContextFactory,
        IInterceptorsResolver interceptorsResolver, int? currentUserId)
        : this(newDbContextFactory, interceptorsResolver, new DbContextUtilities())
    {
        _currentUserId = currentUserId;
    }

    internal UnitOfWork(INewDbContextFactory<DbContext> newDbContextFactory,
        IInterceptorsResolver interceptorsResolver, IDbContextUtilities contextUtilities)
    {
        _dbContext = newDbContextFactory.CreateDbContext();
        _globalInterceptors = interceptorsResolver.GetGlobalInterceptors().ToList();

        _contextUtilities = contextUtilities;
    }

    public Task<T> GetByIdAsync<T>(params object[] keyValues) where T : class
    {
        return _dbContext.Set<T>().FindAsync(keyValues).AsTask();
    }

    public IQueryable<T> GetEntities<T>() where T : class
    {
        return _dbContext.Set<T>();
    }
        
    public EntityEntry<T> Entry<T>(T entity) where T : class
    {
        return _dbContext.Entry<T>(entity);
    }

    public IQueryable<T> Query<T>(string sql) where T : class
    {
        return _dbContext.Set<T>().FromSqlRaw(sql);
    }

    public T GetById<T>(params object[] keyValues) where T : class
    {
        return _dbContext.Set<T>().Find(keyValues);
    }

    public IUnitOfWork CreateUnitOfWork()
    {
        return this;
    }

    public void Add<T>(T entity) where T : class
    {
        _dbContext.Entry(entity).State = EntityState.Added;
    }

    public void AddRange<T>(IEnumerable<T> entities) where T : class
    {
        foreach (T entity in entities)
        {
            Add(entity);
        }
    }

    public void Update<T>(T entity) where T : class
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void UpdateRange<T>(IEnumerable<T> entities) where T : class
    {
        foreach (T entity in entities)
        {
            Update(entity);
        }
    }

    public void Delete<T>(T entity) where T : class
    {
        _dbContext.Entry(entity).State = EntityState.Deleted;
    }

    public void DeleteRange<T>(IEnumerable<T> entities) where T : class
    {
        foreach (T entity in entities)
        {
            Delete(entity);
        }
    }

    public void SaveChanges(bool disableInterceptors = false)
    {
#if DEBUG
        try
        {
            CallSaveChanges(disableInterceptors);
        }
        catch (Exception exception)
        {
            // ReSharper disable once PossibleIntendedRethrow
            throw exception;
        }
#else
            CallSaveChanges();
#endif
    }

    private void CallSaveChanges(bool disableInterceptors = false)
    {
        if (!disableInterceptors)
        {
            InterceptSave(new List<object>());
        }

        _dbContext.SaveChanges();
        _dbContextTransaction?.Commit();
    }

    public async Task SaveChangesAsync(bool disableInterceptors = false)
    {
        if (!disableInterceptors)
        {
            InterceptSave(new List<object>());
        }

        await _dbContext.SaveChangesAsync();
        _dbContextTransaction?.Commit();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public void BeginTransaction()
    {
        _dbContextTransaction = _dbContext.Database.BeginTransaction();
    }

    public void BulkInsert<T>(IEnumerable<T> entities) where T : class
    {
        _dbContext.BulkInsert(entities);
    }

    public void ExecuteSqlCommand(string query, params object[] parameters)
    {
        _dbContext.Database.ExecuteSqlRaw(query, parameters);
    }

    public Task<int> ExecuteSqlCommandAsync(string query, params object[] parameters)
    {
        return _dbContext.Database.ExecuteSqlRawAsync(query, parameters);
    }

    public void SetCommandTimeout(int seconds)
    {
        _dbContext.Database.SetCommandTimeout(seconds);
    }

    public void SetUserId(int? userId)
    {
        throw new NotSupportedException();
    }

    public string GetTableName<T>() where T : class
    {
        throw new NotSupportedException();
    }

    [SuppressMessage("ReSharper", "EF1000")]
    [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
    public IQueryable<T> ExecuteFunction<T>(string functionName, params object[] parameters) where T : class
    {
        SqlParameter[] sqlParameters = parameters.Select((x, i) => new SqlParameter($"p{i}", x)).ToArray();

        string sqlCommand = $"select * from {functionName}(" +
                            string.Join(", ", parameters.Select((x, i) => $"@p{i}")) + ")";

        return _dbContext.Set<T>().FromSqlRaw(sqlCommand, sqlParameters);
    }

    public void DisableDetectChanges()
    {
        _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public int? GetCurrentUserId()
    {
        return _currentUserId;
    }

    private void InterceptSave(List<object> interceptedEntities)
    {
        List<IInterceptorEntityEntry> modifiedAndNotIntercepted = GetModifiedEntities(_dbContext)
            .Where(e => !interceptedEntities.Contains(e.Entity)).ToList();

        if (modifiedAndNotIntercepted.Count == 0)
        {
            return;
        }

        foreach (IInterceptorEntityEntry entry in modifiedAndNotIntercepted)
        {
            object entity = entry.Entity;

            if (entry.State == EntityEntryState.Added)
            {
                Intercept(_globalInterceptors, entry, (i, e) => i.OnAdd(e, this));
            }
            else if (entry.State == EntityEntryState.Modified)
            {
                Intercept(_globalInterceptors, entry, (i, e) => i.OnUpdate(e, this));
            }

            if (!interceptedEntities.Contains(entity))
                interceptedEntities.Add(entity);
        }

        InterceptSave(interceptedEntities);
    }

    private IEnumerable<IInterceptorEntityEntry> GetModifiedEntities(DbContext context)
    {
        IEnumerable<IInterceptorEntityEntry> modifiedEntities =
            _contextUtilities.GetChangedEntities(context, s => s == EntityState.Added
                                                              || s == EntityState.Modified
                                                              || s == EntityState.Deleted);

        return modifiedEntities;
    }

    private static void Intercept<T>(IEnumerable<T> interceptors, IInterceptorEntityEntry entry,
        Action<T, IInterceptorEntityEntry> intercept)
    {
        foreach (T interceptor in interceptors)
        {
            intercept(interceptor, entry);
        }
    }
    
    
}