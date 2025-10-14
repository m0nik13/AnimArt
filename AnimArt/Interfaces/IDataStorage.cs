// Interfaces/IDataStorage.cs
using System.Collections.Generic;

namespace AnimArt.Interfaces
{
    public interface IDataStorage<T> where T : IEntity
    {
        void Save(List<T> items);
        List<T> Load();
    }
}