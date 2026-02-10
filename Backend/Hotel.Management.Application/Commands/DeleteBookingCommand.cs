using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class DeleteBookingCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
    public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand, bool>
    {
        private readonly IBookingRepository _bookingRepository;

        public DeleteBookingCommandHandler(IBookingRepository BookingRepository)
        {
            _bookingRepository = BookingRepository;
        }

        public async Task<bool> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(request.Id);
            if (booking == null)
                return false;

            await _bookingRepository.DeleteBookingAsync(booking);
            return true;
            
        }
    }
}