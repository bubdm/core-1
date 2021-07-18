using System.Collections.Generic;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Interfaces.Services
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
