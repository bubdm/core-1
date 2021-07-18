using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Domain.Identity;
using WebApplication1.Interfaces.Adresses;
using WebApplication1.Interfaces.Services.Identity;
using WebApplication1.WebAPI.Clients.Base;

namespace WebApplication1.WebAPI.Clients.Identity
{
    public class RolesClient : BaseClient, IRolesClient
    {
        public RolesClient(HttpClient client) : base(client, WebAPIInfo.Identity.Roles) { }

        #region IRoleStore<Role>
        
        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancel)
        {
            var response = await PostAsync(Address, role, cancel).ConfigureAwait(false);
            var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancel)
        {
            var response = await PutAsync(Address, role, cancel).ConfigureAwait(false);
            var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/delete", role, cancel).ConfigureAwait(false);
            var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/getroleid", role, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/getrolename", role, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task SetRoleNameAsync(Role role, string name, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/setrolename/{name}", role, cancel).ConfigureAwait(false);
            role.Name = await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/getnormalizedrolename", role, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/setnormalizedrolename/{normalizedName}", role, cancel).ConfigureAwait(false);
            role.NormalizedName = await response.Content.ReadAsStringAsync();
        }

        public async Task<Role> FindByIdAsync(string id, CancellationToken cancel)
        {
            return await GetAsync<Role>($"{Address}/findbyid/{id}", cancel).ConfigureAwait(false);
        }

        public async Task<Role> FindByNameAsync(string name, CancellationToken cancel)
        {
            return await GetAsync<Role>($"{Address}/findbyname/{name}", cancel).ConfigureAwait(false);
        }
        
        #endregion
    }
}
