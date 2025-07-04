﻿using hyCommerce.Domain.Entities.Base;

namespace hyCommerce.Domain.Entities;

public class Product : AuditEntity, ISoftDelete
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public long Price { get; set; }
    public string PictureUrl { get; set; }
    public int QuantityInStock { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int BrandId { get; set; }
    public Brand Brand { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
}