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
       var dictionary =  _authenticationService.Login(loginDetails);
Console.WriteLine(dictionary);
     
        return Ok(dictionary);
        
     
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
       User? existUser = _repository.GetEntities<User>().FirstOrDefault(x => x.Email == user.Email);
        // if (user==null)
        // {
        //     return BadRequest("User already exists");
        // }
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
    
    [Authorize]
    [HttpGet("me")]
    public IActionResult GetUserFromToken( )
    {
        Console.WriteLine("intra in au");
        User? currentUser = _userService.GetCurrentUser(Request);
        if (currentUser != null)
        {
            Console.WriteLine("id"+currentUser.Id);
            return Ok(currentUser);
        }
        return BadRequest(new { error = "User not found" });
    }

    [HttpPost("refresh-token")]
    public IActionResult RefreshToken(TokenApiDto tokenApiModel)
    {
        Console.WriteLine(tokenApiModel.AccessToken);
        Console.WriteLine(tokenApiModel.RefreshToken);
         Console.WriteLine("ce masa");
        string accessToken = tokenApiModel.AccessToken;
        string refreshToken = tokenApiModel.RefreshToken;
        var principal = _authenticationService.GetPrincipalFromExpiredToken(accessToken);
        var email = principal.Identity.Name;
        var user = _userService.getUserByEmail(email);
        Console.WriteLine("o trecut");
        if (user==null)
        {
            Console.WriteLine(1);
            return BadRequest("User Null");

        }

        if (user.RefreshToken != refreshToken)
        {
            Console.WriteLine(user.RefreshToken);
            Console.WriteLine(refreshToken);

            return BadRequest("refrskToken");

        }

        if (user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            Console.WriteLine(DateTime.Now);
            Console.WriteLine(3);
            Console.WriteLine(user.RefreshTokenExpiryTime);
            return BadRequest("data");
        }

        if (user==null|| user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return BadRequest("Invalid client request");
        var newAccessToken = _authenticationService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _authenticationService.GenerateRefreshToken();
        Console.WriteLine("aici crapa????");
        user.RefreshToken = newRefreshToken;
        using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            unitOfWork.Update(user);
            unitOfWork.SaveChanges();
        }
        return Ok(new TokenApiDto()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }
   


}