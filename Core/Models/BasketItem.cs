namespace eCommerceAPI.Core.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = new();
        public int Quantity { get; set; }
    }
}
