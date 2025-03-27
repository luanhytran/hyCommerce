using eCommerceAPI.Core.Models;

namespace eCommerceAPI.Core.Contracts.Services
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
    }
}
