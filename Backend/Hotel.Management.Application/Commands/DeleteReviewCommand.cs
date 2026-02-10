using MediatR;
using Hotel.Management.Application.DTOs;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Application.Commands
{
    public class DeleteReviewCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly IReviewRepository _reviewRepository;

        public DeleteReviewCommandHandler(IReviewRepository ReviewRepository)
        {
            _reviewRepository = ReviewRepository;
        }

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(request.Id);

            if (review == null)
                return false;

            await _reviewRepository.DeleteReviewAsync(review);
            return true;
        }
    }
}