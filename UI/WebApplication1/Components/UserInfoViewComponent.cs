using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Components
{
    public class UserInfoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            if (User.Identity?.IsAuthenticated == true)
                return View("UserInfo");
            else
                return View();
        }
    }
}
