namespace hyCommerce.Domain.Entities.Order
{
    public class ProductItemOrdered
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public required string PictureUrl { get; set; }
    }
}
