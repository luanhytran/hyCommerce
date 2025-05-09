using System.ComponentModel.DataAnnotations;

namespace hyCommerce.Domain.Entities.Order
{
    public class Order : AuditEntity
    {
        public int BuyerId { get; set; }

        [Required]
        public required ShippingAdress ShippingAdress { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public int DiscountId { get; set; }
        public required Discount Discount { get; set; }
        public int PaymentId { get; set; }
        public bool IsDeleted { get; set; }

        public decimal GetTotal()
        {
            return Subtotal + DeliveryFee - Discount.Amount;
        }
    }
}
