using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.WebModel
{
    /// <summary> Веб модель для отображения хлебных крошек </summary>
    public class BreadCrumbsWebModel
    {
        public Section Section { get; set; }
        public Brand Brand { get; set; }
        public string Product { get; set; }
    }
}
