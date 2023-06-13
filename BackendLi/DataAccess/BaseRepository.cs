using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Data.SqlClient;
namespace BackendLi.DataAccess;


public class BaseRepository : IRepository
{
    private readonly INewDbContextFactory<DbContext> _dbContextFactory;
    private readonly DbContext _dbContext;
    private readonly IInterceptorsResolver _interceptorsResolver;
    private int? _currentUserId;

    public BaseRepository(INewDbContextFactory<DbContext> dbContextFactory, IInterceptorsResolver interceptorsResolver)
    {
        _dbContextFactory = dbContextFactory;
        _dbContext = dbContextFactory.CreateDbContext();
        _interceptorsResolver = interceptorsResolver;
        _currentUserId = null;
    }

    public Task<T> GetByIdAsync<T>(params object[] keyValues) where T : class
    {
        return _dbContext.Set<T>().FindAsync(keyValues).AsTask();
    }

    public EntityEntry<T> Entry<T>(T entity) where T : class
    {
        return _dbContext.Entry<T>(entity);
    }

    public IQueryable<T> GetEntities<T>() where T : class
    {
        return _dbContext.Set<T>().AsNoTracking();
    }

    public IQueryable<T> Query<T>(string sql) where T : class
    {
        return _dbContext.Set<T>().FromSqlRaw(sql);
    }

    public IUnitOfWork CreateUnitOfWork()
    {
        return new UnitOfWork(_dbContextFactory, _interceptorsResolver, _currentUserId);
    }

    public void SetCommandTimeout(int seconds)
    {
        _dbContext.Database.SetCommandTimeout(seconds);
    }

    public void SetUserId(int? userId)
    {
        _currentUserId = userId;
    }

    public string GetTableName<T>() where T : class
    {
        IEntityType entityType = _dbContext.Model.FindEntityType(typeof(T));
        string schema = entityType.GetSchema();
        string tableName = entityType.GetTableName();
            
        return string.IsNullOrEmpty(schema) ? $"[{tableName}]" : $"[{schema}].[{tableName}]";
    }

    [SuppressMessage("ReSharper", "EF1000")]
    [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
    public IQueryable<T> ExecuteFunction<T>(string functionName, params object[] parameters) where T : class
    {
        SqlParameter[] sqlParameters = parameters.Select((x, i) => new SqlParameter($"p{i}", x)).ToArray();

        string sqlCommand = $"SELECT * FROM {functionName}(" +
                            string.Join(", ", parameters.Select((x, i) => $"@p{i}")) + ")";

        return _dbContext.Set<T>().FromSqlRaw(sqlCommand, sqlParameters);
    }

    public T GetById<T>(params object[] keyValues) where T : class
    {
        return _dbContext.Set<T>().Find(keyValues);
    }
    
}