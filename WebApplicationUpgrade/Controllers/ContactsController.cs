using System.Text;
using WebApplicationUpgrade.Models;
using WebApplicationUpgrade.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationUpgrade.Controllers
{
    public class ContactsController : Controller
    {

        private readonly ISavingInfo _savingInfo;

        public ContactsController(ISavingInfo savingInfo)
        {
            _savingInfo = savingInfo;
        }
        
        public IActionResult Index()
        {
            return View(new ContactModel());
        }
        
        public ActionResult<ContactModel> GetEmptyContact()
        {
            return Ok(new ContactModel());
        }

        [HttpPost]
        public IActionResult SaveInfo(ContactModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Пожалуйста, заполните все поля.";
            }

            if (_savingInfo.SavingIntoDbase(model))
            {
                if (_savingInfo.SavingIntoFile(model))
                {
                    TempData["SuccessMessage"] = "Данные успешно сохранены и отправлены!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Произошла ошибка при отправке сообщения.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Произошла ошибка при сохранении данных в базу.";
            }

            return RedirectToAction("Index");
        }
    }
}