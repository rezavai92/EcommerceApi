using EcommerceApi.Controllers;
using EcommerceApi.Models;
using EcommerceApi.services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ECommerceApi.Tests
{
    public class CustomersControllerTests
    {
        private Mock<ICustomerService> _customerServiceMock;
        private CustomersController _customersController;

        public CustomersControllerTests()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _customersController = new CustomersController(_customerServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllCustomers()
        {
            var customers = new List<Customer> {
                    new Customer { Id = 1, Name = "Alice", Email = "alice@example.com" }
                };

           _customerServiceMock.Setup(s => s.GetAllCustomersAsync()).ReturnsAsync(customers);

            var result = await _customersController.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultCustomers = Assert.IsAssignableFrom<IEnumerable<Customer>>(okResult.Value);

            Assert.Single(resultCustomers);
        }

        [Fact]
        public async Task GetById_Unknown_ReturnsNotFound()
        {
            var mockService = new Mock<ICustomerService>();
            mockService.Setup(s => s.GetCustomerByIdAsync(99)).ReturnsAsync((Customer?)null);
            var controller = new CustomersController(mockService.Object);

            var result = await controller.GetById(99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
