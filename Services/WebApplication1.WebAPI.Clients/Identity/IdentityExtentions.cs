using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Domain.Identity;

namespace WebApplication1.WebAPI.Clients.Identity
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityApplication1APIClients(this IServiceCollection services)
        {
            services.AddHttpClient("Application1API", (s,c) => c.BaseAddress = new Uri(s.GetRequiredService<IConfiguration>()["WebAPI"]))
                .AddTypedClient<IUserStore<User>, UsersClient>()
                .AddTypedClient<IUserRoleStore<User>, UsersClient>()
                .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
                .AddTypedClient<IUserEmailStore<User>, UsersClient>()
                .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
                .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
                .AddTypedClient<IUserClaimStore<User>, UsersClient>()
                .AddTypedClient<IUserLoginStore<User>, UsersClient>()
                .AddTypedClient<IRoleStore<Role>, RolesClient>();

            return services;
        }

        public static IdentityBuilder AddIdentityApplication1WebAPIClients(this IdentityBuilder builder)
        {
            builder.Services.AddIdentityApplication1APIClients();

            return builder;
        }
    }
}
