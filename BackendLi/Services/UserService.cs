using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BackendLi.DataAccess;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Services;
[Service(typeof(IUserService))]
public class UserService: IUserService
{
    private readonly IRepository repository;

    public UserService(IRepository repository)
    {
        this.repository = repository;
    }
    
    public User? GetUser(int userId)
    {
        return repository.GetEntities<User>().FirstOrDefault(x => x.Id == userId);
    }

    public User? getUserByEmail(string Email)
    {
        return repository.GetEntities<User>().FirstOrDefault(x => x.Email == Email);
      
    }
    
    
    
    
    public int GetNumberOfImagesForUser(int userId)
    {
        var numberOfImages = repository.GetEntities<User>()
            .Where(x => x.Id == userId)
            .SelectMany(x => x.Images)
            .Count();

        return numberOfImages;
    }

    public void ResetPassword(string Email, string newPass)
    {
        User? user = repository.GetEntities<User>().FirstOrDefault(x => x.Email == Email);
        if (user==null)
        {
            Console.WriteLine("user not found"); //TODO excepti
        }
        else
        {
            using (IUnitOfWork unitOfWork = repository.CreateUnitOfWork())
            {
                using (var sha256 = SHA256.Create())
                {

                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(newPass));
                    user.Password = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                }

                unitOfWork.Update(user);
                unitOfWork.SaveChanges();
            }
        }

    }


    public User? GetCurrentUser(HttpRequest request)
    {
        string authorizationHeader = request.Headers["Authorization"];
        string accessToken = authorizationHeader.Replace("Bearer ", "");

        var tokenHandler = new JwtSecurityTokenHandler();
        var decodedToken = tokenHandler.ReadJwtToken(accessToken);
        string userId = decodedToken.Subject;

        var emailClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        return repository.GetEntities<User>().FirstOrDefault(u => u.Email == emailClaim);
    }
}