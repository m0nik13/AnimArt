using AnimArt.Repositories;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace AnimArt.Entities
{
    public class User : IEntity
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
    
}
