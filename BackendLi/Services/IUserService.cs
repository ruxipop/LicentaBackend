using BackendLi.Entities;

namespace BackendLi.Services;

public interface IUserService
{
    
    User GetUser(int UserId);

    User getUserByEmail(string Email);
}