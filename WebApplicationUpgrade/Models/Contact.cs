using System.ComponentModel.DataAnnotations;

namespace WebApplicationUpgrade.Models
{
    public class Contact
    {
        public int Id {get; set;}
        
        [Display(Name = "Введите имя")]
        [Required(ErrorMessage = "Вам нужно ввести имя!")]
        public string Name { get; set; }

        [Display(Name = "Введите фамилию")]
        [Required(ErrorMessage = "Вам нужно ввести фамилию!")]
        public string Surname { get; set; }

        [Display(Name = "Введите возраст")]
        [Required(ErrorMessage = "Вам нужно ввести возраст!")]
        [Range(1, 120, ErrorMessage = "Возраст должен быть от 1 до 120 лет.")]
        public int? Age { get; set; }

        [Display(Name = "Введите вашу почту")]
        [Required(ErrorMessage = "Вам нужно ввести вашу почту!")]
        [EmailAddress(ErrorMessage = "Некорректный email адрес.")]
        public string Email { get; set; }

        [Display(Name = "Введите почту получателя")]
        [Required(ErrorMessage = "Вам нужно ввести почту получателя!")]
        [EmailAddress(ErrorMessage = "Некорректный email адрес.")]
        public string RecipientEmail { get; set; }

        [Display(Name = "Тема сообщения")]
        [Required(ErrorMessage = "Вам нужно ввести тему!")]
        public string Subject { get; set; }

        [Display(Name = "Введите сообщение")]
        [Required(ErrorMessage = "Вам нужно ввести сообщение!")]
        [StringLength(500, ErrorMessage = "Сообщение не может превышать 500 символов.")]
        public string Body { get; set; }
    }
}