using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using hyCommerce.Core.Models;
using hyCommerce.Infrastructures.Persistence.Data;
using hyCommerce.Core.DTOs;

namespace hyCommerce.Core.Services;

public interface ITokenService
{
    public Task<AuthResult> CreateTokenAsync(User user);
    public Task<AuthResult> RefreshTokenAsync(string token);
    public Task<bool> RevokeRefreshTokenAsync(string token);
}

public class TokenService(IConfiguration config, UserManager<User> userManager, AppDbContext context)
    : ITokenService
{
    public async Task<AuthResult> CreateTokenAsync(User user)
    {
        var accessToken = await GenerateTokenAsync(user);
        var refreshToken = GenerateRefreshToken(user);
        
        context.RefreshTokens.Add(refreshToken);
        
        user.RefreshTokens.Add(refreshToken);

        await context.SaveChangesAsync();

        return new AuthResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToInt32(config["JWTSettings:ExpiryMinutes"]))
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

        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTSettings:TokenKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(config["JWTSettings:ExpiryMinutes"])),
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
        var refreshToken = await context.RefreshTokens
            .SingleOrDefaultAsync(rt => rt.Token == token)
            ?? throw new SecurityTokenException("Invalid refresh token");

        if (!refreshToken.IsActive)
            throw new SecurityTokenException("Refresh token expired or revoked");

        var user = await userManager.Users
            .FirstOrDefaultAsync(u => u.Id == refreshToken.UserId)
            ?? throw new SecurityTokenException("User not found");

        refreshToken.Revoked = DateTime.UtcNow;
        
        await context.SaveChangesAsync();

        var result = await CreateTokenAsync(user);

        return result;
    }

    public async Task<bool> RevokeRefreshTokenAsync(string token)
    {
        var refreshToken = await context.RefreshTokens
            .SingleOrDefaultAsync(rt => rt.Token == token) 
            ?? throw new SecurityTokenException("Invalid refresh token");

        if (!refreshToken.IsActive)
            throw new SecurityTokenException("Refresh token expired or revoked");

        refreshToken.Revoked = DateTime.UtcNow;

        await context.SaveChangesAsync();
        
        return true;
    }
}
