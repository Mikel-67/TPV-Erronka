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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UsersView : System.Windows.Controls.UserControl
    {
        private readonly string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=tpvdb";
        public ObservableCollection<Userrak> Userrak { get; set; }
        public UsersView(ObservableCollection<Userrak> userrak)
        {
            InitializeComponent();
            Userrak = userrak;
            dgUserrak.ItemsSource = userrak;
        }
        private void GehituUser(object sender, RoutedEventArgs e)
        {
            var dialog = new UserrakDialog();
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true)
            {
                var nuevo = dialog.NuevoUser;

                using var conn = new Npgsql.NpgsqlConnection(connectionString);
                conn.Open();
                using var cmd = new Npgsql.NpgsqlCommand(
                    "INSERT INTO userrak (izena, pasahitza, admin) VALUES (@izena,@pasahitza,@admin) RETURNING id",
                    conn);
                cmd.Parameters.AddWithValue("izena", nuevo.Izena);
                cmd.Parameters.AddWithValue("pasahitza", nuevo.Pasahitza);
                cmd.Parameters.AddWithValue("admin", nuevo.Admin);

                nuevo.Id = (int)cmd.ExecuteScalar();

                Userrak.Add(nuevo);
            }
        }
        private void EditatuUser(object sender, RoutedEventArgs e)
        {
            using var conn = new Npgsql.NpgsqlConnection(connectionString);
            conn.Open();

            foreach (Userrak p in dgUserrak.Items)
            {
                if (p.Id > 0)
                {
                    using var cmd = new Npgsql.NpgsqlCommand(
                        "UPDATE userrak SET izena=@izena, pasahitza=@pasahitza, admin=@admin WHERE id=@id",
                        conn);
                    cmd.Parameters.AddWithValue("izena", p.Izena);
                    cmd.Parameters.AddWithValue("pasahitza", p.Pasahitza);
                    cmd.Parameters.AddWithValue("admin", p.Admin);
                    cmd.Parameters.AddWithValue("id", p.Id);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Aldaketak gorde dira.");
        }
        private void EzabatuUser(object sender, RoutedEventArgs e)
        {
            if (dgUserrak.SelectedItem is Userrak p)
            {
                if (MessageBox.Show($"{p.Izena} borratu nahi duzu?", "Baieztatu", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (p.Id > 0)
                    {
                        using var conn = new Npgsql.NpgsqlConnection(connectionString);
                        conn.Open();
                        using var cmd = new Npgsql.NpgsqlCommand("DELETE FROM userrak WHERE id=@id", conn);
                        cmd.Parameters.AddWithValue("id", p.Id);
                        cmd.ExecuteNonQuery();
                    }

                    Userrak.Remove(p);
                }
            }
            else
            {
                MessageBox.Show("Aukeratu produktu bat ezabatzeko.");
            }
        }
    }
}
