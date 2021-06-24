using System.Collections.Generic;

namespace WebApplication1.Domain.WebModel
{
    public class CatalogWebModel
    {
        public int? SectionId { get; set; }
        public int? BrandId { get; set; }
        public IEnumerable<ProductWebModel> Products { get; set; }
    }
}