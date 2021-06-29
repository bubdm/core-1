using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Model;
using WebApplication1.Interfaces.Adresses;
using WebApplication1.Interfaces.Services;
using WebApplication1.WebAPI.Clients.Base;

namespace WebApplication1.WebAPI.Clients.Products
{
    public class ProductsClient : BaseSyncClient, IProductData
    {
        public ProductsClient(HttpClient client) : base(client, WebAPIInfo.Products) { }

        public IEnumerable<Section> GetSections()
        {
            return Get<IEnumerable<Section>>($"{Address}/sections");
        }

        public Section GetSection(int id)
        {
            return Get<Section>($"{Address}/sections/{id}");
        }

        public IEnumerable<Brand> GetBrands()
        {
            return Get<IEnumerable<Brand>>($"{Address}/brands");
        }

        public Brand GetBrand(int id)
        {
            return Get<Brand>($"{Address}/brands/{id}");
        }

        public IEnumerable<Product> GetProducts(ProductFilter productFilter = null)
        {
            var response = Post(Address, productFilter);
            var products = response.Content.ReadFromJsonAsync<IEnumerable<Product>>().Result;
            return products;
        }

        public Product GetProductById(int id)
        {
            return Get<Product>($"{Address}/{id}");
        }

        public int Add(Product product)
        {
            throw new NotImplementedException();
        }

        public void Update(Product product)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
