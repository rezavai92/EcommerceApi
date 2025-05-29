using EcommerceApi.Models;

namespace EcommerceApi.services
{
    public class CustomerService : ICustomerService
    {
        private readonly List<Customer> _customers = new()
        {
            new Customer { Id = 1, Name = "Alice", Email = "alice@example.com" },
            new Customer { Id = 2, Name = "Bob", Email = "bob@example.com" }
        };
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await Task.FromResult(_customers);
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await Task.FromResult(_customers.FirstOrDefault(x=>x.Id == id));
        }
    }
}
