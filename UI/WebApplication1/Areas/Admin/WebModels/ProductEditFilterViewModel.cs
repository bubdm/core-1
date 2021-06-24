namespace WebApplication1.Areas.Admin.WebModels
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
