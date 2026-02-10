using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;

namespace Hotel.Management.Application.Queries
{
    public class GetAllReviewsQuery : IRequest<List<ReviewResponseDTO>>
    {
    }
    public class GetAllReviewsQueryHandler : IRequestHandler<GetAllReviewsQuery, List<ReviewResponseDTO>>
    {
        private readonly IReviewRepository _ReviewRepository;

        public GetAllReviewsQueryHandler(IReviewRepository ReviewRepository)
        {
            _ReviewRepository = ReviewRepository;
        }

        public async Task<List<ReviewResponseDTO>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            var Reviews = await _ReviewRepository.GetAllReviewsAsync();
            var dtoList = Reviews.Select(r => new ReviewResponseDTO
            {
                Id = r.Id,
                HotelName = r.HotelClass?.Name ?? "Unknown",
                CustomerName = r.Customer?.FullName ?? "Unknown",
                Rating = r.Rating,
                Comment = r.Comment,
                ReviewDate = r.ReviewDate
            })
            .ToList();
            return dtoList;
        }
    }
}