using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using BackendLi.DataAccess;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.IdentityModel.Tokens;

namespace BackendLi.Services;


[Service(typeof(IEmailService))]
public class EmailService:IEmailService

{
    private readonly IRepository _repository;
    private readonly IConfiguration _configuration;

    public EmailService(IRepository repository,IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }
    public string GenerateEmailToken(string Email)
    {
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Email),
        
        };
        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
        
    

    }

    public void AddEmailToken(string token, Boolean isValid, DateTime expirationDate)
    {
        using (IUnitOfWork unitOfWork = _repository.CreateUnitOfWork())
        {
            unitOfWork.Add(new Token(token,isValid,expirationDate));
            unitOfWork.SaveChanges();
        }
    }

    public DateTime getEmailTokenExpiration(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var expClaim = jwtToken.Payload["exp"];
        var expUniTimestamp = Convert.ToInt64(expClaim);
        var expDateTime = DateTimeOffset.FromUnixTimeSeconds(expUniTimestamp).UtcDateTime;
        return expDateTime;
    }

    public void SendEmail(string Email, String Link, string Username)
    {
        string from = "vizo.photography@gmail.com"; // Adresa de email a expeditorului
        string password = "ysvuepqnvqblheql"; // Parola pentru adresa de email a expeditorului
        string host = "smtp.gmail.com";
        int port = 587;

        MailMessage message = new MailMessage();
        message.From = new MailAddress(from);
        message.To.Add(Email);
        message.Subject = "Reset your password";

        string content =
            "<p style='font-family:DM Sans; text-align: center; font-size: 36px;'>Travel Points&#9992;&#65039;</p>\n" +
            "<div style='font-family:DM Sans; text-align: center; font-size: 18px'>\n" +
            "<p >Hi, " +  "!</p>\n" +
            "  </br>\n" +
            "  <p>We've received a request to reset your password.</p>\n" +
            "  <p>If you didn't make this request, then please ignore this notification. Otherwise, you can reset your password using this link: </p>\n" +
            "</br>\n" +
            "  <a href=\""  +
            "\"> <button style=\"background-color:#619CE6;border-radius: 10px; color: white;\n" +
            "  font-size: 18px; font-weight: bold; height: 50px;\n" +
            "  width: 250px; border: 2px solid #619CE6FF; cursor:pointer\">RESET PASSWORD</button>\n" +
            "</div>";

        message.Body = content;
        message.IsBodyHtml = true;

        SmtpClient smtpClient = new SmtpClient(host, port);
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(from, password);
        smtpClient.EnableSsl = true;

        try
        {
            smtpClient.Send(message);
            Console.WriteLine("Email sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error sending email: " + ex.Message);
        }
    
}
}