using AnimArt.Repositories;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace AnimArt.Entities
{
    public class Anime
    {
        public int Id { get; set; }
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
        public int DurationPerEpisode { get; set; } // в хвилинах
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
    public interface IAnimeRepository : IRepository<Anime>
    {
        Task<IEnumerable<Anime>> GetByStatusAsync(AnimeStatus status);
        Task<IEnumerable<Anime>> GetByTypeAsync(AnimeType type);
        Task<IEnumerable<Anime>> GetByGenreAsync(int genreId);
        Task<IEnumerable<Anime>> GetByStudioAsync(int studioId);
        Task<IEnumerable<Anime>> SearchAsync(string searchTerm);
        Task<IEnumerable<Anime>> GetRecentAsync(int count);
        Task<double> GetAverageRatingAsync(int animeId);
        Task<IEnumerable<Anime>> GetTopRatedAsync(int count);
    }
}
