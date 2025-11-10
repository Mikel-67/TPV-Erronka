using Erronka1.Bistak.Views;
using Erronka1.Modeloak;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Produktua> produktuList { get; set; } = new ObservableCollection<Produktua>();
        private List<Userrak> userList = new List<Userrak>();
        private readonly string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=tpvdb";
        public AdminWindow()
        {
            InitializeComponent();
            ProduktuakEskuratu();
            UserrakEskuratu();

            Zeerenda.Content = new StockView(produktuList); // Defektuzko bista
        }

        private void BtnUserrak(Object sender, RoutedEventArgs e)
        {
            Zeerenda.Content = new UsersView(userList);
        }
        private void BtnStocka(Object sender, RoutedEventArgs e)
        {
            Zeerenda.Content = new StockView(produktuList);
        }
        private void SesioaItxi(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void ProduktuakEskuratu()
        {
            using (var konexioa = new Npgsql.NpgsqlConnection(connectionString))
            {
                konexioa.Open();
                String query = "SELECT * FROM produktuak";
                using (var cmd = new Npgsql.NpgsqlCommand(query, konexioa))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Produktuen datuak prozesatu
                            int id = reader.GetInt32(reader.GetOrdinal("id"));
                            string izena = reader.GetString(reader.GetOrdinal("izena"));
                            decimal prezioa = reader.GetDecimal(reader.GetOrdinal("prezioa"));
                            int stocka = reader.GetInt32(reader.GetOrdinal("stock"));

                            produktuList.Add(new Produktua(id, izena, prezioa, stocka));
                        }
                    }
                }
                konexioa.Close();
            }
        }
        private void UserrakEskuratu()
        {
            using (var konn = new Npgsql.NpgsqlConnection(connectionString))
            {
                konn.Open();
                String query = "SELECT * FROM userrak";
                using (var cmd = new Npgsql.NpgsqlCommand(query, konn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Userraken datuak prozesatu
                            int id = reader.GetInt32(reader.GetOrdinal("id"));
                            string izena = reader.GetString(reader.GetOrdinal("izena"));
                            string pasahitza = reader.GetString(reader.GetOrdinal("pasahitza"));
                            bool admin = reader.GetBoolean(reader.GetOrdinal("admin"));
                            // Hemen egin dezake

                            userList.Add(new Userrak(id, izena, pasahitza, admin));
                        }
                    }
                }
            }
        }
    }
}