using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class UpdateHotelCommand : IRequest<HotelResponseDTO>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, HotelResponseDTO>
    {
        private readonly IHotelRepository _hotelRepository;

        public UpdateHotelCommandHandler(IHotelRepository HotelRepository)
        {
            _hotelRepository = HotelRepository;
        }

        public async Task<HotelResponseDTO> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetHotelByIdAsync(request.Id);
            if (hotel == null)
                return null;

            hotel.Name = request.Name;
            hotel.Address = request.Address;
            hotel.City = request.City;
            hotel.Country = request.Country;
            hotel.PhoneNumber = request.PhoneNumber;

            await _hotelRepository.UpdateHotelAsync(hotel);

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