using Hotel.Management.Domain.Entities;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Management.Infrastructure.Implementations
{
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelDbContext _dbcontext;

        public HotelRepository(HotelDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<HotelClass> AddHotelAsync(HotelClass hotel)
        {
            _dbcontext.Hotels.Add(hotel);
            await _dbcontext.SaveChangesAsync();
            return hotel;
        }
        public async Task UpdateHotelAsync(HotelClass hotel)
        {
            _dbcontext.Entry(hotel).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();
        }
        public async Task DeleteHotelAsync(HotelClass hotel)
        {
            _dbcontext.Hotels.Remove(hotel);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<HotelClass> GetHotelByIdAsync(int id)
        {
            return await _dbcontext.Hotels.FirstOrDefaultAsync(h => h.Id == id);
        }
        public async Task<List<HotelClass>> GetAllHotelsAsync(DateTime? checkIn, DateTime? checkOut)
        {
            var query = _dbcontext.Hotels.AsQueryable();

            if (checkIn.HasValue && checkOut.HasValue)
            {
                query = query.Where(h => h.Rooms.Any(r =>
                        !r.Bookings.Any(b =>
                            b.CheckInDate < checkOut.Value &&
                            b.CheckOutDate > checkIn.Value
                            )
                        ));
            }

            return await query.ToListAsync();
        }

        public async Task<List<HotelClass>> SearchAvailableHotelsAsync(string destination, DateTime? checkIn, DateTime? checkOut)
        {
            var query = _dbcontext.Hotels
                .Include(h => h.Reviews)
                .Include(h => h.Rooms)
                    .ThenInclude(r => r.Bookings)
                .AsQueryable();

            if (!string.IsNullOrEmpty(destination))
            {
                query = query.Where(h =>
                    h.City.Contains(destination) ||
                    h.Name.Contains(destination)
                );
            }

            if (checkIn.HasValue && checkOut.HasValue)
            {
                query = query.Where(h => h.Rooms.Any(r =>
                    r.Status == RoomStatus.Available &&
                    !r.Bookings.Any(b =>
                        b.CheckInDate < checkOut.Value &&
                        b.CheckOutDate > checkIn.Value &&
                        b.Status != BookingStatus.Cancelled
                    )
                ));
            }

            return await query.ToListAsync();
        }
    }
}
