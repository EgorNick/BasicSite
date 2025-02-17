namespace WebApplicationUpgrade.Services;

public interface IJwtAuthenticationManager
{
    Task<object> Authenticate(string userName, string password);

    Task<object> RefreshToken(string oldToken);
    
    Task<bool> RevokeRefreshToken(string oldToken);
}