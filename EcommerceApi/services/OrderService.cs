using EcommerceApi.Models;

namespace EcommerceApi.services
{
    public class OrderService : IOrderService
    {
        private readonly List<Order> _orders = new()
        {
            new Order {
                Id = 1,
                CustomerId = 1,
                Items = new List<OrderItem> {
                    new OrderItem { ProductId = 1, Quantity = 2 }
                }
            }
        };
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await Task.FromResult(_orders);
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await Task.FromResult(_orders.FirstOrDefault(x=>x.Id == id));
        }
    }
}
