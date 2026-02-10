
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> AddCustomerAsync(Customer Customer);
        Task UpdateCustomerAsync(Customer Customer);
        Task DeleteCustomerAsync(Customer Customer);
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<List<Customer>> GetAllCustomersAsync();

        Task<bool> CustomerExistsAsync(string username, string email);
    }
}