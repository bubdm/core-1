using System.ComponentModel.DataAnnotations;
using WebApplication1.Domain.Entities.Base.Interfaces;

namespace WebApplication1.Domain.Entities.Base
{
    public abstract class Entity : IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
