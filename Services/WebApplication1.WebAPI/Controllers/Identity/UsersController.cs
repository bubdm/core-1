using System.Collections.Generic;
using System.Security.Claims;
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
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userStore.Users.ToArrayAsync().ConfigureAwait(false);
        }

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

        #region Claims

        [HttpPost("getclaims")]
        public async Task<IList<Claim>> GetClaimsAsync([FromBody] User user)
        {
            return await _userStore.GetClaimsAsync(user).ConfigureAwait(false);
        }

        [HttpPost("addclaims")]
        public async Task AddClaimsAsync([FromBody] AddClaimDTO claimDto)
        {
            await _userStore.AddClaimsAsync(claimDto.User, claimDto.Claims).ConfigureAwait(false);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("replaceclaim")]
        public async Task ReplaceClaimAsync([FromBody] ReplaceClaimDTO claimDto)
        {
            await _userStore.ReplaceClaimAsync(claimDto.User, claimDto.Claim, claimDto.NewClaim).ConfigureAwait(false);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("removeclaim")]
        public async Task RemoveClaimsAsync([FromBody] RemoveClaimDTO claimDto)
        {
            await _userStore.RemoveClaimsAsync(claimDto.User, claimDto.Claims);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("getusersforclaim")]
        public async Task<IList<User>> GetUsersForClaimAsync([FromBody] Claim claim)
        {
            return await _userStore.GetUsersForClaimAsync(claim).ConfigureAwait(false);
        }

        #endregion

        #region TwoFactor

        [HttpPost("gettwofactorenabled")]
        public async Task<bool> GetTwoFactorEnabledAsync([FromBody] User user)
        {
            return await _userStore.GetTwoFactorEnabledAsync(user).ConfigureAwait(false);
        }

        [HttpPost("settwofactorenabled/{enable}")]
        public async Task<bool> SetTwoFactorEnabledAsync([FromBody] User user, bool enable)
        {
            await _userStore.SetTwoFactorEnabledAsync(user, enable).ConfigureAwait(false);
            await _userStore.UpdateAsync(user);
            return user.TwoFactorEnabled;
        }

        #endregion

        #region Email/Phone

        [HttpPost("getemail")]
        public async Task<string> GetEmailAsync([FromBody] User user)
        {
            return await _userStore.GetEmailAsync(user).ConfigureAwait(false);
        }

        [HttpPost("setemail/{email}")]
        public async Task<string> SetEmailAsync([FromBody] User user, string email)
        {
            await _userStore.SetEmailAsync(user, email).ConfigureAwait(false);
            await _userStore.UpdateAsync(user);
            return user.Email;
        }

        [HttpPost("getemailconfirmed")]
        public async Task<bool> GetEmailConfirmedAsync([FromBody] User user)
        {
            return await _userStore.GetEmailConfirmedAsync(user).ConfigureAwait(false);
        }

        [HttpPost("setrmailconfirmed/{enable:bool}")]
        public async Task<bool> SetEmailConfirmedAsync([FromBody] User user, bool enable)
        {
            await _userStore.SetEmailConfirmedAsync(user, enable).ConfigureAwait(false);
            await _userStore.UpdateAsync(user);
            return user.EmailConfirmed;
        }

        [HttpGet("userfindbyemail/{email}")]
        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userStore.FindByEmailAsync(email).ConfigureAwait(false);
        }

        [HttpPost("getnormalizedemail")]
        public async Task<string> GetNormalizedEmailAsync([FromBody] User user)
        {
            return await _userStore.GetNormalizedEmailAsync(user).ConfigureAwait(false);
        }

        [HttpPost("setnormalizedemail/{email?}")]
        public async Task<string> SetNormalizedEmailAsync([FromBody] User user, string email)
        {
            await _userStore.SetNormalizedEmailAsync(user, email).ConfigureAwait(false);
            await _userStore.UpdateAsync(user);
            return user.NormalizedEmail;
        }

        [HttpPost("getphonenumber")]
        public async Task<string> GetPhoneNumberAsync([FromBody] User user)
        {
            return await _userStore.GetPhoneNumberAsync(user).ConfigureAwait(false);
        }

        [HttpPost("setphonenumber/{phone}")]
        public async Task<string> SetPhoneNumberAsync([FromBody] User user, string phone)
        {
            await _userStore.SetPhoneNumberAsync(user, phone).ConfigureAwait(false);
            await _userStore.UpdateAsync(user);
            return user.PhoneNumber;
        }

        [HttpPost("getphonenumberconfirmed")]
        public async Task<bool> GetPhoneNumberConfirmedAsync([FromBody] User user)
        {
            return await _userStore.GetPhoneNumberConfirmedAsync(user).ConfigureAwait(false);
        }

        [HttpPost("setphonenumberconfirmed/{confirmed:bool}")]
        public async Task<bool> SetPhoneNumberConfirmedAsync([FromBody] User user, bool confirmed)
        {
            await _userStore.SetPhoneNumberConfirmedAsync(user, confirmed);
            await _userStore.UpdateAsync(user);
            return user.PhoneNumberConfirmed;
        }

        #endregion
    }
}
