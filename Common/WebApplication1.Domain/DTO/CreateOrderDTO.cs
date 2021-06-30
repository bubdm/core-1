using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Domain.WebModel;

namespace WebApplication1.Domain.DTO
{
    public class CreateOrderDTO
    {
        public OrderWebModel Order { get; set; }
        public IEnumerable<OrderItemDTO> Items { get; set; }
    }
}
