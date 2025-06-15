using hyCommerce.Domain.Entities;
using hyCommerce.Domain.Interfaces;

namespace hyCommerce.Application.Services
{
    public interface ICartService
    {
        Task<Cart> GetCart();
        Task<Cart> AddItemToCart(int productId, int quantity);
        Task RemoveCartItem(int productId, int quantity);
        Task<Cart> AddCouponCode(string code);
        Task RemoveCouponFromCart();
    }

    public class CartService(ICartRepository cartRepository) : ICartService
    {
        public Task<Cart> GetCart()
        {
            throw new NotImplementedException();
        }

        public Task<Cart> AddItemToCart(int productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveCartItem(int productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task<Cart> AddCouponCode(string code)
        {
            throw new NotImplementedException();
        }

        public Task RemoveCouponFromCart()
        {
            throw new NotImplementedException();
        }
    }
}