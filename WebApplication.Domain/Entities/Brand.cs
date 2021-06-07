using System.Collections.Generic;
using WebApplication.Domain.Entities.Base;
using WebApplication.Domain.Entities.Base.Interfaces;

namespace WebApplication.Domain.Entities
{
    public class Brand : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
