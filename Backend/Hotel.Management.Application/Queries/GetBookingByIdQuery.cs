using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Queries
{
    public class GetBookingByIdQuery : IRequest<BookingResponseDTO>
    {
        public int Id { get; set; }
    }
    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery,BookingResponseDTO>
    {
        private readonly IBookingRepository _bookingRepository;


        public GetBookingByIdQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<BookingResponseDTO> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var booking = await  _bookingRepository.GetBookingByIdAsync(request.Id);
            if (booking == null)
            {
                return null;
            }
            return new BookingResponseDTO
            {
                Id = booking.Id,
                CustomerName = booking.Customer.FullName,
                CustomerId = booking.CustomerId,
                RoomNumber = booking.Room.RoomNumber,
                RoomId = booking.RoomId,
                Status = booking.Status,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate
            };
        }
    }
}