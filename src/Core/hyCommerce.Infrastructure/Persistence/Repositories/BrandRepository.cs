using hyCommerce.Domain.Entities;
using hyCommerce.Domain.Interfaces;
using hyCommerce.Infrastructure.Persistence.Data;

namespace hyCommerce.Infrastructure.Persistence.Repositories;

public class BrandRepository(AppDbContext context) : Repository<Brand>(context), IBrandRepository
{
    
}