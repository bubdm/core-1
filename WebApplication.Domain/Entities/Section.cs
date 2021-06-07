using WebApplication.Domain.Entities.Base;
using WebApplication.Domain.Entities.Base.Interfaces;

namespace WebApplication.Domain.Entities
{
    public class Section : Entity, IOrderedEntity
    {
        public int Order { get; set; }
        public int? ParentId { get; set; }
    }
}
