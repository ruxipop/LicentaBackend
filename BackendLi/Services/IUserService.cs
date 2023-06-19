using BackendLi.DTOs;
using BackendLi.Entities;

namespace BackendLi.Services;

public interface IUserService
{
    public void UpdateUser(UserDto userDto);
        User? GetUser(int UserId);

    User getUserByEmail(string Email);
    
    void ResetPassword(string Email, string newPass);
    int GetNumberOfImagesForUser(int userId);

    User? GetCurrentUser(HttpRequest request);

}