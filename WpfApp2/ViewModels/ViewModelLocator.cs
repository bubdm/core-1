using Microsoft.Extensions.DependencyInjection;

namespace WpfApp2.ViewModels
{
    class ViewModelLocator
    {
        public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();
    }
}
