using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Domain.Entities.Orders;
using WebApplication1.ViewModel;

namespace WebApplication1.Services.Interfaces
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
        /// <param name="UserName">Имя пользователя</param> <param name="cart">корзина</param>
        /// <param name="model">заказ</param> <returns>Заказ</returns>
        Task<Order> CreateOrder(string UserName, CartViewModel cart, OrderViewModel model); 
    }
}
