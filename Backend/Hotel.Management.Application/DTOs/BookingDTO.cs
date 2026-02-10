
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.DTOs
{
    public class AddBookingDTO
    {
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public BookingStatus Status { get; set; }
    }

    public class UpdateBookingDTO
    {
        public BookingStatus Status { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int RoomId { get; set; }
        public int CustomerId { get; set; }
    }

    public class BookingResponseDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string RoomNumber { get; set; }
        public string HotelName { get; set; }
        public int RoomId { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }

}
