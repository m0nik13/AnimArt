// Repositories/UserRepository.cs
using AnimArt.Data;
using AnimArt.Entities;
using AnimArt.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnimArt.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public User GetByUsername(string username)
        {
            return _context.Users
                .FirstOrDefault(u => u.Username == username);
        }

        public IEnumerable<User> GetByRole(User.UserRole role)
        {
            return _context.Users
                .Where(u => u.Role == role)
                .ToList();
        }

        // Якщо потрібно завантажити профіль з історією переглядів
        public User GetProfileData(string username)
        {
            return _context.Users
               .Include(u => u.UserAnimeLists).ThenInclude(ual => ual.Anime)
               .Include(u => u.Reviews)
               .FirstOrDefault(u => u.Username == username);
        }
    }
}