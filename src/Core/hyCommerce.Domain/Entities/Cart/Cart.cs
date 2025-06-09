using hyCommerce.Domain.Entities.Base;

namespace hyCommerce.Domain.Entities.Cart
{
    public class Cart : AuditEntity
    {
        public string BuyerId { get; set; } = default!;
        public ICollection<CartItem> CartItems { get; set; } = [];
    }
}
