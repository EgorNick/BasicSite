using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationUpgrade.Data;
using WebApplicationUpgrade.Models;
using WebApplicationUpgrade.Services;

namespace WebApplicationUpgrade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly ILogger _logger;

        public IdentityController(UserManager<ApplicationUser> userManager, IJwtAuthenticationManager jwtAuthenticationManager, ILogger<IdentityController> logger)
        {
            _userManager = userManager;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username, 
                Email = model.Email,
            };
            
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
            var token = await _jwtAuthenticationManager.Authenticate(model.Username, model.Password);
            if (token == null)
            {
                return Unauthorized("Invalid credentials");
            }
            return Ok(new {Token = token});
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
        {
            var newAccessToken = await _jwtAuthenticationManager.RefreshToken(model.OldToken);
            if (newAccessToken == null)
            {
                return Unauthorized("Invalid refresh token");
            }
            return Ok(new {newAccessToken = newAccessToken});
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("accessToken");
            return Ok(new {Message = "Logged out successfully"});
        }
        
        [HttpGet]
        public IActionResult Test()
        {
            return Ok(new { Message = "Identity Controller is working!" });
        }
    }
}

