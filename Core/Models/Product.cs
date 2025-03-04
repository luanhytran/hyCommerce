namespace ECommerceAPI.Core.Models;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string PictureUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Stock { get; set; }
    public decimal Price { get; set; }
}