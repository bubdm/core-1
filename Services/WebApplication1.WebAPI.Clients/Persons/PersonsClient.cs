using System.Collections.Generic;
using System.Net.Http;
using WebApplication1.Domain.Entities;
using WebApplication1.Interfaces.Services;
using WebApplication1.WebAPI.Clients.Base;

namespace WebApplication1.WebAPI.Clients.Persons
{
    public class PersonsClient : BaseSyncClient, IPersonsData
    {
        public PersonsClient(HttpClient client) : base(client, "api/persons") { }


        public IEnumerable<Person> GetAll()
        {
            
        }

        public Person Get(int id)
        {
            
        }

        public int Add(Person person)
        {
            
        }

        public void Update(Person person)
        {
            
        }

        public bool Delete(int id)
        {
            
        }
    }
}
