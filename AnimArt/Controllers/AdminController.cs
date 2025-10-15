// Controllers/AdminController.cs
using AnimArt.Entities;
using AnimArt.Interfaces;
using AnimArt.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AnimArt.Entities.User;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IAnimeRepository _animeRepository;
    private readonly IRepository<Genre> _genreRepository;
    private readonly IRepository<Studio> _studioRepository;
    private readonly IRepository<VoiceStudio> _voiceStudioRepository;
    private readonly IRepository<Review> _reviewRepository;
    private readonly IRepository<Rating> _ratingRepository;

    public AdminController(
        IUserRepository userRepository,
        IAnimeRepository animeRepository,
        IRepository<Genre> genreRepository,
        IRepository<Studio> studioRepository,
        IRepository<VoiceStudio> voiceStudioRepository,
        IRepository<Review> reviewRepository,
        IRepository<Rating> ratingRepository)
    {
        _userRepository = userRepository;
        _animeRepository = animeRepository;
        _genreRepository = genreRepository;
        _studioRepository = studioRepository;
        _voiceStudioRepository = voiceStudioRepository;
        _reviewRepository = reviewRepository;
        _ratingRepository = ratingRepository;
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

    // Керування користувачами
    public IActionResult Users()
    {
        var users = _userRepository.GetAll();
        return View(users);
    }

    [HttpPost]
    public IActionResult CreateUser(string Username, string Password, string Role)
    {
        if (ModelState.IsValid && !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
        {
            // Перевірка на унікальність імені
            if (_userRepository.GetByUsername(Username) != null)
            {
                TempData["ErrorMessage"] = "Користувач з таким іменем вже існує";
                return RedirectToAction("Users");
            }

            var user = new User
            {
                Id = _userRepository.GetAll().Any() ?
                    _userRepository.GetAll().Max(u => u.Id) + 1 : 1,
                Username = Username,
                Role = Role == "Admin" ? UserRole.Admin : UserRole.User,
                RegistrationDate = DateTime.Now,
                LastLogin = DateTime.Now
            };

            user.SetPassword(Password);
            _userRepository.Add(user);
            _userRepository.SaveChanges();

            TempData["SuccessMessage"] = "Користувача успішно створено";
        }
        else
        {
            TempData["ErrorMessage"] = "Помилка при створенні користувача";
        }

        return RedirectToAction("Users");
    }

    [HttpPost]
    public IActionResult UpdateUser(int Id, string Username, string Role)
    {
        if (ModelState.IsValid)
        {
            var existingUser = _userRepository.GetById(Id);
            if (existingUser != null)
            {
                existingUser.Username = Username;
                existingUser.Role = Role == "Admin" ? UserRole.Admin : UserRole.User;

                _userRepository.Update(existingUser);
                _userRepository.SaveChanges();

                TempData["SuccessMessage"] = "Користувача успішно оновлено";
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Помилка при оновленні користувача";
        }

        return RedirectToAction("Users");
    }

    [HttpPost]
    public IActionResult DeleteUser(int id)
    {
        var user = _userRepository.GetById(id);
        if (user != null && user.Role != UserRole.Admin)
        {
            _userRepository.Remove(user);
            _userRepository.SaveChanges();
            TempData["SuccessMessage"] = "Користувача успішно видалено";
        }
        else
        {
            TempData["ErrorMessage"] = "Неможливо видалити адміністратора";
        }
        return RedirectToAction("Users");
    }

    // Керування аніме
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
                                   DateTime ReleaseDate)
    {
        if (ModelState.IsValid && !string.IsNullOrEmpty(Title))
        {
            var anime = new Anime
            {
                Id = _animeRepository.GetAll().Any() ?
                    _animeRepository.GetAll().Max(a => a.Id) + 1 : 1,
                Title = Title,
                OriginalTitle = OriginalTitle ?? Title,
                Description = Description ?? "",
                Status = Status,
                Type = Type,
                TotalEpisodes = TotalEpisodes,
                ReleasedEpisodes = 0,
                ReleaseDate = ReleaseDate,
                DurationPerEpisode = 24, // За замовчуванням
                AgeRating = "PG-13" // За замовчуванням
            };

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
                                   DateTime ReleaseDate)
    {
        if (ModelState.IsValid)
        {
            var existingAnime = _animeRepository.GetById(Id);
            if (existingAnime != null)
            {
                existingAnime.Title = Title;
                existingAnime.OriginalTitle = OriginalTitle;
                existingAnime.Description = Description;
                existingAnime.Status = Status;
                existingAnime.Type = Type;
                existingAnime.TotalEpisodes = TotalEpisodes;
                existingAnime.ReleasedEpisodes = ReleasedEpisodes;
                existingAnime.ReleaseDate = ReleaseDate;

                _animeRepository.Update(existingAnime);
                _animeRepository.SaveChanges();

                TempData["SuccessMessage"] = "Аніме успішно оновлено";
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Помилка при оновленні аніме";
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
            TempData["ErrorMessage"] = "Помилка при видаленні аніме";
        }
        return RedirectToAction("Anime");
    }

    // Керування жанрами
    public IActionResult Genres()
    {
        var genres = _genreRepository.GetAll();
        return View(genres);
    }

    [HttpPost]
    public IActionResult CreateGenre(string Name, string Description)
    {
        if (ModelState.IsValid && !string.IsNullOrEmpty(Name))
        {
            var genre = new Genre
            {
                Id = _genreRepository.GetAll().Any() ?
                    _genreRepository.GetAll().Max(g => g.Id) + 1 : 1,
                Name = Name,
                Description = Description ?? ""
            };

            _genreRepository.Add(genre);
            _genreRepository.SaveChanges();

            TempData["SuccessMessage"] = "Жанр успішно додано";
        }
        else
        {
            TempData["ErrorMessage"] = "Помилка при додаванні жанру";
        }

        return RedirectToAction("Genres");
    }

    [HttpPost]
    public IActionResult UpdateGenre(int Id, string Name, string Description)
    {
        if (ModelState.IsValid)
        {
            var existingGenre = _genreRepository.GetById(Id);
            if (existingGenre != null)
            {
                existingGenre.Name = Name;
                existingGenre.Description = Description;

                _genreRepository.Update(existingGenre);
                _genreRepository.SaveChanges();

                TempData["SuccessMessage"] = "Жанр успішно оновлено";
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Помилка при оновленні жанру";
        }

        return RedirectToAction("Genres");
    }

    [HttpPost]
    public IActionResult DeleteGenre(int id)
    {
        var genre = _genreRepository.GetById(id);
        if (genre != null)
        {
            _genreRepository.Remove(genre);
            _genreRepository.SaveChanges();
            TempData["SuccessMessage"] = "Жанр успішно видалено";
        }
        else
        {
            TempData["ErrorMessage"] = "Помилка при видаленні жанру";
        }
        return RedirectToAction("Genres");
    }

    // Керування студіями
    public IActionResult Studios()
    {
        var studios = _studioRepository.GetAll();
        return View(studios);
    }

    [HttpPost]
    public IActionResult CreateStudio(string Name, string JapaneseName, string Description, DateTime FoundedDate)
    {
        if (ModelState.IsValid && !string.IsNullOrEmpty(Name))
        {
            var studio = new Studio
            {
                Id = _studioRepository.GetAll().Any() ?
                    _studioRepository.GetAll().Max(s => s.Id) + 1 : 1,
                Name = Name,
                JapaneseName = JapaneseName ?? "",
                Description = Description ?? "",
                FoundedDate = FoundedDate
            };

            _studioRepository.Add(studio);
            _studioRepository.SaveChanges();

            TempData["SuccessMessage"] = "Студію успішно додано";
        }
        else
        {
            TempData["ErrorMessage"] = "Помилка при додаванні студії";
        }

        return RedirectToAction("Studios");
    }

    [HttpPost]
    public IActionResult UpdateStudio(int Id, string Name, string JapaneseName, string Description, DateTime FoundedDate)
    {
        if (ModelState.IsValid)
        {
            var existingStudio = _studioRepository.GetById(Id);
            if (existingStudio != null)
            {
                existingStudio.Name = Name;
                existingStudio.JapaneseName = JapaneseName;
                existingStudio.Description = Description;
                existingStudio.FoundedDate = FoundedDate;

                _studioRepository.Update(existingStudio);
                _studioRepository.SaveChanges();

                TempData["SuccessMessage"] = "Студію успішно оновлено";
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Помилка при оновленні студії";
        }

        return RedirectToAction("Studios");
    }

    [HttpPost]
    public IActionResult DeleteStudio(int id)
    {
        var studio = _studioRepository.GetById(id);
        if (studio != null)
        {
            _studioRepository.Remove(studio);
            _studioRepository.SaveChanges();
            TempData["SuccessMessage"] = "Студію успішно видалено";
        }
        else
        {
            TempData["ErrorMessage"] = "Помилка при видаленні студії";
        }
        return RedirectToAction("Studios");
    }

    // Керування студіями озвучення
    public IActionResult VoiceStudios()
    {
        var voiceStudios = _voiceStudioRepository.GetAll();
        return View(voiceStudios);
    }

    [HttpPost]
    public IActionResult CreateVoiceStudio(string Name, string Country, string Language)
    {
        if (ModelState.IsValid && !string.IsNullOrEmpty(Name))
        {
            var voiceStudio = new VoiceStudio
            {
                Id = _voiceStudioRepository.GetAll().Any() ?
                    _voiceStudioRepository.GetAll().Max(v => v.Id) + 1 : 1,
                Name = Name,
                Country = Country ?? "",
                Language = Language ?? ""
            };

            _voiceStudioRepository.Add(voiceStudio);
            _voiceStudioRepository.SaveChanges();

            TempData["SuccessMessage"] = "Студію озвучення успішно додано";
        }
        else
        {
            TempData["ErrorMessage"] = "Помилка при додаванні студії озвучення";
        }

        return RedirectToAction("VoiceStudios");
    }

    [HttpPost]
    public IActionResult UpdateVoiceStudio(int Id, string Name, string Country, string Language)
    {
        if (ModelState.IsValid)
        {
            var existingVoiceStudio = _voiceStudioRepository.GetById(Id);
            if (existingVoiceStudio != null)
            {
                existingVoiceStudio.Name = Name;
                existingVoiceStudio.Country = Country;
                existingVoiceStudio.Language = Language;

                _voiceStudioRepository.Update(existingVoiceStudio);
                _voiceStudioRepository.SaveChanges();

                TempData["SuccessMessage"] = "Студію озвучення успішно оновлено";
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Помилка при оновленні студії озвучення";
        }

        return RedirectToAction("VoiceStudios");
    }

    [HttpPost]
    public IActionResult DeleteVoiceStudio(int id)
    {
        var voiceStudio = _voiceStudioRepository.GetById(id);
        if (voiceStudio != null)
        {
            _voiceStudioRepository.Remove(voiceStudio);
            _voiceStudioRepository.SaveChanges();
            TempData["SuccessMessage"] = "Студію озвучення успішно видалено";
        }
        else
        {
            TempData["ErrorMessage"] = "Помилка при видаленні студії озвучення";
        }
        return RedirectToAction("VoiceStudios");
    }

    // Керування відгуками
    public IActionResult Reviews()
    {
        var reviews = _reviewRepository.GetAll();
        return View(reviews);
    }

    [HttpPost]
    public IActionResult DeleteReview(int id)
    {
        var review = _reviewRepository.GetById(id);
        if (review != null)
        {
            _reviewRepository.Remove(review);
            _reviewRepository.SaveChanges();
            TempData["SuccessMessage"] = "Відгук успішно видалено";
        }
        else
        {
            TempData["ErrorMessage"] = "Помилка при видаленні відгуку";
        }
        return RedirectToAction("Reviews");
    }

    // Керування рейтингами
    public IActionResult Ratings()
    {
        var ratings = _ratingRepository.GetAll();
        return View(ratings);
    }

    [HttpPost]
    public IActionResult DeleteRating(int id)
    {
        var rating = _ratingRepository.GetById(id);
        if (rating != null)
        {
            _ratingRepository.Remove(rating);
            _ratingRepository.SaveChanges();
            TempData["SuccessMessage"] = "Рейтинг успішно видалено";
        }
        else
        {
            TempData["ErrorMessage"] = "Помилка при видаленні рейтингу";
        }
        return RedirectToAction("Ratings");
    }
}

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