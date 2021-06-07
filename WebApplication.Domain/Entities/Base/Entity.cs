using System.ComponentModel.DataAnnotations;
using WebApplication.Domain.Entities.Base.Interfaces;

namespace WebApplication.Domain.Entities.Base
{
    public abstract class Entity : IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
