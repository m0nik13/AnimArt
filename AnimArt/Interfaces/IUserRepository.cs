// Interfaces/IUserRepository.cs
using System.Collections.Generic;
using AnimArt.Entities;
using static AnimArt.Entities.User;

namespace AnimArt.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUsername(string username);
        User GetByEmail(string email);
        IEnumerable<User> GetByRole(UserRole role);
    }
}