using WpfApp1.ViewModels.Base;

namespace WpfApp1.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
        }
        private string _title = "Тестовое приложение";
        public string Title
        {
            get => _title;
            set
            {
                Set(ref _title, value);
            }
        }

    }
}
