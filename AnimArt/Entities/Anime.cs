using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AnimArt.Interfaces;

namespace AnimArt.Entities
{
    public class Anime : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string Description { get; set; }
        public string PosterUrl { get; set; }
        public string TrailerUrl { get; set; }

        public int TotalEpisodes { get; set; }
        public int ReleasedEpisodes { get; set; }
        public AnimeStatus Status { get; set; }
        public AnimeType Type { get; set; }

        public DateTime ReleaseDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int DurationPerEpisode { get; set; }
        public string AgeRating { get; set; }

        public virtual ICollection<AnimeGenre> AnimeGenres { get; set; } = new List<AnimeGenre>();
        public virtual ICollection<AnimeStudio> AnimeStudios { get; set; } = new List<AnimeStudio>();
        public virtual ICollection<AnimeVoiceStudio> AnimeVoiceStudios { get; set; } = new List<AnimeVoiceStudio>();

        public virtual ICollection<Episode> Episodes { get; set; } = new List<Episode>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<UserLists> UserLists { get; set; } = new List<UserLists>();

        [NotMapped]
        public double AverageRating
        {
            get
            {
                if (Reviews == null || !Reviews.Any()) return 0;
                return Math.Round(Reviews.Average(r => r.Rating), 1);
            }
        }
    }

    public enum AnimeStatus
    {
        Ongoing,
        Completed,
        Announced,
        Hiatus
    }

    public enum AnimeType
    {
        Series,
        Movie,
        OVA,
        ONA,
        Special
    }
}