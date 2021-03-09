using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ConsoleAppTest.Entities.Base;

namespace ConsoleAppTest.Entities
{
    public class Student : Entity
    {
        [MaxLength(150), Required]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public DateTime BirthDay { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
