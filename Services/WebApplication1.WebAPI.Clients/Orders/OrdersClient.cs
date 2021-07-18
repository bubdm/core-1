using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebApplication1.Domain.DTO;
using WebApplication1.Domain.DTO.Mappers;
using WebApplication1.Domain.Entities.Orders;
using WebApplication1.Domain.WebModel;
using WebApplication1.Interfaces.Adresses;
using WebApplication1.Interfaces.Services;
using WebApplication1.WebAPI.Clients.Base;

namespace WebApplication1.WebAPI.Clients.Orders
{
    public class OrdersClient : BaseSyncClient, IOrderService
    {
        public OrdersClient(HttpClient client) : base(client, WebAPIInfo.Orders) { }

        public async Task<IEnumerable<Order>> GetUserOrders(string userName)
        {
            var orders = await GetAsync<IEnumerable<OrderDTO>>($"{Address}/user/{userName}").ConfigureAwait(false);
            return orders.FromDTO();
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await GetAsync<OrderDTO>($"{Address}/{id}").ConfigureAwait(false);
            return order.FromDTO();
        }

        public async Task<Order> CreateOrder(string userName, CartWebModel cart, OrderWebModel orderModel)
        {
            var model = new CreateOrderDTO
            {
                Order = orderModel,
                Items = cart.ToDTO(),
            };
            var response = await PostAsync($"{Address}/{userName}", model).ConfigureAwait(false);
            var order = await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<OrderDTO>().ConfigureAwait(false);
            return order.FromDTO();
        }
    }
}
