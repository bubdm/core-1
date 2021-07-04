using Microsoft.AspNetCore.Identity;
using WebApplication1.Domain.Identity;

namespace WebApplication1.Interfaces.Services.Identity
{
    public interface IUsersClient : IUserRoleStore<User>, IUserPasswordStore<User>, IUserEmailStore<User>, 
        IUserPhoneNumberStore<User>, IUserTwoFactorStore<User>, IUserLoginStore<User>, IUserLockoutStore<User>, IUserClaimStore<User>
    {
    }
}
