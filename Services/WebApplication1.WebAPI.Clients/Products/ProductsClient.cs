using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using WebApplication1.Domain.DTO;
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
            return DTOMappers.MapperSectionFromDTO
                .Map<IEnumerable<Section>>(Get<IEnumerable<SectionDTO>>($"{Address}/sections"));
        }

        public Section GetSection(int id)
        {
            return DTOMappers.MapperSectionFromDTO
                .Map<Section>(Get<SectionDTO>($"{Address}/sections/{id}"));
        }

        public IEnumerable<Brand> GetBrands()
        {
            return DTOMappers.MapperBrandFromDTO
                .Map<IEnumerable<Brand>>(Get<IEnumerable<BrandDTO>>($"{Address}/brands"));
        }

        public Brand GetBrand(int id)
        {
            return DTOMappers.MapperBrandFromDTO
                .Map<Brand>(Get<Brand>($"{Address}/brands/{id}"));
        }

        public IEnumerable<Product> GetProducts(ProductFilter productFilter = null)
        {
            var response = Post($"{Address}/get", productFilter ?? new ProductFilter());
            var products =  response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>().Result;
            return DTOMappers.MapperProductFromDTO.Map<IEnumerable<Product>>(products);
        }

        public Product GetProductById(int id)
        {
            return DTOMappers.MapperProductFromDTO.Map<Product>(Get<ProductDTO>($"{Address}/{id}"));
        }

        public int Add(Product product)
        {
            var response = Post(Address, DTOMappers.MapperProductToDTO.Map<ProductDTO>(product));
            var id = response.Content.ReadFromJsonAsync<int>().Result;
            return id;
        }

        public void Update(Product product)
        {
            var response = Put(Address, DTOMappers.MapperProductToDTO.Map<ProductDTO>(product));
        }

        public bool Delete(int id)
        {
            var result = Delete($"{Address}/{id}").IsSuccessStatusCode;
            return result;
        }
    }
}
