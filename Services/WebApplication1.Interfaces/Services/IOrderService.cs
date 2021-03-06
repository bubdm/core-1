using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Domain.Entities.Orders;
using WebApplication1.Domain.WebModel;

namespace WebApplication1.Interfaces.Services
{
    public interface IOrderService
    {
        /// <summary> Получить все заказы пользователя </summary>
        /// <param name="userName">Имя пользователя</param> <returns>Заказы</returns>
        Task<IEnumerable<Order>> GetUserOrders(string userName);
        /// <summary> Получить заказ по ид </summary>
        /// <param name="id">Ид заказа</param> <returns>Заказ</returns>
        Task<Order> GetOrderById(int id);
        /// <summary> Создать заказ новый </summary>
        /// <param name="userName">Имя пользователя</param> <param name="cart">корзина</param>
        /// <param name="model">заказ</param> <returns>Заказ</returns>
        Task<Order> CreateOrder(string userName, CartWebModel cart, OrderWebModel model); 
    }
}
