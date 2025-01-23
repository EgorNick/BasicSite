using System.ComponentModel.DataAnnotations;

namespace WebApplicationUpgrade.Data
{
    public class ContactEntity
    {
        public int Id {get; set;}
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}