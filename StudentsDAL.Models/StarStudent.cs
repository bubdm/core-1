using System.ComponentModel.DataAnnotations.Schema;

namespace StudentsDAL.Models
{
    [Table("StarStudents")]
    public class StarStudent : Student
    {
        public int Star { get; set; }
        public override string ToString()
        {
            return $"Особый студент {FullName} питомец: {this.Pet ?? "Без питомца"} звезность: {Star}";
        }
    }
}
