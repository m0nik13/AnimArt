using AnimArt.Entities;
using AnimArt.Interfaces;

namespace AnimArt.Repositories
{
    public class AnimeRepository : Repository<Anime>, IAnimeRepository
    {
        public AnimeRepository(IDataStorage<Anime> storage) : base(storage)
        {
        }

        public IEnumerable<Anime> GetByTitle(string title)
        {
            return _entities.Where(a =>
                a.Title.Contains(title, System.StringComparison.OrdinalIgnoreCase) ||
                a.OriginalTitle.Contains(title, System.StringComparison.OrdinalIgnoreCase)
            );
        }

        public IEnumerable<Anime> GetByStatus(AnimeStatus status)
        {
            return _entities.Where(a => a.Status == status);
        }

        public IEnumerable<Anime> GetByType(AnimeType type)
        {
            return _entities.Where(a => a.Type == type);
        }

        public IEnumerable<Anime> GetByGenre(string genre)
        {
            // Тимчасова реалізація - поки не маємо доступу до Genre через навігаційну властивість
            // Пізніше можна буде оновити, коли буде реалізовано зв'язок з Genre
            return _entities.Where(a =>
                a.Description.Contains(genre, System.StringComparison.OrdinalIgnoreCase) ||
                a.Title.Contains(genre, System.StringComparison.OrdinalIgnoreCase)
            );
        }

        public IEnumerable<Anime> GetRecent(int count)
        {
            return _entities
                .OrderByDescending(a => a.ReleaseDate)
                .Take(count);
        }

        public IEnumerable<Anime> GetSortedByRating()
        {
            // Тимчасова реалізація - поки не маємо рейтингів
            // Пізніше можна буде оновити, коли буде реалізовано зв'язок з Rating
            return _entities
                .OrderByDescending(a => CalculateAverageRating(a)) // Тимчасова логіка
                .ThenBy(a => a.Title);
        }

        public IEnumerable<Anime> GetSortedByReleaseDate()
        {
            return _entities
                .OrderByDescending(a => a.ReleaseDate)
                .ThenBy(a => a.Title);
        }

        // Допоміжний метод для тимчасової реалізації рейтингу
        private double CalculateAverageRating(Anime anime)
        {
            // Тимчасова логіка - можна змінити пізніше
            // Наразі використовуємо ID як псевдо-рейтинг для демонстрації
            return anime.Id % 5 + 3; // Повертає значення від 3.0 до 7.0
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
