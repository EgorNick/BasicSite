using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationUpgrade.Data;
using WebApplicationUpgrade.Services;

namespace WebApplicationUpgrade.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ProfileController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    [Authorize]
    [HttpGet("getProfile")]
    public async Task<IActionResult> GetProfileInfo()
    {
            
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        return Ok(new
        {
            Username = user.UserName,
            Email = user.Email
        });
    }
}