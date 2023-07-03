using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BackendLi.DataAccess;
using BackendLi.DTOs;
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
        return repository.GetEntities<User>().Include(x=>x.Location).FirstOrDefault(x => x.Id == userId);
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
    
    
    public void UpdateUser(UserDto userDto)
    {

        var user = repository.GetEntities<User>().FirstOrDefault(x => userDto.Id == x.Id);
        if (userDto.Location != null)
        {
            var location = repository.GetEntities<Location>()
                .FirstOrDefault(l => l.City == userDto.Location.City && l.Country == userDto.Location.Country);

            if (location == null)
            {
                location = userDto.Location;
                using (IUnitOfWork unitOfWork = repository.CreateUnitOfWork())
                {
                    unitOfWork.Add(location);
                    unitOfWork.SaveChanges();

                }
            }
            user.LocationId = location.Id;
        }
        else
        {
            user.LocationId = null;
        }
        user.ProfilePhoto = userDto.ProfilePhoto;
        user.BackgroundPhoto = userDto.BackgroundPhoto;
        user.Name = userDto.Name;
        user.Username = userDto.Username;
        user.Description = userDto.Description;
        
        using (IUnitOfWork unitOfWork = repository.CreateUnitOfWork())
        {
            unitOfWork.Update(user);
            unitOfWork.SaveChanges();
                
        }



    }
}