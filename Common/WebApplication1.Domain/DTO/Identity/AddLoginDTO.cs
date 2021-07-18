using Microsoft.AspNetCore.Identity;
using WebApplication1.Domain.DTO.Identity.Base;

namespace WebApplication1.Domain.DTO.Identity
{
    public class AddLoginDTO : UserDTO
    {
        public UserLoginInfo UserLoginInfo { get; set; }
    }
}