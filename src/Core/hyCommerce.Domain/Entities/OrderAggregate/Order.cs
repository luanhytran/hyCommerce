using hyCommerce.Domain.Entities.Base;

namespace hyCommerce.Domain.Entities.OrderAggregate
{
    public class Order : AuditEntity, ISoftDelete
    {
        public required string BuyerEmail { get; set; }
        public required ShippingAddress ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public List<OrderItem> OrderItems { get; set; } = [];
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public long Discount { get; set; }
        public required string PaymentIntentId { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public required PaymentSummary PaymentSummary { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }

        public decimal GetTotal()
        {
            return Subtotal + DeliveryFee - Discount;
        }
    }
}
