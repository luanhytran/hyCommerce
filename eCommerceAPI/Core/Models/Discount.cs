namespace eCommerceAPI.Core.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public required string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
