using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Domain.Model;

namespace WebApplication1.Domain.DTO.Mappers
{
    public static class ProductPageDTOMapper
    {
        public static ProductPageDTO ToDTO(this ProductsPage Page) => new ProductPageDTO {Products = Page.Products.ToDTO(), TotalCount = Page.TotalCount};
        public static ProductsPage FromDTO(this ProductPageDTO Page) => new ProductsPage {Products = Page.Products.FromDTO(), TotalCount = Page.TotalCount};
    }
}
