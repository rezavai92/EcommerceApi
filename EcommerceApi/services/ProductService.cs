using EcommerceApi.Models;

namespace EcommerceApi.services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Laptop", Price = 1500 },
            new Product { Id = 2, Name = "Smartphone", Price = 800 }
        };

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await Task.FromResult(_products);
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return await Task.FromResult(product);
        }

    }
}
