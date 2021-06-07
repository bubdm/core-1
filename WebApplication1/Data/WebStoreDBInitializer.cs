using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication.Domain.Entities;
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
                InitProducts();
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
                _context.Sections.AddRange(_Sections);

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] ON"); // Костыль!!!
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                _context.Database.CommitTransaction();
            }
            using (_context.Database.BeginTransaction())
            {
                _context.Brands.AddRange(_Brands);

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] ON"); // Костыль!!!
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] OFF");

                _context.Database.CommitTransaction();
            }
            using (_context.Database.BeginTransaction())
            {
                _context.Products.AddRange(_Products);

                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON"); // Костыль!!!
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");

                _context.Database.CommitTransaction();
            }
        }

        #region Стремные данные без базы данных

        private readonly IEnumerable<Section> _Sections = new[]
        {
            new Section { Id = 1, Name = "Спорт", Order = 0 },
            new Section { Id = 2, Name = "Nike", Order = 0, ParentId = 1 },
            new Section { Id = 3, Name = "Under Armour", Order = 1, ParentId = 1 },
            new Section { Id = 4, Name = "Adidas", Order = 2, ParentId = 1 },
            new Section { Id = 5, Name = "Puma", Order = 3, ParentId = 1 },
            new Section { Id = 6, Name = "ASICS", Order = 4, ParentId = 1 },
            new Section { Id = 7, Name = "Для мужчин", Order = 1 },
            new Section { Id = 8, Name = "Fendi", Order = 0, ParentId = 7 },
            new Section { Id = 9, Name = "Guess", Order = 1, ParentId = 7 },
            new Section { Id = 10, Name = "Valentino", Order = 2, ParentId = 7 },
            new Section { Id = 11, Name = "Диор", Order = 3, ParentId = 7 },
            new Section { Id = 12, Name = "Версачи", Order = 4, ParentId = 7 },
            new Section { Id = 13, Name = "Армани", Order = 5, ParentId = 7 },
            new Section { Id = 14, Name = "Prada", Order = 6, ParentId = 7 },
            new Section { Id = 15, Name = "Дольче и Габбана", Order = 7, ParentId = 7 },
            new Section { Id = 16, Name = "Шанель", Order = 8, ParentId = 7 },
            new Section { Id = 17, Name = "Гуччи", Order = 9, ParentId = 7 },
            new Section { Id = 18, Name = "Для женщин", Order = 2 },
            new Section { Id = 19, Name = "Fendi", Order = 0, ParentId = 18 },
            new Section { Id = 20, Name = "Guess", Order = 1, ParentId = 18 },
            new Section { Id = 21, Name = "Valentino", Order = 2, ParentId = 18 },
            new Section { Id = 22, Name = "Dior", Order = 3, ParentId = 18 },
            new Section { Id = 23, Name = "Versace", Order = 4, ParentId = 18 },
            new Section { Id = 24, Name = "Для детей", Order = 3 },
            new Section { Id = 25, Name = "Мода", Order = 4 },
            new Section { Id = 26, Name = "Для дома", Order = 5 },
            new Section { Id = 27, Name = "Интерьер", Order = 6 },
            new Section { Id = 28, Name = "Одежда", Order = 7 },
            new Section { Id = 29, Name = "Сумки", Order = 8 },
            new Section { Id = 30, Name = "Обувь", Order = 9 },
        };
        private readonly IEnumerable<Brand> _Brands = new[]
        {
            new Brand { Id = 1, Name = "Acne", Order = 0 },
            new Brand { Id = 2, Name = "Grune Erde", Order = 1 },
            new Brand { Id = 3, Name = "Albiro", Order = 2 },
            new Brand { Id = 4, Name = "Ronhill", Order = 3 },
            new Brand { Id = 5, Name = "Oddmolly", Order = 4 },
            new Brand { Id = 6, Name = "Boudestijn", Order = 5 },
            new Brand { Id = 7, Name = "Rosch creative culture", Order = 6 },
        };
        private readonly IEnumerable<Product> _Products  = new[]
        {
            new Product { Id = 1, Name = "Белое платье", Price = 1025, ImageUrl = "product1.jpg", Order = 0, SectionId = 2, BrandId = 1 },
            new Product { Id = 2, Name = "Розовое платье", Price = 1025, ImageUrl = "product2.jpg", Order = 1, SectionId = 2, BrandId = 1 },
            new Product { Id = 3, Name = "Красное платье", Price = 1025, ImageUrl = "product3.jpg", Order = 2, SectionId = 2, BrandId = 1 },
            new Product { Id = 4, Name = "Джинсы", Price = 1025, ImageUrl = "product4.jpg", Order = 3, SectionId = 2, BrandId = 1 },
            new Product { Id = 5, Name = "Лёгкая майка", Price = 1025, ImageUrl = "product5.jpg", Order = 4, SectionId = 2, BrandId = 2 },
            new Product { Id = 6, Name = "Лёгкое голубое поло", Price = 1025, ImageUrl = "product6.jpg", Order = 5, SectionId = 2, BrandId = 1 },
            new Product { Id = 7, Name = "Платье белое", Price = 1025, ImageUrl = "product7.jpg", Order = 6, SectionId = 2, BrandId = 1 },
            new Product { Id = 8, Name = "Костюм кролика", Price = 1025, ImageUrl = "product8.jpg", Order = 7, SectionId = 25, BrandId = 1 },
            new Product { Id = 9, Name = "Красное китайское платье", Price = 1025, ImageUrl = "product9.jpg", Order = 8, SectionId = 25, BrandId = 1 },
            new Product { Id = 10, Name = "Женские джинсы", Price = 1025, ImageUrl = "product10.jpg", Order = 9, SectionId = 25, BrandId = 3 },
            new Product { Id = 11, Name = "Джинсы женские", Price = 1025, ImageUrl = "product11.jpg", Order = 10, SectionId = 25, BrandId = 3 },
            new Product { Id = 12, Name = "Летний костюм", Price = 1025, ImageUrl = "product12.jpg", Order = 11, SectionId = 25, BrandId = 3 },
        };

        #endregion
    }
}
