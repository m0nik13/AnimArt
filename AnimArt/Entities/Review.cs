// Entities/Review.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimArt.Entities
{
    public class Review : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public int AnimeId { get; set; }

        [ForeignKey("AnimeId")]
        public virtual Anime Anime { get; set; }

        [Required(ErrorMessage = "Заголовок обов'язковий")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Текст відгуку обов'язковий")]
        public string Content { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Оцінка має бути від 1 до 5")]
        public int Rating { get; set; } // Оцінка прив'язана до відгуку

        public int Likes { get; set; } = 0;
        public int Dislikes { get; set; } = 0;
        public bool ContainsSpoilers { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}