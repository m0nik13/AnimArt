// Entities/ViewModels/AnimeDetailsViewModel.cs
using AnimArt.Entities;

public class AnimeDetailsViewModel
{
    public Anime Anime { get; set; }
    public IEnumerable<Genre> Genres { get; set; }
    public IEnumerable<Studio> Studios { get; set; }
    public IEnumerable<VoiceStudio> VoiceStudios { get; set; }
    public IEnumerable<Review> Reviews { get; set; }
    public double AverageRating { get; set; }
    public int TotalRatings { get; set; }
}