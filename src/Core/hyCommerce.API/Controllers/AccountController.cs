using hyCommerce.Application.DTOs;
using hyCommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using hyCommerce.Infrastructure.Persistence;

namespace hyCommerce.API.Controllers
{
    public class AccountController(ITokenService tokenService, IIdentityService identityService)
        : BaseApiController
    {
        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login(LoginDto loginDto)
        {
            var result = await identityService.Login(loginDto);

            if (result.IsSuccess)
                return Ok(result.Data);
            
            return Unauthorized(result.Message);
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(RegisterDto registerDto)
        {
            var baseUrl = Request.Scheme + "://" + Request.Host;

            var result = await identityService.RegisterUser(baseUrl, registerDto);
            
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