using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebApplicationUpgrade.Data;

namespace WebApplicationUpgrade.Services
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public JwtAuthenticationManager(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> Authenticate(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return null;
            }

            // Создание JWT
            var accessToken = GenerateJwtToken(user);

            // Генерация refresh-токена
            var refreshToken = GenerateRefreshToken();
            
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return accessToken;
        }


        public async Task<string> RefreshToken(string oldToken)
        {
            var principal = GetPrincipalFromExpiredToken(oldToken);
            if (principal == null)
            {
                return null;
            }

            var username = principal.Identity?.Name;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || string.IsNullOrEmpty(user.RefreshToken) || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }
            
            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();
            
            user.RefreshToken = newRefreshToken;
            
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            
            _userManager.UpdateAsync(user).Wait();

            return newAccessToken;
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = false // Разрешаем истёкшие токены
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
        }
    }
}