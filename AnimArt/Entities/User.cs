// Entities/User.cs
using System.ComponentModel.DataAnnotations;
using AnimArt.Interfaces;
using BCrypt.Net;

namespace AnimArt.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; } // Вже хешований

        public string AvatarUrl { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public DateTime LastLogin { get; set; }
        public UserRole Role { get; set; }

        public virtual ICollection<UserLists> UserAnimeLists { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

        public void SetPassword(string password) => PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        public bool VerifyPassword(string password) => BCrypt.Net.BCrypt.Verify(password, PasswordHash);

        public enum UserRole { User, Admin }
    }
}