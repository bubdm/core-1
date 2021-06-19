using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Domain.Entities;
using WebApplication1.Services.Interfaces;
using WebApplication1.ViewModel;

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
            return View(new CartOrderViewModel{ Cart = _CartService.GetViewModel()});
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
        public async Task<IActionResult> CheckOut(OrderViewModel model, [FromServices] IOrderService orderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index),
                    new CartOrderViewModel {Cart = _CartService.GetViewModel(), Order = model});

            var order = await _OrderService.CreateOrder(User.Identity!.Name, _CartService.GetViewModel(), model);

            return RedirectToAction(nameof(OrderConfirmed), new {order.Id});
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            ViewBag.Name = _OrderService.GetOrderById(id).GetAwaiter().GetResult();
            return View();
        }
    }
}
