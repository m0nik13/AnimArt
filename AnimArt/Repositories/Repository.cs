using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AnimArt.Entities;
namespace AnimArt.Repositories
{
    public class Repository<T> : IRepository<T> where T : IEntity
    {
        protected List<T> _entities = new List<T>();
        private int _nextId = 1;

        public T GetById(int id)
        {
            return _entities.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.AsReadOnly();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _entities.AsQueryable().Where(predicate);
        }

        public void Add(T entity)
        {
            entity.Id = _nextId++;
            _entities.Add(entity);
        }

        public void Update(T entity)
        {
            var existingEntity = _entities.FirstOrDefault(e => e.Id == entity.Id);
            if (existingEntity != null)
            {
                var index = _entities.IndexOf(existingEntity);
                _entities[index] = entity;
            }
        }

        public void Delete(T entity)
        {
            _entities.RemoveAll(e => e.Id == entity.Id);
        }

        public void SaveChanges()
        {
            // Для in-memory репозиторію нічого не робимо
            // У реальному додатку тут буде збереження в БД
        }
    }
}