using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _CartService;

        public CartController(ICartService cartService)
        {
            _CartService = cartService;
        }
        public IActionResult Index()
        {
            return View(_CartService.GetViewModel());
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
    }
}
