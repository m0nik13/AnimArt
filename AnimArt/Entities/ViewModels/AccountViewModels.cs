// Models/ViewModels/AccountViewModels.cs
using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Required(ErrorMessage = "Ім'я користувача обов'язкове")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Пароль обов'язковий")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

public class RegisterViewModel
{
    [Required(ErrorMessage = "Ім'я користувача обов'язкове")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Ім'я повинно містити від 3 до 20 символів")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email обов'язковий")]
    [EmailAddress(ErrorMessage = "Невірний формат email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Пароль обов'язковий")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль повинен містити мінімум 6 символів")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Підтвердження пароля обов'язкове")]
    [Compare("Password", ErrorMessage = "Паролі не співпадають")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}

public class ProfileViewModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime LastLogin { get; set; }
    public int WatchedAnime { get; set; }
    public int FavoritesCount { get; set; }
    public int ReviewsCount { get; set; }
}