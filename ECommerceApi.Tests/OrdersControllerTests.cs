using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [Fact]
        public async Task GetById_Existing_ReturnsOrder()
        {
           
            var order = new Order { Id = 1, CustomerId = 1 };
            _orderServiceMock.Setup(s => s.GetOrderByIdAsync(1)).ReturnsAsync(order);

            var result = await _ordersController.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOrder = Assert.IsType<Order>(okResult.Value);
            Assert.Equal(1, returnedOrder.Id);
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
