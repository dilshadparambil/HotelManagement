namespace Hotel.Management.Application.DTOs
{
    public class AddReviewDTO
    {
        public int HotelClassId { get; set; }
        public int CustomerId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
    public class UpdateReviewDTO
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
    public class ReviewResponseDTO
    {
        public int Id { get; set; }
        public string HotelName { get; set; }
        public string CustomerName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }

}
