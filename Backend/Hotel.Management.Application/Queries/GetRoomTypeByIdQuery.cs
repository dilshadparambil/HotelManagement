using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Queries
{
    public class GetRoomTypeByIdQuery : IRequest<RoomTypeResponseDTO>
    {
        public int Id { get; set; }
    }
    public class GetRoomTypeByIdQueryHandler : IRequestHandler<GetRoomTypeByIdQuery,RoomTypeResponseDTO>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;


        public GetRoomTypeByIdQueryHandler(IRoomTypeRepository roomTypeRepository)
        {
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<RoomTypeResponseDTO> Handle(GetRoomTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var roomType = await  _roomTypeRepository.GetRoomTypeByIdAsync(request.Id);
            if (roomType == null)
            {
                return null;
            }
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