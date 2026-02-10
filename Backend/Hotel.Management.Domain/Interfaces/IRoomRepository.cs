
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Domain.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room> AddRoomAsync(Room Room);
        Task UpdateRoomAsync(Room Room);
        Task DeleteRoomAsync(Room Room);
        Task<Room> GetRoomByIdAsync(int id);
        Task<List<Room>> GetAllRoomsAsync();
    }
}