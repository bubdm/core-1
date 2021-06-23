using WebApplication1.Domain.ViewModel;

namespace WebApplication1.Interfaces.Services
{
    public interface ICartService
    {
        void Add(int id);
        void Minus(int id);
        void Remove(int id);
        void Clear();
        CartViewModel GetViewModel();
    }
}
