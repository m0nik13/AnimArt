namespace AnimArt.Entities
{
    public class Rating : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AnimeId { get; set; }
        public int Score { get; set; }
        public DateTime RatedAt { get; set; }

        public virtual User User { get; set; }
        public virtual Anime Anime { get; set; }
    }
}
