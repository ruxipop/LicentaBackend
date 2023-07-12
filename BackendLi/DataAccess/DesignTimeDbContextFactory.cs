using BackendLi.Helpers;
using Microsoft.EntityFrameworkCore.Design;

namespace BackendLi.DataAccess;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        return new DataContext("server=localhost;database=vizo_db;user=root;password=root;PersistSecurityInfo=True");
    }
}