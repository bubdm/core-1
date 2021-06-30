using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApplication1.Domain.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime DateTime { get; set; }
        public IEnumerable<OrderItemDTO> Items { get; set; }
    }
}
