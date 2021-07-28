using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplication1.Domain.DTO
{
    public class ProductPageDTO
    {
        public IEnumerable<ProductDTO> Products { get; set; }
        public int TotalCount { get; set; }
    }
}
