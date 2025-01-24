using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebApplicationUpgrade.Data;

namespace WebApplicationUpgrade.Services;

public class JwtAuthenticationManager : IJwtAuthenticationManager
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public JwtAuthenticationManager(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager; 
        _configuration = configuration;
    }

    public string Authenticate(string username, string password)
    {
        var user = _userManager.FindByNameAsync(username).GetAwaiter().GetResult();
        if (user == null)
        {
            return null;
        }

        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string RefreshToken(string oldToken)
    {
        return string.Empty;
    }
}