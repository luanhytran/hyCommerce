using eCommerceAPI.Core.Contracts.Services;
using eCommerceAPI.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using eCommerceAPI.Core.DTOs;
using eCommerceAPI.Infrastructures.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerceAPI.Core.Services
{
    public class TokenService(IConfiguration config, UserManager<User> userManager, AppDbContext context) : ITokenService
    {
        private readonly IConfiguration _config = config;
        private readonly UserManager<User> _userManager = userManager;
        private readonly AppDbContext _context = context;

        public async Task<AuthResult> CreateTokenAsync(User user)
        {
            var accessToken = await GenerateTokenAsync(user);
            var refreshToken = GenerateRefreshToken(user);
            
            _context.RefreshTokens.Add(refreshToken);
            
            user.RefreshTokens.Add(refreshToken);

            await _context.SaveChangesAsync();

            return new AuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["JWTSettings:ExpiryMinutes"]))
            };
        }

        private async Task<string> GenerateTokenAsync(User user)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSettings:TokenKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["JWTSettings:ExpiryMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private RefreshToken GenerateRefreshToken(User user)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                UserId = user.Id,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7)
            };
        }

        public async Task<AuthResult> RefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .SingleOrDefaultAsync(rt => rt.Token == token)
                ?? throw new SecurityTokenException("Invalid refresh token");

            if (!refreshToken.IsActive)
                throw new SecurityTokenException("Refresh token expired or revoked");

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == refreshToken.UserId)
                ?? throw new SecurityTokenException("User not found");

            refreshToken.Revoked = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();

            var result = await CreateTokenAsync(user);

            return result;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string token)
        {
            var refreshToken = await _context.RefreshTokens
                .SingleOrDefaultAsync(rt => rt.Token == token) 
                ?? throw new SecurityTokenException("Invalid refresh token");

            if (!refreshToken.IsActive)
                throw new SecurityTokenException("Refresh token expired or revoked");

            refreshToken.Revoked = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}
