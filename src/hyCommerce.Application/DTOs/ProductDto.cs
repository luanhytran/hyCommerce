namespace hyCommerce.Application.DTOs;

public class ProductDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public long Price { get; set; }
    public string PictureUrl { get; set; }
    public int QuantityInStock { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
}