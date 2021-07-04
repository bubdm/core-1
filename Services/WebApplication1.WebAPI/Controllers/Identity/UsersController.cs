using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
            return await _userStore.Users.ToArrayAsync();
        }

        #region Users

        [HttpPost("userid")]
        public async Task<string> GetUserIdAsync([FromBody] User user)
        {
            return await _userStore.GetUserIdAsync(user);
        }

        [HttpPost("username")]
        public async Task<string> GetUserNameAsync([FromBody] User user)
        {
            return await _userStore.GetUserNameAsync(user);
        }

        [HttpPost("username/{name}")]
        public async Task<string> SetUserNameAsync([FromBody] User user, string name)
        {
            await _userStore.SetUserNameAsync(user, name);
            await _userStore.UpdateAsync(user);
            if (!string.Equals(user.UserName, name)) _logger.LogError($"Изменение имени пользователя {user.UserName} на {name} завершилось неудачей");
            return user.UserName;
        }

        [HttpPost("normalizedusername")]
        public async Task<string> GetNormalizedUserNameAsync([FromBody] User user)
        {;
            return await _userStore.GetNormalizedEmailAsync(user);
        }

        [HttpPost("normalizedusername/{name}")]
        public async Task<string> SenNormalizedUserNameAsync([FromBody] User user, string name)
        {
            await _userStore.SetNormalizedUserNameAsync(user, name);
            await _userStore.UpdateAsync(user);
            if (!string.Equals(user.NormalizedUserName, name)) _logger.LogError($"Изменение имени пользователя {user.NormalizedUserName} на {name} завершилось неудачей");
            return user.NormalizedUserName;
        }

        [HttpPost("user")]
        public async Task<bool> CreateAsync([FromBody] User user)
        {
            var result = await _userStore.CreateAsync(user);
            if (result.Succeeded == false) _logger.LogError($"Создание пользователя {user.UserName} завершилось неудачей");
            return result.Succeeded;
        }

        [HttpPut("user")]
        public async Task<bool> UpdateAsync([FromBody] User user)
        {
            var result = await _userStore.UpdateAsync(user);
            if (result.Succeeded == false) _logger.LogError($"Изменение пользователя {user.UserName} завершилось неудачей");
            return result.Succeeded;
        }

        [HttpPost("user/delete")]
        public async Task<bool> DeleteAsync([FromBody] User user)
        {
            var result = await _userStore.DeleteAsync(user);
            if (result.Succeeded == false) _logger.LogError($"Удаление пользователя {user.UserName} завершилось неудачей");
            return result.Succeeded;
        }

        [HttpGet("user/find/{id}")]
        public async Task<User> FindByIdAsync(string id)
        {
            return await _userStore.FindByIdAsync(id);
        }

        [HttpGet("user/name/{name}")]
        public async Task<User> FindByNameAsync(string name)
        {
            return await _userStore.FindByNameAsync(name);
        }

        [HttpPost("role/{role}")]
        public async Task AddToRoleAsync([FromBody] User user, string role)
        {
            await _userStore.AddToRoleAsync(user, role);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("role/delete/{role}")]
        public async Task RemoveFromRoleAsync([FromBody] User user, string role)
        {
            await _userStore.RemoveFromRoleAsync(user, role);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("roles")]
        public async Task<IList<string>> GetRolesAsync([FromBody] User user)
        {
            return await _userStore.GetRolesAsync(user);
        }

        [HttpPost("inrole/{role}")]
        public async Task<bool> IsInRoleAsync([FromBody] User user, string role)
        {
            return await _userStore.IsInRoleAsync(user, role);
        }

        [HttpGet("usersinrole/{role}")]
        public async Task<IList<User>> GetUsersInRoleAsync(string role)
        {
            return await _userStore.GetUsersInRoleAsync(role);
        }

        [HttpPost("setpasswordhash")]
        public async Task<string> SetPasswordHashAsync([FromBody] PasswordHashDTO hash)
        {
            await _userStore.SetPasswordHashAsync(hash.User, hash.Hash);
            await _userStore.UpdateAsync(hash.User);
            if (!string.Equals(hash.User.PasswordHash, hash.Hash)) _logger.LogError($"Изменение пароля у пользователя {hash.User.UserName} завершилось неудачей");
            return hash.User.PasswordHash;
        }

        [HttpPost("getpasswordhash")]
        public async Task<string> GetPasswordHashAsync([FromBody] User user)
        {
            return await _userStore.GetPasswordHashAsync(user);
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
            return await _userStore.GetClaimsAsync(user);
        }

        [HttpPost("addclaims")]
        public async Task AddClaimsAsync([FromBody] AddClaimDTO claimDto)
        {
            await _userStore.AddClaimsAsync(claimDto.User, claimDto.Claims);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("replaceclaim")]
        public async Task ReplaceClaimAsync([FromBody] ReplaceClaimDTO claimDto)
        {
            await _userStore.ReplaceClaimAsync(claimDto.User, claimDto.Claim, claimDto.NewClaim);
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
            return await _userStore.GetUsersForClaimAsync(claim);
        }

        #endregion

        #region TwoFactor

        [HttpPost("gettwofactorenabled")]
        public async Task<bool> GetTwoFactorEnabledAsync([FromBody] User user)
        {
            return await _userStore.GetTwoFactorEnabledAsync(user);
        }

        [HttpPost("settwofactorenabled/{enable}")]
        public async Task<bool> SetTwoFactorEnabledAsync([FromBody] User user, bool enable)
        {
            await _userStore.SetTwoFactorEnabledAsync(user, enable);
            await _userStore.UpdateAsync(user);
            return user.TwoFactorEnabled;
        }

        #endregion

        #region Email/Phone

        [HttpPost("getemail")]
        public async Task<string> GetEmailAsync([FromBody] User user)
        {
            return await _userStore.GetEmailAsync(user);
        }

        [HttpPost("setemail/{email}")]
        public async Task<string> SetEmailAsync([FromBody] User user, string email)
        {
            await _userStore.SetEmailAsync(user, email);
            await _userStore.UpdateAsync(user);
            return user.Email;
        }

        [HttpPost("getemailconfirmed")]
        public async Task<bool> GetEmailConfirmedAsync([FromBody] User user)
        {
            return await _userStore.GetEmailConfirmedAsync(user);
        }

        [HttpPost("setemailconfirmed/{enable:bool}")]
        public async Task<bool> SetEmailConfirmedAsync([FromBody] User user, bool enable)
        {
            await _userStore.SetEmailConfirmedAsync(user, enable);
            await _userStore.UpdateAsync(user);
            return user.EmailConfirmed;
        }

        [HttpGet("userfindbyemail/{email}")]
        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userStore.FindByEmailAsync(email);
        }

        [HttpPost("getnormalizedemail")]
        public async Task<string> GetNormalizedEmailAsync([FromBody] User user)
        {
            return await _userStore.GetNormalizedEmailAsync(user);
        }

        [HttpPost("setnormalizedemail/{email?}")]
        public async Task<string> SetNormalizedEmailAsync([FromBody] User user, string email)
        {
            await _userStore.SetNormalizedEmailAsync(user, email);
            await _userStore.UpdateAsync(user);
            return user.NormalizedEmail;
        }

        [HttpPost("getphonenumber")]
        public async Task<string> GetPhoneNumberAsync([FromBody] User user)
        {
            return await _userStore.GetPhoneNumberAsync(user);
        }

        [HttpPost("setphonenumber/{phone}")]
        public async Task<string> SetPhoneNumberAsync([FromBody] User user, string phone)
        {
            await _userStore.SetPhoneNumberAsync(user, phone);
            await _userStore.UpdateAsync(user);
            return user.PhoneNumber;
        }

        [HttpPost("getphonenumberconfirmed")]
        public async Task<bool> GetPhoneNumberConfirmedAsync([FromBody] User user)
        {
            return await _userStore.GetPhoneNumberConfirmedAsync(user);
        }

        [HttpPost("setphonenumberconfirmed/{confirmed:bool}")]
        public async Task<bool> SetPhoneNumberConfirmedAsync([FromBody] User user, bool confirmed)
        {
            await _userStore.SetPhoneNumberConfirmedAsync(user, confirmed);
            await _userStore.UpdateAsync(user);
            return user.PhoneNumberConfirmed;
        }

        #endregion

        #region Login/Lockout

        [HttpPost("addlogin")]
        public async Task AddLoginAsync([FromBody] AddLoginDTO loginDto)
        {
            await _userStore.AddLoginAsync(loginDto.User, loginDto.UserLoginInfo);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("removelogin/{loginProvider}/{providerKey}")]
        public async Task RemoveLoginAsync([FromBody] User user, string loginProvider, string providerKey)
        {
            await _userStore.RemoveLoginAsync(user, loginProvider, providerKey);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("getlogins")]
        public async Task<IList<UserLoginInfo>> GetLoginsAsync([FromBody] User user)
        {
            return await _userStore.GetLoginsAsync(user);
        }

        [HttpGet("userfindbylogin/{loginProvider}/{providerKey}")]
        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey)
        {
            return await _userStore.FindByLoginAsync(loginProvider, providerKey);
        }

        [HttpPost("getlockoutenddate")]
        public async Task<DateTimeOffset?> GetLockoutEndDateAsync([FromBody] User user)
        {
            return await _userStore.GetLockoutEndDateAsync(user);
        }

        [HttpPost("setlockoutenddate")]
        public async Task<DateTimeOffset?> SetLockoutEndDateAsync([FromBody] SetLockoutDTO lockoutDto)
        {
            await _userStore.SetLockoutEndDateAsync(lockoutDto.User, lockoutDto.LockoutEnd);
            await _userStore.UpdateAsync(lockoutDto.User);
            return lockoutDto.User.LockoutEnd;
        }

        [HttpPost("incrementaccessfailedcount")]
        public async Task<int> IncrementAccessFailedCountAsync([FromBody] User user)
        {
            var count = await _userStore.IncrementAccessFailedCountAsync(user);
            await _userStore.UpdateAsync(user);
            return count;
        }

        [HttpPost("resetaccessfailedcount")]
        public async Task<int> ResetAccessFailedCountAsync([FromBody] User user)
        {
            await _userStore.ResetAccessFailedCountAsync(user);
            await _userStore.UpdateAsync(user);
            return user.AccessFailedCount;
        }

        [HttpPost("getaccessfailedcount")]
        public async Task<int> GetAccessFailedCountAsync([FromBody] User user)
        {
            return await _userStore.GetAccessFailedCountAsync(user);
        }

        [HttpPost("getlockoutenabled")]
        public async Task<bool> GetLockoutEnabledAsync([FromBody] User user)
        {
            return await _userStore.GetLockoutEnabledAsync(user);
        }

        [HttpPost("setlockountebanled/{enable:bool}")]
        public async Task<bool> SetLockoutEnabledAsync([FromBody] User user, bool enable)
        {
            await _userStore.SetLockoutEnabledAsync(user, enable);
            await _userStore.UpdateAsync(user);
            return user.LockoutEnabled;
        }

        #endregion
    }
}
