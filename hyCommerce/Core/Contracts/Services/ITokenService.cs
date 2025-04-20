using hyCommerce.Core.DTOs;
using hyCommerce.Core.Models;

namespace hyCommerce.Core.Contracts.Services
{
    public interface ITokenService
    {
        public Task<AuthResult> CreateTokenAsync(User user);
        public Task<AuthResult> RefreshTokenAsync(string token);
        public Task<bool> RevokeRefreshTokenAsync(string token);
    }
}
