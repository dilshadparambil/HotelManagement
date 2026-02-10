using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class UpdateReviewCommand : IRequest<ReviewResponseDTO>
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, ReviewResponseDTO>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly ICustomerRepository _customerRepository;

        public UpdateReviewCommandHandler(IReviewRepository ReviewRepository, IHotelRepository HotelRepository, ICustomerRepository CustomerRepository)
        {
            _reviewRepository = ReviewRepository;
            _hotelRepository = HotelRepository;
            _customerRepository = CustomerRepository;
        }

        public async Task<ReviewResponseDTO> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(request.Id);

            if (review == null)
                return null;

            var hotel = await _hotelRepository.GetHotelByIdAsync(review.HotelClassId);
            var customer = await _customerRepository.GetCustomerByIdAsync(review.CustomerId);

            review.Rating = request.Rating;
            review.Comment = request.Comment;

            await _reviewRepository.UpdateReviewAsync(review);
            return new ReviewResponseDTO
            {
                Id = review.Id,
                HotelName = hotel.Name,
                CustomerName = customer.FullName,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate
            };
        }
    }
}