using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Domain.Identity
{
    public class Role : IdentityRole
    {
        public const string Administrators = "Administrators";
        public const string Users = "Users";
        public const string Clients = "Clients";
    }
}
