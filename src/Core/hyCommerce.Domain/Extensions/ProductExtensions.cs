using hyCommerce.Domain.Entities;

namespace hyCommerce.Domain.Extensions
{
    public static class ProductExtensions
    {
        public static IQueryable<Product> Sort(this IQueryable<Product> query, string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy)) return query.OrderBy(p => p.Name);

            query = orderBy switch
            {
                "price" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };

            return query;
        }

        public static IQueryable<Product> Search(this IQueryable<Product> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            return query.Where(p => p.Name.Contains(searchTerm.Trim().ToLower()));
        }

        public static IQueryable<Product> Filter(this IQueryable<Product> query, string category, string brand)
        {
            List<string> categories = new List<string>();
            List<string> brands = new List<string>();

            if (!string.IsNullOrEmpty(category))
                categories.AddRange(category.ToLower().Split(",").ToList());

            if (!string.IsNullOrEmpty(brand))
                brands.AddRange(brand.ToLower().Split(",").ToList());

            query = query.Where(
                p => categories.Count == 0 ||
                categories.Contains(p.Category.Name.ToLower()));

            query = query.Where(
                p => brands.Count == 0 ||
                brands.Contains(p.Brand.Name.ToLower()));

            return query;
        }
    }
}
