using AnimArt.Repositories;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace AnimArt.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLogin { get; set; }
        public UserRole Role { get; set; }

        // Навігаційні властивості
        public virtual ICollection<UserLists> UserAnimeLists { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }

    public enum UserRole
    {
        User,
        Moderator,
        Admin
    }
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
    }
}
