
using BackendLi.Entities.Attributes;
using BackendLi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.DataAccess;


[Service(typeof(INewDbContextFactory<DbContext>))]
public class NewDbContextFactory : INewDbContextFactory<DbContext>
{
    private readonly ConnectionStrings connectionStrings;

    public NewDbContextFactory(ConnectionStrings connectionStrings)
    {
        this.connectionStrings = connectionStrings;
    }

    public DbContext CreateDbContext()
    {
        
      
        return new DataContext(connectionStrings.Database);
    }
    

    
}
