using System.Collections.Generic;
using System.Collections.Immutable;
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
                .Returns(new ProductsPage
                {
                    Products = Enumerable.Range(1, 5).Select(i => new Product
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
                    }),
                    TotalCount = 5,
                });

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

        [TestMethod]
        public void CartService_Add_Correct()
        {
            _cart.Items.Clear();
            const int expId = 1;
            const int expCount = 1;

            _cartService.Add(expId);

            Assert.AreEqual(expCount, _cart.ItemsCount);
            Assert.AreEqual(expCount, _cart.Items.Count);
            Assert.AreEqual(expId, _cart.Items.First().ProductId);
        }

        [TestMethod]
        public void CartService_Remove_Correct()
        {
            const int removedId = 1;
            const int expFirstProductId = 2;
            const int expCount = 2;
            const int expItemsCount = 5;

            _cartService.Remove(removedId);

            Assert.AreEqual(expCount, _cart.Items.Count);
            Assert.AreEqual(expItemsCount, _cart.ItemsCount);
            Assert.AreEqual(expFirstProductId, _cart.Items.First().ProductId);
        }

        [TestMethod]
        public void CartService_Clear_Correct()
        {
            _cartService.Clear();

            Assert.AreEqual(0, _cart.Items.Count);
        }

        [TestMethod]
        public void CartService_Minus_Correct()
        {
            const int minusId = 2;
            const int expCountItem = 1;
            const int expCount = 3;
            const int expItemsCount = 5;

            _cartService.Minus(minusId);

            Assert.AreEqual(expCount, _cart.Items.Count);
            Assert.AreEqual(expItemsCount, _cart.ItemsCount);
            var items = _cart.Items.ToImmutableArray();
            Assert.AreEqual(minusId, items[1].ProductId);
            Assert.AreEqual(expCountItem, items[1].Quantity);
        }

        [TestMethod]
        public void CartService_Remove_Item_When_Decrement_to_0()
        {
            const int minusId = 1;
            const int expCount = 2;
            const int expItemsCount = 5;

            _cartService.Minus(minusId);

            Assert.AreEqual(expCount, _cart.Items.Count);
            Assert.AreEqual(expItemsCount, _cart.ItemsCount);
        }

        [TestMethod]
        public void CartService_GetWebModel_WorkCorrect()
        {
            const int expItemsCount = 6;
            const decimal expFirstProductPrice = 1.1m;

            var result = _cartService.GetWebModel();

            Assert.AreEqual(expItemsCount, result.ItemsCount);
            Assert.AreEqual(expFirstProductPrice, result.Items.First().Product.Price);
        }
    }
}
