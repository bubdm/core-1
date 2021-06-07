using WebApplication.Domain.Entities.Base;
using WebApplication.Domain.Entities.Base.Interfaces;

namespace WebApplication.Domain.Entities
{
    public class Brand : Entity, IOrderedEntity
    {
        public int Order { get; set; }
    }
}
