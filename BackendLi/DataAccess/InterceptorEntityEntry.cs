using BackendLi.DataAccess;
using BackendLi.DataAccess.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BackendLi.DataAccess;

sealed class InterceptorEntityEntry<T> : IInterceptorEntityEntry<T> where T : class
{
    private readonly EntityEntry<T> entityEntry;

    public InterceptorEntityEntry(EntityEntry<T> entityEntry)
    {
        this.entityEntry = entityEntry;
    }

    public T Entity => entityEntry.Entity;

    public EntityEntryState State
    {
        get => (EntityEntryState)entityEntry.State;
        set => entityEntry.State = (EntityState)value;
    }
}

sealed class InterceptorEntityEntry : IInterceptorEntityEntry
{
    private readonly EntityEntry entityEntry;

    public InterceptorEntityEntry(EntityEntry entityEntry)
    {
        this.entityEntry = entityEntry;
    }

    public object Entity => entityEntry.Entity;

    public EntityEntryState State
    {
        get => (EntityEntryState)entityEntry.State;
        set => entityEntry.State = (EntityState)value;
    }
}