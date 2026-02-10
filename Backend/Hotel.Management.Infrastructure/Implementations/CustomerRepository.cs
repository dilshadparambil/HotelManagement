using Hotel.Management.Domain.Entities;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Management.Infrastructure.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly HotelDbContext _dbcontext;

        public CustomerRepository(HotelDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            _dbcontext.Customers.Add(customer);
            await _dbcontext.SaveChangesAsync();
            return customer;
        }
        public async Task UpdateCustomerAsync(Customer customer)
        {
            _dbcontext.Entry(customer).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();
        }
        public async Task DeleteCustomerAsync(Customer customer)
        {
            _dbcontext.Customers.Remove(customer);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _dbcontext.Customers.FirstOrDefaultAsync(h => h.Id == id);
        }
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _dbcontext.Customers.ToListAsync();
        }
        public async Task<bool> CustomerExistsAsync(string username, string email)
        {
            return await _dbcontext.Customers
                .AnyAsync(c => c.Username == username || c.Email == email);
        }
    }
}
