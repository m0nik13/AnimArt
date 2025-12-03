// Interfaces/IAnimeRepository.cs
using System.Collections.Generic;
using AnimArt.Entities;

namespace AnimArt.Interfaces
{
    public interface IAnimeRepository : IRepository<Anime>
    {
        IEnumerable<Anime> GetByTitle(string title);
        IEnumerable<Anime> GetByStatus(AnimeStatus status);
        IEnumerable<Anime> GetByType(AnimeType type);
        IEnumerable<Anime> GetSortedByRating();
        IEnumerable<Anime> GetSortedByReleaseDate();
        IEnumerable<Anime> GetByGenre(int genreId); // Новий метод
    }
}