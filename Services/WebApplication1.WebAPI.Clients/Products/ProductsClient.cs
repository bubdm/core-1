using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using WebApplication1.Domain.DTO;
using WebApplication1.Domain.DTO.Mappers;
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
            return Get<IEnumerable<SectionDTO>>($"{Address}/sections").FromDTO();
        }
        public Section GetSection(int id)
        {
            return Get<SectionDTO>($"{Address}/sections/{id}").FromDTO();
        }
        public IEnumerable<Brand> GetBrands()
        {
            return Get<IEnumerable<BrandDTO>>($"{Address}/brands").FromDTO();
        }
        public Brand GetBrand(int id)
        {
            return Get<BrandDTO>($"{Address}/brands/{id}").FromDTO();
        }
        public ProductsPage GetProducts(ProductFilter productFilter = null)
        {
            var response = Post($"{Address}/get", productFilter ?? new ProductFilter());
            var products =  response.Content.ReadFromJsonAsync<ProductPageDTO>().Result;
            return products.FromDTO();
        }
        public Product GetProductById(int id)
        {
            return Get<ProductDTO>($"{Address}/{id}").FromDTO();
        }
        public int Add(Product product)
        {
            var response = Post(Address, product.ToDTO());
            var id = response.Content.ReadFromJsonAsync<int>().Result;
            return id;
        }
        public void Update(Product product)
        {
            Put(Address, product.ToDTO());
        }
        public bool Delete(int id)
        {
            var result = Delete($"{Address}/{id}").IsSuccessStatusCode;
            return result;
        }
        public IEnumerable<Keyword> GetKeywords()
        {
            return Get<IEnumerable<KeywordDTO>>($"{Address}/keywords").FromDto();
        }
        public Keyword GetKeyword(int id)
        {
            return Get<KeywordDTO>($"{Address}/keywords/{id}").FromDto();
        }
    }
}
