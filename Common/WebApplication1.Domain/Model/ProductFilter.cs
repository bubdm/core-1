namespace WebApplication1.Domain.Model
{
    public class ProductFilter
    {
        public int? SectionId { get; set; }
        public int? BrandId { get; set; }
        public int Page { get; set; }
        public int? PageSize { get; set; }
        public int[] Ids { get; set; }
    }
}
