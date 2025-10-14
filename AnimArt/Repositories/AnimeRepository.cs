using AnimArt.Entities;

namespace AnimArt.Repositories
{
    public class AnimeRepository : Repository<Anime>, IAnimeRepository
    {
        public IEnumerable<Anime> GetByTitle(string title)
        {
            return _entities
                .Where(a => a.Title.Contains(title, System.StringComparison.OrdinalIgnoreCase) ||
                           a.OriginalTitle.Contains(title, System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public IEnumerable<Anime> GetByStatus(AnimeStatus status)
        {
            return _entities.Where(a => a.Status == status).ToList();
        }

        public IEnumerable<Anime> GetByType(AnimeType type)
        {
            return _entities.Where(a => a.Type == type).ToList();
        }

        public IEnumerable<Anime> GetSortedByRating()
        {
            // Сортування за середнім рейтингом (якщо б були реальні рейтинги)
            return _entities.OrderByDescending(a => a.Ratings?.Average(r => r.Score) ?? 0).ToList();
        }

        public IEnumerable<Anime> GetSortedByReleaseDate()
        {
            return _entities.OrderByDescending(a => a.ReleaseDate).ToList();
        }
    }
    public interface IAnimeRepository : IRepository<Anime>
    {
        IEnumerable<Anime> GetByTitle(string title);
        IEnumerable<Anime> GetByStatus(AnimeStatus status);
        IEnumerable<Anime> GetByType(AnimeType type);
        IEnumerable<Anime> GetSortedByRating();
        IEnumerable<Anime> GetSortedByReleaseDate();
    }
}
