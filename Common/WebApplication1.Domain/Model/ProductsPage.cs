using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.Model
{
    /// <summary> Для пагинации </summary>
    public class ProductsPage
    {
        public IEnumerable<Product> Products { get; set; }
        public int TotalCount { get; set; }

        public void Deconstruct(out IEnumerable<Product> productsPage, out int totalCount)
        {
            productsPage = Products;
            totalCount = TotalCount;
        }
    }
}
