using AnimArt.Interfaces;
using System.Text.Json.Serialization;

namespace AnimArt.Entities
{
    public class Anime : BaseEntity
    {
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

        // Списки ID для зв'язків
        public List<int> GenreIds { get; set; } = new List<int>();
        public List<int> StudioIds { get; set; } = new List<int>();
        public List<int> VoiceStudioIds { get; set; } = new List<int>();

        public Anime()
        {
            Title = string.Empty;
            OriginalTitle = string.Empty;
            Description = string.Empty;
            PosterUrl = string.Empty;
            TrailerUrl = string.Empty;
            AgeRating = string.Empty;
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
        TV,
        Movie,
        OVA,
        ONA,
        Special
    }
}