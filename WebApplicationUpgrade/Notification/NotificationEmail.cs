using WebApplicationUpgrade.Models;
using WebApplicationUpgrade.Data;
namespace WebApplicationUpgrade.Services;

public class NotificationEmail : INotification
{
    public bool Notificate(ContactModel model)
    {
            try
            {
                var directoryPath = @"C:\Users\nisma\OneDrive\Документы\aspnet\WebApplicationUpgrade\WebApplicationUpgrade\";
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, "ContactMessges.txt");
            
                var data = $"Имя: {model.Name}, Почта: {model.Email}, Сообщение: {model.Body}, Дата: {DateTime.Now}\n";
                System.IO.File.AppendAllText(filePath, data);
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в отправке e-mail сообщения: {ex.Message}");
                return false;
            }
    }

}