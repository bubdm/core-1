using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Components
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;
        public CartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.CartProductsCount = _cartService.GetWebModel().ItemsCount;
            return View();
        }
    }
}
