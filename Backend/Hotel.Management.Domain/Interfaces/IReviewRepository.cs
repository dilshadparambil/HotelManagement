
using Hotel.Management.Domain.Entities;

namespace Hotel.Management.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> AddReviewAsync(Review Review);
        Task UpdateReviewAsync(Review Review);
        Task DeleteReviewAsync(Review Review);
        Task<Review> GetReviewByIdAsync(int id);
        Task<List<Review>> GetAllReviewsAsync();
    }
}