using hyCommerce.Domain.Entities.Base;

namespace hyCommerce.Domain.Entities.OrderAggregate
{
    public class OrderItem : AuditEntity
    {
        public required ProductItemOrdered ItemOrdered { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
    }
}
