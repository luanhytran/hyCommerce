using hyCommerce.Domain.Entities.Base;

namespace hyCommerce.Domain.Entities
{
    public class Basket : AuditEntity
    {
        public int UserId { get; set; }
        public required User User { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
    }
}
