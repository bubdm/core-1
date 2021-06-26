using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebApplication1.WebAPI.Clients.Values;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _webAPI = "http://localhost:5001";
        private ValuesClient _client;
        public MainWindow()
        {
            InitializeComponent();
            _client = new ValuesClient(new HttpClient{BaseAddress = new Uri(_webAPI)});
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            var items = _client.GetAll();
            ListBoxItems.ItemsSource = items;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            _client.Add("Новый элемент");
            var items = _client.GetAll();
            ListBoxItems.ItemsSource = items;
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            _client.Delete(0);
            var items = _client.GetAll();
            ListBoxItems.ItemsSource = items;
        }
    }
}
