// Entities/User.cs
using AnimArt.Interfaces;
using BCrypt.Net;

namespace AnimArt.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        private string _passwordHash;
        public string PasswordHash
        {
            get => _passwordHash;
            set => _passwordHash = value;
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
            PasswordHash = string.Empty;
            AvatarUrl = string.Empty;
            Role = UserRole.User;
        }

        public void SetPassword(string password)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }
        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }
        public enum UserRole
        {
            User,
            Admin
        }
    }

}