using System;
using System.IO;
using System.Threading;
using System.Windows;
using Microsoft.Win32;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int valueTh1 = default;
        private int valueth2 = default;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonOpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Thread.Sleep(4000);
                valueTh1 = valueTh1 * valueth2 + 1;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TextBlockTest.Text = valueTh1.ToString();
                });
            });
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Thread.Sleep(10000);
                valueth2 = valueTh1 * valueth2 + 1;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TextBlockTest2.Text = valueth2.ToString();
                });
            });

            

            
        }
    }
}
