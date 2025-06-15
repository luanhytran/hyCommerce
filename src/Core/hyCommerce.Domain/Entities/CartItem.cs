using hyCommerce.Domain.Entities.Base;

namespace hyCommerce.Domain.Entities
{
    public class CartItem : AuditEntity
    {
        public int Quantity { get; set; }

        // navigation properties
        public int ProductId { get; set; }
        public required Product Product { get; set; }

        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;
    }
}