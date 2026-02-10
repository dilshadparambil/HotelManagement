
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Domain.Interfaces
{
    public interface IRoomTypeRepository
    {
        Task<RoomType> AddRoomTypeAsync(RoomType RoomType);
        Task UpdateRoomTypeAsync(RoomType RoomType);
        Task DeleteRoomTypeAsync(RoomType RoomType);
        Task<RoomType> GetRoomTypeByIdAsync(int id);
        Task<List<RoomType>> GetAllRoomTypesAsync();
    }
}