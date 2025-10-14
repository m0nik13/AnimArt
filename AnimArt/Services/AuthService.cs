// Services/AuthService.cs
using AnimArt.Entities;

public class AuthService
{
    private readonly UserRepository _userRepository;

    public AuthService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User Authenticate(string username, string password)
    {
        var user = _userRepository.GetByUsername(username);
        return user != null && user.VerifyPassword(password) ? user : null;
    }

    public void Register(User user)
    {
        _userRepository.Add(user);
    }
}