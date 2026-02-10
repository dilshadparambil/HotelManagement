using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class DeletePaymentCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
    public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, bool>
    {
        private readonly IPaymentRepository _paymentRepository;

        public DeletePaymentCommandHandler(IPaymentRepository PaymentRepository)
        {
            _paymentRepository = PaymentRepository;
        }

        public async Task<bool> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(request.Id);
            if (payment == null)
                return false;

            await _paymentRepository.DeletePaymentAsync(payment);
            return true;
        }
    }
}