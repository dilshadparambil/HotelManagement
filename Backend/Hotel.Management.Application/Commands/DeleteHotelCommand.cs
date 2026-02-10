using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class DeleteHotelCommand : IRequest<bool>
    {
        public int Id { get; set; }

    }
    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand, bool>
    {
        private readonly IHotelRepository _hotelRepository;

        public DeleteHotelCommandHandler(IHotelRepository HotelRepository)
        {
            _hotelRepository = HotelRepository;
        }

        public async Task<bool> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetHotelByIdAsync(request.Id);
            if (hotel == null)
                return false;

            await _hotelRepository.DeleteHotelAsync(hotel);
            return true;
        }
    }
}