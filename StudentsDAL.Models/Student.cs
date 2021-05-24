using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StudentsDAL.Models.Base;

namespace StudentsDAL.Models
{
    /// <summary> Студент </summary>
    public partial class Student : Entity
    {
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string Patronymic { get; set; }
        public DateTime Birthday { get; set; }
        public float Rating { get; set; }
        [MaxLength(50)]
        public string Pet { get; set; }
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        public Group Group { get; set; }
    }
}
