using BackendLi.Entities;

namespace BackendLi.Services;

public interface IUserService
{
    
    User? GetUser(int UserId);

    User getUserByEmail(string Email);
    
    void ResetPassword(string Email, string newPass);
    int GetNumberOfImagesForUser(int userId);

    User? GetCurrentUser(HttpRequest request);

}