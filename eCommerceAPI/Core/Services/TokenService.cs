using eCommerceAPI.Core.Contracts.Services;
using eCommerceAPI.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using eCommerceAPI.Core.DTOs;
using eCommerceAPI.Infrastructures.Persistence;
using eCommerceAPI.Infrastructures.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math;

namespace eCommerceAPI.Core.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public TokenService(IConfiguration config, UserManager<User> userManager, AppDbContext context)
        {
            _config = config;
            _userManager = userManager;
            _context = context;
        }

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
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSettings:TokenKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
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

        public async Task<AuthResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            
            if (principal == null)
                throw new SecurityTokenException("Invalid access token");

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userId, out int id)) throw new SecurityTokenException("Invalid token");
            
            var user = _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefault(u => u.Id == id);
                
            if (user == null)
                throw new SecurityTokenException("User not found");
                
            var storedRefreshToken = user.RefreshTokens.SingleOrDefault(rt => rt.Token == refreshToken);
                
            if (storedRefreshToken == null)
                throw new SecurityTokenException("Invalid refresh token");
            
            if (!storedRefreshToken.IsActive)
                throw new SecurityTokenException("Refresh token expired or revoked");
                
            storedRefreshToken.Revoked = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();

            var result = await CreateTokenAsync(user);

            return result;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var validationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(_config["JWTSettings:TokenKey"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal =
                    tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> RevokeRefreshTokenAsync(string token)
        {
            var user = _userManager.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == token && rt.IsActive));

            if (user == null)
                return false;

            var refreshToken = _context.RefreshTokens.Single(rt => rt.Token == token);
            refreshToken.Revoked = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}
