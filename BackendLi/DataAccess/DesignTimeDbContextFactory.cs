using BackendLi.Helpers;
using Microsoft.EntityFrameworkCore.Design;

namespace BackendLi.DataAccess;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    
    public DataContext CreateDbContext(string[] args)
    {
        return new DataContext("Server=eu-cdbr-west-03.cleardb.net;Database=heroku_b1ac73683ad42cb;Uid=bad24ad5363e70;Pwd=7fca4e2b;");
    }
}