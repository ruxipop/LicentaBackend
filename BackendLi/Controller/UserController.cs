using System.Security.Cryptography;
using System.Text;
using BackendLi.DataAccess;
using BackendLi.DTOs;
using BackendLi.Entities;
using BackendLi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendLi.Controller;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IEmailService _emailService;
    private readonly IRepository _repository;
    private readonly IUserService _userService;


    public UserController(IEmailService emailService, IUserService userService,
        IAuthenticationService authenticationService, IRepository repository)
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
        var result = _authenticationService.Login(loginDetails);

        if (result == null) return BadRequest(new { error = "User not found!" });

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        var existUser = _repository.GetEntities<User>().FirstOrDefault(x => x.Email == user.Email);
        if (existUser != null) return BadRequest("The email address already exists. Please choose another one.");

        var existUser1 = _repository.GetEntities<User>().FirstOrDefault(x => x.Username == user.Username);
        if (existUser1 != null) return BadRequest("The username already exists. Please choose another one.");
        using (var unitOfWork = _repository.CreateUnitOfWork())
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
        if (!ModelState.IsValid)
            return BadRequest();
        var user = _userService.GetUserByEmail(forgotPasswordDto.Email);
        if (user != null)
        {
            var token = _emailService.GenerateEmailToken(forgotPasswordDto.Email);
            _emailService.AddEmailToken(token, true, _emailService.GetEmailTokenExpiration(token));
            var resetPasswordURL = "auth/reset-password";
            var resetPasswordLink = "http://localhost:4200/" + resetPasswordURL + "?token=" + token;
            _emailService.SendEmail(user.Email, resetPasswordLink, user.Name, "reset");
        }

        return Ok();
    }


    [AllowAnonymous]
    [HttpPost("sent-email")]
    public IActionResult SendEmail([FromBody] ReportEmailDto reportEmailDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        _emailService.SendEmail(reportEmailDto.Email, reportEmailDto.Message, reportEmailDto.Name, "email");


        return Ok();
    }


    [AllowAnonymous]
    [HttpGet("verify-token")]
    public IActionResult VerifyEmailToken([FromQuery] string token)
    {
        var isExpired = _emailService.IsEmailTokenInvalid(token);
        Console.WriteLine(isExpired);
        return !isExpired ? Ok() : BadRequest("Token has expired!");
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public IActionResult ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        if (_emailService.IsEmailTokenInvalid(resetPasswordDto.Token)) return BadRequest("Token has expired!");
        var email = _emailService.DecodeEmailToken(resetPasswordDto.Token);
        _userService.ResetPassword(email, resetPasswordDto.NewPassword);
        _emailService.InvalidateEmailToken(resetPasswordDto.Token);
        return Ok();
    }


    [AllowAnonymous]
    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        var user = _userService.GetUser(id);
        if (user != null) return Ok(user);

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
    public IActionResult GetUserFromToken()
    {
        var currentUser = _userService.GetCurrentUser(Request);
        if (currentUser != null) return Ok(currentUser);
        return BadRequest(new { error = "User not found" });
    }

    [HttpPost("refresh-token")]
    public IActionResult RefreshToken(TokenApiDto tokenApiModel)
    {
        var accessToken = tokenApiModel.AccessToken;
        var refreshToken = tokenApiModel.RefreshToken;
        var principal = _authenticationService.GetPrincipalFromExpiredToken(accessToken);
        var email = principal.Identity.Name;
        var user = _userService.GetUserByEmail(email);
        if (user == null) return BadRequest("User Null");

        if (user.RefreshToken != refreshToken) return BadRequest("Can't regenerate refresh-token");

        if (user.RefreshTokenExpiryTime <= DateTime.Now) return BadRequest("data");

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return BadRequest("Invalid client request");
        var newAccessToken = _authenticationService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _authenticationService.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            unitOfWork.Update(user);
            unitOfWork.SaveChanges();
        }

        return Ok(new TokenApiDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
    }

    [HttpPut("update")]
    [Authorize]
    public IActionResult Update([FromBody] UserDto user)
    {
        _userService.UpdateUser(user);

        return Ok();
    }
}