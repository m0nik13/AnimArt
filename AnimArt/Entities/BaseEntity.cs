using AnimArt.Interfaces;
namespace AnimArt.Entities
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }
        private static int _latestId = 0;
        public BaseEntity() { Id = _latestId++; }
    }
}
