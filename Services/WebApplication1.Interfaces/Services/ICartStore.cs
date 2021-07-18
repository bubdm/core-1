using WebApplication1.Domain.Entities;

namespace WebApplication1.Interfaces.Services
{
    public interface ICartStore
    {
        Cart Cart { get; set; }
    }
}
