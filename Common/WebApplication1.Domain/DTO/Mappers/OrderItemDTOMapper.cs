using System.Linq;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Entities.Orders;

namespace WebApplication1.Domain.DTO.Mappers
{
    public static class OrderItemDTOMapper
    {
        public static OrderItemDTO ToDTO(this OrderItem item)
        {
            return item is null
                ? null
                : new OrderItemDTO
                {
                    Id = item.Id,
                    ProductId = item.Product.Id,
                    Price = item.Price,
                    Quantity = item.Quantity,
                };
        }
        public static OrderItem FromDTO(this OrderItemDTO item)
        {
            return item is null
                ? null
                : new OrderItem
                {
                    Id = item.Id,
                    Product = new Product {Id = item.Id},
                    Price = item.Price,
                    Quantity = item.Quantity,
                };
        }
    }
}