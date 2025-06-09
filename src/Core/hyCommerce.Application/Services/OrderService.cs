using hyCommerce.Domain.Entities;
using hyCommerce.Domain.Entities.Order;
using hyCommerce.Domain.Interfaces;

namespace hyCommerce.Application.Services
{
    public interface IOrderService
    {
        Task<Order?> GetOrderById(int id);
        Task<List<Order>> GetOrdersByUserId(int userId);
        Task<Order> CreateOrderFromCart(int userId);
    }

    public class OrderService(IOrderRepository orderRepository, ICartRepository cartRepository) : IOrderService
    {
        public async Task<Order?> GetOrderById(int id)
        {
            return await orderRepository.GetOrderByIdAsync(id);
        }

        public async Task<List<Order>> GetOrdersByUserId(int userId)
        {
            return await orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<Order> CreateOrderFromCart(int userId)
        {
            var cart = await cartRepository.GetCartByUserIdAsync(userId.ToString());
            if (cart?.CartItems == null || cart.CartItems.Count == 0)
                throw new InvalidOperationException("Cart is empty or not found.");

            var orderItems = cart.CartItems.Select(ci => new OrderItem
            {
                ProductItemOrdered = new ProductItemOrdered
                {
                    Id = ci.ProductId,
                    Name = ci.Product?.Name ?? "",
                    PictureUrl = ci.Product?.PictureUrl ?? ""
                },
                Price = ci.Price,
                Quantity = ci.Quantity
            }).ToList();

            // TODO: Replace with actual ShippingAdress and Discount retrieval logic
            // var shippingAddress = new ShippingAdress
            // {
            //     FullName = null,
            //     Address1 = null,
            //     Address2 = null,
            //     City = null,
            //     State = null,
            //     Zip = null,
            //     Country = null
            // };
            // var discount = new Discount
            // {
            //     Code = null,
            //     Description = null
            // };

            var order = new Order
            {
                BuyerId = userId,
                OrderItems = orderItems,
                OrderDate = DateTime.UtcNow,
                // ShippingAdress = shippingAddress,
                // Discount = discount,
                // Set other order properties as needed
            };

            var createdOrder = await orderRepository.CreateOrderAsync(order);

            await cartRepository.RemoveCartAsync(cart.Id);

            return createdOrder;
        }
    }
}