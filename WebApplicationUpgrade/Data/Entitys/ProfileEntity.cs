using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationUpgrade.Data;

public class ProfileEntity
{
    [Key]
    [ForeignKey("User")]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    
    public string AvatarUrl { get; set; }
    
    public string Timezone { get; set; }
    
    public DateTime Birthday { get; set; }
    
    public string Location { get; set; }
}