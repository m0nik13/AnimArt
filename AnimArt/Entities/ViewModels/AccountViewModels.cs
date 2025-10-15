
using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Required(ErrorMessage = "Ім'я користувача обов'язкове")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обов'язковий")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public class RegisterViewModel
{
    [Required(ErrorMessage = "Ім'я користувача обов'язкове")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Ім'я повинно містити від 3 до 20 символів")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обов'язковий")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль повинен містити мінімум 6 символів")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Підтвердження пароля обов'язкове")]
    [Compare("Password", ErrorMessage = "Паролі не співпадають")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Оберіть роль")]
    public string Role { get; set; } = "User";
}
// Models/ViewModels/ProfileViewModel.cs
public class ProfileViewModel
{
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; }
    public DateTime LastLogin { get; set; }
    public int WatchedAnime { get; set; }
    public int FavoritesCount { get; set; }
    public int ReviewsCount { get; set; }
}