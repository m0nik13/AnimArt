// Controllers/HomeController.cs
using System.Security.Claims;
using AnimArt.Interfaces;
using AnimArt.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimArt.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnimeRepository _animeRepository;

        public HomeController(IAnimeRepository animeRepository)
        {
            _animeRepository = animeRepository;
        }

        public IActionResult Index()
        {
            // Перевіряємо, чи користувач адмін
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            var recentAnime = _animeRepository.GetSortedByReleaseDate();
            return View(recentAnime);
        }
    }
}