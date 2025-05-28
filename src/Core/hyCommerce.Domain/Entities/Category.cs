using hyCommerce.Domain.Entities.Base;

namespace hyCommerce.Domain.Entities
{
    public class Category : SoftDeleteEntity
    {
        public required string Name { get; set; }
    }
}
