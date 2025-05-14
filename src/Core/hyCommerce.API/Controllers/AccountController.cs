using hyCommerce.Application.DTOs;
using hyCommerce.Application.Services;
using hyCommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Web;
using hyCommerce.Infrastructure.Persistence;

namespace hyCommerce.API.Controllers
{
    public class AccountController(ApplicationUserManager userManager, ITokenService tokenService, IIdentityService identityService)
        : BaseApiController
    {
        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login(LoginDto loginDto)
        {
            var result = await identityService.Login(loginDto);

            if (result.IsSuccess)
                return Ok(result.Data);
            
            return Unauthorized(result.ErrorMessage);
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(RegisterDto registerDto)
        {
            var user = new User { Email = registerDto.Email, UserName = registerDto.Email };
            
            var createUserResult = await userManager.CreateAsync(user, registerDto.Password);

            if (!createUserResult.Succeeded)
            {
                foreach (var error in createUserResult.Errors)
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

            var result = await identityService.RegisterUser(user, confirmationLink);
            
            if (result.IsSuccess)
                return Ok(result.Message);
            
            return BadRequest(result.Message);
        }

        [HttpGet("confirm-email")]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await identityService.ConfirmEmail(userId, token);

            if (result.IsSuccess)
                return Ok(result.Message);

            return BadRequest(result.Message);
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