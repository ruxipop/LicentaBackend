using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using BackendLi.DataAccess;
using BackendLi.Entities;
using BackendLi.Entities.Attributes;
using Microsoft.IdentityModel.Tokens;

namespace BackendLi.Services.ServiceImpl;

[Service(typeof(IEmailService))]
public class EmailService : IEmailService

{
    private readonly IConfiguration _configuration;
    private readonly IRepository _repository;
    public EmailService(IRepository repository, IConfiguration configuration){
        _repository = repository;
        _configuration = configuration;
    }

    public string GenerateEmailToken(string email)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, email)
        };
        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public void AddEmailToken(string token, bool isValid, DateTime expirationDate)
    {
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            unitOfWork.Add(new Token(token, isValid, expirationDate));
            unitOfWork.SaveChanges();
        }
    }

    public DateTime GetEmailTokenExpiration(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var expClaim = jwtToken.Payload["exp"];
        var expUniTimestamp = Convert.ToInt64(expClaim);
        var expDateTime = DateTimeOffset.FromUnixTimeSeconds(expUniTimestamp).UtcDateTime;
        return expDateTime;
    }


    public void SendEmail(string email, string link, string name, string type)
    {
        var from = "vizo.photography@gmail.com";
        var password = "ysvuepqnvqblheql";
        var host = "smtp.gmail.com";
        var port = 587;
        var message = new MailMessage();
        message.From = new MailAddress(from);
        message.To.Add(email);

        if (type == "reset")
        {
            message.Subject = "Reset your password";
            message.Body = ContentReset(name, link);
        }
        else
        {
            message.Subject = "The answer to your question";
            message.Body = ContentQuestion(name, link);
        }

        message.IsBodyHtml = true;
        var smtpClient = new SmtpClient(host, port);
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

    public bool IsEmailTokenInvalid(string tokenInput)
    {
        var token = _repository.GetEntities<Token>().FirstOrDefault(x => x.TokenValue == tokenInput);
        if (token == null) return false;

        var now = DateTimeOffset.UtcNow;
        if (!token.IsValid) return true;

        if (now > token.ExpirationDate)
        {
            InvalidateEmailToken(token.TokenValue!);
            return true;
        }

        return false;
    }

    public void InvalidateEmailToken(string tokenInput)
    {
        var token = _repository.GetEntities<Token>().FirstOrDefault(x => x.TokenValue == tokenInput);
        using (var unitOfWork = _repository.CreateUnitOfWork())
        {
            token.IsValid = false;
            unitOfWork.Update(token);
            unitOfWork.SaveChanges();
        }
    }

    public string DecodeEmailToken(string tokenInput)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var decodedToken = tokenHandler.ReadJwtToken(tokenInput);

        var emailClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

        return emailClaim.Value;
    }

    private string ContentReset(string name, string link)
    {
        return "<p style='font-family:DM Sans; text-align: center; font-size: 36px;'>Vizo &#x1F4F7;</p>\n" +
               "<div style='font-family:DM Sans; text-align: center; font-size: 18px'>\n" +
               "<p >Hi, " + name + "!</p>\n" +
               "  </br>\n" +
               "  <p>We've received a request to reset your password.</p>\n" +
               "  <p>If you didn't make this request, then please ignore this notification. Otherwise, you can reset your password using this link: </p>\n" +
               "</br>\n" +
               "  <a href=\"" + link +
               "\"> <button style=\"background-color:#619CE6;border-radius: 10px; color: white;\n" +
               "  font-size: 18px; font-weight: bold; height: 50px;\n" +
               "  width: 250px; border: 2px solid #619CE6FF; cursor:pointer\">RESET PASSWORD</button>\n" +
               "</div>";
    }

    private string ContentQuestion(string name, string message)
    {
        return "<div style='font-family: DM Sans; font-size: 18px'>\n" +
               "<p>Dear <strong>" + name + "</strong>, thank you for your question.</p>\n" +
               "<p>Here is the answer:</p>\n" +
               "<p>" + message + "</p>\n" +
               "</div>";
    }
}