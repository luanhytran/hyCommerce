using System.Security.Claims;
using hyCommerce.Application.DTOs;
using hyCommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace hyCommerce.API.Controllers
{
    public class AccountController(ITokenService tokenService, IIdentityService identityService)
        : BaseApiController
    {
        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login([FromBody] LoginDto loginDto)
        {
            var result = await identityService.Login(loginDto);

            if (result.IsSuccess)
                return Ok(result.Value);

            return Unauthorized(result.Error.Description);
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterDto registerDto)
        {
            var baseUrl = Request.Scheme + "://" + Request.Host;

            var result = await identityService.RegisterUser(baseUrl, registerDto);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Error.Description);
        }

        [HttpGet("confirm-email")]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await identityService.ConfirmEmail(userId, token);

            if (result.IsSuccess)
                return Ok("Email confirmed successfully");

            return BadRequest(result.Error.Description);
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

        [Authorize]
        [HttpPut("update-user/{userId}")]
        public async Task<ActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto updateUserDto)
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var isAdmin = User.IsInRole("Admin");

            var result = await identityService.UpdateUser(currentUserId, userId, updateUserDto, isAdmin);

            if (result.IsSuccess)
                return Ok("User updated successfully");

            return BadRequest(result.Error.Description);
        }

        [HttpPost("request-reset-password")]
        public async Task<ActionResult> RequestResetPassword([FromBody] RequestResetPasswordDto requestResetPasswordDto)
        {
            var baseUrl = Request.Scheme + "://" + Request.Host;

            var result = await identityService.RequestResetPassword(requestResetPasswordDto.Email, baseUrl);

            if (result.IsSuccess)
                return Ok("Reset password link sent to your email");

            return BadRequest(result.Error.Description);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var result = await identityService.ResetPassword(resetPasswordDto);

            if (result.IsSuccess)
                return Ok("Password reset successfully");

            return BadRequest(result.Error.Description);
        }
    }
}