using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Domain.Identity
{
    public class User : IdentityUser
    {
        public const string Administrator = "Admin";
        public const string DefaultAdministratorPassword = "123";
    }
}
