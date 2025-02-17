using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationUpgrade.Data;
using WebApplicationUpgrade.Models;
using WebApplicationUpgrade.Services;
using System.Threading.Tasks;

namespace WebApplicationUpgrade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly AppDbContext _context;

        public IdentityController(UserManager<ApplicationUser> userManager, IJwtAuthenticationManager jwtAuthenticationManager, AppDbContext context)
        {
            _userManager = userManager;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            if (model.ConfirmPassword != model.Password)
            {
                return BadRequest("Passwords do not match");
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var profile = new ProfileEntity()
                {
                    UserId = user.Id,
                    AvatarUrl = "default.png",
                    Timezone = "UTC",
                    Location = "default",
                };
                _context.Profiles.Add(profile);
                await _context.SaveChangesAsync();
                
                return Ok(new { Message = "User registered successfully" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var tokens = await _jwtAuthenticationManager.Authenticate(model.Username, model.Password);
            if (tokens == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(tokens); // Теперь возвращает оба токена
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
        {
            var tokens = await _jwtAuthenticationManager.RefreshToken(model.RefreshToken);
            if (tokens == null)
            {
                return Unauthorized("Invalid refresh token");
            }

            return Ok(tokens);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenModel model)
        {
            bool revoked = await _jwtAuthenticationManager.RevokeRefreshToken(model.RefreshToken);
            if (!revoked)
            {
                return BadRequest("Invalid refresh token");
            }

            return Ok(new { Message = "Logged out successfully" });
        }
    }
}