using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1.WebAPI.Clients.Base
{
    public abstract class BaseClient : IDisposable
    {
        protected readonly HttpClient _client;
        protected readonly string Address;
        public BaseClient(HttpClient client, string address)
        {
            _client = client;
            Address = address;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;
            if (disposing)
            {
                //уничтожить управляемые ресурсы
            }
            //уничтожить неуправляемые ресурсы
        }

        protected async Task<T> GetAsync<T>(string url, CancellationToken cancel = default)
        {
            var response = await _client
                .GetAsync(url, cancel).ConfigureAwait(false);
            return await response
                .EnsureSuccessStatusCode()
                .Content.ReadFromJsonAsync<T>(cancellationToken:cancel);
        }
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken cancel = default)
        {
            var response = await _client
                .PostAsJsonAsync(url, item, cancel).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken cancel = default)
        {
            var response = await _client
                .PutAsJsonAsync(url, item, cancel).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }
        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancel = default)
        {
            var response = await _client
                .DeleteAsync(url, cancel).ConfigureAwait(false);
            return response;
        }
    }
}
