using System.Collections.Generic;

namespace AnimArt.Entities
{
    public class Anime : IEntity
    {
        // Видалити public int Id { get; set; } - тепер воно в BaseEntity
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

        // Навігаційні властивості
        public virtual ICollection<AnimeGenre> AnimeGenres { get; set; }
        public virtual ICollection<AnimeStudio> AnimeStudios { get; set; }
        public virtual ICollection<AnimeVoiceStudio> AnimeVoiceStudios { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<UserLists> UserAnimeLists { get; set; }
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