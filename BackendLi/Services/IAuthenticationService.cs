using BackendLi.DTOs;
using BackendLi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Services;


    public interface IAuthenticationService
    {
        LoginDetailsDto? Login(LoginDetails loginDetails);

        void Logout(string tokenValue);
    }
