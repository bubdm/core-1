using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Domain.Entities.Base;
using WebApplication1.Domain.Entities.Base.Interfaces;

namespace WebApplication1.Domain.Entities
{
    public class Section : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public Section Parent { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
