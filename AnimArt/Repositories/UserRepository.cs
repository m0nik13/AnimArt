using AnimArt.Entities;

namespace AnimArt.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public User GetByUsername(string username)
        {
            return _entities.FirstOrDefault(u => u.Username == username);
        }

        public User GetByEmail(string email)
        {
            return _entities.FirstOrDefault(u => u.Email == email);
        }

        public IEnumerable<User> GetByRole(UserRole role)
        {
            return _entities.Where(u => u.Role == role).ToList();
        }
    }
    public interface IUserRepository : IRepository<User>
    {
        User GetByUsername(string username);
        User GetByEmail(string email);
        IEnumerable<User> GetByRole(UserRole role);
    }
}
