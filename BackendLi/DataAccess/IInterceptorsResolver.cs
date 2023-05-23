namespace BackendLi.DataAccess;

public interface IInterceptorsResolver
{
    IEnumerable<IEntityInterceptor> GetGlobalInterceptors();
}