using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SQLitePCL;
using WebApplication1.Dal.Context;
using WebApplication1.Domain.DTO.Identity;
using WebApplication1.Domain.Identity;
using WebApplication1.Interfaces.Adresses;

namespace WebApplication1.WebAPI.Controllers.Identity
{
    [Route(WebAPIInfo.Identity.Users), ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UserStore<User, Role, Application1Context> _userStore;
        public UsersController(Application1Context context, ILogger<UsersController> logger)
        {
            _logger = logger;
            _userStore = new UserStore<User, Role, Application1Context>(context);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<User>> GetAllUsers() => 
            await _userStore.Users.ToArrayAsync();

        #region Users

        [HttpPost("userid")]
        public async Task<string> GetUserIdAsync([FromBody] User user)
        {
            return await _userStore.GetUserIdAsync(user).ConfigureAwait(false);
        }

        [HttpPost("username")]
        public async Task<string> GetUserNameAsync([FromBody] User user)
        {
            return await _userStore.GetUserNameAsync(user).ConfigureAwait(false);
        }

        [HttpPost("username/{name}")]
        public async Task<string> SetUserNameAsync([FromBody] User user, string name)
        {
            await _userStore.SetUserNameAsync(user, name).ConfigureAwait(false);
            await _userStore.UpdateAsync(user);
            if (!string.Equals(user.UserName, name)) _logger.LogError($"Изменение имени пользователя {user.UserName} на {name} завершилось неудачей");
            return user.UserName;
        }

        [HttpPost("normalizedusername")]
        public async Task<string> GetNormalizedUserNameAsync([FromBody] User user)
        {
            return await _userStore.GetNormalizedEmailAsync(user).ConfigureAwait(false);
        }

        [HttpPost("normalizedusername/{name}")]
        public async Task<string> SenNormalizedUserNameAsync([FromBody] User user, string name)
        {
            await _userStore.SetNormalizedUserNameAsync(user, name).ConfigureAwait(false);
            await _userStore.UpdateAsync(user);
            if (!string.Equals(user.NormalizedUserName, name)) _logger.LogError($"Изменение имени пользователя {user.NormalizedUserName} на {name} завершилось неудачей");
            return user.NormalizedUserName;
        }

        [HttpPost("user")]
        public async Task<bool> CreateAsync([FromBody] User user)
        {
            var result = await _userStore.CreateAsync(user).ConfigureAwait(false);
            if (result.Succeeded == false) _logger.LogError($"Создание пользователя {user.UserName} завершилось неудачей");
            return result.Succeeded;
        }

        [HttpPut("user")]
        public async Task<bool> UpdateAsync([FromBody] User user)
        {
            var result = await _userStore.UpdateAsync(user).ConfigureAwait(false);
            if (result.Succeeded == false) _logger.LogError($"Изменение пользователя {user.UserName} завершилось неудачей");
            return result.Succeeded;
        }

        [HttpPost("user/delete")]
        public async Task<bool> DeleteAsync([FromBody] User user)
        {
            var result = await _userStore.DeleteAsync(user).ConfigureAwait(false);
            if (result.Succeeded == false) _logger.LogError($"Удаление пользователя {user.UserName} завершилось неудачей");
            return result.Succeeded;
        }

        [HttpGet("user/find/{id}")]
        public async Task<User> FindByIdAsync(string id)
        {
            return await _userStore.FindByIdAsync(id).ConfigureAwait(false);
        }

        [HttpGet("user/name/{name}")]
        public async Task<User> FindByNameAsync(string name)
        {
            return await _userStore.FindByNameAsync(name).ConfigureAwait(false);
        }

        [HttpPost("role/{role}")]
        public async Task AddToRoleAsync([FromBody] User user, string role)
        {
            await _userStore.AddToRoleAsync(user, role).ConfigureAwait(false);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("role/{role}")]
        public async Task RemoveFromRoleAsync([FromBody] User user, string role)
        {
            await _userStore.RemoveFromRoleAsync(user, role).ConfigureAwait(false);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("roles")]
        public async Task<IList<string>> GetRolesAsync([FromBody] User user)
        {
            return await _userStore.GetRolesAsync(user).ConfigureAwait(false);
        }

        [HttpPost("inrole/{role}")]
        public async Task<bool> IsInRoleAsync([FromBody] User user, string role)
        {
            return await _userStore.IsInRoleAsync(user, role).ConfigureAwait(false);
        }

        [HttpGet("usersinrole/{role}")]
        public async Task<IList<User>> GetUsersInRoleAsync(string role)
        {
            return await _userStore.GetUsersInRoleAsync(role).ConfigureAwait(false);
        }

        [HttpPost("getpasswordhash")]
        public async Task<string> GetPasswordHashAsync([FromBody] User user)
        {
            return await _userStore.GetPasswordHashAsync(user).ConfigureAwait(false);
        }

        [HttpPost("setpasswordhash")]
        public async Task<string> SetPasswordHashAsync([FromBody] PasswordHashDTO hash)
        {
            await _userStore.SetPasswordHashAsync(hash.User, hash.Hash).ConfigureAwait(false);
            await _userStore.UpdateAsync(hash.User);
            if (!string.Equals(hash.User.PasswordHash, hash.Hash)) _logger.LogError($"Изменение пароля у пользователя {hash.User.UserName} завершилось неудачей");
            return hash.User.PasswordHash;
        }

        [HttpPost("haspassword")]
        public async Task<bool> HasPasswordAsync([FromBody] User user)
        {
            return await _userStore.HasPasswordAsync(user);
        }

        #endregion

    }
}
