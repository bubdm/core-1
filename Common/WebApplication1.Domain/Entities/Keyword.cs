using System.Collections.Generic;
using WebApplication1.Domain.Entities.Base;

namespace WebApplication1.Domain.Entities
{
    public class Keyword : Entity
    {
        public string Word { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
