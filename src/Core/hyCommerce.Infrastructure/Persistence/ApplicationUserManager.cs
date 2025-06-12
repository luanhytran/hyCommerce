using System.Security.Claims;
using hyCommerce.Domain.Entities;
using hyCommerce.Extensions.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace hyCommerce.Infrastructure.Persistence;

public class ApplicationUserManager : UserManager<User>
{
    private readonly string _userId = "System";
    
    public ApplicationUserManager(
        IUserStore<User> store, 
        IOptions<IdentityOptions> optionsAccessor, 
        IPasswordHasher<User> passwordHasher, 
        IEnumerable<IUserValidator<User>> userValidators, 
        IEnumerable<IPasswordValidator<User>> passwordValidators, 
        ILookupNormalizer keyNormalizer, 
        IdentityErrorDescriber errors, 
        IServiceProvider services, 
        ILogger<UserManager<User>> logger,
        IHttpContextAccessor httpContextAccessor)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        var httpContext = httpContextAccessor?.HttpContext;
        
        if (httpContext != null && httpContext.User != null && httpContext.User.Claims != null)
        {
            _userId = string.IsNullOrEmpty(httpContext.User.Claims.GetClaimValue(ClaimTypes.NameIdentifier))
                ? "System"
                : httpContext.User.Claims.GetClaimValue(ClaimTypes.NameIdentifier);
        }
    }
    
    public override Task<IdentityResult> CreateAsync(User user, string password)
    {
        user.CreatedBy = _userId;
        user.CreatedAt = DateTime.UtcNow;
        user.ModifiedBy = _userId;
        user.ModifiedAt = DateTime.UtcNow;
        return base.CreateAsync(user, password);
    }

    public override Task<IdentityResult> ConfirmEmailAsync(User user, string token)
    {
        user.ModifiedBy = _userId;
        user.ModifiedAt = DateTime.UtcNow;
        return base.ConfirmEmailAsync(user, token);
    }

    public override Task<IdentityResult> UpdateAsync(User user)
    {
        user.ModifiedBy = _userId;
        user.ModifiedAt = DateTime.UtcNow;
        return base.UpdateAsync(user);
    }

    public override Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
    {
        user.ModifiedBy = _userId;
        user.ModifiedAt = DateTime.UtcNow;
        return base.ResetPasswordAsync(user, token, newPassword);   
    }   
}