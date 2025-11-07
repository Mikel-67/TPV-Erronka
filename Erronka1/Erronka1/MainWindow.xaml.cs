using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Erronka1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=tpvdb";
        private Npgsql.NpgsqlConnection konexioa;
        public MainWindow()
        {
            InitializeComponent();
            KonexioaSortu();
        }

        private void KonexioaSortu()
        {
            try
            {
                konexioa = new Npgsql.NpgsqlConnection(connectionString);
                konexioa.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión:\n{ex.Message}");
            }
        }
        private void login(object sender, RoutedEventArgs e)
        {
            string username = tbUserName.Text;
            string pasahitza = tbPasahitza.Password;

            string query = "SELECT * FROM userrak WHERE izena = @izena AND pasahitza = @pasahitza";
            using (var cmd = new Npgsql.NpgsqlCommand(query, konexioa))
            {
                cmd.Parameters.AddWithValue("izena", username);
                cmd.Parameters.AddWithValue("pasahitza", pasahitza);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool admin = reader.GetBoolean(reader.GetOrdinal("admin"));
                        if (admin)
                        {
                            Erronka1.Bistak.AdminWindow adminWindow = new Erronka1.Bistak.AdminWindow(konexioa);
                            adminWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            Erronka1.Bistak.ErabiltzaileWindow userWindow = new Erronka1.Bistak.ErabiltzaileWindow();
                            userWindow.Show();
                            this.Close();
                        }
                    }else{
                        MessageBox.Show("❌ Izena edo pasahitza okerra.");
                    }
                }
            }
        }
    }
}