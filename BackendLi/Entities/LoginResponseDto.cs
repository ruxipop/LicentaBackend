using BackendLi.DTOs;

namespace BackendLi.Entities;

public class LoginResponseDto
{
    public Dictionary<string, string> Tokens { get; set; }
}