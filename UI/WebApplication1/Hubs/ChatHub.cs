using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WebApplication1.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string Message) => 
            await Clients.All.SendAsync("MessageFromClient", Message);
    }
}
