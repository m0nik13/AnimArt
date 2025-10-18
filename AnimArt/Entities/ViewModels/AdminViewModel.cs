namespace AnimArt.Entities.ViewModels
{
    public class AdminStatsViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalAnime { get; set; }
        public int TotalGenres { get; set; }
        public int TotalStudios { get; set; }
        public int TotalVoiceStudios { get; set; }
        public int TotalReviews { get; set; }
        public IEnumerable<User> RecentUsers { get; set; }
    }

    public class AnimeManagementViewModel
    {
        public IEnumerable<Anime> AnimeList { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Studio> Studios { get; set; }
        public IEnumerable<VoiceStudio> VoiceStudios { get; set; }
    }


}
