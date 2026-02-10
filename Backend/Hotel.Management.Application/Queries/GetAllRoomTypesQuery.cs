using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Queries
{
    public class GetAllRoomTypesQuery : IRequest<List<RoomTypeResponseDTO>>
    {
    }
    public class GetAllRoomTypesQueryHandler : IRequestHandler<GetAllRoomTypesQuery, List<RoomTypeResponseDTO>>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;

        public GetAllRoomTypesQueryHandler(IRoomTypeRepository RoomTypeRepository)
        {
            _roomTypeRepository = RoomTypeRepository;
        }

        public async Task<List<RoomTypeResponseDTO>> Handle(GetAllRoomTypesQuery request, CancellationToken cancellationToken)
        {
            var RoomTypes = await _roomTypeRepository.GetAllRoomTypesAsync();
            var dtoList = RoomTypes.Select(rt => new RoomTypeResponseDTO
            {
                Id = rt.Id,
                TypeName = rt.TypeName,
                Description = rt.Description,
                Capacity = rt.Capacity
            })
            .ToList();
            return dtoList;
        }
    }
}