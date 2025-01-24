namespace WebApplicationUpgrade.Services;

public interface IJwtAuthenticationManager
{
    string Authenticate(string username, string password);
    string RefreshToken(string oldToken);
}