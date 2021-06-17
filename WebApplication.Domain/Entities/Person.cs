using System;
using System.ComponentModel.DataAnnotations;
using WebApplication.Domain.Entities.Base;

namespace WebApplication.Domain.Entities
{
    public class Person : Entity
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string Patronymic { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; } = new DateTime();
        public int CountChildren { get; set; }
    }
}
