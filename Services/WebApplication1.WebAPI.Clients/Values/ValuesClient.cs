﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;
using WebApplication1.Interfaces.WebAPI;
using WebApplication1.WebAPI.Clients.Base;

namespace WebApplication1.WebAPI.Clients.Values
{
    public class ValuesClient : BaseClient, IValuesService
    {
        private AsyncRetryPolicy _policy = Policy
            .Handle<HttpRequestException>()
            .RetryAsync(3);
        public ValuesClient(HttpClient client) : base(client, "api/values") { }
        
        public IEnumerable<string> GetAll()
        {
            var response = _policy.ExecuteAsync(async () => 
                await _Client.GetAsync(_Address)).Result;
            if (response.IsSuccessStatusCode)
                return response.Content
                    .ReadFromJsonAsync<IEnumerable<string>>()
                    .Result;
            return Enumerable.Empty<string>();
        }

        public string GetById(int id)
        {
            var response = _policy.ExecuteAsync(async () => 
                await _Client.GetAsync($"{_Address}/{id}")).Result;
            if (response.IsSuccessStatusCode)
                return response.Content
                    .ReadFromJsonAsync<string>()
                    .Result;
            return default;
        }

        public void Add(string str)
        {
            var response = _Client.GetAsync($"{_Address}/add?str={str}").Result;
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
