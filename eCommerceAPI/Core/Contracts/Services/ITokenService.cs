using eCommerceAPI.Core.DTOs;
using eCommerceAPI.Core.Models;

namespace eCommerceAPI.Core.Contracts.Services
{
    public interface ITokenService
    {
        public Task<AuthResult> CreateTokenAsync(User user);
        public Task<AuthResult> RefreshTokenAsync(string token);
        public Task<bool> RevokeRefreshTokenAsync(string token);
    }
}
