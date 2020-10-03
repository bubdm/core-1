using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using WpfApp1.ViewModels;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IHost __hosting;
        public static IHost Hosting
        {
            get
            {
                if (__hosting != null) return __hosting;
                var host_builder = Host.CreateDefaultBuilder(Environment.GetCommandLineArgs());
                host_builder.ConfigureServices(ConfigureServices);
                return __hosting = host_builder.Build();
            }
        }
        public static IServiceProvider Services => Hosting.Services;
        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddSingleton<IDialogService, WindowDialog>();
            services.AddSingleton<MainWindowViewModel>();
        }
    }
    #region Тестовый сервис
    interface IDialogService
    {
        void ShowInfo(string msg);
    }
    class WindowDialog : IDialogService
    {
        public void ShowInfo(string msg) => MessageBox.Show(msg);
    }
    #endregion
}
