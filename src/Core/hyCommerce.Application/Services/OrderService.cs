using hyCommerce.Application.DTOs;
using hyCommerce.Domain.Entities.OrderAggregate;
using hyCommerce.Domain.Interfaces;

namespace hyCommerce.Application.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetOrders(string buyerEmail);
        Task<Order> GetOrderDetails(int id);

        Task<Order> CreateOrder(CreateOrderDto orderDto);
    }

    public class OrderService(IOrderRepository orderRepository, ICartRepository cartRepository) : IOrderService
    {
        public async Task<List<OrderDto>> GetOrders(string buyerEmail)
        {
            // TODO: project to OrderDto here
            //return await orderRepository.GetOrders(buyerEmail);
            return [];
        }

        public Task<Order> GetOrderDetails(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> CreateOrder(CreateOrderDto orderDto)
        {
            throw new NotImplementedException();
        }
    }
}