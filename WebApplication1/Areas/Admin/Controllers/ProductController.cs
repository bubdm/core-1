using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication.Domain.Entities;
using WebApplication.Domain.Identity;
using WebApplication1.Areas.Admin.Models;
using WebApplication1.Services.Interfaces;
using WebApplication1.ViewModel;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = Role.Administrators)]
    public class ProductController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly IWebHostEnvironment _AppEnvironment;

        private readonly Mapper _mapperProductToView =
            new(new MapperConfiguration(c => c.CreateMap<Product, ProductEditViewModel>()            
                .ForMember("SectionName", o => o.MapFrom(p => p.Section.Name))
                .ForMember("BrandName", o => o.MapFrom(p => p.Brand.Name))));
        private readonly Mapper _mapperProductFromView =
            new(new MapperConfiguration(c => c.CreateMap<ProductEditViewModel, Product>()));

        public ProductController(IProductData productData, IWebHostEnvironment appEnvironment)
        {
            _ProductData = productData;
            _AppEnvironment = appEnvironment;
        }

        public IActionResult Index(string name, int page = 1, ProductEditSortState sortOrder = ProductEditSortState.NameAsc)
        {
            var products = _ProductData.GetProducts();

            if (!String.IsNullOrEmpty(name))
            {
                products = products.Where(p => p.Name.Contains(name));
            }

            products = sortOrder switch
            {
                ProductEditSortState.NameDesc => products.OrderByDescending(p => p.Name),
                ProductEditSortState.OrderAsc => products.OrderBy(p => p.Order),
                ProductEditSortState.OrderDesc => products.OrderByDescending(p => p.Order),
                ProductEditSortState.PriceAsc => products.OrderBy(p => p.Price),
                ProductEditSortState.PriceDesc => products.OrderByDescending(p => p.Price),
                ProductEditSortState.SectionAsc => products.OrderBy(p => p.Section.Name),
                ProductEditSortState.SectionDesc => products.OrderByDescending(p => p.Section.Name),
                ProductEditSortState.BrandAsc => products.OrderBy(p => p.Brand.Name),
                ProductEditSortState.BrandDesc => products.OrderByDescending(p => p.Brand.Name),
                _ => products.OrderBy(p => p.Name),
            };

            int pageSize = 6;
            var count = products!.Count();
            products = products.Skip((page - 1) * pageSize).Take(pageSize);

            //var pageModel = new PageViewModel(count, page, pageSize);


            var webModel = new IndexProductEditViewModel
            {
                FilterViewModel = new ProductEditFilterViewModel(name),
                SortViewModel = new ProductEditSortViewModel(sortOrder),
                PageViewModel = new PageViewModel(count, page, pageSize),
                Products = _mapperProductToView.Map<IEnumerable<ProductEditViewModel>>(products.ToList()),
            };
            return View(webModel);
        }

        public IActionResult Create()
        {
            ViewBag.Sections = new SelectList(_ProductData.GetSections(), "Id", "Name");
            ViewBag.Brands = new SelectList(_ProductData.GetBrands(), "Id", "Name");
            return View("Edit", new ProductEditViewModel());
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
                return BadRequest();
            if (_ProductData.GetProductById(id) is { } product)
            {
                ViewBag.Sections = new SelectList(_ProductData.GetSections(), "Id", "Name");
                ViewBag.Brands = new SelectList(_ProductData.GetBrands(), "Id", "Name");
                return View(_mapperProductToView.Map<ProductEditViewModel>(product));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditViewModel model, IFormFile uploadedFile)
        {
            if (model is null)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Sections = new SelectList(_ProductData.GetSections(), "Id", "Name");
                ViewBag.Brands = new SelectList(_ProductData.GetBrands(), "Id", "Name");
                return View(model);
            }

            var product = _mapperProductFromView.Map<Product>(model);

            if (uploadedFile is not null)
            {
                string path = "/images/home/" + uploadedFile.FileName;
                await using (var fileStream = new FileStream(_AppEnvironment.WebRootPath + path, FileMode.Create))
                    await uploadedFile.CopyToAsync(fileStream);
                product.ImageUrl = uploadedFile.FileName;
            }

            if (product.Id == 0)
                _ProductData.Add(product);
            else
                _ProductData.Update(product);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();
            if (_ProductData.GetProductById(id) is { } product)
            {
                return View(_mapperProductToView.Map<ProductEditViewModel>(product));
            }
            return NotFound();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id <= 0)
                return BadRequest();
            _ProductData.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
