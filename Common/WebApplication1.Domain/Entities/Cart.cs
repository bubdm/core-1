using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Domain.Entities
{
    public class Cart
    {
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
        public int ItemsCount => Items?.Sum(i => i.Quantity) + 5 ?? 0;
    }
}
