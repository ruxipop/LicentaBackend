using BackendLi.Helpers;
using Microsoft.EntityFrameworkCore.Design;

namespace BackendLi.DataAccess;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        return new DataContext("server=eu-cdbr-west-03.cleardb.net;database=heroku_b1ac73683ad42cb;user=bad24ad5363e70;password=7fca4e2b");
    }
}