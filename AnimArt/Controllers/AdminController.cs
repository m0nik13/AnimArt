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
    private readonly IRepository<Review> _reviewRepository;

    public AdminController(
        IUserRepository userRepository,
        IAnimeRepository animeRepository,
        IRepository<Review> reviewRepository)
    {
        _userRepository = userRepository;
        _animeRepository = animeRepository;
        _reviewRepository = reviewRepository;
    }

    public IActionResult Index()
    {
        var stats = new AdminStatsViewModel
        {
            TotalUsers = _userRepository.GetAll().Count(),
            TotalAnime = _animeRepository.GetAll().Count(),
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
    public IActionResult DeleteUser(int id)
    {
        var user = _userRepository.GetById(id);
        if (user != null && user.Role != UserRole.Admin) // Не дозволяємо видаляти адмінів
        {
            _userRepository.Remove(user);
        }
        return RedirectToAction("Users");
    }

    [HttpPost]
    public IActionResult ChangeUserRole(int id, UserRole newRole)
    {
        var user = _userRepository.GetById(id);
        if (user != null)
        {
            user.Role = newRole;
            _userRepository.Update(user);
        }
        return RedirectToAction("Users");
    }

    // Керування аніме
    public IActionResult Anime()
    {
        var anime = _animeRepository.GetAll();
        return View(anime);
    }

    public IActionResult CreateAnime()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateAnime(Anime anime)
    {
        if (ModelState.IsValid)
        {
            anime.Id = _animeRepository.GetAll().Any() ?
                _animeRepository.GetAll().Max(a => a.Id) + 1 : 1;

            _animeRepository.Add(anime);
            return RedirectToAction("Anime");
        }
        return View(anime);
    }

    public IActionResult EditAnime(int id)
    {
        var anime = _animeRepository.GetById(id);
        if (anime == null) return NotFound();
        return View(anime);
    }

    [HttpPost]
    public IActionResult EditAnime(Anime anime)
    {
        if (ModelState.IsValid)
        {
            _animeRepository.Update(anime);
            return RedirectToAction("Anime");
        }
        return View(anime);
    }

    [HttpPost]
    public IActionResult DeleteAnime(int id)
    {
        var anime = _animeRepository.GetById(id);
        if (anime != null)
        {
            _animeRepository.Remove(anime);
        }
        return RedirectToAction("Anime");
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
        }
        return RedirectToAction("Reviews");
    }
}

public class AdminStatsViewModel
{
    public int TotalUsers { get; set; }
    public int TotalAnime { get; set; }
    public int TotalReviews { get; set; }
    public IEnumerable<User> RecentUsers { get; set; }
}