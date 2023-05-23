namespace BackendLi.DataAccess;

public interface IEntityInterceptor
{
    void OnAdd(IInterceptorEntityEntry entry, IUnitOfWork unitOfWork);
    void OnUpdate(IInterceptorEntityEntry entry, IUnitOfWork unitOfWork);
}

public interface IEntityInterceptor<in T> where T : class
{
    void OnAdd(IInterceptorEntityEntry<T> entry, IUnitOfWork unitOfWork);
    void OnUpdate(IInterceptorEntityEntry<T> entry, IUnitOfWork unitOfWork);
}