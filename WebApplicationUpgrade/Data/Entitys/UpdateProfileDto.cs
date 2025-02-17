namespace WebApplicationUpgrade.Data;

public class UpdateProfileDto
{
    public string Timezone { get; set; }
    public string Location { get; set; }
    public DateTime Birthday { get; set; }
    public IFormFile AvatarFile { get; set; }
}