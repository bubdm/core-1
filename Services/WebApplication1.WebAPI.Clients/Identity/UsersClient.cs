using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Domain.DTO.Identity;
using WebApplication1.Domain.Identity;
using WebApplication1.Interfaces.Adresses;
using WebApplication1.Interfaces.Services.Identity;
using WebApplication1.WebAPI.Clients.Base;

namespace WebApplication1.WebAPI.Clients.Identity
{
    public class UsersClient : BaseClient, IUsersClient
    {
        public UsersClient(HttpClient client) : base(client, WebAPIInfo.Identity.Users) { }

        #region IUserStore<User>

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/userid", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/username", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task SetUserNameAsync(User user, string name, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/username/{name}", user, cancel);
            user.UserName = await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/normalizedusername", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task SetNormalizedUserNameAsync(User user, string name, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/normalizedusername/{name}", user, cancel).ConfigureAwait(false);
            user.NormalizedUserName = await response.Content.ReadAsStringAsync();
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/user", user, cancel).ConfigureAwait(false);
            var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancel)
        {
            var response = await PutAsync($"{Address}/user", user, cancel).ConfigureAwait(false);
            var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/user/delete", user, cancel).ConfigureAwait(false);
            var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<User> FindByIdAsync(string id, CancellationToken cancel)
        {
            return await GetAsync<User>($"{Address}/user/find/{id}", cancel).ConfigureAwait(false);
        }

        public async Task<User> FindByNameAsync(string name, CancellationToken cancel)
        {
            return await GetAsync<User>($"{Address}/user/name/{name}", cancel).ConfigureAwait(false);
        }

        #endregion

        #region IUserRoleStore<User>
        
        public async Task AddToRoleAsync(User user, string name, CancellationToken cancel)
        {
            await PostAsync($"{Address}/role/{name}", user, cancel).ConfigureAwait(false);
        }

        public async Task RemoveFromRoleAsync(User user, string name, CancellationToken cancel)
        {
            await PostAsync($"{Address}/role/delete/{name}", user, cancel).ConfigureAwait(false);
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/roles", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<IList<string>>(cancellationToken: cancel);
        }

        public async Task<bool> IsInRoleAsync(User user, string name, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/inrole/{name}", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string name, CancellationToken cancel)
        {
            return await GetAsync<List<User>>($"{Address}/usersinrole/{name}", cancel).ConfigureAwait(false);
        }

        #endregion

        #region IUserPasswordStore<User>

        public async Task SetPasswordHashAsync(User user, string hash, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/setpasswordhash", new PasswordHashDTO {User = user, Hash = hash}, cancel)
                    .ConfigureAwait(false);
            user.PasswordHash = await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/getpasswordhash", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/haspassword", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        #endregion

        #region IUserEmailStore<User>
        
        public async Task SetEmailAsync(User user, string email, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/setemail/{email}", user, cancel).ConfigureAwait(false);
            user.Email = await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/getemail", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/getemailconfirmed", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/setemailconfirmed/{confirmed}", user, cancel).ConfigureAwait(false);
            user.EmailConfirmed = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        public async Task<User> FindByEmailAsync(string email, CancellationToken cancel)
        {
            return await GetAsync<User>($"{Address}/userfindbyemail/{email}", cancel).ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/getnormalizedemail", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task SetNormalizedEmailAsync(User user, string email, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/setnormalizedemail/{email}", user, cancel).ConfigureAwait(false);
            user.NormalizedEmail = await response.Content.ReadAsStringAsync();
        }
        
        #endregion

        #region IUserPhoneNumberStore<User>
        
        public async Task SetPhoneNumberAsync(User user, string phone, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/setphonenumber/{phone}", user, cancel).ConfigureAwait(false);
            user.PhoneNumber = await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/getphonenumber", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/getphonenumberconfirmed", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken:cancel);
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/setphonenumberconfirmed/{confirmed}", user, cancel).ConfigureAwait(false);
            user.PhoneNumberConfirmed = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        #endregion

        #region IUserLoginStore<User>

        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancel)
        {
            await PostAsync($"{Address}/addlogin", new AddLoginDTO {User = user, UserLoginInfo = login}, cancel)
                .ConfigureAwait(false);
        }

        public async Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancel)
        {
            await PostAsync($"{Address}/removelogin/{loginProvider}/{providerKey}", user, cancel).ConfigureAwait(false);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/getlogins", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<List<UserLoginInfo>>(cancellationToken: cancel);
        }

        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancel)
        {
            return await GetAsync<User>($"{Address}/userfindbylogin/{loginProvider}/{providerKey}", cancel)
                .ConfigureAwait(false);
        }

        #endregion

        #region IUserLockoutStore<User>

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/getlockoutenddate", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<DateTimeOffset?>(cancellationToken: cancel);
        }

        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/setlockoutenddate",
                new SetLockoutDTO {User = user, LockoutEnd = lockoutEnd}, cancel).ConfigureAwait(false);
            user.LockoutEnd = await response.Content.ReadFromJsonAsync<DateTimeOffset?>(cancellationToken: cancel);
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/incrementaccessfailedcount", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<int>(cancellationToken: cancel);
        }

        public async Task ResetAccessFailedCountAsync(User user, CancellationToken cancel)
        {
            await PostAsync($"{Address}/resetaccessfailedcount", user, cancel).ConfigureAwait(false);
        }

        public async Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/getaccessfailedcount", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<int>(cancellationToken: cancel);
        }

        public async Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/getlockoutenabled", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        public async Task SetLockoutEnabledAsync(User user, bool enable, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/setlockountebanled/{enable}", user, cancel)
                .ConfigureAwait(false);
            user.LockoutEnabled = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        #endregion

        #region IUserTwoFactorStore<User>

        public async Task SetTwoFactorEnabledAsync(User user, bool enable, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/settwofactorenabled/{enable}", user, cancel)
                .ConfigureAwait(false);
            user.TwoFactorEnabled = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/gettwofactorenabled", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        #endregion

        #region IUserClaimStore<User>

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/getclaims", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<List<Claim>>(cancellationToken: cancel);
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync($"{Address}/addclaims", new AddClaimDTO {User = user, Claims = claims}, cancel)
                .ConfigureAwait(false);
        }

        public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancel)
        {
            await PostAsync($"{Address}/replaceclaim",
                new ReplaceClaimDTO {User = user, Claim = claim, NewClaim = newClaim}, cancel).ConfigureAwait(false);
        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync($"{Address}/removeclaim", new RemoveClaimDTO {User = user, Claims = claims}, cancel)
                .ConfigureAwait(false);
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/getusersforclaim", claim, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<List<User>>(cancellationToken: cancel);
        }

        #endregion
    }
}
