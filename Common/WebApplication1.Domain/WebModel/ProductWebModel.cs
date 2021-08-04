using System.Collections.Generic;
using WebApplication1.Domain.DTO;

namespace WebApplication1.Domain.WebModel
{
    public class ProductWebModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string Section { get; set; }
        public string Brand { get; set; }
        public ICollection<string> Keywords { get; set; }
    }
}
