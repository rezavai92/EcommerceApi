using EcommerceApi.Controllers;
using EcommerceApi.Models;
using EcommerceApi.services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ECommerceApi.Tests
{
    public class OrdersControllerTests
    {
        private Mock<IOrderService> _orderServiceMock;
        private OrdersController _ordersController;

        public OrdersControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _ordersController = new OrdersController(_orderServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllOrders()
        {
            var orders = new List<Order> {
            new Order { Id = 1, CustomerId = 1}
                };

            _orderServiceMock.Setup(s => s.GetAllOrdersAsync()).ReturnsAsync(orders);

            var result = await _ordersController.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultOrders = Assert.IsAssignableFrom<IEnumerable<Order>>(okResult.Value);

            Assert.Single(resultOrders);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(-7)]
        public async Task GetById_ShoudReturnOrder_WhenValidIdIsProvided(int orderId)
        {

            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    CustomerId = 1,
                },
                new Order
                {
                    Id = 2,
                    CustomerId = 2,
                },
            };

            var matchedOrder = orders.FirstOrDefault(o => o.Id == orderId);

            _orderServiceMock.Setup(s => s.GetOrderByIdAsync(orderId)).ReturnsAsync(matchedOrder);

            var result = await _ordersController.GetById(orderId);

            // Assert
            if (matchedOrder != null)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnedOrder = Assert.IsType<Order>(okResult.Value);
                Assert.Equal(orderId, returnedOrder.Id);
            }
            else
            {
                Assert.IsType<NotFoundResult>(result);
            }

            _orderServiceMock.Verify(x=>x.GetOrderByIdAsync(orderId),Times.Once);

        }

        [Fact]
        public async Task GetById_Unknown_ReturnsNotFound()
        {
            _orderServiceMock.Setup(s => s.GetOrderByIdAsync(999)).ReturnsAsync((Order?)null);

            var result = await _ordersController.GetById(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
