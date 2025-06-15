using hyCommerce.Domain.Entities.Base;

namespace hyCommerce.Domain.Entities
{
    public class Cart : AuditEntity
    {
        public required string CartId { get; set; }
        public List<CartItem> Items { get; set; } = [];
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public AppCoupon? Coupon { get; set; }
    }
}
