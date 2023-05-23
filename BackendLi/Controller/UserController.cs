using System.Security.Cryptography;
using System.Text;
using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;

namespace BackendLi.Controller;
using Microsoft.AspNetCore.Mvc;
[Route("api/[controller]")]
[ApiController]
public class UserController:ControllerBase
{
    private readonly IRepository _repository;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;


    public UserController(IEmailService emailService,IUserService userService,IAuthenticationService authenticationService,IRepository repository)
    {
        _repository = repository;
        _authenticationService = authenticationService;
        _userService = userService;
        _emailService = emailService;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDetails loginDetails)
    {
        Console.WriteLine(_authenticationService==null);
        LoginDetailsDto? loginDetailsDto =  _authenticationService.Login(loginDetails);

        if (loginDetailsDto != null)
        {
            return Ok(loginDetailsDto);
        }
        Console.WriteLine("sfarist");

        return NotFound("User not found");
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        Console.WriteLine("sa");
        using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
        {
            using (var sha256 = SHA256.Create())
            {
                
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                user.Password = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower(); 
            }
            unitOfWork.Add(user);
            unitOfWork.SaveChanges();
        }

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public IActionResult ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        Console.WriteLine("cee");
        if (!ModelState.IsValid)
            return BadRequest();
        User? user = _userService.getUserByEmail(forgotPasswordDto.Email);
        Console.WriteLine("ok");
        if (user != null)
        {
            string token = _emailService.GenerateEmailToken(forgotPasswordDto.Email);
            _emailService.AddEmailToken(token,true,_emailService.getEmailTokenExpiration(token));
            string resetPasswordURL = "User/reset-password";
            string resetPasswordLink = "http://localhost:5244/api/" + resetPasswordURL + "?token=" + token;
            _emailService.SendEmail(forgotPasswordDto.Email,resetPasswordLink,"f");

        }
        return Ok();
    }
    
}