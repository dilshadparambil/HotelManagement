using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class CreatePaymentCommand : IRequest<PaymentResponseDTO>
    {
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
    }
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, PaymentResponseDTO>
    {
        private readonly IPaymentRepository _paymentRepository;

        public CreatePaymentCommandHandler(IPaymentRepository PaymentRepository)
        {
            _paymentRepository = PaymentRepository;
        }

        public async Task<PaymentResponseDTO> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new Payment
            {
                BookingId = request.BookingId,
                Amount = request.Amount,
                Method = (PaymentMethod)request.Method,
                Status = PaymentStatus.Paid,
                PaymentDate = DateTime.UtcNow
            };

            var result = await _paymentRepository.AddPaymentAsync(payment);

            return new PaymentResponseDTO
            {
                Id = result.Id,
                BookingId = result.BookingId,
                Amount = result.Amount,
                PaymentMethod = result.Method.ToString(),
                Status = result.Status.ToString(),
                PaymentDate = result.PaymentDate
            };
        }
    }
}