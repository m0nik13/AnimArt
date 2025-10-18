// Entities/Genre.cs
namespace AnimArt.Entities
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<AnimeGenre> AnimeGenres { get; set; }
    }

    public class AnimeGenre
    {
        public int AnimeId { get; set; }
        public int GenreId { get; set; }

        public virtual Anime Anime { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
