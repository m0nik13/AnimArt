// Repositories/AnimeRepository.cs
using AnimArt.Data;
using AnimArt.Entities;
using AnimArt.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnimArt.Repositories
{
    public class AnimeRepository : Repository<Anime>, IAnimeRepository
    {
        public AnimeRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Перевизначаємо GetById, щоб завантажити всі зв'язки (жанри, відгуки, серії)
        public override Anime GetById(int id)
        {
            return _context.Animes
                .Include(a => a.AnimeGenres).ThenInclude(ag => ag.Genre)
                .Include(a => a.AnimeStudios).ThenInclude(ast => ast.Studio)
                .Include(a => a.AnimeVoiceStudios).ThenInclude(av => av.VoiceStudio)
                .Include(a => a.Episodes)
                .Include(a => a.Reviews).ThenInclude(r => r.User) // Важливо для рейтингу
                .FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Anime> GetSortedByReleaseDate()
        {
            // Для списків нам не обов'язково тягнути епізоди, але потрібен постер і рейтинг
            return _context.Animes
                .Include(a => a.Reviews) // Потрібно для AverageRating
                .OrderByDescending(a => a.ReleaseDate)
                .ThenBy(a => a.Title)
                .ToList();
        }

        public IEnumerable<Anime> GetSortedByRating()
        {
            // Сортування по вираховуваному полю.
            // Примітка: AverageRating обчислюється в пам'яті (клієнтська оцінка),
            // тому спочатку вантажимо дані, потім сортуємо.
            // Для великих баз краще робити це через SQL View або Computed Column.
            var animes = _context.Animes
                .Include(a => a.Reviews)
                .ToList();

            return animes.OrderByDescending(a => a.AverageRating).ToList();
        }

        public IEnumerable<Anime> GetByTitle(string title)
        {
            return _context.Animes
                .Include(a => a.Reviews)
                .Where(a => a.Title.Contains(title) || a.OriginalTitle.Contains(title))
                .ToList();
        }

        public IEnumerable<Anime> GetByStatus(AnimeStatus status)
        {
            return _context.Animes
                .Include(a => a.Reviews)
                .Where(a => a.Status == status)
                .ToList();
        }

        public IEnumerable<Anime> GetByType(AnimeType type)
        {
            return _context.Animes
                .Include(a => a.Reviews)
                .Where(a => a.Type == type)
                .ToList();
        }

        // Метод для складного пошуку (за жанром)
        public IEnumerable<Anime> GetByGenre(int genreId)
        {
            return _context.Animes
                .Include(a => a.AnimeGenres)
                .Include(a => a.Reviews)
                .Where(a => a.AnimeGenres.Any(ag => ag.GenreId == genreId))
                .ToList();
        }
    }
}