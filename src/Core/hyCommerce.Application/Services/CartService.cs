using hyCommerce.Domain.Entities.Cart;
using hyCommerce.Domain.Interfaces;

namespace hyCommerce.Application.Services
{
    public interface ICartService
    {
        Task<Cart?> GetCart(string userId);
        Task<Cart> AddOrUpdateCart(string userId, List<CartItem> items);
        Task<bool> RemoveCart(int cartId);
    }

    public class CartService(ICartRepository cartRepository) : ICartService
    {
        public async Task<Cart?> GetCart(string userId)
        {
            return await cartRepository.GetCartByUserIdAsync(userId);
        }

        public async Task<Cart> AddOrUpdateCart(string userId, List<CartItem> items)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId) ?? new Cart { BuyerId = userId, CartItems = new List<CartItem>() };
            cart.CartItems = items;
            return await cartRepository.CreateOrUpdateCartAsync(cart);
        }

        public async Task<bool> RemoveCart(int cartId)
        {
            return await cartRepository.RemoveCartAsync(cartId);
        }
    }
}