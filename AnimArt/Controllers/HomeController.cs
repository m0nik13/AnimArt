using Microsoft.AspNetCore.Mvc;
using AnimArt.Repositories;
using AnimArt.Entities;

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
            var animes = _animeRepository.GetAll();
            return View(animes);
        }

        public IActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return RedirectToAction("Index");
            }

            var results = _animeRepository.GetByTitle(query);
            return View("Index", results);
        }

        public IActionResult Details(int id)
        {
            var anime = _animeRepository.GetById(id);
            if (anime == null)
            {
                return NotFound();
            }
            return View(anime);
        }

        [HttpPost]
        public IActionResult AddAnime(Anime anime)
        {
            if (ModelState.IsValid)
            {
                _animeRepository.Add(anime);
                _animeRepository.SaveChanges();
                return RedirectToAction("Index");
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
                _animeRepository.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Демонстрація LINQ сортування
        public IActionResult SortedByRating()
        {
            var sortedAnime = _animeRepository.GetSortedByRating();
            return View("Index", sortedAnime);
        }

        public IActionResult SortedByReleaseDate()
        {
            var sortedAnime = _animeRepository.GetSortedByReleaseDate();
            return View("Index", sortedAnime);
        }
    }
}