namespace eCommerceAPI.Core.Models;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public long Price { get; set; }
    public required string PictureUrl { get; set; }
    public int QuantityInStock { get; set; }
    public int CategoryId { get; set; }
    public required Category Category { get; set; }
    public int BrandId { get; set; }
    public required Brand Brand { get; set; }
    public bool IsDeleted { get; set; }
}