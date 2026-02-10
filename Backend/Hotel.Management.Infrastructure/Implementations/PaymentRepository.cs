using Hotel.Management.Domain.Entities;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Management.Infrastructure.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly HotelDbContext _dbcontext;

        public PaymentRepository(HotelDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            _dbcontext.Payments.Add(payment);
            await _dbcontext.SaveChangesAsync();
            return payment;
        }
        public async Task UpdatePaymentAsync(Payment payment)
        {
            _dbcontext.Entry(payment).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();
        }
        public async Task DeletePaymentAsync(Payment payment)
        {
            _dbcontext.Payments.Remove(payment);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<Payment> GetPaymentByIdAsync(int id)
        { 
            return await _dbcontext.Payments.FirstOrDefaultAsync(h => h.Id == id);
        }
        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            return await _dbcontext.Payments.ToListAsync();
        }
    }
}
