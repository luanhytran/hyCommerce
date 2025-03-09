using Microsoft.EntityFrameworkCore;

namespace eCommerceAPI.Core.Models.Order
{
    [Owned]
    public class ProductItemOrdered
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public required string PictureUrl { get; set; }
    }
}
