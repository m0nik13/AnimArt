// Models/UserRepository.cs
using AnimArt.Data;
using AnimArt.Interfaces;
using AnimArt.Repositories;

namespace AnimArt.Entities
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(IDataStorage<User> storage) : base(storage) { }

        public User GetByUsername(string username)
        {
            return GetAll().FirstOrDefault(u => u.Username == username);
        }
    }
}