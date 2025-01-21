using System.ComponentModel.DataAnnotations;

namespace WebApplicationUpgrade.Models.ViewModel;

public class LoginViewModel
{
    [Required(ErrorMessage = "Введите првильно свою почту")]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Введите правильно свой пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Display(Name = "Запомнить меня?")]
    public bool RememberMe { get; set; }
}