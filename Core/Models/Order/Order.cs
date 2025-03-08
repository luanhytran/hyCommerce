using System.ComponentModel.DataAnnotations;

namespace eCommerceAPI.Core.Models.Order
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required]
        public ShippingAdress ShippingAdress { get; set; } = new ShippingAdress();
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public string DiscountId { get; set; }
        public Discount Discount { get; set; } = new();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public int PaymentId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }

        public decimal GetTotal()
        {
               return Subtotal + DeliveryFee - Discount.Amount;
        }
    }
}
