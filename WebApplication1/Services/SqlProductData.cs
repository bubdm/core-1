using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Domain;
using WebApplication.Domain.Entities;
using WebApplication1.Dal.Context;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class SqlProductData : IProductData
    {
        private readonly Application1DB _context;
        public SqlProductData(Application1DB context)
        {
            _context = context;
        }

        public IEnumerable<Section> GetSections() => _context.Sections;

        public IEnumerable<Brand> GetBrands() => _context.Brands;

        public IEnumerable<Product> GetProducts(ProductFilter productFilter = null)
        {
            IQueryable<Product> query = _context.Products;
            if (productFilter?.SectionId is { } sectionId)
                query = query.Where(q => q.SectionId == sectionId);
            if (productFilter?.BrandId is { } brandId)
                query = query.Where(q => q.BrandId == brandId);
            return query;
        }
    }
}
