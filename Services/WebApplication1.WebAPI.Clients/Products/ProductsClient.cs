using System;
using System.Collections.Generic;
using System.Net.Http;
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
            throw new NotImplementedException();
        }

        public Section GetSection(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Brand> GetBrands()
        {
            throw new NotImplementedException();
        }

        public Brand GetBrand(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetProducts(ProductFilter productFilter = null)
        {
            throw new NotImplementedException();
        }

        public Product GetProductById(int id)
        {
            throw new NotImplementedException();
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
