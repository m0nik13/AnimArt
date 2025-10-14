namespace AnimArt.Entities
{
    public class UserLists
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AnimeId { get; set; }
        public AnimeListStatus Status { get; set; }
        public int WatchedEpisodes { get; set; }
        public int? UserRating { get; set; }
        public DateTime AddedToDate { get; set; }
        public bool IsFavorite { get; set; }

        // Навігаційні властивості
        public virtual User User { get; set; }
        public virtual Anime Anime { get; set; }
    }

    public enum AnimeListStatus
    {
        Planned,        
        Watching,       
        Completed,      
        OnHold,         
        Dropped         
    }
}
