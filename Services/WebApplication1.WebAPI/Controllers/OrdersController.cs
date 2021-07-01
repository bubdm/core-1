using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.DTO;
using WebApplication1.Domain.DTO.Mappers;
using WebApplication1.Interfaces.Adresses;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.WebAPI.Controllers
{
    [Route(WebAPIInfo.Orders), ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _OrderService;
        public OrdersController(IOrderService orderService)
        {
            _OrderService = orderService;
        }

        [HttpGet("user/{userName}")]
        public async Task<IActionResult> GetUserOrders(string userName)
        {
            var orders = await _OrderService.GetUserOrders(userName);
            return Ok(orders.ToDTO());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _OrderService.GetOrderById(id);
            return Ok(order.ToDTO());
        }

        [HttpPost("{userName}")]
        public async Task<IActionResult> CreateOrder(string userName, [FromBody] CreateOrderDTO orderModel)
        {
            var order = await _OrderService.CreateOrder(userName, orderModel.Items.FromDTO(), orderModel.Order);
            return Ok(order.ToDTO());
        }
    }
}
