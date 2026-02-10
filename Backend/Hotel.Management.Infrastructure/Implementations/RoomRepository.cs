using Hotel.Management.Domain.Entities;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Management.Infrastructure.Implementations
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelDbContext _dbcontext;

        public RoomRepository(HotelDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Room> AddRoomAsync(Room room)
        {
            _dbcontext.Rooms.Add(room);
            await _dbcontext.SaveChangesAsync();
            return room;
        }
        public async Task UpdateRoomAsync(Room room)
        {
            _dbcontext.Entry(room).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();
        }
        public async Task DeleteRoomAsync(Room room)
        {
            _dbcontext.Rooms.Remove(room);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _dbcontext.Rooms
                .Include(r => r.HotelClass)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(h => h.Id == id);
        }
        public async Task<List<Room>> GetAllRoomsAsync()
        {
            return await _dbcontext.Rooms
                .Include(r => r.HotelClass)
                .Include(r => r.RoomType)
                .Include(r => r.Bookings)
                .ToListAsync();
        }
    }
}
