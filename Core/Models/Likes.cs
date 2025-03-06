namespace eCommerceAPI.Core.Models
{
    public class Likes
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = new();
        public int ProductId { get; set; }
        public Product Product { get; set; } = new();
    }
}
