namespace Hotel.Management.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public int? HotelClassId { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }

        public string Username { get; set; } = string.Empty; 
        public string Password { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }

        public HotelClass? HotelClass { get; set; }
    }
}
