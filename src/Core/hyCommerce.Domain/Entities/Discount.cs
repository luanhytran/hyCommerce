using hyCommerce.Domain.Entities.Base;

namespace hyCommerce.Domain.Entities
{
    public class Discount : SoftDeleteEntity
    {
        public required string Code { get; set; }
        public required string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
