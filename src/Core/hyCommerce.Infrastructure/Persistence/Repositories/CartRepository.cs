using hyCommerce.Domain.Entities;
using hyCommerce.Domain.Interfaces;
using hyCommerce.Infrastructure.Persistence.Data;

namespace hyCommerce.Infrastructure.Persistence.Repositories;

public class CartRepository(AppDbContext context) : ICartRepository
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
