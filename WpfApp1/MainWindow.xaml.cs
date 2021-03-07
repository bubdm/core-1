using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace WpfApp1
{
    /// <summary> Interaction logic for MainWindow.xaml </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource;

        public MainWindow() => InitializeComponent();

        private async void StartButtonClick(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine($"Начало: {Thread.CurrentThread.ManagedThreadId}");
            var button = (Button) sender;
            button.IsEnabled = false;
            ButtonCancel.IsEnabled = true;

            //var result = await Task.Run(() => Factorial(20)).ConfigureAwait(true);
            //var result = await FactorialAsunc(20).ConfigureAwait(true);

            _cancellationTokenSource = new CancellationTokenSource();
            var progress = new Progress<double>(v =>
            {
                ProgressBarInformer.Value = v;
                TextBlockInformer.Text = v.ToString("p");
            });

            try
            {
                var set = int.Parse(TextBoxSets.Text);
                var result = await SumAsync(set, progress, _cancellationTokenSource.Token).ConfigureAwait(true);
                ((IProgress<double>)progress).Report(1);

                TextBlockResult.Text = result.ToString();
                ButtonCancel.IsEnabled = false;
                button.IsEnabled = true;
            }
            catch (OperationCanceledException exception)
            {
                TextBlockResult.Text = "Отменено";
                ((IProgress<double>)progress).Report(0);
                ButtonCancel.IsEnabled = false;
                button.IsEnabled = true;
            }
            
            
            //Debug.WriteLine($"Окончание: {Thread.CurrentThread.ManagedThreadId}");
        }

        private void StopButtonClick(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }

        private static long Factorial(long x)
        {
            Debug.WriteLine($"поток задачи: {Thread.CurrentThread.ManagedThreadId}");
            var result = 1l;
            while (x > 1)
            {
                result *= x;
                x--;
                Thread.Sleep(500);
            }
            return result;
        }

        private static async Task<long> FactorialAsunc(long x)
        {
            var result = 1L;
            while (x > 1)
            {
                result *= x;
                x--;
                await Task.Delay(500);
            }
            return result;
        }

        private static async Task<long> SumAsync(
            long x, 
            IProgress<double> progress = null, 
            CancellationToken cancel = default)
        {
            cancel.ThrowIfCancellationRequested();

            if (x < 0) return await SumAsync(-x).ConfigureAwait(false);

            var result = 0L;
            var x0 = 1;
            while (x0 < x)
            {
                cancel.ThrowIfCancellationRequested();
                result += x0;
                x0++;
                progress?.Report((double)x0 / x);
                await Task.Delay(100, cancel).ConfigureAwait(false);
            }
            return result;
        }
    }


}
