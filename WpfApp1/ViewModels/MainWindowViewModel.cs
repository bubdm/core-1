using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
