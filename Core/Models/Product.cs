namespace eCommerceAPI.Core.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PictureUrl { get; set; }
    public string Description { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = new();
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = new();
    public bool IsDeleted { get; set; }
}