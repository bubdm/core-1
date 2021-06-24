namespace WebApplication1.Areas.Admin.WebModels
{
    public class ProductEditSortViewModel
    {
        public ProductEditSortState NameSort { get; set; } = ProductEditSortState.NameAsc;
        public ProductEditSortState OrderSort { get; set; } = ProductEditSortState.OrderAsc;
        public ProductEditSortState PriceSort { get; set; } = ProductEditSortState.PriceAsc;
        public ProductEditSortState SectionSort { get; set; } = ProductEditSortState.SectionAsc;
        public ProductEditSortState BrandSort { get; set; } = ProductEditSortState.BrandAsc;
        public ProductEditSortState Current { get; set; }
        public bool Up { get; set; } = true;

        public ProductEditSortViewModel(ProductEditSortState sortOrder)
        {
            if (sortOrder == ProductEditSortState.NameDesc || sortOrder == ProductEditSortState.OrderDesc ||
                sortOrder == ProductEditSortState.PriceDesc || sortOrder == ProductEditSortState.SectionDesc ||
                sortOrder == ProductEditSortState.BrandDesc)
            {
                Up = false;
            }

            switch (sortOrder)
            {
                case ProductEditSortState.NameDesc:
                    Current = NameSort = ProductEditSortState.NameAsc;
                    break;
                case ProductEditSortState.OrderAsc:
                    Current = OrderSort = ProductEditSortState.OrderDesc;
                    break;
                case ProductEditSortState.OrderDesc:
                    Current = OrderSort = ProductEditSortState.OrderAsc;
                    break;
                case ProductEditSortState.PriceAsc:
                    Current = PriceSort = ProductEditSortState.PriceDesc;
                    break;
                case ProductEditSortState.PriceDesc:
                    Current = PriceSort = ProductEditSortState.PriceAsc;
                    break;
                case ProductEditSortState.SectionAsc:
                    Current = SectionSort = ProductEditSortState.SectionDesc;
                    break;
                case ProductEditSortState.SectionDesc:
                    Current = SectionSort = ProductEditSortState.SectionAsc;
                    break;
                case ProductEditSortState.BrandAsc:
                    Current = BrandSort = ProductEditSortState.BrandDesc;
                    break;
                case ProductEditSortState.BrandDesc:
                    Current = BrandSort = ProductEditSortState.BrandAsc;
                    break;
                default:
                    Current = NameSort = ProductEditSortState.NameDesc;
                    break;
            }
        }
    }
}
