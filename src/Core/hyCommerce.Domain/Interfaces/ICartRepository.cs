using hyCommerce.Domain.Entities;

namespace hyCommerce.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCart();
        Task<Cart> AddItemToCart(int productId, int quantity);
        Task RemoveCartItem(int productId, int quantity);
        Task<Cart> AddCouponCode(string code);
        Task RemoveCouponFromCart();
    }
}
