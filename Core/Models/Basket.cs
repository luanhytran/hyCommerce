namespace eCommerceAPI.Core.Models
{
    public class Basket
    {
        public int Id   { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = new();
        public ICollection<BasketItem> BasketItems = new List<BasketItem>();
    }
}
