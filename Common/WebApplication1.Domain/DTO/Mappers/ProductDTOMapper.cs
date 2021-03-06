using System.Collections.Generic;
using System.Linq;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.DTO.Mappers
{
    public static class ProductDTOMapper
    {
        public static ProductDTO ToDTO(this Product product)
        {
            return product is null
                ? null
                : new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Order = product.Order,
                    Brand = product.Brand.ToDTO(),
                    Section = product.Section.ToDTO(),
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                };
        }
        public static Product FromDTO(this ProductDTO product)
        {
            return product is null
                ? null
                : new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Order = product.Order,
                    Brand = product.Brand.FromDTO(),
                    Section = product.Section.FromDTO(),
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                };
        }
        public static IEnumerable<ProductDTO> ToDTO(this IEnumerable<Product> products) => products.Select(ToDTO);
        public static IEnumerable<Product> FromDTO(this IEnumerable<ProductDTO> products) => products.Select(FromDTO);
    }
}