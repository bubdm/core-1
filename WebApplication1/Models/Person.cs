using System;
using WebApplication1.Models.Base;

namespace WebApplication1.Models
{
    public class Person : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public int CountChildren { get; set; }
    }
}
