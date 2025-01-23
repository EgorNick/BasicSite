using WebApplicationUpgrade.Data;
using WebApplicationUpgrade.Models;

public class SavingInfo : ISavingInfo
{
    private readonly AppDbContext _context;
    private readonly ILogger<SavingInfo> _logger;

    public SavingInfo(AppDbContext context, ILogger<SavingInfo> logger){
        _context = context;
        _logger = _logger;
    }
    public bool SavingIntoDbase(ContactModel model)
    {
        try
        {
            var entity = new ContactEntity()
            {
                Name = model.Name,
                Surname = model.Surname,
                Age = model.Age,
                Email = model.Email,
                RecipientEmail = model.RecipientEmail,
                Body = model.Body,
            };
            _context.Contacts.Add(entity);
            _context.SaveChanges(); 
        }
        catch (Exception ex) {
            _logger.LogError(ex, ex.Message);
            return false;
        }

        return true;
    }
}