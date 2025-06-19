using EcommerceApi.Controllers;
using EcommerceApi.Models;
using EcommerceApi.services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ECommerceApi.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductsController _controller;
        public ProductsControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _controller = new ProductsController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllProducts()
        {
            var products = new List<Product> {
            new Product { Id = 1, Name = "Test1", Price = 10 },
            new Product { Id = 2, Name = "Test2", Price = 20 }
            };
            _mockService.Setup(s => s.GetAllProductsAsync()).ReturnsAsync(products);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);

            Assert.Equal(2, ((List<Product>)returnProducts).Count);
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsProduct()
        {
            var product = new Product { Id = 1, Name = "Test1", Price = 10 };
            _mockService.Setup(s => s.GetProductByIdAsync(1)).ReturnsAsync(product);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);

            Assert.Equal(1, returnProduct.Id);
        }

        [Fact]
        public async Task GetById_UnknownId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetProductByIdAsync(99)).ReturnsAsync((Product?)null);

            var result = await _controller.GetById(99);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddProduct_ValidProduct_InvokesServiceAndReturnsCreated()
        {
            var productToAdd = new Product { Id = 3, Name = "Tablet", Price = 400 };
            Product? capturedProduct = null;

            _mockService.Setup(s => s.AddProductAsync(It.IsAny<Product>()))
                        .Callback<Product>(p => capturedProduct = p)
                        .Returns(Task.CompletedTask);

            var result = await _controller.Add(productToAdd);

            _mockService.Verify(s => s.AddProductAsync(It.IsAny<Product>()), Times.Once);

            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
            
            Assert.IsType<Product>(createdAtResult.Value);

            // Verify callback captured correct product
            Assert.Equal(productToAdd.Id, capturedProduct?.Id);
            Assert.Equal(productToAdd.Name, capturedProduct?.Name);
        }

    }
}