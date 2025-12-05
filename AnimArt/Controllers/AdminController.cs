// Controllers/AdminController.cs
using System.Security.Claims;
using AnimArt.Entities;
using AnimArt.Entities.ViewModels;
using AnimArt.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AnimArt.Entities.User;

namespace AnimArt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IAnimeRepository _animeRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<Studio> _studioRepository;
        private readonly IRepository<VoiceStudio> _voiceStudioRepository;
        private readonly IRepository<Review> _reviewRepository;

        public AdminController(
            IUserRepository userRepository,
            IAnimeRepository animeRepository,
            IRepository<Genre> genreRepository,
            IRepository<Studio> studioRepository,
            IRepository<VoiceStudio> voiceStudioRepository,
            IRepository<Review> reviewRepository)
        {
            _userRepository = userRepository;
            _animeRepository = animeRepository;
            _genreRepository = genreRepository;
            _studioRepository = studioRepository;
            _voiceStudioRepository = voiceStudioRepository;
            _reviewRepository = reviewRepository;
        }

        public IActionResult Index()
        {
            var stats = new AdminStatsViewModel
            {
                TotalUsers = _userRepository.GetAll().Count(),
                TotalAnime = _animeRepository.GetAll().Count(),
                TotalGenres = _genreRepository.GetAll().Count(),
                TotalStudios = _studioRepository.GetAll().Count(),
                TotalVoiceStudios = _voiceStudioRepository.GetAll().Count(),
                TotalReviews = _reviewRepository.GetAll().Count(),
                RecentUsers = _userRepository.GetAll().OrderByDescending(u => u.RegistrationDate).Take(5)
            };

            return View(stats);
        }

        // --- ANIME MANAGEMENT ---

        public IActionResult Anime()
        {
            var anime = _animeRepository.GetAll();
            var viewModel = new AnimeManagementViewModel
            {
                AnimeList = anime,
                Genres = _genreRepository.GetAll(),
                Studios = _studioRepository.GetAll(),
                VoiceStudios = _voiceStudioRepository.GetAll()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateAnime(string Title, string OriginalTitle, string Description,
                                       AnimeStatus Status, AnimeType Type, int TotalEpisodes,
                                       DateTime ReleaseDate, int? DurationPerEpisode, string AgeRating,
                                       string PosterUrl, List<int> GenreIds, List<int> StudioIds, List<int> VoiceStudioIds)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(Title))
            {
                var anime = new Anime
                {
                    Title = Title,
                    OriginalTitle = OriginalTitle ?? Title,
                    Description = Description ?? "",
                    Status = Status,
                    Type = Type,
                    TotalEpisodes = TotalEpisodes,
                    ReleasedEpisodes = 0,
                    ReleaseDate = ReleaseDate,
                    DurationPerEpisode = DurationPerEpisode ?? 24,
                    AgeRating = AgeRating ?? "PG-13",
                    PosterUrl = PosterUrl ?? ""
                };

                // Ручне додавання зв'язків для Entity Framework
                if (GenreIds != null)
                {
                    foreach (var genreId in GenreIds)
                    {
                        anime.AnimeGenres.Add(new AnimeGenre { GenreId = genreId });
                    }
                }

                if (StudioIds != null)
                {
                    foreach (var studioId in StudioIds)
                    {
                        anime.AnimeStudios.Add(new AnimeStudio { StudioId = studioId, IsMainStudio = true });
                    }
                }

                if (VoiceStudioIds != null)
                {
                    foreach (var voiceId in VoiceStudioIds)
                    {
                        anime.AnimeVoiceStudios.Add(new AnimeVoiceStudio { VoiceStudioId = voiceId });
                    }
                }

                _animeRepository.Add(anime);
                _animeRepository.SaveChanges();

                TempData["SuccessMessage"] = "Аніме успішно додано";
            }
            else
            {
                TempData["ErrorMessage"] = "Помилка при додаванні аніме";
            }

            return RedirectToAction("Anime");
        }

        [HttpPost]
        public IActionResult UpdateAnime(int Id, string Title, string OriginalTitle, string Description,
                                         AnimeStatus Status, AnimeType Type, int TotalEpisodes, int ReleasedEpisodes,
                                         DateTime ReleaseDate, int? DurationPerEpisode, string AgeRating,
                                         string PosterUrl, List<int> GenreIds, List<int> StudioIds, List<int> VoiceStudioIds)
        {
            // Отримуємо аніме з усіма поточними зв'язками (завдяки Include в репозиторії)
            var existingAnime = _animeRepository.GetById(Id);

            if (existingAnime != null)
            {
                // 1. Оновлюємо прості поля
                existingAnime.Title = Title;
                existingAnime.OriginalTitle = OriginalTitle;
                existingAnime.Description = Description;
                existingAnime.Status = Status;
                existingAnime.Type = Type;
                existingAnime.TotalEpisodes = TotalEpisodes;
                existingAnime.ReleasedEpisodes = ReleasedEpisodes;
                existingAnime.ReleaseDate = ReleaseDate;
                existingAnime.DurationPerEpisode = DurationPerEpisode ?? existingAnime.DurationPerEpisode;
                existingAnime.AgeRating = AgeRating;
                existingAnime.PosterUrl = PosterUrl;

                // 2. Оновлюємо Жанри (очищаємо старі та додаємо нові)
                existingAnime.AnimeGenres.Clear();
                if (GenreIds != null)
                {
                    foreach (var gid in GenreIds)
                        existingAnime.AnimeGenres.Add(new AnimeGenre { AnimeId = Id, GenreId = gid });
                }

                // 3. Оновлюємо Студії
                existingAnime.AnimeStudios.Clear();
                if (StudioIds != null)
                {
                    foreach (var sid in StudioIds)
                        existingAnime.AnimeStudios.Add(new AnimeStudio { AnimeId = Id, StudioId = sid, IsMainStudio = true });
                }

                // 4. Оновлюємо Озвучку
                existingAnime.AnimeVoiceStudios.Clear();
                if (VoiceStudioIds != null)
                {
                    foreach (var vid in VoiceStudioIds)
                        existingAnime.AnimeVoiceStudios.Add(new AnimeVoiceStudio { AnimeId = Id, VoiceStudioId = vid });
                }

                _animeRepository.Update(existingAnime);
                _animeRepository.SaveChanges();

                TempData["SuccessMessage"] = "Аніме успішно оновлено";
            }
            else
            {
                TempData["ErrorMessage"] = "Аніме не знайдено";
            }

            return RedirectToAction("Anime");
        }

        [HttpPost]
        public IActionResult DeleteAnime(int id)
        {
            var anime = _animeRepository.GetById(id);
            if (anime != null)
            {
                _animeRepository.Remove(anime);
                _animeRepository.SaveChanges();
                TempData["SuccessMessage"] = "Аніме успішно видалено";
            }
            else
            {
                TempData["ErrorMessage"] = "Аніме не знайдено";
            }
            return RedirectToAction("Anime");
        }

        // --- MANAGE EPISODES ---

        public IActionResult ManageEpisodes(int animeId)
        {
            var anime = _animeRepository.GetById(animeId);
            if (anime == null) return NotFound();
            return View(anime);
        }

        [HttpPost]
        public IActionResult AddEpisode(int animeId, int episodeNumber, string videoUrl, string title)
        {
            var anime = _animeRepository.GetById(animeId);
            if (anime != null && !string.IsNullOrEmpty(videoUrl))
            {
                // Шукаємо, чи є вже серія з таким номером
                var existingEpisode = anime.Episodes.FirstOrDefault(e => e.EpisodeNumber == episodeNumber);

                if (existingEpisode != null)
                {
                    existingEpisode.VideoUrl = videoUrl;
                    existingEpisode.Title = title;
                }
                else
                {
                    anime.Episodes.Add(new Episode
                    {
                        AnimeId = animeId,
                        EpisodeNumber = episodeNumber,
                        Title = title ?? $"Серія {episodeNumber}",
                        VideoUrl = videoUrl
                    });
                }

                _animeRepository.Update(anime);
                _animeRepository.SaveChanges();
                TempData["SuccessMessage"] = "Серію збережено";
            }
            return RedirectToAction("ManageEpisodes", new { animeId = animeId });
        }

        [HttpPost]
        public IActionResult DeleteEpisode(int animeId, int episodeNumber)
        {
            var anime = _animeRepository.GetById(animeId);
            if (anime != null)
            {
                var episode = anime.Episodes.FirstOrDefault(e => e.EpisodeNumber == episodeNumber);
                if (episode != null)
                {
                    anime.Episodes.Remove(episode);
                    _animeRepository.Update(anime);
                    _animeRepository.SaveChanges();
                    TempData["SuccessMessage"] = "Серію видалено";
                }
            }
            return RedirectToAction("ManageEpisodes", new { animeId = animeId });
        }

        // --- USER MANAGEMENT ---

        public IActionResult Users()
        {
            return View(_userRepository.GetAll());
        }

        [HttpPost]
        public IActionResult CreateUser(string Username, string Password, string Role)
        {
            if (_userRepository.GetByUsername(Username) != null)
            {
                TempData["ErrorMessage"] = "Користувач з таким іменем вже існує";
                return RedirectToAction("Users");
            }

            var user = new User
            {
                Username = Username,
                Role = Role == "Admin" ? UserRole.Admin : UserRole.User,
                RegistrationDate = DateTime.Now,
                LastLogin = DateTime.Now
            };
            user.SetPassword(Password);

            _userRepository.Add(user);
            _userRepository.SaveChanges();

            TempData["SuccessMessage"] = "Користувача успішно створено";
            return RedirectToAction("Users");
        }

        [HttpPost]
        public IActionResult UpdateUser(int Id, string Username, string Role)
        {
            var existingUser = _userRepository.GetById(Id);
            if (existingUser != null)
            {
                existingUser.Username = Username;
                existingUser.Role = Role == "Admin" ? UserRole.Admin : UserRole.User;

                _userRepository.Update(existingUser);
                _userRepository.SaveChanges();
                TempData["SuccessMessage"] = "Користувача оновлено";
            }
            return RedirectToAction("Users");
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var currentUserIdStr = User.FindFirst("UserId")?.Value;
            if (currentUserIdStr != null && int.Parse(currentUserIdStr) == id)
            {
                TempData["ErrorMessage"] = "Ви не можете видалити себе";
                return RedirectToAction("Users");
            }

            var user = _userRepository.GetById(id);
            if (user != null)
            {
                _userRepository.Remove(user);
                _userRepository.SaveChanges();
                TempData["SuccessMessage"] = "Користувача видалено";
            }
            return RedirectToAction("Users");
        }

        // --- GENRES, STUDIOS, VOICE STUDIOS ---
        // (Ці методи простіші, оскільки там немає складних зв'язків)

        public IActionResult Genres() => View(_genreRepository.GetAll());
        public IActionResult Studios() => View(_studioRepository.GetAll());
        public IActionResult VoiceStudios() => View(_voiceStudioRepository.GetAll());
        public IActionResult Reviews() => View(_reviewRepository.GetAll()); // Додано перегляд відгуків

        [HttpPost]
        public IActionResult CreateGenre(string Name)
        {
            _genreRepository.Add(new Genre { Name = Name });
            _genreRepository.SaveChanges();
            return RedirectToAction("Genres");
        }

        [HttpPost]
        public IActionResult UpdateGenre(int Id, string Name)
        {
            var genre = _genreRepository.GetById(Id);
            if (genre != null) { genre.Name = Name; _genreRepository.Update(genre); _genreRepository.SaveChanges(); }
            return RedirectToAction("Genres");
        }

        [HttpPost]
        public IActionResult DeleteGenre(int Id)
        {
            var genre = _genreRepository.GetById(Id);
            if (genre != null) { _genreRepository.Remove(genre); _genreRepository.SaveChanges(); }
            return RedirectToAction("Genres");
        }

        // Аналогічні методи для Studio та VoiceStudio (код ідентичний Genre, тільки типи інші)
        [HttpPost]
        public IActionResult CreateStudio(string Name, string JapaneseName) { _studioRepository.Add(new Studio { Name = Name, JapaneseName = JapaneseName }); _studioRepository.SaveChanges(); return RedirectToAction("Studios"); }
        [HttpPost]
        public IActionResult UpdateStudio(int Id, string Name, string JapaneseName) { var s = _studioRepository.GetById(Id); if (s != null) { s.Name = Name; s.JapaneseName = JapaneseName; _studioRepository.Update(s); _studioRepository.SaveChanges(); } return RedirectToAction("Studios"); }
        [HttpPost]
        public IActionResult DeleteStudio(int Id) { var s = _studioRepository.GetById(Id); if (s != null) { _studioRepository.Remove(s); _studioRepository.SaveChanges(); } return RedirectToAction("Studios"); }

        [HttpPost]
        public IActionResult CreateVoiceStudio(string Name, string Country, string Language) { _voiceStudioRepository.Add(new VoiceStudio { Name = Name, Country = Country, Language = Language }); _voiceStudioRepository.SaveChanges(); return RedirectToAction("VoiceStudios"); }
        [HttpPost]
        public IActionResult UpdateVoiceStudio(int Id, string Name, string Country, string Language) { var v = _voiceStudioRepository.GetById(Id); if (v != null) { v.Name = Name; v.Country = Country; v.Language = Language; _voiceStudioRepository.Update(v); _voiceStudioRepository.SaveChanges(); } return RedirectToAction("VoiceStudios"); }
        [HttpPost]
        public IActionResult DeleteVoiceStudio(int Id) { var v = _voiceStudioRepository.GetById(Id); if (v != null) { _voiceStudioRepository.Remove(v); _voiceStudioRepository.SaveChanges(); } return RedirectToAction("VoiceStudios"); }

        [HttpPost]
        public IActionResult DeleteReview(int id)
        {
            var review = _reviewRepository.GetById(id);
            if (review != null)
            {
                _reviewRepository.Remove(review);
                _reviewRepository.SaveChanges();
            }
            return RedirectToAction("Reviews");
        }
    }
}