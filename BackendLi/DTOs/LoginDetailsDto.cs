namespace BackendLi.DTOs;

public class LoginDetailsDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string AuthToken { get; set; }
}