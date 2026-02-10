using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Queries
{
    public class GetAllPaymentsQuery : IRequest<List<PaymentResponseDTO>>
    {
    }
    public class GetAllPaymentsQueryHandler : IRequestHandler<GetAllPaymentsQuery, List<PaymentResponseDTO>>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetAllPaymentsQueryHandler(IPaymentRepository PaymentRepository)
        {
            _paymentRepository = PaymentRepository;
        }

        public async Task<List<PaymentResponseDTO>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
        {
            var Payments = await _paymentRepository.GetAllPaymentsAsync();
            var dtoList = Payments.Select(p => new PaymentResponseDTO
            {
                Id = p.Id,
                BookingId = p.BookingId,
                Amount = p.Amount,
                PaymentMethod = p.Method.ToString(),
                Status = p.Status.ToString(),
                PaymentDate = p.PaymentDate
            })
            .ToList();
            return dtoList;
        }
    }
}