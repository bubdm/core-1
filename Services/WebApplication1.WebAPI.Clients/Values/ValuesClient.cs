using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using WebApplication1.Interfaces.WebAPI;
using WebApplication1.WebAPI.Clients.Base;

namespace WebApplication1.WebAPI.Clients.Values
{
    public class ValuesClient : BaseClient, IValuesService
    {
        public ValuesClient(HttpClient client) : base(client, "api/values") { }
        
        public IEnumerable<string> GetAll()
        {
            var response = _Client.GetAsync(_Address).Result;
            if (response.IsSuccessStatusCode)
                return response.Content
                    .ReadFromJsonAsync<IEnumerable<string>>()
                    .Result;
            return Enumerable.Empty<string>();
        }

        public string GetById(int id)
        {
            var response = _Client.GetAsync($"{_Address}/{id}").Result;
            if (response.IsSuccessStatusCode)
                return response.Content
                    .ReadFromJsonAsync<string>()
                    .Result;
            return default;
        }

        public void Add(string str)
        {
            var response = _Client.PostAsJsonAsync(_Address, str).Result;
            response.EnsureSuccessStatusCode();
        }

        public void Edit(int id, string str)
        {
            var response = _Client.PutAsJsonAsync($"{_Address}/{id}", str).Result;
            response.EnsureSuccessStatusCode();
        }

        public bool Delete(int id)
        {
            var response = _Client.DeleteAsync($"{_Address}/{id}").Result;
            return response.IsSuccessStatusCode;
        }
    }
}
