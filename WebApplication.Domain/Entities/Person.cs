using System;
using WebApplication.Domain.Entities.Base;

namespace WebApplication.Domain.Entities
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
