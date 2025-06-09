using hyCommerce.Domain.Entities.Cart;
using hyCommerce.Domain.Interfaces;
using hyCommerce.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace hyCommerce.Infrastructure.Persistence.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;
    public CartRepository(AppDbContext context) => _context = context;

    public async Task<Cart?> GetCartByUserIdAsync(string userId)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.BuyerId == userId);
    }

    public async Task<Cart> CreateOrUpdateCartAsync(Cart cart)
    {
        var existing = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.BuyerId == cart.BuyerId);

        if (existing == null)
        {
            _context.Carts.Add(cart);
        }
        else
        {
            existing.CartItems = cart.CartItems;
            _context.Carts.Update(existing);
        }

        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<bool> RemoveCartAsync(int cartId)
    {
        var cart = await _context.Carts.FindAsync(cartId);
        if (cart == null) return false;
        _context.Carts.Remove(cart);
        return await _context.SaveChangesAsync() > 0;
    }
}
