using hyCommerce.Domain.Entities.OrderAggregate;
using hyCommerce.Domain.Interfaces;
using hyCommerce.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace hyCommerce.Infrastructure.Persistence.Repositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task<List<Order>> GetOrders(string buyerEmail)
    {
        return await context.Orders
            .Where(o => o.BuyerEmail == buyerEmail)
            .ToListAsync();
    }

    public Task<Order> GetOrderDetails(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Order> CreateOrder(string paymentIntentId)
    {
        throw new NotImplementedException();
    }
}
