using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Domain.Identity;
using WebApplication1.Domain.WebModel;

namespace WebApplication1.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterUserWebModel());

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserWebModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            #region Лог
            _logger.LogInformation($"Регистрация нового пользователя {model.UserName}");
            #endregion
            var user = new User
            {
                UserName = model.UserName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                #region Log

                _logger.LogInformation($"Пользователь {user.UserName} успешно добавлен");

                #endregion
                var resultAdd = await _userManager.AddToRoleAsync(user, Role.Clients);
                #region Log

                _logger.LogInformation($"Пользователь {user.UserName} наделен ролью {Role.Clients}");

                #endregion
                await _signInManager.SignInAsync(user, false);
                #region Log

                _logger.LogInformation($"Пользователь {user.UserName} автоматически вошел после регистрации");

                #endregion
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors) 
                ModelState.AddModelError("", error.Description);
            #region Лог
            _logger.LogWarning($"При регистрации пользователя {model.UserName} возникли ошибки: {string.Join(",", result.Errors.Select(e => e.Description))}");
            #endregion
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl) => 
            View(new LoginWebModel {ReturnUrl = returnUrl});

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> Login(LoginWebModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            #region Лог
            _logger.LogInformation($"Вход пользователя в систему {model.UserName}");
            #endregion
            var result =
                await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe,
#if DEBUG
                    false
#else
                    true
#endif
                );

            if (result.Succeeded)
            {
                return LocalRedirect(model.ReturnUrl ?? "/");
            }

            ModelState.AddModelError("", "Ошибка в имени пользователя, либо в пароле");
            #region Лог
            _logger.LogInformation($"Ошибка в имени пользователя, либо в пароле");
            #endregion
            return View();
        }

        public async Task<IActionResult> Logout(string returnUrl)
        {
            var username = User.Identity!.Name;
            await _signInManager.SignOutAsync();
            #region Log

            _logger.LogInformation($"Пользователь {username} вышел из системы");

            #endregion
            return LocalRedirect(returnUrl ?? "/"); 
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            #region Log
            _logger.LogInformation("В доступе отказано");
            #endregion
            return View();
        }
    }
}
