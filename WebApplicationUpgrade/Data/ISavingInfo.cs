using WebApplicationUpgrade.Data;
using WebApplicationUpgrade.Models;

public interface ISavingInfo
{
    public bool SavingIntoDbase(ContactModel model);
}