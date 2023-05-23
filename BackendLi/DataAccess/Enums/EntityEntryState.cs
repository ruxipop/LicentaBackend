using Microsoft.EntityFrameworkCore;

namespace BackendLi.DataAccess.Enums;

[Flags]
public enum EntityEntryState
{
    Detached = EntityState.Detached,
    Unchanged = EntityState.Unchanged,
    Added = EntityState.Added,
    Deleted = EntityState.Deleted,
    Modified = EntityState.Modified,
}