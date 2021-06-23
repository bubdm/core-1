using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Identity;
using WebApplication1.Dal.Context;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Data
{
    public class WebStoreDBInitializer
    {
        private readonly Application1Context _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<WebStoreDBInitializer> _logger;
        public WebStoreDBInitializer(
            Application1Context context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILogger<WebStoreDBInitializer> Logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = Logger;
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

            try
            {
                InitIdentityAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка в инициализации данных Identity");
                throw;
            }

            try
            {
                InitPersons();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка в инициализации данных сотрудников");
                throw;
            }

        }

        private void InitProducts()
        {
            if (_context.Products.Any())
            {
                _logger.LogInformation("БД не нуждается в обновлении");
                return;
            }

            var sectionsPool = _Sections.ToDictionary(s => s.Id);
            var brandsPool = _Brands.ToDictionary(b => b.Id);
            foreach (var section in _Sections.Where(s => s.ParentId != null))
                section.Parent = sectionsPool[(int) section.ParentId!];

            foreach (var product in _Products)
            {
                product.Section = sectionsPool[product.SectionId];
                if (product.BrandId is { } brandId)
                    product.Brand = brandsPool[(int)product.BrandId!];
                product.Id = 0;
                product.BrandId = 0;
                product.SectionId = 0;
            }
            foreach (var section in _Sections)
            {
                section.Id = 0;
                section.ParentId = null;
            }
            foreach (var brand in _Brands)
            {
                brand.Id = 0;
            }

            using (_context.Database.BeginTransaction())
            {
                _context.Sections.AddRange(_Sections);
                _context.Brands.AddRange(_Brands);
                _context.Products.AddRange(_Products);

                _context.SaveChanges();
                _context.Database.CommitTransaction();

                _logger.LogInformation("Добавлены тестовые данные в базу данных");
            }

        }

        private async Task InitIdentityAsync()
        {
            await CheckRoleAsync(Role.Administrators, _roleManager);
            await CheckRoleAsync(Role.Users, _roleManager);
            await CheckRoleAsync(Role.Clients, _roleManager);

            if (await _userManager.FindByNameAsync(User.Administrator) is null)
            {
                _logger.LogInformation($"Пользователь {User.Administrator} отсутствует в базе данных, создаю");
                var admin = new User
                {
                    UserName = User.Administrator
                };
                var result = await _userManager.CreateAsync(admin, User.DefaultAdministratorPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, Role.Administrators);
                    _logger.LogInformation($"Пользователь {admin.UserName} успешно создан и назначен правами {Role.Administrators}");
                }
                else
                {
                    var errors = result.Errors.Select(e => e.Description).ToArray();
                    _logger.LogError($"Учётная запись администратора не создана по причине: {string.Join(",", errors)}");
                    throw new InvalidOperationException($"Ошибка при создании пользователя {User.Administrator}:{string.Join(",", errors)}");
                }
                _logger.LogInformation("Инициализация данных БД системы Identity выполнена.");
            }
            
            static async Task CheckRoleAsync(string RoleName, RoleManager<Role> roleManager)
            {
                if (!await roleManager.RoleExistsAsync(RoleName))
                {
                    await roleManager.CreateAsync(new Role { Name = RoleName });
                }
            }
        }

        private void InitPersons()
        {
            if (_context.Persons.Any())
            {
                _logger.LogInformation("В бд уже есть работники");
                return;
            }
            _context.Persons.AddRange(_Persons);
            _context.SaveChanges();
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
        private readonly ICollection<Person> _Persons = Enumerable.Range(1, 10).Select(p => new Person
        {
            FirstName = $"Иван_{p}",
            LastName = $"Иванов_{p + 1}",
            Patronymic = $"Иванович_{p + 2}",
            Age = p + 20,
            Birthday = new DateTime(1980 + p, 1, 1),
            CountChildren = p,
        }).ToList();

        #endregion
    }
}
