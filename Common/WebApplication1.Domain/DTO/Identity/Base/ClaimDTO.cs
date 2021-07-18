using System.Collections.Generic;
using System.Security.Claims;

namespace WebApplication1.Domain.DTO.Identity.Base
{
    public abstract class ClaimDTO : UserDTO
    {
        public IEnumerable<Claim> Claims { get; set; }
    }
}
