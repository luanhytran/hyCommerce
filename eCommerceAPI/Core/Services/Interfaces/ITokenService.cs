using eCommerceAPI.Core.Models;

namespace eCommerceAPI.Core.Services.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
    }
}
