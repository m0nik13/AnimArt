// Entities/User.cs
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;

namespace AnimArt.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        private string _password;
        public string PasswordHash
        {
            get => _password;
            set => _password = BCrypt.Net.BCrypt.EnhancedHashPassword(value, 13);
        }

        public string AvatarUrl { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public DateTime LastLogin { get; set; }
        public UserRole Role { get; set; }

        // Навігаційні властивості
        public virtual ICollection<UserLists> UserAnimeLists { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public User()
        {
            Username = string.Empty;
            Email = string.Empty;
            PasswordHash = string.Empty;
            AvatarUrl = string.Empty;
            Role = UserRole.User;
        }
        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }
        public enum UserRole
        {
            User,
            Moderator,
            Admin
        }
    }

    public interface IEntity
    {
        int Id { get; }
    }
}