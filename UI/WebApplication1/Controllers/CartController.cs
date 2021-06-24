using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.Entities;
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
            return View(new CartOrderWebModel{ Cart = _CartService.GetViewModel()});
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
        public async Task<IActionResult> CheckOut(OrderWebModel model, [FromServices] IOrderService orderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index),
                    new CartOrderWebModel {Cart = _CartService.GetViewModel(), Order = model});

            var order = await _OrderService.CreateOrder(User.Identity!.Name, _CartService.GetViewModel(), model);

            _CartService.Clear();

            return RedirectToAction(nameof(OrderConfirmed), new {order.Id});
        }

        public async Task<IActionResult> OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            ViewBag.Name = (await _OrderService.GetOrderById(id)).Name;
            return View();
        }
    }
}
