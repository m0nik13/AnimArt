using AnimArt.Repositories;
using AnimArt.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Реєструємо репозиторії
builder.Services.AddSingleton<IAnimeRepository, AnimeRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
// Додайте інші репозиторії за потреби

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Ініціалізація тестових даних
InitializeTestData(app);

app.Run();

void InitializeTestData(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var animeRepository = scope.ServiceProvider.GetRequiredService<IAnimeRepository>();

    // Додаємо тестові дані, якщо репозиторій порожній
    if (!animeRepository.GetAll().Any())
    {
        var testAnime = new List<Anime>
        {
            new Anime
            {
                Title = "Ця парцелянова лялечка закохалась",
                OriginalTitle = "Sono Bisque Doll wa Koi wo Suru",
                Description = "Старшокласник Вакана Ґоджьо мріє стати майстром ляльок хіна...",
                PosterUrl = "assets/porce....jpg",
                TotalEpisodes = 12,
                ReleasedEpisodes = 12,
                Status = AnimeStatus.Completed,
                Type = AnimeType.TV,
                ReleaseDate = new DateTime(2022, 1, 9),
                DurationPerEpisode = 24,
                AgeRating = "PG-13"
            },
            new Anime
            {
                Title = "Дандадан / Dandadan",
                OriginalTitle = "Dandadan",
                Description = "Старшокласниця Момо Аясе побилась об заклад з місцевим ґіком...",
                PosterUrl = "assets/dandadan.jpg",
                TotalEpisodes = 12,
                ReleasedEpisodes = 12,
                Status = AnimeStatus.Completed,
                Type = AnimeType.TV,
                ReleaseDate = new DateTime(2024, 10, 1),
                DurationPerEpisode = 24,
                AgeRating = "PG-13"
            }
            // Додайте інші аніме з вашого Views/Home/Index.cshtml
        };

        foreach (var anime in testAnime)
        {
            animeRepository.Add(anime);
        }
        animeRepository.SaveChanges();
    }
}