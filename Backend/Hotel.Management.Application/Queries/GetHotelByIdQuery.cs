using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Queries
{
    public class GetHotelByIdQuery : IRequest<HotelResponseDTO>
    {
        public int Id { get; set; }
    }
    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery,HotelResponseDTO>
    {
        private readonly IHotelRepository _hotelRepository;


        public GetHotelByIdQueryHandler(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<HotelResponseDTO> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetHotelByIdAsync(request.Id);
            if (hotel == null)
            {
                return null;
            }
            return new HotelResponseDTO
            {
                Id = hotel.Id,
                Name = hotel.Name,
                City = hotel.City,
                Address = hotel.Address,
                Country = hotel.Country,
                PhoneNumber = hotel.PhoneNumber
            };
        }
    }
}