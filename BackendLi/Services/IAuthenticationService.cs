using System.Security.Claims;
using BackendLi.DTOs;
using BackendLi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Services;


public interface IAuthenticationService
{
   TokenApiDto Login(LoginDetails loginDetails);

        void Logout(string tokenValue);
        public string GenerateAccessToken(IEnumerable<Claim> claims);
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        public string GenerateRefreshToken();

}
