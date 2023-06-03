using BackendLi.Entities;

namespace BackendLi.Services;

public interface IEmailService
{
    string GenerateEmailToken(string Email);
    void AddEmailToken(string token, Boolean isValid, DateTime expirationDate);
    public DateTime getEmailTokenExpiration(string token);

    public void SendEmail(string Email, string Link, string Username);
    public Boolean isEmailTokenInvalid(string Token);
    
    public void invalidateEmailToken(string Token);

    public string decodeEmailToken(string Token);
}