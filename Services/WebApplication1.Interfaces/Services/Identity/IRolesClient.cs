using Microsoft.AspNetCore.Identity;
using WebApplication1.Domain.Identity;

namespace WebApplication1.Interfaces.Services.Identity
{
    public interface IRolesClient : IRoleStore<Role>
    {
    }
}
