namespace ECommerceAPI.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty; 
        public int PaymentId { get; set; }
        public decimal Total { get; set; }
        public decimal DeliveryFee { get; set; }
        public int UserId { get; set; }
    }
}
