using System.Security.Claims;
using WebApplication1.Domain.DTO.Identity.Base;

namespace WebApplication1.Domain.DTO.Identity
{
    public class ReplaceClaimDTO : UserDTO
    {
        public Claim Claim { get; set; }
        public Claim NewClaim { get; set; }
    }
}