using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using WpfApp2.Infrastructure.Commands;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        private string _title = "Главное окно программы";
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        public MainWindowViewModel()
        {
        }
        #region Команды
        /// <summary> Команда показа сообщения </summary>
        public ICommand ShowDialogCommand 
            => _showDialogCommand ??= new LambdaCommand(OnShowDialogCommandExecuted);
        private ICommand _showDialogCommand;
        private void OnShowDialogCommandExecuted(object p)
        {
            var message = p as string ?? "Привет мир!";
            App.Services.GetService<IDialogService>().ShowInfo(message);
        }
        #endregion
    }
}
