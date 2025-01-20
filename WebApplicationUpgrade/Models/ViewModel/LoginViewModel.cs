using System.ComponentModel.DataAnnotations;

namespace WebApplicationUpgrade.Models.ViewModel;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Display(Name = "Запомнить меня?")]
    public bool RememberMe { get; set; }
}