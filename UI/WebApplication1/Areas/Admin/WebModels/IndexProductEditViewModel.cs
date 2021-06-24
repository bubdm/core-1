using System.Collections.Generic;
using WebApplication1.Domain.WebModel;

namespace WebApplication1.Areas.Admin.WebModels
{
    public class IndexProductEditViewModel
    {
        public ProductEditFilterViewModel FilterViewModel { get; set; }
        public ProductEditSortViewModel SortViewModel { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public IEnumerable<EditProductWebModel> Products { get; set; }

    }
}
