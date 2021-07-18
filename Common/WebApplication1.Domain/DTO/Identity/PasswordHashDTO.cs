using WebApplication1.Domain.DTO.Identity.Base;

namespace WebApplication1.Domain.DTO.Identity
{
    public class PasswordHashDTO : UserDTO
    {
        public string Hash { get; set; }
    }
}