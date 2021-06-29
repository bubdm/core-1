using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebApplication1.WebAPI.Clients.Base
{
    public abstract class BaseClient
    {
        protected readonly HttpClient _client;
        protected readonly string Address;
        public BaseClient(HttpClient client, string address)
        {
            _client = client;
            Address = address;
        }
        protected async Task<T> GetAsync<T>(string url)
        {
            var response = await _client.GetAsync(url).ConfigureAwait(false);
            return await response
                .EnsureSuccessStatusCode().Content.ReadFromJsonAsync<T>().ConfigureAwait(false);
        }
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item)
        {
            var response = await _client.PostAsJsonAsync(url, item).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item)
        {
            var response = await _client.PutAsJsonAsync(url, item).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }
        protected async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var response = await _client.DeleteAsync(url).ConfigureAwait(false);
            return response;
        }
    }
}
