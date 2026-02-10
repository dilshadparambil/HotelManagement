using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Queries
{
    public class GetRoomByIdQuery : IRequest<RoomResponseDTO>
    {
        public int Id { get; set; }
    }
    public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery,RoomResponseDTO>
    {
        private readonly IRoomRepository _roomRepository;


        public GetRoomByIdQueryHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<RoomResponseDTO> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        {
            var room = await  _roomRepository.GetRoomByIdAsync(request.Id);
            if (room == null)
            {
                return null;
            }
            return new RoomResponseDTO
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                HotelName = room.HotelClass.Name,
                HotelClassId = room.HotelClassId,
                RoomType = room.RoomType.TypeName,
                RoomTypeId = room.RoomTypeId,
                PricePerNight = room.PricePerNight,
                Status = room.Status,
                Capacity = room.RoomType.Capacity,
                Description = room.RoomType.Description
            };
        }
    }
}