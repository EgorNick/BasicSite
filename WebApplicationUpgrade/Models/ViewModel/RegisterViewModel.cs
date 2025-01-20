using System.ComponentModel.DataAnnotations;

namespace WebApplicationUpgrade.Models.ViewModel;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароль не совпадает.")]
    public string ConfirmPassword { get; set; }
}