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

        public static List<Person> GetPersons => Enumerable.Range(1, 5).Select(p => new Person
        {
            Id = p,
            FirstName = $"Иван_{p}",
            LastName = $"Иванов_{p + 1}",
            Patronymic = $"Иванович_{p + 2}",
            Age = p + 20,
        }).ToList();
    }
}
