using System.Security.Claims;
using AnimArt.Entities;
using AnimArt.Entities.ViewModels;
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
            if (_userRepository.GetByUsername(Username) != null)
            {
                TempData["ErrorMessage"] = "Користувач з таким іменем вже існує";
                return RedirectToAction("Users");
            }

            var user = new User
            {
                Id = _userRepository.GetAll().Any() ? _userRepository.GetAll().Max(u => u.Id) + 1 : 1,
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
        Console.WriteLine($"UpdateUser called: Id={Id}, Username={Username}, Role={Role}");

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
            else
            {
                TempData["ErrorMessage"] = "Користувача не знайдено";
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
        Console.WriteLine($"DeleteUser called: id={id}");

        var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        if (id == currentUserId)
        {
            TempData["ErrorMessage"] = "Ви не можете видалити власний акаунт";
            return RedirectToAction("Users");
        }

        var user = _userRepository.GetById(id);
        if (user != null)
        {
            _userRepository.Remove(user);
            _userRepository.SaveChanges();
            TempData["SuccessMessage"] = "Користувача успішно видалено";
        }
        else
        {
            TempData["ErrorMessage"] = "Користувача не знайдено";
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
                               DateTime ReleaseDate, int? DurationPerEpisode, string AgeRating,
                               string PosterUrl, List<int> GenreIds, List<int> StudioIds, List<int> VoiceStudioIds)
    {
        if (ModelState.IsValid && !string.IsNullOrEmpty(Title))
        {
            var anime = new Anime
            {
                Id = _animeRepository.GetAll().Any() ? _animeRepository.GetAll().Max(a => a.Id) + 1 : 1,
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
                PosterUrl = PosterUrl ?? "", // Додано постер
                GenreIds = GenreIds ?? new List<int>(),
                StudioIds = StudioIds ?? new List<int>(),
                VoiceStudioIds = VoiceStudioIds ?? new List<int>()
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
                                   DateTime ReleaseDate, int? DurationPerEpisode, string AgeRating,
                                   string PosterUrl, List<int> GenreIds, List<int> StudioIds, List<int> VoiceStudioIds)
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
                existingAnime.DurationPerEpisode = DurationPerEpisode ?? existingAnime.DurationPerEpisode;
                existingAnime.AgeRating = AgeRating ?? existingAnime.AgeRating;
                existingAnime.PosterUrl = PosterUrl ?? existingAnime.PosterUrl; // Додано оновлення постера
                existingAnime.GenreIds = GenreIds ?? existingAnime.GenreIds;
                existingAnime.StudioIds = StudioIds ?? existingAnime.StudioIds;
                existingAnime.VoiceStudioIds = VoiceStudioIds ?? existingAnime.VoiceStudioIds;

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
        Console.WriteLine($"DeleteAnime called: id={id}");

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

    // Керування жанрами
    public IActionResult Genres()
    {
        var genres = _genreRepository.GetAll();
        return View(genres);
    }

    [HttpPost]
    public IActionResult CreateGenre(string Name)
    {
        if (ModelState.IsValid && !string.IsNullOrEmpty(Name))
        {
            var genre = new Genre
            {
                Id = _genreRepository.GetAll().Any() ? _genreRepository.GetAll().Max(g => g.Id) + 1 : 1,
                Name = Name
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
    public IActionResult UpdateGenre(int Id, string Name)
    {
        Console.WriteLine($"UpdateGenre called: Id={Id}, Name={Name}");

        if (ModelState.IsValid)
        {
            var existingGenre = _genreRepository.GetById(Id);
            if (existingGenre != null)
            {
                existingGenre.Name = Name;

                _genreRepository.Update(existingGenre);
                _genreRepository.SaveChanges();

                TempData["SuccessMessage"] = "Жанр успішно оновлено";
            }
            else
            {
                TempData["ErrorMessage"] = "Жанр не знайдено";
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
        Console.WriteLine($"DeleteGenre called: id={id}");

        var genre = _genreRepository.GetById(id);
        if (genre != null)
        {
            _genreRepository.Remove(genre);
            _genreRepository.SaveChanges();
            TempData["SuccessMessage"] = "Жанр успішно видалено";
        }
        else
        {
            TempData["ErrorMessage"] = "Жанр не знайдено";
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
    public IActionResult CreateStudio(string Name, string JapaneseName)
    {
        if (ModelState.IsValid && !string.IsNullOrEmpty(Name))
        {
            var studio = new Studio
            {
                Id = _studioRepository.GetAll().Any() ? _studioRepository.GetAll().Max(s => s.Id) + 1 : 1,
                Name = Name,
                JapaneseName = JapaneseName ?? ""
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
    public IActionResult UpdateStudio(int Id, string Name, string JapaneseName)
    {
        Console.WriteLine($"UpdateStudio called: Id={Id}, Name={Name}");

        if (ModelState.IsValid)
        {
            var existingStudio = _studioRepository.GetById(Id);
            if (existingStudio != null)
            {
                existingStudio.Name = Name;
                existingStudio.JapaneseName = JapaneseName;

                _studioRepository.Update(existingStudio);
                _studioRepository.SaveChanges();

                TempData["SuccessMessage"] = "Студію успішно оновлено";
            }
            else
            {
                TempData["ErrorMessage"] = "Студію не знайдено";
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
        Console.WriteLine($"DeleteStudio called: id={id}");

        var studio = _studioRepository.GetById(id);
        if (studio != null)
        {
            _studioRepository.Remove(studio);
            _studioRepository.SaveChanges();
            TempData["SuccessMessage"] = "Студію успішно видалено";
        }
        else
        {
            TempData["ErrorMessage"] = "Студію не знайдено";
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
                Id = _voiceStudioRepository.GetAll().Any() ? _voiceStudioRepository.GetAll().Max(v => v.Id) + 1 : 1,
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
        Console.WriteLine($"UpdateVoiceStudio called: Id={Id}, Name={Name}");

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
            else
            {
                TempData["ErrorMessage"] = "Студію озвучення не знайдено";
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
        Console.WriteLine($"DeleteVoiceStudio called: id={id}");

        var voiceStudio = _voiceStudioRepository.GetById(id);
        if (voiceStudio != null)
        {
            _voiceStudioRepository.Remove(voiceStudio);
            _voiceStudioRepository.SaveChanges();
            TempData["SuccessMessage"] = "Студію озвучення успішно видалено";
        }
        else
        {
            TempData["ErrorMessage"] = "Студію озвучення не знайдено";
        }
        return RedirectToAction("VoiceStudios");
    }

}