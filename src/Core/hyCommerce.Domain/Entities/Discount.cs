namespace hyCommerce.Domain.Entities
{
    public class Discount : AuditEntity
    {
        public required string Code { get; set; }
        public required string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
