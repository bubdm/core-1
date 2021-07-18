using System.Collections.Generic;
using System.Linq;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Model;
using WebApplication1.Domain.WebModel;
using WebApplication1.Interfaces.Services;
using WebApplication1.Services.Services;

namespace WebApplication1.Services.Tests.Services
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _cart;
        private Mock<IProductData> _productDataMock;
        private Mock<ICartStore> _cartStoreMock;
        private ICartService _cartService;

        [TestInitialize]
        public void Initialize()
        {
            _cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem(){ProductId = 1, Quantity = 1},
                    new CartItem(){ProductId = 2, Quantity = 2},
                    new CartItem(){ProductId = 3, Quantity = 3}
                }
            };

            _productDataMock = new Mock<IProductData>();
            _productDataMock
                .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
                .Returns(Enumerable.Range(1, 5).Select(i => new Product
                {
                    Id = i,
                    Name = $"Product {i}",
                    Price = 1.1m * i,
                    Order = i,
                    ImageUrl = $"image_{i}.jpg",
                    BrandId = i,
                    Brand = new Brand{Id = i, Name = $"Тестовый бренд {i}", Order = i},
                    SectionId = i,
                    Section = new Section{Id = i, Name = $"Категория {i}", Order = i},
                }));

            _cartStoreMock = new Mock<ICartStore>();
            _cartStoreMock.Setup(c => c.Cart).Returns(_cart);

            _cartService = new CartService(_cartStoreMock.Object, _productDataMock.Object);
        }

        [TestMethod]
        public void Cart_Class_ItemsCount_Returns_Correct_Quantity()
        {
            var cart = _cart;
            var expectedCount = cart.Items.Sum(i => i.Quantity);

            var actualCount = cart.ItemsCount;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void CartWebModel_Returns_Correct_ItemsCount()
        {
            const int expectedCount = 3;
            var cartWebModel = new CartWebModel
            {
                Items = new List<(ProductWebModel Product, int Quantity, decimal Sum)>{
                    new (new ProductWebModel { Id = 1, Name = "Product 1", Price = 0.5m }, 1, 0.5m),
                    new (new ProductWebModel { Id = 2, Name = "Product 2", Price = 1.5m }, 2, 3m)
                }
            };

            var actualCount = cartWebModel.ItemsCount;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void CartWebModel_Returns_Correct_TotalPrice()
        {
            var cartWebModel = new CartWebModel
            {
                Items = new List<(ProductWebModel Product, int Quantity, decimal Sum)>{
                    new (new ProductWebModel { Id = 1, Name = "Product 1", Price = 0.5m }, 1, 0.5m),
                    new (new ProductWebModel { Id = 2, Name = "Product 2", Price = 1.5m }, 2, 3m)
                }
            };
            decimal expectedTotalPrice = cartWebModel.Items.Sum(i => i.Quantity * i.Product.Price);

            var actualTotalPrice = cartWebModel.TotalPrice;

            Assert.AreEqual(expectedTotalPrice, actualTotalPrice);
        }

    }
}
