using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Model;
using WebApplication1.Domain.WebModel;
using WebApplication1.Domain.WebModel.Admin;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        [TestMethod]
        public void Index_Returns_Correct_View_Nulls()
        {
            const int expCountProducts = 3;
            const int idFirst = 1;
            const string NameFirst = "Product 1";
            const decimal PriceFirst = 100;
            const int idLast = 3;
            const string NameLast = "Product 3";
            const decimal PriceLast = 300;

            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
                .Returns<ProductFilter>(p => Enumerable.Range(1, expCountProducts).Select(id => new Product
                {
                    Id = id,
                    Name = $"Product {id}",
                    Order = 1,
                    Price = 100 * id,
                    ImageUrl = $"image_{id}.jpg",
                    Brand = new Brand{Id = 1, Name = "Brand", Order = 1},
                    Section = new Section{Id = 1, Name = "Section", Order = 1},
                }));
            var controller = new CatalogController(productDataMock.Object);

            var result = controller.Index(null, null);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult) result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(CatalogWebModel));
            var catalogWebModel = (CatalogWebModel) viewResult.Model;
            Assert.AreEqual(null, catalogWebModel.BrandId);
            Assert.AreEqual(null, catalogWebModel.SectionId);
            var pageModel = catalogWebModel.PageViewModel;
            Assert.IsInstanceOfType(pageModel, typeof(PageViewModel));
            Assert.AreEqual(1, pageModel.PageNumber);
            Assert.AreEqual(1, pageModel.TotalPages);
            var products = catalogWebModel.Products;
            Assert.AreEqual(expCountProducts, products.Count());
            var firstWebModel = products.FirstOrDefault();
            Assert.AreEqual(idFirst, firstWebModel.Id);
            Assert.AreEqual(NameFirst, firstWebModel.Name);
            Assert.AreEqual(PriceFirst, firstWebModel.Price);
            var lastWebModel = products.LastOrDefault();
            Assert.AreEqual(idLast, lastWebModel.Id);
            Assert.AreEqual(NameLast, lastWebModel.Name);
            Assert.AreEqual(PriceLast, lastWebModel.Price);

            productDataMock.Verify(s => s.GetProducts(It.IsAny<ProductFilter>()), Times.Once);
            productDataMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void Details_Returns_Correct_View()
        {
            const decimal expectedPrice = 10m;
            const int expectedId = 1;
            const string expectedName = "Product 1";

            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns<int>(id => new Product
                {
                    Id = id,
                    Name = $"Product {id}",
                    Order = 1,
                    Price = expectedPrice,
                    ImageUrl = $"image_{id}.jpg",
                    Brand = new Brand{Id = 1, Name = "Brand", Order = 1},
                    Section = new Section{Id = 1, Name = "Section", Order = 1},
                });
            var controller = new CatalogController(productDataMock.Object);

            var result = controller.Details(expectedId);
            
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult) result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(ProductWebModel));
            var webModel = (ProductWebModel) viewResult.Model;
            Assert.AreEqual(expectedId, webModel.Id);
            Assert.AreEqual(expectedName, webModel.Name);
            Assert.AreEqual(expectedPrice, webModel.Price);

            productDataMock.Verify(s => s.GetProductById(It.IsAny<int>()), Times.Once);
            productDataMock.VerifyNoOtherCalls();
        }
    }
}
