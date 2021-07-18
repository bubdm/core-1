using System.Collections.Generic;
using WebApplication1.Domain.WebModel;

namespace WebApplication1.Domain.DTO
{
    /// <summary> Информация о создаваемом заказе </summary>
    public class CreateOrderDTO
    {
        /// <summary> Заказ </summary>
        public OrderWebModel Order { get; set; }
        /// <summary> Элементы заказа </summary>
        public IEnumerable<OrderItemDTO> Items { get; set; }
    }
}
