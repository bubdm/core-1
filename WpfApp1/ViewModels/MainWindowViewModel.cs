using System;
using System.Printing;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Infrastructure.Commands;
using WpfApp1.ViewModels.Base;

namespace WpfApp1.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        private string _title = "Главное окно программы";
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public DateTime CurrentTime => DateTime.Now;
        private readonly Timer _timer;

        public MainWindowViewModel()
        {
            _timer = new Timer(100);
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            OnPropertyChanged(nameof(CurrentTime));
        }

        #region Команды
        private ICommand _showDialogCommand;
        public ICommand ShowDialogCommand 
            => _showDialogCommand ??= new LambdaCommand(OnShowDialogCommandExecuted);
        private void OnShowDialogCommandExecuted(object p)
        {
            var message = p as string ?? "Hello World!";
            MessageBox.Show(message, "Сообщение от тестовой команды");
        }
        #endregion
    }
}
