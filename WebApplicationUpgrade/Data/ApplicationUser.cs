using Microsoft.AspNetCore.Identity;

namespace WebApplicationUpgrade.Data;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpiryTime { get; set; }
}