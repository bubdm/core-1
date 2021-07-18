using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain.DTO;
using WebApplication1.Domain.DTO.Mappers;
using WebApplication1.Interfaces.Adresses;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.WebAPI.Controllers
{
    /// <summary> Управление заказами </summary>
    [Route(WebAPIInfo.Orders), ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _OrderService;
        public OrdersController(IOrderService orderService)
        {
            _OrderService = orderService;
        }

        /// <summary> Получение всех заказов указанного имени пользователя </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns>Список заказов</returns>
        [HttpGet("user/{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDTO>))]
        public async Task<IActionResult> GetUserOrders(string userName)
        {
            var orders = await _OrderService.GetUserOrders(userName);
            return Ok(orders.ToDTO());
        }

        /// <summary> Получение заказа по идентификатору </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Заказ</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDTO))]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _OrderService.GetOrderById(id);
            return Ok(order.ToDTO());
        }

        /// <summary> Создание заказа </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="orderModel">Модель заказа</param>
        /// <returns>Новый заказ</returns>
        [HttpPost("{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDTO))]
        public async Task<IActionResult> CreateOrder(string userName, [FromBody] CreateOrderDTO orderModel)
        {
            var order = await _OrderService.CreateOrder(userName, orderModel.Items.FromDTO(), orderModel.Order);
            return Ok(order.ToDTO());
        }
    }
}
