using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Model;
using WebApplication1.Domain.WebModel;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Services.Services
{
    public class CartService : ICartService
    {
        private readonly ICartStore _cartStore;
        private readonly IProductData _productData;
        private readonly Mapper _mapperProductToView = new (new MapperConfiguration(c => c.CreateMap<Product, ProductWebModel>()
            .ForMember("Section", o => o.MapFrom(p => p.Section.Name))
            .ForMember("Brand", o => o.MapFrom(p => p.Brand.Name))));

        public CartService(ICartStore cartStore, IProductData productData)
        {
            _cartStore = cartStore;
            _productData = productData;
        }

        public void Add(int id)
        {
            var cart = _cartStore.Cart;

            var item = cart.Items.FirstOrDefault(c => c.ProductId == id);
            if (item is null)
                cart.Items.Add(new CartItem {ProductId = id});
            else
                item.Quantity++;

            _cartStore.Cart = cart;
        }

        public void Minus(int id)
        {
            var cart = _cartStore.Cart;

            var item = cart.Items.FirstOrDefault(c => c.ProductId == id);
            if (item is null) return;

            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity <= 0)
                cart.Items.Remove(item);

            _cartStore.Cart = cart;
        }

        public void Remove(int id)
        {
            var cart = _cartStore.Cart;

            var item = cart.Items.FirstOrDefault(c => c.ProductId == id);
            if (item is null) return;

            cart.Items.Remove(item);

            _cartStore.Cart = cart;
        }

        public void Clear()
        {
            var cart = _cartStore.Cart;

            cart.Items.Clear();

            _cartStore.Cart = cart;
        }

        public CartWebModel GetWebModel()
        {
            var cart = _cartStore.Cart;

            var products = _productData.GetProducts(new ProductFilter
            {
                Ids = cart.Items.Select(item => item.ProductId).ToArray()
            })?.Products;

            var productViews = _mapperProductToView
                .Map<IEnumerable<ProductWebModel>>(products).ToDictionary(p => p.Id);

            return new CartWebModel
            {
                Items = cart.Items
                    .Where(p => productViews.ContainsKey(p.ProductId))
                    .Select(p => (productViews[p.ProductId], p.Quantity, productViews[p.ProductId].Price * p.Quantity))
            };
        }
    }
}
