using Microsoft.AspNetCore.Identity;

namespace eCommerceAPI.Core.Models
{
    public class User : IdentityUser<int>
    {
        public UserAddress Address { get; set; } = new UserAddress();
    }
}
