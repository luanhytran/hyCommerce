using hyCommerce.Domain.Entities.OrderAggregate;

namespace hyCommerce.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrders(string buyerEmail);
        Task<Order> GetOrderDetails(int id);

        Task<Order> CreateOrder(string PaymentIntentId);
    }
}
