using System.Web;
using eCommerceAPI.Core.Contracts.Services;
using eCommerceAPI.Core.DTOs;
using eCommerceAPI.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceAPI.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<User> userManager, ITokenService tokenService, IEmailService emailService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Unauthorized("Invalid credentials");;
            
            if(!user.EmailConfirmed)
                return Unauthorized("Email not confirmed");

            return await _tokenService.GenerateToken(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(RegisterDto registerDto)
        {
            var user = new User { Email = registerDto.Email, UserName = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);

                return ValidationProblem();
            }

            await _userManager.AddToRoleAsync(user, "Member");
            
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            
            var encodedToken = HttpUtility.UrlEncode(token);
            
            var confirmationLink = Url.Action("ConfirmEmail", "Account", new
            {
                userId = user.Id,
                token = encodedToken,
            }, Request.Scheme);
            
            await _emailService.SendEmailAsync(
                user.Email,
                "Confirm your account",
                $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>",
                true);

            return Ok("Registration successful. Please check your email to confirm your account.");
        }

        [HttpGet("confirm-email")]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user == null)
                return BadRequest("User not found");
            
            var decodedToken = HttpUtility.UrlDecode(token);
            
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
                return Ok("Email confirmed successfully");
            
            return BadRequest("Invalid token or email confirmation failed");
        }
    }
}
