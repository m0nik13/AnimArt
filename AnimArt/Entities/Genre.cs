namespace AnimArt.Entities
{
    public class Genre : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<AnimeGenre> AnimeGenres { get; set; }
    }

    // Допоміжна сутність для many-to-many зв'язку
    public class AnimeGenre
    {
        public int AnimeId { get; set; }
        public int GenreId { get; set; }

        public virtual Anime Anime { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
