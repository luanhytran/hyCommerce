namespace eCommerceAPI.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty; 
        public int PaymentId { get; set; }
        public decimal Total { get; set; }
        public decimal DeliveryFee { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DiscountId { get; set; } = string.Empty;
        public Discount Discount { get; set; } = new();
        public bool IsDeleted { get; set; }

        public ICollection<OrderItem> OrderItems = new List<OrderItem>();
    }
}
