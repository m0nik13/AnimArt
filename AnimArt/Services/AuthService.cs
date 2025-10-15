// Services/AuthService.cs
using AnimArt.Entities;
using AnimArt.Interfaces;

public class AuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
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
        _userRepository.SaveChanges();
    }
}