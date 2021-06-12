using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Domain.Identity;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation($"Регистрация нового пользователя {model.UserName}");

            var user = new User
            {
                UserName = model.UserName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                _logger.LogInformation($"Пользователь успешно зареган {model.UserName}");

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors) 
                ModelState.AddModelError("", error.Description);

            _logger.LogWarning($"При регистрации пользователя {model.UserName} возникли ошибки: {string.Join(",", result.Errors.Select(e => e.Description))}");

            return View();
        }

        public IActionResult Login(string returnUrl) => View(new LoginViewModel {ReturnUrl = returnUrl});

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result =
                await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe,
#if DEBUG
                    false
#else
                    true
#endif
                );

            if (result.Succeeded)
                return LocalRedirect(model.ReturnUrl);

            ModelState.AddModelError("", "Ошибка в имени пользователя, либо в пароле");

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
