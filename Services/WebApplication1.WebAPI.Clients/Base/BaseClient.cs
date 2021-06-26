using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebApplication1.WebAPI.Clients.Base
{
    public abstract class BaseClient
    {
        protected readonly HttpClient _Client;
        protected readonly string _Address;
        public BaseClient(HttpClient client, string address)
        {
            _Client = client;
            _Address = address;
        }
    }
}
