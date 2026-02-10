using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class UpdateRoomCommand : IRequest<RoomResponseDTO>
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public RoomStatus Status { get; set; }
        public decimal PricePerNight { get; set; }
        public int? HotelClassId { get; set; }
        public int? RoomTypeId { get; set; }
    }
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, RoomResponseDTO>
    {
        private readonly IRoomRepository _roomRepository;

        public UpdateRoomCommandHandler(IRoomRepository RoomRepository)
        { 
            _roomRepository = RoomRepository;
        }

        public async Task<RoomResponseDTO> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetRoomByIdAsync(request.Id);
            if (room == null)
                return null;

            room.RoomNumber = request.RoomNumber ?? room.RoomNumber;
            room.Status = request.Status;
            room.PricePerNight = request.PricePerNight;

            if (request.HotelClassId.HasValue)
            {
                room.HotelClassId = request.HotelClassId.Value;
            }

            if (request.RoomTypeId.HasValue)
            {
                room.RoomTypeId = request.RoomTypeId.Value;
            }

            await _roomRepository.UpdateRoomAsync(room);

            var updatedRoom = await _roomRepository.GetRoomByIdAsync(room.Id);

            
            return new RoomResponseDTO
            {
                Id = updatedRoom.Id,
                RoomNumber = updatedRoom.RoomNumber,
                HotelName = updatedRoom.HotelClass?.Name ?? "Unknown", 
                HotelClassId = updatedRoom.HotelClassId, 
                RoomType = updatedRoom.RoomType?.TypeName ?? "Unknown",
                RoomTypeId = updatedRoom.RoomTypeId,
                PricePerNight = updatedRoom.PricePerNight,
                Status = updatedRoom.Status,
                Capacity = updatedRoom.RoomType?.Capacity ?? 0,
                Description = updatedRoom.RoomType?.Description ?? "N/A"
            };
        }
    }
}