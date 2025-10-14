// Interfaces/IRepository.cs
using System.Collections.Generic;

namespace AnimArt.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetSorted();
    }
}