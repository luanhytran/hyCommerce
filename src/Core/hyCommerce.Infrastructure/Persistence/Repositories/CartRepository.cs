using hyCommerce.Domain.Entities.Cart;
using hyCommerce.Domain.Interfaces;
using hyCommerce.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace hyCommerce.Infrastructure.Persistence.Repositories;

public class CartRepository(AppDbContext context) : Repository<Cart>(context), ICartRepository
{
    public async Task<Cart?> GetCartByUserIdAsync(string userId)
    {
        return await context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.BuyerId == userId);
    }

    public async Task<Cart> CreateOrUpdateCartAsync(Cart cart)
    {
        var existing = await context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.BuyerId == cart.BuyerId);

        if (existing == null)
        {
            context.Carts.Add(cart);
        }
        else
        {
            existing.CartItems = cart.CartItems;
            context.Carts.Update(existing);
        }

        await context.SaveChangesAsync();
        return cart;
    }

    public override async Task<bool> DeleteAsync(int id)
    {
        var cart = await context.Carts.FindAsync(id);
        if (cart == null) return false;
        context.Carts.Remove(cart);
        return await context.SaveChangesAsync() > 0;
    }
}
