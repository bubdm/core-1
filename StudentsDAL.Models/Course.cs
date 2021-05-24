using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StudentsDAL.Models.Base;

namespace StudentsDAL.Models
{
    /// <summary> Предмет </summary>
    public class Course : Entity
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
