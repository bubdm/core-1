using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication1.Dal.Context;
using WebApplication1.Domain.Model;
using WebApplication1.Domain.Entities;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Services.Services
{
    public class SqlProductData : IProductData
    {
        private readonly Application1Context _context;
        private readonly ILogger<Application1Context> _logger;
        public SqlProductData(Application1Context context, ILogger<Application1Context> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Section> GetSections() => _context.Sections.Include(s => s.Products);
        public Section GetSection(int id)
        {
            return _context.Sections
                .Include(s => s.Products).FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Brand> GetBrands() => _context.Brands.Include(b => b.Products);
        public Brand GetBrand(int id)
        {
            return _context.Brands
                .Include(b => b.Products).FirstOrDefault(b => b.Id == id);
        }

        public ProductsPage GetProducts(ProductFilter productFilter = null)
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
            var productCount = query.Count();

            if (productFilter is {PageSize: > 0 and var pageSize, Page: > 0 and var pageNumber})
                query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

            _logger.LogInformation($"Запрос SQL: {query.ToQueryString()}");
            return new ProductsPage()
            {
                Products = query.AsEnumerable(),
                TotalCount = productCount,
            };
        }

        public Product GetProductById(int id) => _context.Products
            .Include(p => p.Section)
            .Include(p => p.Brand)
            .Include(p => p.Keywords)
            .SingleOrDefault(p => p.Id == id);

        public int Add(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));
            _context.Add(product);
            _context.SaveChanges();
            return product.Id;
        }

        public void Update(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));
            if (_context.Products.Local.Any(e => e == product)) 
                _context.Update(product);
            else
            {
                var origin = _context.Products.Find(product.Id);
                origin.Name = product.Name;
                origin.Order = product.Order;
                origin.SectionId = product.SectionId;
                origin.BrandId = product.BrandId;
                origin.Price = product.Price;
                origin.ImageUrl = product.ImageUrl;
                _context.Update(origin);
            }
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            if (GetProductById(id) is not { } person) return false;
            _context.Remove(person);
            _context.SaveChanges();
            return true;
        }
    }
}
