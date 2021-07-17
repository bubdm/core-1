using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.WebModel;

namespace WebApplication1.Services.Tests.Services
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _cart;

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
