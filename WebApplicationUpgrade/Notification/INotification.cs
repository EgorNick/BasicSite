using Microsoft.AspNetCore.Mvc;
using WebApplicationUpgrade.Models;
public interface INotification
{
    public bool Notificate(ContactModel model);

}