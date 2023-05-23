namespace BackendLi.Services;

public interface IEmailService
{
    string GenerateEmailToken(string Email);
    void AddEmailToken(string token, Boolean isValid, DateTime expirationDate);
    public DateTime getEmailTokenExpiration(string token);

    public void SendEmail(string Email, String Link, string Username);
}