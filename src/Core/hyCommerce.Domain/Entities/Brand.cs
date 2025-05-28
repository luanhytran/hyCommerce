using hyCommerce.Domain.Entities.Base;

namespace hyCommerce.Domain.Entities
{
    public class Brand : SoftDeleteEntity
    {
        public required string Name { get; set; }
    }
}
