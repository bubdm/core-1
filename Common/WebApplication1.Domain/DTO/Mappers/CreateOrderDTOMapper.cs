using System.Collections.Generic;
using System.Linq;
using WebApplication1.Domain.WebModel;

namespace WebApplication1.Domain.DTO.Mappers
{
    public static class CreateOrderDTOMapper
    {
        public static IEnumerable<OrderItemDTO> ToDTO(this CartWebModel cart)
        {
            return cart.Items.Select(p => new OrderItemDTO
            {
                ProductId = p.Product.Id,
                Price = p.Product.Price,
                Quantity = p.Quantity,
            });
        }
        public static CartWebModel FromDTO(this IEnumerable<OrderItemDTO> items)
        {
            return new CartWebModel()
            {
                Items = items.Select(p => (new ProductWebModel{Id = p.ProductId}, p.Quantity )),
            };
        }
    }
}