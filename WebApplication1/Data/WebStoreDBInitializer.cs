using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication1.Dal.Context;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Data
{
    public class WebStoreDBInitializer
    {
        private readonly Application1DB _context;
        private readonly ILogger<WebStoreDBInitializer> _logger;
        private readonly IProductData _productData;

        public WebStoreDBInitializer(Application1DB context, ILogger<WebStoreDBInitializer> Logger, IProductData productData)
        {
            _context = context;
            _logger = Logger;
            _productData = productData;
        }

        public void Init()
        {
            if (_context.Database.GetPendingMigrations().Any())
                _context.Database.Migrate();

            try
            {

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка при инициализации данных");
                throw;
            }
        }

        private void InitProducts()
        {
            if (_context.Products.Any())
                return;
            using (_context.Database.BeginTransaction())
            {
                _context.Sections.AddRange(_productData.GetSections());

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] ON"); // Костыль!!!
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                _context.Database.CommitTransaction();
            }
            using (_context.Database.BeginTransaction())
            {
                _context.Brands.AddRange(_productData.GetBrands());

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] ON"); // Костыль!!!
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                _context.Database.CommitTransaction();
            }
            using (_context.Database.BeginTransaction())
            {
                _context.Products.AddRange(_productData.GetProducts());

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] ON"); // Костыль!!!
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                _context.Database.CommitTransaction();
            }
        }
    }
}
