using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IServiceProvider _services;
        public static IServiceProvider Services => _services ??= GetServices().BuildServiceProvider();
        private static IServiceCollection GetServices()
        {
            var services = new ServiceCollection();
            InitializeServices(services);
            return services;
        }
        private static void InitializeServices(IServiceCollection services)
        {
            services.AddTransient<IDialogService, WindowDialog>();
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
