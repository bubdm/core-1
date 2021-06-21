using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Areas.Admin.Models
{
    public class ProductEditFilterViewModel
    {
        public string FilterName { get; set; }
        public ProductEditFilterViewModel(string name)
        {
            FilterName = name;
        }
    }
}
