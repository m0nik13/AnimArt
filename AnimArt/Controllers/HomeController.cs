// Controllers/HomeController.cs
using System.Security.Claims;
using AnimArt.Entities;
using AnimArt.Interfaces;
using AnimArt.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimArt.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnimeRepository _animeRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IRepository<Studio> _studioRepository;
        private readonly IRepository<VoiceStudio> _voiceStudioRepository;
        private readonly IRepository<Review> _reviewRepository;
        private readonly IRepository<Rating> _ratingRepository;

        public HomeController(
            IAnimeRepository animeRepository,
            IRepository<Genre> genreRepository,
            IRepository<Studio> studioRepository,
            IRepository<VoiceStudio> voiceStudioRepository,
            IRepository<Review> reviewRepository,
            IRepository<Rating> ratingRepository)
        {
            _animeRepository = animeRepository;
            _genreRepository = genreRepository;
            _studioRepository = studioRepository;
            _voiceStudioRepository = voiceStudioRepository;
            _reviewRepository = reviewRepository;
            _ratingRepository = ratingRepository;
        }

        public IActionResult Index()
        {
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

            // Отримуємо пов'язані дані
            var genres = _genreRepository.GetAll().Where(g => anime.GenreIds.Contains(g.Id));
            var studios = _studioRepository.GetAll().Where(s => anime.StudioIds.Contains(s.Id));
            var voiceStudios = _voiceStudioRepository.GetAll().Where(v => anime.VoiceStudioIds.Contains(v.Id));
            var reviews = _reviewRepository.GetAll().Where(r => r.AnimeId == id);
            var ratings = _ratingRepository.GetAll().Where(r => r.AnimeId == id);

            var viewModel = new AnimeDetailsViewModel
            {
                Anime = anime,
                Genres = genres,
                Studios = studios,
                VoiceStudios = voiceStudios,
                Reviews = reviews,
                Ratings = ratings,
                AverageRating = ratings.Any() ? ratings.Average(r => r.Score) : 0,
                TotalRatings = ratings.Count()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddReview(int animeId, string title, string content, bool containsSpoilers)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
            {
                TempData["ErrorMessage"] = "Заголовок та вміст відгуку обов'язкові";
                return RedirectToAction("AnimeDetails", new { id = animeId });
            }

            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            var review = new Review
            {
                Id = _reviewRepository.GetAll().Any() ? _reviewRepository.GetAll().Max(r => r.Id) + 1 : 1,
                UserId = userId,
                AnimeId = animeId,
                Title = title,
                Content = content,
                ContainsSpoilers = containsSpoilers,
                Likes = 0,
                Dislikes = 0,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _reviewRepository.Add(review);
            _reviewRepository.SaveChanges();

            TempData["SuccessMessage"] = "Відгук успішно додано";
            return RedirectToAction("AnimeDetails", new { id = animeId });
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddRating(int animeId, int score)
        {
            if (score < 1 || score > 10)
            {
                TempData["ErrorMessage"] = "Рейтинг повинен бути від 1 до 10";
                return RedirectToAction("AnimeDetails", new { id = animeId });
            }

            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            // Перевіряємо, чи користувач вже ставив рейтинг
            var existingRating = _ratingRepository.GetAll()
                .FirstOrDefault(r => r.UserId == userId && r.AnimeId == animeId);

            if (existingRating != null)
            {
                existingRating.Score = score;
                existingRating.RatedAt = DateTime.Now;
                _ratingRepository.Update(existingRating);
            }
            else
            {
                var rating = new Rating
                {
                    Id = _ratingRepository.GetAll().Any() ? _ratingRepository.GetAll().Max(r => r.Id) + 1 : 1,
                    UserId = userId,
                    AnimeId = animeId,
                    Score = score,
                    RatedAt = DateTime.Now
                };

                _ratingRepository.Add(rating);
            }

            _ratingRepository.SaveChanges();

            TempData["SuccessMessage"] = "Рейтинг успішно додано";
            return RedirectToAction("AnimeDetails", new { id = animeId });
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddToFavorites(int animeId)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            // Тут буде логіка додавання до обраного
            // Наразі просто повертаємо повідомлення
            TempData["SuccessMessage"] = "Аніме додано до обраного";
            return RedirectToAction("AnimeDetails", new { id = animeId });
        }
    }
}