using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Queries
{
    public class GetAllHotelsQuery : IRequest<List<HotelResponseDTO>>
    {
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
    }
    public class GetAllHotelsQueryHandler : IRequestHandler<GetAllHotelsQuery, List<HotelResponseDTO>>
    {
        private readonly IHotelRepository _hotelRepository;

        public GetAllHotelsQueryHandler(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<List<HotelResponseDTO>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
        {
            var hotels = await _hotelRepository.GetAllHotelsAsync(request.CheckInDate, request.CheckOutDate);
            var dtoList = hotels.Select(h => new HotelResponseDTO
            {
                Id = h.Id,
                Name = h.Name,
                City = h.City,
                Address = h.Address,
                Country = h.Country,
                PhoneNumber = h.PhoneNumber
            })
            .ToList();
            return dtoList;
        }
    }
}