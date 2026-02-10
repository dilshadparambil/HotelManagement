using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class CreateHotelCommand : IRequest<HotelResponseDTO>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, HotelResponseDTO>
    {
        private readonly IHotelRepository _hotelRepository;

        public CreateHotelCommandHandler(IHotelRepository HotelRepository)
        {
            _hotelRepository = HotelRepository;
        }

        public async Task<HotelResponseDTO> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            var hotel = new HotelClass
            {
                Name = request.Name,
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _hotelRepository.AddHotelAsync(hotel);

            return new HotelResponseDTO
            {
                Id = result.Id,
                Name = result.Name,
                City = result.City,
                Address = hotel.Address,
                Country = hotel.Country,
                PhoneNumber = hotel.PhoneNumber
            };
        }
    }
}