using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.WebModel;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _CartService;
        private readonly IOrderService _OrderService;
        public CartController(ICartService cartService, IOrderService orderService)
        {
            _CartService = cartService;
            _OrderService = orderService;
        }
        public IActionResult Index()
        {
            return View(new CartOrderWebModel{ Cart = _CartService.GetWebModel()});
        }
        public IActionResult Add(int id)
        {
            _CartService.Add(id);
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Minus(int id)
        {
            _CartService.Minus(id);
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Remove(int id)
        {
            _CartService.Remove(id);
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult Clear(int id)
        {
            _CartService.Clear();
            return RedirectToAction("Index", "Cart");
        }

        [Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(OrderWebModel model)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index),
                    new CartOrderWebModel {Cart = _CartService.GetWebModel(), Order = model});

            var order = await _OrderService.CreateOrder(User.Identity!.Name, _CartService.GetWebModel(), model);

            _CartService.Clear();

            return RedirectToAction(nameof(OrderConfirmed), new {order.Id});
        }

        public async Task<IActionResult> OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            ViewBag.Name = (await _OrderService.GetOrderById(id)).Name;
            return View();
        }

        #region Web-API

        public IActionResult GetCartView()
        {
            return ViewComponent("Cart");
        }

        public IActionResult ApiAdd(int id)
        {
            _CartService.Add(id);
            return Json(new { id, message = $"Этот товар был добавлен в корзину (id = {id})" }); ;
        }

        public IActionResult ApiMinus(int id)
        {
            _CartService.Minus(id);
            return Json(new { id, message = $"Этот товар убавлен на еденицу (id = {id})" });
        }

        public IActionResult ApiRemove(int id)
        {
            _CartService.Remove(id);
            return Json(new { id, message = $"Этот товар удален из корзины (id = {id})" });
        }

        public IActionResult ApiClear()
        {
            _CartService.Clear();
            return Ok();
        }

        #endregion
    }
}
