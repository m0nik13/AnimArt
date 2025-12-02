// Entities/BaseEntity.cs
using System.ComponentModel.DataAnnotations;
using AnimArt.Interfaces;

namespace AnimArt.Entities
{
    public abstract class BaseEntity : IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}