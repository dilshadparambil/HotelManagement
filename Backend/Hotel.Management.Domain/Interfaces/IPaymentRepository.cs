
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> AddPaymentAsync(Payment Payment);
        Task UpdatePaymentAsync(Payment Payment);
        Task DeletePaymentAsync(Payment Payment);
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<List<Payment>> GetAllPaymentsAsync();
    }
}