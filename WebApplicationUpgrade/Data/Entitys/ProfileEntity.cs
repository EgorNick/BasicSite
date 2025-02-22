using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace WebApplicationUpgrade.Data;

public class ProfileEntity
{
    [Key]
    [ForeignKey("User")]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    
    public string AvatarUrl { get; set; }
    
    public string Timezone { get; set; }
    
    public DateOnly Birthday { get; set; }
    
    public string Location { get; set; }
}