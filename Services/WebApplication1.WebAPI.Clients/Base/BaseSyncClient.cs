using System.Net.Http;

namespace WebApplication1.WebAPI.Clients.Base
{
    public class BaseSyncClient : BaseClient
    {
        public BaseSyncClient(HttpClient client, string address) : base(client, address) { }
        protected T Get<T>(string url) => GetAsync<T>(url).Result;
        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync(url, item).Result;
        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync(url, item).Result;
        protected HttpResponseMessage Delete<T>(string url) => DeleteAsync(url).Result;
    }
}
