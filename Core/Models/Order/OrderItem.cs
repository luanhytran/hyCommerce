namespace eCommerceAPI.Core.Models.Order
{
    public class OrderItem
    {
        public int Id { get; set; }
        public required ProductItemOrdered ProductItemOrdered { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
    }
}
