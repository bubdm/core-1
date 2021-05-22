using System;
using System.Collections.Generic;
using System.Linq;
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

        public static List<Person> GetPersons => Enumerable.Range(1, 5).Select(p => new Person
        {
            Id = p,
            FirstName = $"Иван_{p}",
            LastName = $"Иванов_{p + 1}",
            Patronymic = $"Иванович_{p + 2}",
            Age = p + 20,
            Birthday = new DateTime(1980 + p, 1, 1),
            CountChildren = p,
        }).ToList();
    }
}
