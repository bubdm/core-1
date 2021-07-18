namespace WebApplication1.Domain.WebModel.Admin
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
