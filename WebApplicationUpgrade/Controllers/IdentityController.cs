using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationUpgrade.Data;
using WebApplicationUpgrade.Models;
using WebApplicationUpgrade.Services;

namespace WebApplicationUpgrade.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public IdentityController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok(new {Message = "User registered successfully"});
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = _jwtAuthenticationManager.Authenticate(model.Username, model.Password);
            if (token == null)
            {
                return Unauthorized("Invalid credentials");
            }
            return Ok(new {Token = token});
        }

        [HttpPost("refreshToken")]
        public IActionResult RefreshToken([FromBody] RefreshTokenModel model)
        {
            var token = _jwtAuthenticationManager.RefreshToken(model.OldToken);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Invalid refresh token");
            }
            return Ok(new {Token = token});
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new {Message = "Logged out successfully"});
        }
    }
}

