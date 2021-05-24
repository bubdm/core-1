using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudentsDAL.Models.MetaData;

namespace StudentsDAL.Models
{
    [MetadataType(typeof(StudentMetaData))]
    public partial class Student
    {
        public override string ToString()
        {
            return $"{FullName} питомец: {this.Pet ?? "Без питомца"}";
        }
        [NotMapped]
        public string FullName => $"{LastName} {FirstName} {Patronymic}";
    }
}
