using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.EntityFrameworkCore;

namespace BackendLi.Services.ServiceImpl;

[Service(typeof(IUserService))]
public class UserService : IUserService
{
    private readonly IRepository _repository;


    public UserService(IRepository repository)
    {
        _repository = repository;
    }

    public User? GetUser(int userId)
    {
        return _repository.GetEntities<User>().Include(x => x.Location).FirstOrDefault(x => x.Id == userId);
    }

    public User? GetUserByEmail(string email)
    {
        return _repository.GetEntities<User>().FirstOrDefault(x => x.Email == email);
    }


    public int GetNumberOfImagesForUser(int userId)
    {
        var numberOfImages = _repository.GetEntities<User>()
            .Where(x => x.Id == userId)
            .SelectMany(x => x.Images!)
            .Count();

        return numberOfImages;
    }

    public void ResetPassword(string email, string newPass)
    {
        var user = _repository.GetEntities<User>().FirstOrDefault(x => x.Email == email);
        if (user != null)
            using (var unitOfWork = _repository.CreateUnitOfWork())
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


    public User? GetCurrentUser(HttpRequest request)
    {
        string authorizationHeader = request.Headers["Authorization"];
        var accessToken = authorizationHeader.Replace("Bearer ", "");

        var tokenHandler = new JwtSecurityTokenHandler();
        var decodedToken = tokenHandler.ReadJwtToken(accessToken);
        var emailClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        return _repository.GetEntities<User>().FirstOrDefault(u => u.Email == emailClaim);
    }


    public void UpdateUser(UserDto userDto)
    {
        var user = _repository.GetEntities<User>().FirstOrDefault(x => userDto.Id == x.Id);
        if (userDto.Location != null)
        {
            var location = _repository.GetEntities<Location>()
                .FirstOrDefault(l => l.City == userDto.Location.City && l.Country == userDto.Location.Country);

            if (location == null)
            {
                location = userDto.Location;
                using (var unitOfWork = _repository.CreateUnitOfWork())
                {
                    unitOfWork.Add(location);
                    unitOfWork.SaveChanges();
                }
            }

            user!.LocationId = location.Id;
        }
        else
        {
            user!.LocationId = null;
        }

        user.ProfilePhoto = userDto.ProfilePhoto;
        user.BackgroundPhoto = userDto.BackgroundPhoto;
        user.Name = userDto.Name;
        user.Username = userDto.Username;
        user.Description = userDto.Description;

        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            unitOfWork.Update(user);
            unitOfWork.SaveChanges();
        }
    }
}