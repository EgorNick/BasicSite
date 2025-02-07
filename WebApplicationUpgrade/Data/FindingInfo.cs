namespace WebApplicationUpgrade.Data;

using WebApplicationUpgrade.Data;
using WebApplicationUpgrade.Models;

public class FindingInfo
{
    private readonly AppDbContext _context;
    private readonly ILogger<SavingInfo> _logger;

    public FindingInfo(AppDbContext context, ILogger<SavingInfo> logger){
        _context = context;
        _logger = logger;
    }
    public bool FindingFromDbase(ContactModel model)
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
                Subject = model.Subject,
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