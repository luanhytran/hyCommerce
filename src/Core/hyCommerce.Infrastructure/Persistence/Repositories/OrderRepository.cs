using hyCommerce.Domain.Entities.Order;
using hyCommerce.Domain.Interfaces;
using hyCommerce.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace hyCommerce.Infrastructure.Persistence.Repositories;

public class OrderRepository(AppDbContext context) : Repository<Order>(context), IOrderRepository
{
    public override async Task<Order?> GetByIdAsync(int id)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.ProductItemOrdered)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
    {
        return await context.Orders
            .Where(o => o.BuyerId == userId)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.ProductItemOrdered)
            .ToListAsync();
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        context.Orders.Add(order);
        await context.SaveChangesAsync();
        return order;
    }
}
