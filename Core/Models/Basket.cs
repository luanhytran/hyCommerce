namespace eCommerceAPI.Core.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
    }
}
