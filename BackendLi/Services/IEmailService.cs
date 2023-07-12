namespace BackendLi.Services;

public interface IEmailService
{
    string GenerateEmailToken(string email);
    void AddEmailToken(string token, bool isValid, DateTime expirationDate);
    public DateTime GetEmailTokenExpiration(string token);

    public void SendEmail(string email, string link, string name, string type);
    public bool IsEmailTokenInvalid(string tokenInput);

    public void InvalidateEmailToken(string tokenInput);

    public string DecodeEmailToken(string tokenInput);
}