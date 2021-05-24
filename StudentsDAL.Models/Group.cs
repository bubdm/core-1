using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StudentsDAL.Models.Base;

namespace StudentsDAL.Models
{
    /// <summary> Группа </summary>
    public class Group : Entity
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
