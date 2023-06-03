using System.ComponentModel.DataAnnotations;

namespace BackendLi.DTOs;

public class ResetPasswordDto
{
    [Required]
    [StringLength(100, ErrorMessage = "This password must be at least 8 characters long", MinimumLength = 8)]
    [RegularExpression("^(?=.*?\\d)(?=.*?[a-zA-Z])[a-zA-Z\\d]+$", ErrorMessage = "The password must contain at least one letter and one digit.")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "The token is required")]
    public string Token { get; set; }
}