using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Domain.WebModel.AjaxTest;

namespace WebApplication1.Controllers
{
    public class AjaxTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetHTML(int? id, string msg, int Delay = 1000)
        {
            await Task.Delay(Delay);

            return PartialView("Partial/_TestDataViewPartial", 
                new AjaxTestDataWebModel
                {
                    Id = id ?? 1,
                    Message = msg,
                });
        }

        public async Task<IActionResult> GetJSON(int? id, string msg, int Delay = 1000)
        {
            await Task.Delay(Delay);

            return Json(new
            {
                Id = id ?? 1,
                Message = $"Response (id:{id ?? 1}): {msg ?? "<null>"}",
                ServerTime = DateTime.Now
            });
        }
    }
}
