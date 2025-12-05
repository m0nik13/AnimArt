// Controllers/AccountController.cs
using System.Security.Claims;
using AnimArt.Entities;
using AnimArt.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AnimArt.Entities.User;

namespace AnimArt.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository, AuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _authService.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                ViewData["ErrorMessage"] = "Невірний логін або пароль";
                return View(model);
            }

            user.LastLogin = DateTime.Now;
            _userRepository.Update(user);
            _userRepository.SaveChanges();

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.Role.ToString()),
                new("UserId", user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);

            if (user.Role == UserRole.Admin)
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_userRepository.GetByUsername(model.Username) != null)
            {
                ModelState.AddModelError("Username", "Користувач вже існує");
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                Role = model.Role == "Admin" ? UserRole.Admin : UserRole.User,
                RegistrationDate = DateTime.Now,
                LastLogin = DateTime.Now
            };

            user.SetPassword(model.Password);
            _authService.Register(user);

            return RedirectToAction("Login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Profile()
        {
            var username = User.Identity.Name;
            var user = _userRepository.GetByUsername(username);

            if (user == null) return RedirectToAction("Login");

            var model = new ProfileViewModel
            {
                Username = user.Username,
                Role = user.Role.ToString(),
                RegistrationDate = user.RegistrationDate,
                LastLogin = user.LastLogin,
                WatchedAnime = 0, // Поки заглушка
                FavoritesCount = 0,
                ReviewsCount = 0
            };

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() => View();
    }
}