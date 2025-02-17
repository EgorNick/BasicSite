using System.Security.Claims;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationUpgrade.Data;
using WebApplicationUpgrade.Services;

namespace WebApplicationUpgrade.Controllers; // с Profile не работает

[Authorize]
[Route("api/[controller]")]
[ApiController]

public class ProfileController : ControllerBase
{
    private readonly AppDbContext _context;
    
    private readonly CloudinaryService _cloudinaryService;

    public ProfileController(AppDbContext context, CloudinaryService cloudinary)
    {
        _context = context;
        _cloudinaryService = cloudinary;
    }
    
    
    [HttpGet("getProfile")]
    public async Task<IActionResult> GetProfileInfo()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine(userId);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Invalid token");
        }

        var profile = await _context.Profiles.Include(p => p.User).FirstOrDefaultAsync(p => p.User.Id == userId);
        if (profile == null)
        {
            return NotFound("Profile not found");
        }
        
        return Ok(new
        {
            profile.User.UserName,
            profile.User.Email,
            profile.Timezone,
            profile.AvatarUrl,
            profile.Birthday,
            profile.Location,
        });
    }
    
    [HttpPut("updateProfile")]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDto updateProfile)
    {
        if (updateProfile == null || !ModelState.IsValid)
        {
            return BadRequest("Invalid profile data");
        }
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) // проблема с refreshToken и accessToken 
        {
            return NotFound("User not found");
        }
        
        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null)
        {
            return NotFound("Profile not found");
        }
        
        profile.Location = updateProfile.Location;
        profile.Birthday = updateProfile.Birthday;
        profile.Timezone = updateProfile.Timezone;

        if (updateProfile.AvatarFile != null)
        {
            var avatarUrl = await _cloudinaryService.UploadImageAsync(updateProfile.AvatarFile);
            profile.AvatarUrl = avatarUrl;
        }
        await _context.SaveChangesAsync();
        
        return Ok(new {Message = "Profile updated"});
    }
    
}