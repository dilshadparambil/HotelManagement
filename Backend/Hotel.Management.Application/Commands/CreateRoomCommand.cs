using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class CreateRoomCommand : IRequest<RoomResponseDTO>
    {
        public string RoomNumber { get; set; }
        public int HotelClassId { get; set; }
        public int RoomTypeId { get; set; }
        public RoomStatus Status { get; set; }
        public decimal PricePerNight { get; set; }
    }
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomResponseDTO>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;

        public CreateRoomCommandHandler(IRoomRepository RoomRepository, IHotelRepository HotelRepository, IRoomTypeRepository RoomTypeRepository)
        {
            _roomRepository = RoomRepository;
            _hotelRepository = HotelRepository;
            _roomTypeRepository = RoomTypeRepository;
        }

        public async Task<RoomResponseDTO> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = new Room
            {
                RoomNumber = request.RoomNumber,
                HotelClassId = request.HotelClassId,
                RoomTypeId = request.RoomTypeId,
                PricePerNight = request.PricePerNight,
                Status = request.Status
            };

            var result = await _roomRepository.AddRoomAsync(room);
            var hotel = await _hotelRepository.GetHotelByIdAsync(result.HotelClassId);
            var roomType = await _roomTypeRepository.GetRoomTypeByIdAsync(result.RoomTypeId);

            return new RoomResponseDTO
            {
                Id = result.Id,
                RoomNumber = result.RoomNumber,
                HotelName = hotel.Name,
                HotelClassId = result.HotelClassId,
                RoomType = roomType.TypeName,
                RoomTypeId = result.RoomTypeId,
                PricePerNight = result.PricePerNight,
                Status = result.Status,
                Capacity = roomType?.Capacity ?? 0,
                Description = roomType?.Description ?? "N/A"
            };
        }
    }
}