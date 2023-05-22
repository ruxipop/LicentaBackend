using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.IdentityModel.Tokens;

namespace BackendLi.Services;
using System;

[Service(typeof(IAuthenticationService))]
public class AuthenticationService:IAuthenticationService
{
    private readonly IRepository repository;
    private readonly IConfiguration configuration;
    public LoginDetailsDto? Login(LoginDetails loginDetails)
    {
        User? user = Authenticate(loginDetails);
        
        if (user != null)
        {
            string token = GenerateJSONWebToken(user);
            return new LoginDetailsDto
            {
                Email= user.Email,
                Password = user.Password,
                Role = user.Role,
                AuthToken = token
            };
        }

        return null;
    }
    public void Logout(string tokenValue)
    {
        throw new NotImplementedException();
    }


    private User? Authenticate(LoginDetails loginDetails)
    {
        using var sha256=SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(loginDetails.Password));
        var enteredPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        User? currentUser = repository.GetEntities<User>()
            .FirstOrDefault(x => x.Email == loginDetails.Email && x.Password == enteredPassword);
        return currentUser;
    }
    private string GenerateJSONWebToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Email),
            new Claim(ClaimTypes.GivenName, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}