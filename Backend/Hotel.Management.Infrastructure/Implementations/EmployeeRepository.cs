using Hotel.Management.Domain.Entities;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Management.Infrastructure.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HotelDbContext _dbcontext;

        public EmployeeRepository(HotelDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            _dbcontext.Employees.Add(employee);
            await _dbcontext.SaveChangesAsync();
            return employee;

        }
        public async Task UpdateEmployeeAsync(Employee employee)
        {

            _dbcontext.Entry(employee).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();

        }
        public async Task DeleteEmployeeAsync(Employee employee)
        {

            _dbcontext.Employees.Remove(employee);
            await _dbcontext.SaveChangesAsync();

        }
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {

            return await _dbcontext.Employees
                .Include(e => e.HotelClass)
                .FirstOrDefaultAsync(h => h.Id == id);

        }
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {

            return await _dbcontext.Employees.Include(e => e.HotelClass).ToListAsync();

        }
    }
}
