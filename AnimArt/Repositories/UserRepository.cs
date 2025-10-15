// Repositories/UserRepository.cs
using System.Linq;
using AnimArt.Data;
using AnimArt.Entities;
using AnimArt.Interfaces;
using static AnimArt.Entities.User;

namespace AnimArt.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IDataStorage<User> storage) : base(storage) { }

        public User GetByUsername(string username)
        {
            return GetAll().FirstOrDefault(u => u.Username == username);
        }

        public User GetByEmail(string email)
        {
            return GetAll().FirstOrDefault(u => u.Email == email);
        }

        public IEnumerable<User> GetByRole(UserRole role)
        {
            return GetAll().Where(u => u.Role == role);
        }
    }
}