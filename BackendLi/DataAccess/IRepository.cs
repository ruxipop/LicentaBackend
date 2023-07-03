using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BackendLi.DataAccess;

public interface IRepository
{

    T GetById<T>(params object[] keyValues) where T : class;
    IQueryable<T> GetEntities<T>() where T : class;
    IUnitOfWork CreateUnitOfWork();

}