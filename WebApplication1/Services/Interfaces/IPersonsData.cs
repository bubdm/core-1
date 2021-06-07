using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Services.Interfaces
{
    public interface IPersonsData
    {
        IEnumerable<Person> GetAll();
        Person Get(int id);
        int Add(Person person);
        void Update(Person person);
        bool Delete(int id);
    }
}
