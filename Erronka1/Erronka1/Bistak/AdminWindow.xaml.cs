using Erronka1.Bistak.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Erronka1.Bistak
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            Zeerenda.Content = new StockView(); // Defektuzko bista
        }

        private void BtnUserrak(Object sender, RoutedEventArgs e)
        {
            Zeerenda.Content = new UsersView();
        }
        private void BtnStocka(Object sender, RoutedEventArgs e)
        {
            Zeerenda.Content = new StockView();
        }
        private void SesioaItxi(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
