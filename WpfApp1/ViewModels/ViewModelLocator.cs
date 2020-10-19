﻿using Microsoft.Extensions.DependencyInjection;

namespace WpfApp1.ViewModels
{
    class ViewModelLocator
    {
        public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();
    }
}
