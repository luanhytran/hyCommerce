namespace hyCommerce.Domain.Entities
{
    public class Likes : AuditEntity
    {
        public int UserId { get; set; }
        public required User User { get; set; }
        public int ProductId { get; set; }
        public required Product Product { get; set; }
    }
}
