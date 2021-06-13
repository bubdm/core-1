using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication.Domain;
using WebApplication.Domain.Entities;
using WebApplication1.Dal.Context;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class SqlProductData : IProductData
    {
        private readonly Application1DB _context;
        private readonly ILogger<Application1DB> _logger;
        public SqlProductData(Application1DB context, ILogger<Application1DB> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Section> GetSections() => _context.Sections;

        public IEnumerable<Brand> GetBrands() => _context.Brands;

        public IEnumerable<Product> GetProducts(ProductFilter productFilter = null)
        {
            IQueryable<Product> query = _context.Products
                .Include(p => p.Section)
                .Include(p => p.Brand);
            if (productFilter?.Ids?.Length > 0)
            {
                query = query.Where(p => productFilter.Ids.Contains(p.Id));
            }
            else
            {
                if (productFilter?.SectionId is { } sectionId)
                    query = query.Where(q => q.SectionId == sectionId);
                if (productFilter?.BrandId is { } brandId)
                    query = query.Where(q => q.BrandId == brandId);
            }
            _logger.LogInformation($"Запрос SQL: {query.ToQueryString()}");
            return query;
        }

        public Product GetProductById(int Id) => _context.Products
            .Include(p => p.Section)
            .Include(p => p.Brand)
            .SingleOrDefault(p => p.Id == Id);
    }
}
