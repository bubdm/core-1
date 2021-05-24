using System.ComponentModel.DataAnnotations;

namespace StudentsDAL.Models.MetaData
{
    public class StudentMetaData
    {
        [Display(Name = "Pet Name")]
        public string Pet;
        [StringLength(100, ErrorMessage = "Вводите фамилию длинной меньше 100 символов")]
        public string LastName;
        [StringLength(100, ErrorMessage = "Вводите имя длинной меньше 100 символов")]
        public string FirstName;
        [StringLength(100, ErrorMessage = "Вводите отчество длинной меньше 100 символов")]
        public string Patronymic;
    }
}
