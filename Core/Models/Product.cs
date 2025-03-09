namespace eCommerceAPI.Core.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PictureUrl { get; set; }
    public string Description { get; set; }
    public int QuantityInStock { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int BrandId { get; set; }
    public Brand Brand { get; set; }
    public bool IsDeleted { get; set; }
}