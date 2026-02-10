using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class UpdatePaymentCommand : IRequest<PaymentResponseDTO>
    {
        public int Id { get; set; }
        public int Status { get; set; }
    }
    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, PaymentResponseDTO>
    {
        private readonly IPaymentRepository _paymentRepository;

        public UpdatePaymentCommandHandler(IPaymentRepository PaymentRepository)
        {
            _paymentRepository = PaymentRepository;
        }

        public async Task<PaymentResponseDTO> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(request.Id);
            if (payment == null)
                return null;

            payment.Status = (PaymentStatus)request.Status;

            await _paymentRepository.UpdatePaymentAsync(payment);
            return new PaymentResponseDTO
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                Amount = payment.Amount,
                PaymentMethod = payment.Method.ToString(),
                Status = payment.Status.ToString(),
                PaymentDate = payment.PaymentDate
            };
        }
    }
}