using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class CreateBookingCommand : IRequest<BookingResponseDTO>
    {
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public BookingStatus Status { get; set; }
    }
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResponseDTO>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRoomRepository _roomRepository;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository,
                                     ICustomerRepository customerRepository,
                                     IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _customerRepository = customerRepository;
            _roomRepository = roomRepository;
        }

        public async Task<BookingResponseDTO> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var allBookings = await _bookingRepository.GetAllBookingsAsync();



            var hasConflict = allBookings
                .Where(existingBooking => existingBooking.RoomId == request.RoomId)
                .Any(existingBooking =>
                    request.CheckInDate < existingBooking.CheckOutDate && 
                    request.CheckOutDate > existingBooking.CheckInDate    
                );

            if (hasConflict)
            {
                
                throw new Exception("This room is unavailable for the selected dates.");
            }

            var booking = new Booking
            {
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                CustomerId = request.CustomerId,
                RoomId = request.RoomId,
                Status = request.Status,
                TotalAmount = 0
            };

            var result = await _bookingRepository.AddBookingAsync(booking);
            var customer = await _customerRepository.GetCustomerByIdAsync(result.CustomerId);
            var room = await _roomRepository.GetRoomByIdAsync(result.RoomId);

            return new BookingResponseDTO
            {
                Id = result.Id,
                CustomerName = customer?.FullName ?? "N/A",
                CustomerId = result.CustomerId,
                RoomNumber = room?.RoomNumber ?? "N/A",
                Status = result.Status,
                RoomId = result.RoomId,
                CheckInDate = result.CheckInDate,
                CheckOutDate = result.CheckOutDate

            };
        }
    }
}