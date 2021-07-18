using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Domain.Entities.Orders;
using WebApplication1.Domain.WebModel;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public async Task CheckOut_ModelState_Invalid_Returns_View_With_Models()
        {
            const string expOrderName = "Test order";
            var cartServiceStub = Mock
                .Of<ICartService>();
            var orderServiceStub = Mock
                .Of<IOrderService>();
            var controller = new CartController(cartServiceStub, orderServiceStub);
            controller.ModelState.AddModelError("error", "InvalidError");
            var orderModel = new OrderWebModel
            {
                Name = expOrderName,
            };
            var result = await controller.CheckOut(orderModel);
            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult) result;
            Assert
                .IsInstanceOfType(viewResult.Model, typeof(CartOrderWebModel));
            var cartModel = (CartOrderWebModel) viewResult.Model;
            Assert
                .AreEqual(expOrderName, cartModel.Order.Name);
        }

        [TestMethod]
        public async Task CheckOut_ModelState_Valid_Call_Service_And_Returns_Redirect()
        {
            const int expProductId = 1;
            const string expProductName = "Test product";
            const decimal expProductPrice = 1m;
            const int expProductCount = 1;
            const string expImageName = "TestImage.jpg";
            const int expOrderId = 1;
            const string expOrderName = "Test order";
            const string expOrderAddress = "Test address";
            const string expOrderPhone = "123";
            const string expUserName = "TestUser";

            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock
                .Setup(c => c.GetWebModel())
                .Returns(new CartWebModel
                {
                    Items = new [] { ( new ProductWebModel
                    {
                        Id = expProductId,
                        Name = expProductName,
                        Price = expProductPrice,
                        Brand = "Test Brand",
                        Section = "Test Section",
                        ImageUrl = expImageName,
                    }, expProductCount, expProductPrice * expProductCount ) }
                });
            var orderServiceMock = new Mock<IOrderService>();
            orderServiceMock
                .Setup(o => o.CreateOrder(It.IsAny<string>(), It.IsAny<CartWebModel>(), It.IsAny<OrderWebModel>()))
                .ReturnsAsync(new Order
                {
                    Id = expOrderId,
                    Name = expOrderName,
                    Address = expOrderAddress,
                    Phone = expOrderPhone,
                    DateTime = DateTime.Now,
                    Items = Array.Empty<OrderItem>(),
                });
            var controller = new CartController(cartServiceMock.Object, orderServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new [] {new Claim(ClaimTypes.Name, expUserName)}))
                    }
                }
            };
            var orderModel = new OrderWebModel
            {
                Name = expOrderName,
                Address = expOrderAddress,
                Phone = expOrderPhone,
            };

            var result = await controller.CheckOut(orderModel);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult) result;
            Assert.AreEqual(nameof(CartController.OrderConfirmed), redirectResult.ActionName);
            Assert.IsNull(redirectResult.ControllerName);
            Assert.AreEqual(expOrderId, redirectResult.RouteValues["Id"]);
        }
    }
}
