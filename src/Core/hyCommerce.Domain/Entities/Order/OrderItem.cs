using hyCommerce.Domain.Entities.Base;

namespace hyCommerce.Domain.Entities.Order
{
    public class OrderItem : AuditEntity
    {
        public required ProductItemOrdered ProductItemOrdered { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
    }
}
