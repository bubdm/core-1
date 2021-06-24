using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebApplication1.Domain.Model;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.ViewModel;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Services.Services
{
    public class InCookiesCartService : ICartService
    {
        private readonly IHttpContextAccessor _ContextAccessor;
        private readonly IProductData _ProductData;
        private readonly string _cartName;
        private readonly Mapper _mapperProductToView = new (new MapperConfiguration(c => c.CreateMap<Product, ProductViewModel>()
            .ForMember("Section", o => o.MapFrom(p => p.Section.Name))
            .ForMember("Brand", o => o.MapFrom(p => p.Brand.Name))));

        private Cart Cart
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

        public InCookiesCartService(IHttpContextAccessor contextAccessor, IProductData productData)
        {
            _ContextAccessor = contextAccessor;
            _ProductData = productData;

            var user = _ContextAccessor.HttpContext!.User;
            var userName = user!.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _cartName = $"WebStore.Cart{userName}";
        }

        public void Add(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(c => c.ProductId == id);
            if (item is null)
                cart.Items.Add(new CartItem {ProductId = id});
            else
                item.Quantity++;

            Cart = cart;
        }

        public void Minus(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(c => c.ProductId == id);
            if (item is null) return;

            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity <= 0)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public void Remove(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(c => c.ProductId == id);
            if (item is null) return;

            cart.Items.Remove(item);

            Cart = cart;
        }

        public void Clear()
        {
            var cart = Cart;

            cart.Items.Clear();

            Cart = cart;
        }

        public CartViewModel GetViewModel()
        {
            var products = _ProductData.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(item => item.ProductId).ToArray()
            });

            var productViews = _mapperProductToView
                .Map<IEnumerable<ProductViewModel>>(products).ToDictionary(p => p.Id);

            return new CartViewModel
            {
                Items = Cart.Items
                    .Where(p => productViews.ContainsKey(p.ProductId))
                    .Select(p => (productViews[p.ProductId], p.Quantity, productViews[p.ProductId].Price * p.Quantity))
            };
        }
    }
}
