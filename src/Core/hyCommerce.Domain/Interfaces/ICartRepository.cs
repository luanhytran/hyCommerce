using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hyCommerce.Domain.Entities.Cart;

namespace hyCommerce.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);
        Task<Cart> CreateOrUpdateCartAsync(Cart cart);
        Task<bool> RemoveCartAsync(int cartId);
    }
}
