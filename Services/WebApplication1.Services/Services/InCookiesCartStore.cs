using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebApplication1.Domain.Entities;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Services.Services
{
    public class InCookiesCartStore : ICartStore
    {
        private readonly IHttpContextAccessor _ContextAccessor;
        private readonly string _cartName;

        public Cart Cart
        {
            get
            {
                var context = _ContextAccessor.HttpContext;
                if (!context!.Request.Cookies.ContainsKey(_cartName))
                {
                    var cart = new Cart();
                    context.Response.Cookies.Append(_cartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }
                return JsonConvert.DeserializeObject<Cart>(context.Request.Cookies[_cartName]);
            }
            set => _ContextAccessor.HttpContext!.Response.Cookies.Append(_cartName, JsonConvert.SerializeObject(value));
        }

        public InCookiesCartStore(IHttpContextAccessor contextAccessor)
        {
            _ContextAccessor = contextAccessor;

            var user = _ContextAccessor.HttpContext!.User;
            var userName = user!.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _cartName = $"WebStore.Cart{userName}";
        }
    }
}
