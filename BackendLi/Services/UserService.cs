using BackendLi.DataAccess;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;

namespace BackendLi.Services;
[Service(typeof(IUserService))]
public class UserService: IUserService
{
    private readonly IRepository repository;

    public UserService(IRepository repository)
    {
        this.repository = repository;
    }
    
    public User GetUser(int userId)
    {
        return repository.GetEntities<User>().FirstOrDefault(x => x.Id == userId);
    }

    public User? getUserByEmail(string Email)
    {
        return repository.GetEntities<User>().FirstOrDefault(x => x.Email == Email);
      
    }
}