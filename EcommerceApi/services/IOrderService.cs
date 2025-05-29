using EcommerceApi.Models;

namespace EcommerceApi.services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
    }
}
