using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication.Domain.Entities.Base;
using WebApplication.Domain.Identity;

namespace WebApplication.Domain.Entities.Orders
{
    public class Order : NamedEntity
    {
        [Required]
        public User User { get; set; }

        [Required, MaxLength(50)]
        public string Phone { get; set; }

        [Required, MaxLength(600)]
        public string Address { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
