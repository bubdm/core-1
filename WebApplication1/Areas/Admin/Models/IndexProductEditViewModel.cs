using System.Collections.Generic;
using WebApplication1.ViewModel;

namespace WebApplication1.Areas.Admin.Models
{
    public class IndexProductEditViewModel
    {
        public ProductEditSortViewModel SortViewModel { get; set; }
        public IEnumerable<ProductEditViewModel> Products { get; set; }
        public string FilterName { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
