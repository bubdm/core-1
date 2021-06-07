using System.ComponentModel.DataAnnotations;
using WebApplication.Domain.Entities.Base.Interfaces;

namespace WebApplication.Domain.Entities.Base
{
    public abstract class NamedEntity : Entity, INamedEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
