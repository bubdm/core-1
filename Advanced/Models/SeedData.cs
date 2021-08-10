using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Advanced.Models
{
    public static class SeedData
    {
        public static void SeedDatabase(DataContext context)
        {
            context.Database.Migrate();
            if (!context.Persons.Any() && !context.Departments.Any() && !context.Locations.Any())
            {
                var d1 = new Department {Name = "Продажа"};
                var d2 = new Department {Name = "Разработка"};
                var d3 = new Department {Name = "Поддержка"};
                var d4 = new Department {Name = "Обслуживание"};
                context.Departments.AddRange(d1, d2, d3, d4);
                context.SaveChanges();

                var l1 = new Location {City = "Ульяновск", State = "Ульяновская область"};
                var l2 = new Location {City = "Самара", State = "Самарская область"};
                var l3 = new Location {City = "Пенза", State = "Пензенская область"};
                context.Locations.AddRange(l1, l2, l3);
                context.SaveChanges();

                context.Persons.AddRange
                (
                    new Person{FirstName = "Вася", SurName = "Пупкин", Department = d1, Location = l1},
                    new Person{FirstName = "Иван", SurName = "Иванов", Department = d1, Location = l2},
                    new Person{FirstName = "Петр", SurName = "Петров", Department = d2, Location = l3},
                    new Person{FirstName = "Андрей", SurName = "Андреев", Department = d2, Location = l1},
                    new Person{FirstName = "Михаил", SurName = "Михайлов", Department = d3, Location = l2},
                    new Person{FirstName = "Леонид", SurName = "Леонидов", Department = d3, Location = l3},
                    new Person{FirstName = "Агафон", SurName = "Агафонов", Department = d4, Location = l1},
                    new Person{FirstName = "Зоя", SurName = "Зоева", Department = d4, Location = l1},
                    new Person{FirstName = "Антонина", SurName = "Тонидова", Department = d3, Location = l2}
                );
                context.SaveChanges();
            }
        }
    }
}
