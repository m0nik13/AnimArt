// Repositories/Repository.cs
using AnimArt.Data;
using AnimArt.Interfaces;
using System.Linq;

namespace AnimArt.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected List<T> _entities;
        private readonly IDataStorage<T> _storage;

        public Repository(IDataStorage<T> storage)
        {
            _storage = storage;
            _entities = _storage.Load();
        }

        public void Add(T entity)
        {
            if (!_entities.Any(e => e.Id == entity.Id))
            {
                _entities.Add(entity);
                _storage.Save(_entities);
            }
        }

        public void Remove(T entity)
        {
            _entities.Remove(entity);
            _storage.Save(_entities);
        }

        public void Update(T entity)
        {
            var existing = _entities.FirstOrDefault(e => e.Id == entity.Id);
            if (existing != null)
            {
                _entities.Remove(existing);
                _entities.Add(entity);
                _storage.Save(_entities);
            }
        }

        public T GetById(int id) => _entities.FirstOrDefault(e => e.Id == id);
        public IEnumerable<T> GetAll() => _entities;
        public IEnumerable<T> GetSorted() => _entities.OrderBy(e => e.Id);
        public void SaveChanges()
        {
            _storage.Save(_entities);
        }
    }
}