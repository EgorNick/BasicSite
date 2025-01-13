using WebApplicationUpgrade.Data;
using WebApplicationUpgrade.Models;

public class SavingInfo : ISavingInfo
{
    private readonly AppDbContext _context;

    public SavingInfo(AppDbContext context){
        _context = context;
    }
    public bool SavingIntoDbase(Contact model)
    {
        try { 
            _context.Contacts.Add(model);
            _context.SaveChanges(); 
        }
        catch (Exception ex) {
            return false;
        }
    return true;
    }
}