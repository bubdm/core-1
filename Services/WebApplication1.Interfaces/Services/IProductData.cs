using System.Collections.Generic;
using WebApplication1.Domain.Model;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Interfaces.Services
{
    public interface IProductData
    {
        /// <summary> все категории </summary>
        IEnumerable<Section> GetSections();
        /// <summary> Одна категория </summary>
        Section GetSection(int id);
        /// <summary> Все бренды </summary>
        IEnumerable<Brand> GetBrands();
        /// <summary> Один бренд </summary>
        Brand GetBrand(int id);
        /// <summary> Все товары </summary>
        ProductsPage GetProducts(ProductFilter productFilter = null);
        /// <summary> Один продукт по ид </summary>
        Product GetProductById(int id);
        /// <summary> Добавить продукт </summary>
        int Add(Product product);
        /// <summary> Обновить продукт </summary>
        void Update(Product product);
        /// <summary> Удалить продукт </summary>
        bool Delete(int id);
    }
}
