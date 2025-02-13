using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplicationUpgrade.Data;
using WebApplicationUpgrade.Models;
using WebApplicationUpgrade.Services;

namespace WebApplicationUpgrade.Controllers;

public class AccountController : Controller
{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _userManager = userManager;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _signInManager = signInManager;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    
                    var accessToken = await _jwtAuthenticationManager.Authenticate(model.Username, model.Password);

                    HttpContext.Session.SetString("AccessToken", accessToken);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            
                if (result.Succeeded)
                {
                    var accessToken = await _jwtAuthenticationManager.Authenticate(model.Username, model.Password);
                    
                    HttpContext.Session.SetString("AccessToken", accessToken);
                    
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }
        
            return View(model);
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
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new {Message = "Logout"});
        }
        
        [HttpGet]
        public IActionResult Test()
        {
            return Ok(new { Message = "Identity Controller is working!" });
        }
    }