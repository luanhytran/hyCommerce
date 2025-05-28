using hyCommerce.Domain.Entities.Base;

namespace hyCommerce.Domain.Entities
{
    public class Discount : AuditEntity, ISoftDelete
    {
        public required string Code { get; set; }
        public required string Description { get; set; }
        public decimal Amount { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
