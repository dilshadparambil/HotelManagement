using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces; 
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Queries
{
    
    public class SearchHotelsQuery : IRequest<List<HotelSearchResponseDTO>>
    {
        public string Destination { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinRating { get; set; }
        public string SortBy { get; set; }
    }

    
    public class SearchHotelsQueryHandler : IRequestHandler<SearchHotelsQuery, List<HotelSearchResponseDTO>>
    {
    
        private readonly IHotelRepository _hotelRepository;

    
        public SearchHotelsQueryHandler(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<List<HotelSearchResponseDTO>> Handle(SearchHotelsQuery request, CancellationToken cancellationToken)
        {
    
            var availableHotels = await _hotelRepository.SearchAvailableHotelsAsync(
                request.Destination,
                request.CheckInDate,
                request.CheckOutDate
            );

    
            var hotelDtos = availableHotels.Select(h => {

                var availableRooms = h.Rooms.Where(r =>
                    r.Status == RoomStatus.Available &&
                    !r.Bookings.Any(b =>
                        request.CheckInDate.HasValue && request.CheckOutDate.HasValue &&
                        b.CheckInDate < request.CheckOutDate.Value &&
                        b.CheckOutDate > request.CheckInDate.Value &&
                        b.Status != BookingStatus.Cancelled
                    )
                );

                return new HotelSearchResponseDTO
                {
                    Id = h.Id,
                    Name = h.Name,
                    Address = h.Address,
                    City = h.City,
                    Country = h.Country,
                    Rating = h.Reviews.Any() ? h.Reviews.Average(rev => rev.Rating) : 0,
                    MinPrice = availableRooms.Any() ? availableRooms.Min(room => room.PricePerNight) : 0,
                    ImageUrl = "https://storage.googleapis.com/uxpilot-auth.appspot.com/da40c9003c-fa87e728b358396c92ff.png"
                };
            }).ToList();

    
            if (request.MaxPrice.HasValue)
            {
                hotelDtos = hotelDtos.Where(h => h.MinPrice > 0 && h.MinPrice <= request.MaxPrice.Value).ToList();
            }
            if (request.MinRating.HasValue)
            {
                hotelDtos = hotelDtos.Where(h => h.Rating >= request.MinRating.Value).ToList();
            }

    
            if (request.SortBy == "Price")
            {
                hotelDtos = hotelDtos.OrderBy(h => h.MinPrice).ToList();
            }
            else
            {
                hotelDtos = hotelDtos.OrderByDescending(h => h.Rating).ToList();
            }

            return hotelDtos;
        }
    }
}