using System.Collections.Generic;
using WebApplication1.Domain.Entities.Base;
using WebApplication1.Domain.Entities.Base.Interfaces;

namespace WebApplication1.Domain.Entities
{
    public class Brand : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
