using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Domain.WebModel
{
    public class CartWebModel
    {
        public IEnumerable<(ProductWebModel Product, int Quantity)> Items { get; set; }
        public int ItemsCount => Items?.Sum(p => p.Quantity) ?? 0;
        public decimal TotalPrice => Items?.Sum(p => p.Product.Price * p.Quantity) ?? 0;
    }
}
