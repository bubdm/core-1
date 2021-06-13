using System.Collections.Generic;
using WebApplication.Domain;
using WebApplication.Domain.Entities;

namespace WebApplication1.Services.Interfaces
{
    public interface IProductData
    {
        /// <summary> все категории </summary>
        IEnumerable<Section> GetSections();
        /// <summary> Все бренды </summary>
        IEnumerable<Brand> GetBrands();
        /// <summary> Все товары </summary>
        IEnumerable<Product> GetProducts(ProductFilter productFilter = null);
        /// <summary> Один продукт по ид </summary>
        Product GetProductById(int Id);
    }
}
