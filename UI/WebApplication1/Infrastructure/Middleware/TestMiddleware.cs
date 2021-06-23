using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Infrastructure.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _Next;
        private ILogger<TestMiddleware> _logger;

        public TestMiddleware(RequestDelegate next, ILogger<TestMiddleware> logger)
        {
            _logger = logger;
            this._Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //до
            var process = _Next(context);
            //во время
            await process;
            //после
        }
    }
}
