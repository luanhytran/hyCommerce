using hyCommerce.Domain.Entities.Base;

namespace hyCommerce.Domain.Entities
{
    public class BasketItem : AuditEntity
    {
        public int ProductId { get; set; }
        public required Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
