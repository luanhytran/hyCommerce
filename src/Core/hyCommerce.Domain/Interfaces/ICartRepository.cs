using hyCommerce.Domain.Entities.Cart;

namespace hyCommerce.Domain.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);
        Task<Cart> CreateOrUpdateCartAsync(Cart cart);
    }
}
