using Microsoft.AspNetCore.Identity;

namespace eCommerceAPI.Core.Models
{
    public class User : IdentityUser<int>
    {
        public UserAddress Address { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
