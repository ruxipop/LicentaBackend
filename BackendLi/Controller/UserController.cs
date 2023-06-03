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
        Dictionary<string,string>? dictionary =  _authenticationService.Login(loginDetails);

        if (dictionary != null)
        {
            return Ok(dictionary);
        }
        Console.WriteLine("sfarist");

        return BadRequest(new { error = "User not found" });
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

    [AllowAnonymous]
    [HttpGet("verify-token")]
    public IActionResult VerifyEmailToken( [FromQuery] string token)
    {
        Boolean isExpired = _emailService.isEmailTokenInvalid(token);
        return !isExpired ? Ok("Token is valid!") : BadRequest("Token has expired!");
    }

    [AllowAnonymous]
    [HttpGet("reset-password")]
    public IActionResult ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        if (_emailService.isEmailTokenInvalid(resetPasswordDto.Token))
        {
            return BadRequest("Token has expired!");
        }

        string email = _emailService.decodeEmailToken(resetPasswordDto.Token);
        _userService.ResetPassword(email,resetPasswordDto.NewPassword);
        _emailService.invalidateEmailToken(resetPasswordDto.Token);
        return Ok();
    }
    
    
    [AllowAnonymous]
    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        User? user = _userService.GetUser(id);
        if (user != null)
        {
            return Ok(user);
        }

        return NotFound("User not found");

    }
    
    [AllowAnonymous]
    [HttpGet("getNbImages/{id}")]
    public IActionResult GetNbImages(int id)
    {
        var number = _userService.GetNumberOfImagesForUser(id);
        return Ok(number);
    }
    
    [AllowAnonymous]
    [HttpGet("me")]
    public IActionResult GetUserFromToken( )
    {
        User? currentUser = _userService.GetCurrentUser(Request);
        if (currentUser != null)
        {
            return Ok(currentUser);
        }
        return BadRequest(new { error = "User not found" });
    }
}