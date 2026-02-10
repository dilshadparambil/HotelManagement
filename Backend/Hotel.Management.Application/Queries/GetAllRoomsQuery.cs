using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Queries
{
    public class GetAllRoomsQuery : IRequest<List<RoomResponseDTO>>
    {
    }
    public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, List<RoomResponseDTO>>
    {
        private readonly IRoomRepository _roomRepository;

        public GetAllRoomsQueryHandler(IRoomRepository RoomRepository)
        {
            _roomRepository = RoomRepository;
        }

        public async Task<List<RoomResponseDTO>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
        {
            var rooms = await _roomRepository.GetAllRoomsAsync();
            var today = DateTime.Today;

            var dtoList = rooms.Select(r =>
            {
                
                var displayStatus = r.Status; 

                
                if (r.Status != RoomStatus.UnderMaintenance)
                {
                
                    bool isOccupiedToday = r.Bookings.Any(b =>
                        b.Status == BookingStatus.Confirmed && 
                        b.CheckInDate.Date <= today &&
                        b.CheckOutDate.Date > today
                    );

                    if (isOccupiedToday)
                    {
                        displayStatus = RoomStatus.Booked; 
                    }
                    else
                    {
                        displayStatus = RoomStatus.Available; 
                    }
                }
                

                return new RoomResponseDTO
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    PricePerNight = r.PricePerNight,

                    Status = displayStatus, 

                    HotelClassId = (int)r.HotelClassId,
                    HotelName = r.HotelClass?.Name ?? "Unknown",
                    RoomTypeId = (int)r.RoomTypeId,
                    RoomType = r.RoomType?.TypeName ?? "Unknown",
                    Capacity = r.RoomType?.Capacity ?? 0,
                    Description = r.RoomType?.Description
                };
            }).ToList();

            return dtoList;
        }
    }
}