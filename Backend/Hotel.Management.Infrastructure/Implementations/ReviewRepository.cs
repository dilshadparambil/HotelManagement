using Hotel.Management.Domain.Entities;
using Hotel.Management.Domain.Interfaces;
using Hotel.Management.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Management.Infrastructure.Implementations
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly HotelDbContext _dbcontext;

        public ReviewRepository(HotelDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Review> AddReviewAsync(Review review)
        {
            _dbcontext.Reviews.Add(review);
            await _dbcontext.SaveChangesAsync();
            return review;
        }
        public async Task UpdateReviewAsync(Review review)
        {
            _dbcontext.Entry(review).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();
        }
        public async Task DeleteReviewAsync(Review review)
        {
            _dbcontext.Reviews.Remove(review);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<Review> GetReviewByIdAsync(int id)
        {
            return await _dbcontext.Reviews
                .Include(r => r.HotelClass)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(h => h.Id == id);
        }
        public async Task<List<Review>> GetAllReviewsAsync()
        {
            return await _dbcontext.Reviews
                .Include(r => r.HotelClass)
                .Include(r => r.Customer)
                .ToListAsync();
        }
    }
}
