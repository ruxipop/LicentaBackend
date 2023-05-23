using System.ComponentModel.DataAnnotations;

namespace BackendLi.DTOs;

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress(ErrorMessage = "must respect the format: name@example.com")]
    public string Email { get; set; }
}