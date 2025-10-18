// Entities/Review.cs
using AnimArt.Interfaces;

namespace AnimArt.Entities
{
    public class Review : BaseEntity
    {
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
}