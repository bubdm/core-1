using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ConsoleAppTest.Entities.Base;

namespace ConsoleAppTest.Entities
{
    public class Course : Entity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
