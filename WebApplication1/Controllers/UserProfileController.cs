﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Domain.Entities.Orders;
using WebApplication1.Services.Interfaces;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly Mapper _mapperOrderToView = 
            new (new MapperConfiguration(c => c.CreateMap<Order, UserOrderViewModel>()
                .ForMember("TotalPrice", o => o
                    .MapFrom(u => u.Items.Sum(i => i.Price)))));
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Orders([FromServices] IOrderService orderService)
        {
            var orders = await orderService.GetUserOrders(User.Identity!.Name);
            return View(_mapperOrderToView.Map<IEnumerable<UserOrderViewModel>>(orders));
        }
    }
}
