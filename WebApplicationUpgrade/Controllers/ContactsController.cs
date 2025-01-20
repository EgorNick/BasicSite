using System.Text;
using WebApplicationUpgrade.Models;
using WebApplicationUpgrade.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationUpgrade.Controllers
{
    public class ContactsController : Controller
    {
        private readonly INotification _notification;

        private readonly ISavingInfo _savingInfo;

        public ContactsController(INotification notification, ISavingInfo savingInfo)
        {
            _notification = notification;
            _savingInfo = savingInfo;
        }
        
        public IActionResult Index()
        {
            return View(new Contact());
        }

        [HttpPost]
        public IActionResult SaveInfo(Contact model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Пожалуйста, заполните все поля";
            }
            if (_savingInfo.SavingIntoDbase(model))
            {
                if (_notification.Notificate(model))
                {
                    TempData["SuccessMessage"] = "Данные успешно сохранены и отправлены!";
                }
                else {
                    TempData["ErrorMessage"] = "Произошла ошибка отправке сообщения!";
                }
            }
            else {
                TempData["ErrorMessage"] = "Произошла ошибка в сохранении записи в бд!";
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}