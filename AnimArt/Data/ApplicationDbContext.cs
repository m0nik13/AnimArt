using AnimArt.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnimArt.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Anime> Animes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<VoiceStudio> VoiceStudios { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<UserLists> UserLists { get; set; }

        // Join Tables
        public DbSet<AnimeGenre> AnimeGenres { get; set; }
        public DbSet<AnimeStudio> AnimeStudios { get; set; }
        public DbSet<AnimeVoiceStudio> AnimeVoiceStudios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Налаштування складених ключів для таблиць зв'язків
            modelBuilder.Entity<AnimeGenre>()
                .HasKey(ag => new { ag.AnimeId, ag.GenreId });

            modelBuilder.Entity<AnimeStudio>()
                .HasKey(as_ => new { as_.AnimeId, as_.StudioId });

            modelBuilder.Entity<AnimeVoiceStudio>()
                .HasKey(av => new { av.AnimeId, av.VoiceStudioId });

            // Налаштування поведінки при видаленні (Cascade Delete)
            // Якщо видаляємо Аніме, видаляються і його відгуки
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Anime)
                .WithMany(a => a.Reviews)
                .HasForeignKey(r => r.AnimeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Episode>()
                .HasOne(e => e.Anime)
                .WithMany(a => a.Episodes)
                .HasForeignKey(e => e.AnimeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}