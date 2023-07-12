using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.IdentityModel.Tokens;

namespace BackendLi.Services.ServiceImpl;

[Service(typeof(IAuthenticationService))]
public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;


    private readonly IRepository _repository;


    public AuthenticationService(IRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public TokenApiDto Login(LoginDetails loginDetails)
    {
        var user = Authenticate(loginDetails);

        if (user != null)
        {
            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var accessToken = GenerateAccessToken(claims);
            var refreshToken = GenerateRefreshToken();
            using (var unitOfWork = _repository.CreateUnitOfWork())
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                unitOfWork.Update(user);
                unitOfWork.SaveChanges();
            }


            return new TokenApiDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        return null;
    }

    public void Logout(string tokenValue)
    {
        throw new NotImplementedException();
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }


    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }


    private User? Authenticate(LoginDetails loginDetails)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(loginDetails.Password));

        var enteredPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

        var currentUser = _repository.GetEntities<User>()
            .FirstOrDefault(x => x.Email == loginDetails.Email && x.Password == enteredPassword);

        return currentUser;
    }
}