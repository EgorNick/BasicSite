namespace WebApplicationUpgrade.Services;

public interface IJwtAuthenticationManager
{
    Task<string> Authenticate(string userName, string password);

    Task<string> RefreshToken(string oldToken);
}