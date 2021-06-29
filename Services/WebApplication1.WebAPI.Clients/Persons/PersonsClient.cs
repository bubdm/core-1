using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using WebApplication1.Domain.Entities;
using WebApplication1.Interfaces.Adresses;
using WebApplication1.Interfaces.Services;
using WebApplication1.WebAPI.Clients.Base;

namespace WebApplication1.WebAPI.Clients.Persons
{
    public class PersonsClient : BaseSyncClient, IPersonsData
    {
        public PersonsClient(HttpClient client) : base(client, WebAPIInfo.Persons) { }


        public IEnumerable<Person> GetAll()
        {
            return Get<IEnumerable<Person>>(Address);
        }

        public Person Get(int id)
        {
            return Get<Person>($"{Address}/{id}");
        }

        public int Add(Person person)
        {
            var response = Post(Address, person);
            var id = response.Content.ReadFromJsonAsync<int>().Result;
            return id;
        }

        public void Update(Person person)
        {
            Put(Address, person);
        }

        public bool Delete(int id)
        {
            var result = Delete($"{Address}/{id}").IsSuccessStatusCode;
            return result;
        }
    }
}
