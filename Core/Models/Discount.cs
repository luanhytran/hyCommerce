namespace eCommerceAPI.Core.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
