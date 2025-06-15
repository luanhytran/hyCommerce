using System.Security.Claims;

namespace hyCommerce.Extensions.Authentication;

public static class AuthConfig
{
    public static string GetClaimValue(this IEnumerable<Claim> claims, string claimType)
    {
        var claim = claims.FirstOrDefault(c => c.Type == claimType);
        return claim == null ? string.Empty : claim.Value;
    }
    
    public static string GetUsername(this ClaimsPrincipal user)
    {
        return user.Identity?.Name ?? throw new UnauthorizedAccessException();
    }
}