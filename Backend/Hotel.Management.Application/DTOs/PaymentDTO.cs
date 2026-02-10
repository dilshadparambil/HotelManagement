

using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.DTOs
{
    public class AddPaymentDTO
    {
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
    }
    public class UpdatePaymentDTO
    {
        public int Status { get; set; }
    }
    public class PaymentResponseDTO
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
    }

}
