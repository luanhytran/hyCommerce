using hyCommerce.Domain.Entities.Base;

namespace hyCommerce.Domain.Entities
{
    public class Likes : AuditEntity, ISoftDelete
    {
        public int UserId { get; set; }
        public required User User { get; set; }
        public int ProductId { get; set; }
        public required Product Product { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
