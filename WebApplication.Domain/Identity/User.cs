using Microsoft.AspNetCore.Identity;

namespace WebApplication.Domain.Identity
{
    public class User : IdentityUser
    {
        public const string Administrator = "Admin";
        public const string DefaultAdministratorPassword = "123";
    }
}
