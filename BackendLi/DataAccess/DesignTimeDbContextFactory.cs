using BackendLi.Helpers;
using Microsoft.EntityFrameworkCore.Design;

namespace BackendLi.DataAccess;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        return new DataContext("Server=us-cdbr-east-06.cleardb.net;Database=heroku_a3ed02fa7d2737c;User=b0dafdcba5d62b;Password=8ae86465");
    }
}