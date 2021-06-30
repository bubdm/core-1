using System.Collections.Generic;
using System.Linq;
using WebApplication1.Domain.Entities.Orders;

namespace WebApplication1.Domain.DTO.Mappers
{
    public static class OrderDTOMapper
    {
        public static OrderDTO ToDTO(this Order order)
        {
            return order is null
                ? null
                : new OrderDTO
                {
                    Id = order.Id,
                    Name = order.Name,
                    Address = order.Address,
                    Phone = order.Phone,
                    DateTime = order.DateTime,
                    Items = order.Items.Select(OrderItemDTOMapper.ToDTO),
                };
        }
        public static Order FromDTO(this OrderDTO order)
        {
            return order is null
                ? null
                : new Order
                {
                    Id = order.Id,
                    Name = order.Name,
                    Address = order.Address,
                    Phone = order.Phone,
                    DateTime = order.DateTime,
                    Items = (ICollection<OrderItem>) order.Items.Select(OrderItemDTOMapper.FromDTO),
                };
        }
        public static IEnumerable<OrderDTO> ToDTO(this IEnumerable<Order> orders) => orders.Select(ToDTO);
        public static IEnumerable<Order> FromDTO(this IEnumerable<OrderDTO> orders) => orders.Select(FromDTO);
    }
}