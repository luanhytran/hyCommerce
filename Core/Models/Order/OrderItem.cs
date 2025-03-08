namespace eCommerceAPI.Core.Models.Order
{
    public class OrderItem
    {
        public int Id { get; set; }
        public ProductItemOrdered ProductItemOrdered { get; set; } = new();
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
