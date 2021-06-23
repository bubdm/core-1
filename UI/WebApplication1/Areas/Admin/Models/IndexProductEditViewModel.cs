using System.Collections.Generic;
using WebApplication1.Domain.ViewModel;

namespace WebApplication1.Areas.Admin.Models
{
    public class IndexProductEditViewModel
    {
        public ProductEditFilterViewModel FilterViewModel { get; set; }
        public ProductEditSortViewModel SortViewModel { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public IEnumerable<ProductEditViewModel> Products { get; set; }

    }
}
