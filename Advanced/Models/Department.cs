using System.Collections.Generic;

namespace Advanced.Models
{
    public class Department
    {
        public long DepartmentId { get; set; }
        public string Name { get; set; }

        public IEnumerable<Person> Persons { get; set; }
    }
}
