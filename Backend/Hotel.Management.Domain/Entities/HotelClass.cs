using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Hotel.Management.Domain.Entities
{
    public class HotelClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        
    }
}
