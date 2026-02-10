using Hotel.Management.Domain.Entities;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Management.Infrastructure.Implementations
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly HotelDbContext _dbcontext;

        public RoomTypeRepository(HotelDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<RoomType> AddRoomTypeAsync(RoomType roomType)
        {
            _dbcontext.RoomTypes.Add(roomType);
            await _dbcontext.SaveChangesAsync();
            return roomType;
        }
        public async Task UpdateRoomTypeAsync(RoomType roomType)
        {
            _dbcontext.Entry(roomType).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();
        }
        public async Task DeleteRoomTypeAsync(RoomType roomType)
        {
            _dbcontext.RoomTypes.Remove(roomType);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<RoomType> GetRoomTypeByIdAsync(int id)
        {
            return await _dbcontext.RoomTypes.FirstOrDefaultAsync(h => h.Id == id);
        }
        public async Task<List<RoomType>> GetAllRoomTypesAsync()
        {
            return await _dbcontext.RoomTypes.ToListAsync();
        }
    }
}
