using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.ViewModel
{
    public class CartViewModel
    {
        public IEnumerable<(ProductViewModel Product, int Quantity, decimal Sum)> Items { get; set; }
        public int ItemsCount => Items?.Sum(p => p.Quantity) ?? 0;
        public decimal TotalPrice => Items?.Sum(p => p.Product.Price * p.Quantity) ?? 0;
    }
}
