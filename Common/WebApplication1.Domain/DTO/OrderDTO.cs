using System;
using System.Collections.Generic;

namespace WebApplication1.Domain.DTO
{
    /// <summary> Заказ </summary>
    public class OrderDTO
    {
        /// <summary> Идентификатор </summary>
        public int Id { get; set; }
        /// <summary> Название </summary>
        public string Name { get; set; }
        /// <summary> Телефон </summary>
        public string Phone { get; set; }
        /// <summary> Адрес </summary>
        public string Address { get; set; }
        /// <summary> Датавремя заказа </summary>
        public DateTime DateTime { get; set; }
        /// <summary> Элементы заказа </summary>
        public IEnumerable<OrderItemDTO> Items { get; set; }
    }
}
