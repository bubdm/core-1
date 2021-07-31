using System.Collections.Generic;
using WebApplication1.Domain.WebModel.Admin;

namespace WebApplication1.Domain.WebModel
{
    public class CatalogWebModel
    {
        public int? SectionId { get; set; }
        public int? BrandId { get; set; }
        public PageWebModel PageWebModel { get; set; }
        public IEnumerable<ProductWebModel> Products { get; set; }
    }
}