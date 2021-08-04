using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.WebModel.Mappers
{
    public static class EditProductWebModelMapper
    {
        public static EditProductWebModel ToWeb(this Product product)
        {
            return product is null
                ? null
                : new EditProductWebModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Order = product.Order,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    SectionId = product.SectionId,
                    SectionName = product.Section.Name,
                    BrandId = product.BrandId,
                    BrandName = product.Brand.Name,
                    Keywords = product.Keywords.Select(k => k.Id).ToList(),
                };
        }
        public static Product FromWeb(this EditProductWebModel product)
        {
            var keywords = (product.Keywords is null)
                ? Array.Empty<Keyword>()
                : product.Keywords?.Select(k => new Keyword {Id = k});
            return product is null
                ? null
                : new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Order = product.Order,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    SectionId = (int)product.SectionId,
                    Section = new Section {Id = (int) product.SectionId},
                    BrandId = (int)product.BrandId,
                    Brand = new Brand {Id = (int) product.BrandId},
                    Keywords = keywords.ToList(),
                };
        }
        public static IEnumerable<EditProductWebModel> ToWeb(this IEnumerable<Product> products) => products.Select(ToWeb);
        public static IEnumerable<Product> FromWeb(this IEnumerable<EditProductWebModel> products) => products.Select(FromWeb);
    }
}
