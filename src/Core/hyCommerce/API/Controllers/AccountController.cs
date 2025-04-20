using System.Web;
using hyCommerce.Core.Contracts.Services;
using hyCommerce.Core.DTOs;
using hyCommerce.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace hyCommerce.API.Controllers
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
        public async Task<ActionResult<AuthResult>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Unauthorized("Invalid credentials");;
            
            if(!user.EmailConfirmed)
                return Unauthorized("Email not confirmed");
            
            return await _tokenService.CreateTokenAsync(user);
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
        
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResult>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var result = await _tokenService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
                return Ok(result);
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<ActionResult> RevokeToken([FromBody] RevokeTokenDto revokeTokenDto)
        {
            var success = await _tokenService.RevokeRefreshTokenAsync(revokeTokenDto.Token);
            
            if (!success)
                return NotFound("Token not found");
                
            return Ok("Token revoked");
        }
    }
}
