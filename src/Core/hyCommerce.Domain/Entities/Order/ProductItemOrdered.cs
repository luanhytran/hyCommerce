namespace hyCommerce.Domain.Entities.Order
{
    public class ProductItemOrdered : AuditEntity
    {
        public required string Name { get; set; }
        public required string PictureUrl { get; set; }
    }
}
