using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Queries
{
    public class GetReviewByIdQuery : IRequest<ReviewResponseDTO>
    {
        public int Id { get; set; }
    }
    public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery,ReviewResponseDTO>
    {
        private readonly IReviewRepository _reviewRepository;


        public GetReviewByIdQueryHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<ReviewResponseDTO> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var review = await  _reviewRepository.GetReviewByIdAsync(request.Id);
            if (review == null)
            {
                return null;
            }
            return new ReviewResponseDTO
            {
                Id = review.Id,
                HotelName = review.HotelClass?.Name,
                CustomerName = review.Customer?.FullName,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate
            };
        }
    }
}