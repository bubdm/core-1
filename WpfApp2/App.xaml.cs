using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfApp2.ViewModels;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IHost __hosting;

        public static IHost __Hosting
        {
            get
            {
                if (__hosting != null) return __hosting;
                var hostBuilder = Host.CreateDefaultBuilder(Environment.GetCommandLineArgs());
                hostBuilder.ConfigureServices(ConfigureServices);
                return __hosting = hostBuilder.Build();
            }
        }
        public static IServiceProvider Services => __Hosting.Services;
        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddSingleton<IDialogService, WindowDialog>();
            services.AddSingleton<MainWindowViewModel>();
        }
    }
    #region Тестовый сервис показа сообщения
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
