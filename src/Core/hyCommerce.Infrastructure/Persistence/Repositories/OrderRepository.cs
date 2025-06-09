using hyCommerce.Domain.Entities.Order;
using hyCommerce.Domain.Interfaces;
using hyCommerce.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace hyCommerce.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context) => _context = context;

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.ProductItemOrdered)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
    {
        return await _context.Orders
            .Where(o => o.BuyerId == userId)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.ProductItemOrdered)
            .ToListAsync();
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }
}
