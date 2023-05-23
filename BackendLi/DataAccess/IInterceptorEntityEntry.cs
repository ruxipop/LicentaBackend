
using BackendLi.DataAccess.Enums;

namespace BackendLi.DataAccess;

public interface IInterceptorEntityEntry<out T> where T : class
{
    T Entity { get; }
    EntityEntryState State { get; set; }
}

public interface IInterceptorEntityEntry
{
    object Entity { get; }
    EntityEntryState State { get; set; }
}