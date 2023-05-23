using Microsoft.EntityFrameworkCore;

namespace BackendLi.DataAccess;

public interface INewDbContextFactory<out TDbContext> where TDbContext : DbContext
{
    TDbContext CreateDbContext();
}