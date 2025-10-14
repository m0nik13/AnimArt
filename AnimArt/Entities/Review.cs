using AnimArt.Interfaces;
using AnimArt.Repositories;
namespace AnimArt.Entities
{
    public class Review : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AnimeId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public bool ContainsSpoilers { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Навігаційні властивості
        public virtual User User { get; set; }
        public virtual Anime Anime { get; set; }
    }
    namespace AnimArt.Repositories
    {
        public interface IReviewRepository : IRepository<Review>
        {
            Task<IEnumerable<Review>> GetByAnimeIdAsync(int animeId);
            Task<IEnumerable<Review>> GetByUserIdAsync(int userId);
            Task<IEnumerable<Review>> GetTopRatedReviewsAsync(int animeId, int count);
            Task<int> GetReviewCountAsync(int animeId);
        }
    }
}
