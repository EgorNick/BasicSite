using System.ComponentModel.DataAnnotations;

namespace WebApplicationUpgrade.Models.ViewModel;
public class RegisterViewModel
{
    [Required(ErrorMessage = "Поле Email обязательно для заполнения.")]
    [EmailAddress(ErrorMessage = "Введите корректный Email.")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Поле Пароль обязательно для заполнения.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Подтверждение пароля обязательно.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
    public string ConfirmPassword { get; set; }
}