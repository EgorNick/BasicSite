namespace WebApplicationUpgrade.Models;

public class RefreshTokenModel
{
    public string OldToken { get; set; }
    public string RefreshToken { get; set; }
}