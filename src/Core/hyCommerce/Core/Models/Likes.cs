namespace hyCommerce.Core.Models
{
    public class Likes
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; }
        public int ProductId { get; set; }
        public required Product Product { get; set; }
    }
}
