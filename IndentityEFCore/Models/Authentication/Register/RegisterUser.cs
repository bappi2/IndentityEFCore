using System.ComponentModel.DataAnnotations;

namespace IndentityEFCore.Models.Authentication.Register;

public class RegisterUser
{
    [Required]
    public string Username { get; set; }
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}