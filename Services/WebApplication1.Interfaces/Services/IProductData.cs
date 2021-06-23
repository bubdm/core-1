﻿using System.Collections.Generic;
using WebApplication1.Domain;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Interfaces.Services
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
        Product GetProductById(int id);
        /// <summary> Добавить продукт </summary>
        int Add(Product product);
        /// <summary> Обновить продукт </summary>
        void Update(Product product);
        /// <summary> Удалить продукт </summary>
        bool Delete(int id);
    }
}
