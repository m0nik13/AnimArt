// Entities/Studio.cs
namespace AnimArt.Entities
{
    public class Studio : BaseEntity
    {
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public string LogoUrl { get; set; }

        public virtual ICollection<AnimeStudio> AnimeStudios { get; set; }
    }

    public class AnimeStudio
    {
        public int AnimeId { get; set; }
        public int StudioId { get; set; }
        public bool IsMainStudio { get; set; }

        public virtual Anime Anime { get; set; }
        public virtual Studio Studio { get; set; }
    }
}