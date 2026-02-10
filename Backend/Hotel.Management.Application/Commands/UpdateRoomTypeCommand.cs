using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class UpdateRoomTypeCommand : IRequest<RoomTypeResponseDTO>
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
    }
    public class UpdateRoomTypeCommandHandler : IRequestHandler<UpdateRoomTypeCommand, RoomTypeResponseDTO>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;

        public UpdateRoomTypeCommandHandler(IRoomTypeRepository RoomTypeRepository)
        {
            _roomTypeRepository = RoomTypeRepository;
        }

        public async Task<RoomTypeResponseDTO> Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken)
        {
            var roomType = await _roomTypeRepository.GetRoomTypeByIdAsync(request.Id);
            if (roomType == null)
                return null;

            roomType.TypeName = request.TypeName;
            roomType.Description = request.Description;
            roomType.Capacity = request.Capacity;

            await _roomTypeRepository.UpdateRoomTypeAsync(roomType);
            return new RoomTypeResponseDTO
            {
                Id = roomType.Id,
                TypeName = roomType.TypeName,
                Description = roomType.Description,
                Capacity = roomType.Capacity
            };
        }
    }
}