// Controllers/HomeController.cs
using System.Security.Claims;
using AnimArt.Entities;
using AnimArt.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimArt.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnimeRepository _animeRepository;
        private readonly IRepository<Review> _reviewRepository;

        // Зверніть увагу: ми прибрали зайві репозиторії, бо AnimeRepository тепер розумний
        // і сам підтягує жанри та студії.
        public HomeController(
            IAnimeRepository animeRepository,
            IRepository<Review> reviewRepository)
        {
            _animeRepository = animeRepository;
            _reviewRepository = reviewRepository;
        }

        public IActionResult Index()
        {
            // Якщо адмін - на адмінку, інакше - список аніме
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            var recentAnime = _animeRepository.GetSortedByReleaseDate();
            return View(recentAnime);
        }

        public IActionResult AnimeDetails(int id)
        {
            var anime = _animeRepository.GetById(id);
            if (anime == null)
            {
                return NotFound();
            }

            // Формуємо модель для відображення
            // Тепер ми беремо жанри та студії прямо з об'єкта Anime через LINQ Select
            var viewModel = new AnimeDetailsViewModel
            {
                Anime = anime,
                Genres = anime.AnimeGenres.Select(ag => ag.Genre).ToList(),
                Studios = anime.AnimeStudios.Select(ast => ast.Studio).ToList(),
                VoiceStudios = anime.AnimeVoiceStudios.Select(av => av.VoiceStudio).ToList(),
                Reviews = anime.Reviews.OrderByDescending(r => r.CreatedAt).ToList(),

                // AverageRating тепер рахується автоматично в моделі Anime, але можна передати явно
                AverageRating = anime.AverageRating,
                TotalRatings = anime.Reviews.Count
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddReview(int animeId, string title, string content, bool containsSpoilers, int rating)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
            {
                TempData["ErrorMessage"] = "Заголовок та текст обов'язкові";
                return RedirectToAction("AnimeDetails", new { id = animeId });
            }

            var userIdString = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdString);

            var review = new Review
            {
                UserId = userId,
                AnimeId = animeId,
                Title = title,
                Content = content,
                ContainsSpoilers = containsSpoilers,
                Rating = rating, // Зберігаємо оцінку (1-5) разом з відгуком
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Likes = 0,
                Dislikes = 0
            };

            _reviewRepository.Add(review);
            _reviewRepository.SaveChanges();

            TempData["SuccessMessage"] = "Відгук додано!";
            return RedirectToAction("AnimeDetails", new { id = animeId });
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddToFavorites(int animeId)
        {
            // Тут буде логіка для UserLists пізніше
            TempData["SuccessMessage"] = "Аніме додано до обраного (функціонал в розробці)";
            return RedirectToAction("AnimeDetails", new { id = animeId });
        }
    }
}