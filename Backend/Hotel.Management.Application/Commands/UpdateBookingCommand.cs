using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class UpdateBookingCommand : IRequest<BookingResponseDTO>
    {
        public int Id { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int? RoomId { get; set; }
        public int? CustomerId { get; set; }
    }
    public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, BookingResponseDTO>
    {
        private readonly IBookingRepository _bookingRepository;

        public UpdateBookingCommandHandler(IBookingRepository BookingRepository)
        {
            _bookingRepository = BookingRepository;
        }

        public async Task<BookingResponseDTO> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(request.Id);
            if (booking == null)
                return null;

            booking.Status = request.Status;
            booking.CheckInDate = request.CheckInDate ?? booking.CheckInDate;
            booking.CheckOutDate = request.CheckOutDate ?? booking.CheckOutDate;

            if (request.RoomId.HasValue) booking.RoomId = request.RoomId.Value;
            if (request.CustomerId.HasValue) booking.CustomerId = request.CustomerId.Value;

            await _bookingRepository.UpdateBookingAsync(booking);
            var updatedBooking = await _bookingRepository.GetBookingByIdAsync(booking.Id);

            return new BookingResponseDTO
            {
                Id = updatedBooking.Id,
                CustomerName = updatedBooking.Customer?.FullName ?? "Unknown",
                CustomerId = updatedBooking.CustomerId,
                RoomNumber = updatedBooking.Room?.RoomNumber ?? "Unknown",
                RoomId = updatedBooking.RoomId,
                Status = updatedBooking.Status,
                CheckInDate = updatedBooking.CheckInDate,
                CheckOutDate = updatedBooking.CheckOutDate
            };
        }
    }
}