using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class CreateReviewCommand : IRequest<ReviewResponseDTO>
    {
        public int HotelClassId { get; set; }
        public int CustomerId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewResponseDTO>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly ICustomerRepository _customerRepository;

        public CreateReviewCommandHandler(IReviewRepository ReviewRepository, IHotelRepository HotelRepository, ICustomerRepository CustomerRepository)
        {
            _reviewRepository = ReviewRepository;
            _hotelRepository = HotelRepository;
            _customerRepository = CustomerRepository;
        }

        public async Task<ReviewResponseDTO> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = new Review
            {
                HotelClassId = request.HotelClassId,
                CustomerId = request.CustomerId,
                Rating = request.Rating,
                Comment = request.Comment,
                ReviewDate = DateTime.UtcNow
            };

            var result = await _reviewRepository.AddReviewAsync(review);
            var hotel = await _hotelRepository.GetHotelByIdAsync(result.HotelClassId);
            var customer = await _customerRepository.GetCustomerByIdAsync(result.CustomerId);

            return new ReviewResponseDTO
            {
                Id = result.Id,
                HotelName = hotel.Name,
                CustomerName = customer.FullName,
                Rating = result.Rating,
                Comment = result.Comment,
                ReviewDate = result.ReviewDate
            };
        }
    }
}