using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class CreateRoomTypeCommand : IRequest<RoomTypeResponseDTO>
    {
        public string TypeName { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
    }
    public class CreateRoomTypeCommandHandler : IRequestHandler<CreateRoomTypeCommand, RoomTypeResponseDTO>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;

        public CreateRoomTypeCommandHandler(IRoomTypeRepository RoomTypeRepository)
        {
            _roomTypeRepository = RoomTypeRepository;
        }

        public async Task<RoomTypeResponseDTO> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
        {
            var RoomType = new RoomType
            {
                TypeName = request.TypeName,
                Description = request.Description,
                Capacity = request.Capacity
            };

            var result = await _roomTypeRepository.AddRoomTypeAsync(RoomType);

            return new RoomTypeResponseDTO
            {
                Id = result.Id,
                TypeName = result.TypeName,
                Description = result.Description,
                Capacity = result.Capacity
            };
        }
    }
}