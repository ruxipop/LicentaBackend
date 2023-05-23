using BackendLi.Entities.Attributes;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.DataAccess;

[Service(typeof(IRepository))]
public class Repository : BaseRepository
{
    public Repository(INewDbContextFactory<DbContext> dbContextFactory, IInterceptorsResolver interceptorsResolver)
        : base(dbContextFactory, interceptorsResolver)
    {
    }
}