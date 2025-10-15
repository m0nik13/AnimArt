namespace AnimArt.Entities
{
    public class VoiceStudio : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string WebsiteUrl { get; set; }

        public virtual ICollection<AnimeVoiceStudio> AnimeVoiceStudios { get; set; }
    }

    public class AnimeVoiceStudio
    {
        public int AnimeId { get; set; }
        public int VoiceStudioId { get; set; }

        public virtual Anime Anime { get; set; }
        public virtual VoiceStudio VoiceStudio { get; set; }
    }
}
