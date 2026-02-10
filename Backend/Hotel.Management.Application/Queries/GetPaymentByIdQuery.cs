using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Queries
{
    public class GetPaymentByIdQuery : IRequest<PaymentResponseDTO>
    {
        public int Id { get; set; }
    }
    public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery,PaymentResponseDTO>
    {
        private readonly IPaymentRepository _paymentRepository;


        public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentResponseDTO> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var payment = await  _paymentRepository.GetPaymentByIdAsync(request.Id);
            if (payment == null)
            {
                return null;
            }
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