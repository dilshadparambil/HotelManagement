
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Domain.Interfaces
{
    public interface IHotelRepository
    {
        Task<HotelClass> AddHotelAsync(HotelClass Hotel);
        Task UpdateHotelAsync(HotelClass Hotel);
        Task DeleteHotelAsync(HotelClass Hotel);
        Task<HotelClass> GetHotelByIdAsync(int id);
        Task<List<HotelClass>> GetAllHotelsAsync(DateTime? checkIn, DateTime? checkOut);
        Task<List<HotelClass>> SearchAvailableHotelsAsync(string destination, DateTime? checkIn, DateTime? checkOut);
    }
}


