using WebApplication1.ViewModel;

namespace WebApplication1.Services.Interfaces
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
