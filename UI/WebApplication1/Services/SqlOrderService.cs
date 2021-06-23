using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication1.Domain.Entities.Orders;
using WebApplication1.Domain.Identity;
using WebApplication1.Dal.Context;
using WebApplication1.Services.Interfaces;
using WebApplication1.ViewModel;

namespace WebApplication1.Services
{
    public class SqlOrderService : IOrderService
    {
        private readonly Application1Context _Context;
        private readonly UserManager<User> _UserManager;
        private readonly ILogger<SqlOrderService> _Logger;
        public SqlOrderService(Application1Context context, UserManager<User> userManager, ILogger<SqlOrderService> logger)
        {
            _Context = context;
            _UserManager = userManager;
            _Logger = logger;
        }
        public async Task<IEnumerable<Order>> GetUserOrders(string userName)
        {
            return await _Context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .Where(o => o.User.UserName == userName)
                .ToArrayAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _Context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel model)
        {
            var user = await _UserManager.FindByNameAsync(userName);
            if (user is null)
            {
                _Logger.LogInformation($"Пользователь {userName} отсустсвует в базе данных");
                throw new InvalidOperationException($"Пользователь {userName} отсутствует в базе данных");
            }


            _Logger.LogInformation("Начало формирования заказа");

            await using var transaction = await _Context.Database.BeginTransactionAsync();
            var order = new Order
            {
                User = user,
                Name = model.Name,
                Phone = model.Phone,
                Address = model.Address
            };
            var ids = cart.Items
                .Select(i => i.Product.Id).ToArray();
            var cartProducts = await _Context.Products
                .Where(p => ids.Contains(p.Id)).ToArrayAsync();
            order.Items = cart.Items.Join(cartProducts, i => i.Product.Id, p => p.Id, (i, p) => new OrderItem
            {
                Order = order,
                Product = p,
                Price = p.Price,
                Quantity = i.Quantity
            }).ToArray();

            _Logger.LogInformation("Заказ успешно сформирован");
            
            await _Context.Orders.AddAsync(order);
            await _Context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            _Logger.LogInformation("Заказ успешно добавлен в базу данных");

            return order;
        }
    }
}
