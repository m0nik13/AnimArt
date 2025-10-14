// Controllers/AccountController.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AnimArt.Entities;
using AnimArt.Repositories;

public class AccountController : Controller
{
    private readonly AuthService _authService;
    private readonly UserRepository _userRepository;

    public AccountController(UserRepository userRepository, AuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login() => View();

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

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("UserId", user.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, "CookieAuth");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("CookieAuth", principal);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register() => View();

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (_userRepository.GetByUsername(model.Username) != null)
        {
            ModelState.AddModelError("Username", "Користувач з таким іменем вже існує");
            return View(model);
        }

        var user = new User
        {
            Id = _userRepository.GetAll().Any() ? _userRepository.GetAll().Max(u => u.Id) + 1 : 1,
            Username = model.Username,
            Email = model.Email,
            Role = UserRole.User
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
        
        if (user == null)
            return RedirectToAction("Login");

        var model = new ProfileViewModel
        {
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.ToString(),
            RegistrationDate = user.RegistrationDate,
            LastLogin = user.LastLogin,
            WatchedAnime = user.UserAnimeLists?.Count(ul => ul.Status == AnimeListStatus.Completed) ?? 0,
            FavoritesCount = user.UserAnimeLists?.Count(ul => ul.IsFavorite) ?? 0,
            ReviewsCount = user.Reviews?.Count ?? 0
        };

        return View(model);
    }

    [AllowAnonymous]
    public IActionResult AccessDenied() => View();
}
