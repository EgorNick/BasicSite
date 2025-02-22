using System.Runtime.InteropServices.JavaScript;

namespace WebApplicationUpgrade.Data;

public class UpdateProfileDto
{
    public string Timezone { get; set; }
    public string Location { get; set; }
    public DateOnly Birthday { get; set; }
    public IFormFile? AvatarFile { get; set; }
}