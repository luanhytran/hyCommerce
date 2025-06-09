using hyCommerce.Domain.Entities.Base;

namespace hyCommerce.Domain.Entities.Cart
{
    public class CartItem : AuditEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; } = default!;
    }
}