using System.Web;
using hyCommerce.Application.DTOs;
using hyCommerce.Application.Services;
using hyCommerce.Domain.Entities;
using hyCommerce.Notification.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace hyCommerce.API.Controllers
{
    public class AccountController(UserManager<User> userManager, ITokenService tokenService, IEmailSender emailSender)
        : BaseApiController
    {
        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByNameAsync(loginDto.UserName);

            if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
                return Unauthorized("Invalid credentials");;
            
            if(!user.EmailConfirmed)
                return Unauthorized("Email not confirmed");
            
            return await tokenService.CreateTokenAsync(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(RegisterDto registerDto)
        {
            var user = new User { Email = registerDto.Email, UserName = registerDto.Email };
            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);

                return ValidationProblem();
            }

            await userManager.AddToRoleAsync(user, "Member");
            
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            
            var encodedToken = HttpUtility.UrlEncode(token);
            
            var confirmationLink = Url.Action("ConfirmEmail", "Account", new
            {
                userId = user.Id,
                token = encodedToken,
            }, Request.Scheme);

            var emailRequest = new EmailRequest<object>
            {
                To = user.Email,
                Subject = "Confirm your account",
                Body = $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>",
                IsHtml = true,
            };

            await emailSender.SendEmailAsync(emailRequest);

            return Ok("Registration successful. Please check your email to confirm your account.");
        }

        [HttpGet("confirm-email")]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            
            if (user == null)
                return BadRequest("User not found");
            
            var decodedToken = HttpUtility.UrlDecode(token);
            
            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
                return Ok("Email confirmed successfully");
            
            return BadRequest("Invalid token or email confirmation failed");
        }
        
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResult>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var result = await tokenService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
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
            var success = await tokenService.RevokeRefreshTokenAsync(revokeTokenDto.Token);
            
            if (!success)
                return NotFound("Token not found");
                
            return Ok("Token revoked");
        }
    }
}
