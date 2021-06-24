using System.Collections.Generic;

namespace WebApplication1.Domain.WebModel.Admin
{
    public class IndexProductEditViewModel
    {
        public ProductEditFilterViewModel FilterViewModel { get; set; }
        public ProductEditSortViewModel SortViewModel { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public IEnumerable<EditProductWebModel> Products { get; set; }

    }
}
