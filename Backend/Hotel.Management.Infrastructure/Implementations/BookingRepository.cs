using Hotel.Management.Domain.Entities;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Management.Infrastructure.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDbContext _dbcontext;

        public BookingRepository(HotelDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Booking> AddBookingAsync(Booking booking)
        {
            _dbcontext.Bookings.Add(booking);
            await _dbcontext.SaveChangesAsync();
            return booking;
        }
        public async Task UpdateBookingAsync(Booking booking)
        {
            _dbcontext.Entry(booking).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();
        }
        public async Task DeleteBookingAsync(Booking booking)
        {
            _dbcontext.Bookings.Remove(booking);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _dbcontext.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(h => h.Id == id);
        }
        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _dbcontext.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .ThenInclude(r => r.HotelClass)
                .ToListAsync();
        }
    }
}
