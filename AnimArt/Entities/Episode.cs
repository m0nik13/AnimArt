using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimArt.Entities
{
    public class Episode : BaseEntity
    {
        [Required]
        public int AnimeId { get; set; }

        [ForeignKey("AnimeId")]
        public virtual Anime Anime { get; set; }

        public int EpisodeNumber { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }
    }
}