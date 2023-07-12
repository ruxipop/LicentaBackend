using Microsoft.EntityFrameworkCore;

namespace BackendLi.DataAccess.Enums;

[Flags]
public enum EntityEntryState
{
    Added = EntityState.Added,
    Modified = EntityState.Modified
}