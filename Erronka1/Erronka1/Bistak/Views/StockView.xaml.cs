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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Erronka1.Bistak.Views
{
    /// <summary>
    /// Interaction logic for StockView.xaml
    /// </summary>
    public partial class StockView : System.Windows.Controls.UserControl
    {
        private readonly string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=tpvdb";
        public ObservableCollection<Produktua> Produktuak { get; set; }
        public StockView(ObservableCollection<Produktua> produktuak)
        {
            InitializeComponent();
            Produktuak = produktuak;
            dgStock.ItemsSource = produktuak;
        }
        private void GehituStock(object sender, RoutedEventArgs e)
        {
            var dialog = new ProduktuakDialog();
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true)
            {
                var nuevo = dialog.NuevoProducto;

                using var conn = new Npgsql.NpgsqlConnection(connectionString);
                conn.Open();
                using var cmd = new Npgsql.NpgsqlCommand(
                    "INSERT INTO produktuak (izena, prezioa, stock) VALUES (@izena,@prezioa,@stock) RETURNING id",
                    conn);
                cmd.Parameters.AddWithValue("izena", nuevo.Izena);
                cmd.Parameters.AddWithValue("prezioa", nuevo.Prezioa);
                cmd.Parameters.AddWithValue("stock", nuevo.Stocka);

                nuevo.Id = (int)cmd.ExecuteScalar();

                Produktuak.Add(nuevo);
            }
        }
        private void EditatuStock(object sender, RoutedEventArgs e)
        {
            using var conn = new Npgsql.NpgsqlConnection(connectionString);
            conn.Open();

            foreach (Produktua p in dgStock.Items)
            {
                if (p.Id > 0)
                {
                    using var cmd = new Npgsql.NpgsqlCommand(
                        "UPDATE produktuak SET izena=@izena, prezioa=@prezioa, stock=@stock WHERE id=@id",
                        conn);
                    cmd.Parameters.AddWithValue("izena", p.Izena);
                    cmd.Parameters.AddWithValue("prezioa", p.Prezioa);
                    cmd.Parameters.AddWithValue("stock", p.Stocka);
                    cmd.Parameters.AddWithValue("id", p.Id);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Aldaketak gorde dira.");
        }
        private void EzabatuStock(object sender, RoutedEventArgs e)
        {
            if (dgStock.SelectedItem is Produktua p)
            {
                if (MessageBox.Show($"{p.Izena} borratu nahi duzu?", "Baieztatu", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (p.Id > 0)
                    {
                        using var conn = new Npgsql.NpgsqlConnection(connectionString);
                        conn.Open();
                        using var cmd = new Npgsql.NpgsqlCommand("DELETE FROM produktuak WHERE id=@id", conn);
                        cmd.Parameters.AddWithValue("id", p.Id);
                        cmd.ExecuteNonQuery();
                    }

                    Produktuak.Remove(p);
                }
            }
            else
            {
                MessageBox.Show("Aukeratu produktu bat ezabatzeko.");
            }
        }
    }
}
