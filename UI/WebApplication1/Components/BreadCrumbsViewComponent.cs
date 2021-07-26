using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Domain.WebModel;

namespace WebApplication1.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new BreadCrumbsWebModel();

            return View(model);
        }
    }
}
