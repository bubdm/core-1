using System;
using WebApplication1.Domain.DTO.Identity.Base;

namespace WebApplication1.Domain.DTO.Identity
{
    public class SetLockoutDTO : UserDTO
    {
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}