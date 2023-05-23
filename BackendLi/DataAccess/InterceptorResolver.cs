using BackendLi.Entities.Attributes;

namespace BackendLi.DataAccess;

[Service(typeof(IInterceptorsResolver))]
public class InterceptorsResolver : IInterceptorsResolver
{
    private readonly IEnumerable<IEntityInterceptor> entityInterceptors;

    public InterceptorsResolver(IEnumerable<IEntityInterceptor> entityInterceptors)
    {
        this.entityInterceptors = entityInterceptors;
    }

    public IEnumerable<IEntityInterceptor> GetGlobalInterceptors()
    {
        return entityInterceptors;
    }
}