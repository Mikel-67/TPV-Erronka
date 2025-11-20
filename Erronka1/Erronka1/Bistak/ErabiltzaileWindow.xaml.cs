using Erronka1.Bistak.Views;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace Erronka1.Bistak
{
    /// <summary>
    /// Interaction logic for ErabiltzaileWindow.xaml
    /// </summary>
    public partial class ErabiltzaileWindow : Window
    {
        private readonly string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=tpvdb";
        public class ProduktuListarako
        {
            public string Izena { get; set; }
            public decimal Prezioa { get; set; }
            public int Kantitatea { get; set; } = 1;
            public decimal Guztira => Prezioa * Kantitatea;
        }
        private ObservableCollection<ProduktuListarako> ProduktuakListarako = new ObservableCollection<ProduktuListarako>();
        public ErabiltzaileWindow()
        {
            InitializeComponent();
            dgProductosSeleccionados.ItemsSource = ProduktuakListarako;
            ProduktuakKargatu();
        }

        private void ProduktuakKargatu()
        {
            wpProduktuak.Children.Clear();
            using (var konexioa = new Npgsql.NpgsqlConnection(connectionString))
            {
                konexioa.Open();
                String query = "SELECT * FROM produktuak";
                using (var cmd = new Npgsql.NpgsqlCommand(query, konexioa))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        StringBuilder produktuak = new StringBuilder();
                        while (reader.Read())
                        {
                            string izena = reader.GetString(reader.GetOrdinal("izena"));
                            decimal prezioa = reader.GetDecimal(reader.GetOrdinal("prezioa"));
                            decimal stock = reader.GetDecimal(reader.GetOrdinal("stock"));
                            var botoia = new Button
                            {
                                Content = izena,
                                FontSize = 14,
                                Height = 80,
                                Margin = new Thickness(5),
                                Tag = new { Izena = izena, Prezioa = prezioa, Stock = stock }
                            };

                            botoia.Click += ProduktuaKlikatu;

                            wpProduktuak.Children.Add(botoia);
                        }
                    }
                }
            }
        }
        private void ProduktuaKlikatu(object sender, RoutedEventArgs e)
        {
            if (sender is Button botoia && botoia.Tag != null)
            {
                var produktua = botoia.Tag;
                var izenaProp = produktua.GetType().GetProperty("Izena");
                var prezioaProp = produktua.GetType().GetProperty("Prezioa");
                var stockProp = produktua.GetType().GetProperty("Stock");

                string izena = izenaProp?.GetValue(produktua)?.ToString() ?? "";
                decimal prezioa = prezioaProp != null ? (decimal)prezioaProp.GetValue(produktua) : 0;
                decimal stock = stockProp != null ? (decimal)stockProp.GetValue(produktua) : 0;

                if (stock <= 0)
                {
                    MessageBox.Show("❌ Stock agortuta dago.");
                    return;
                }

                botoia.Tag = new { Izena = izena, Prezioa = prezioa, Stock = stock - 1 };

                ProduktuaListaratu(izena, prezioa);
            }
        }
        private void ProduktuaListaratu(string izena, decimal prezioa)
        {
            var listanDago = ProduktuakListarako.FirstOrDefault(p => p.Izena == izena);
            if (listanDago != null)
            {
                listanDago.Kantitatea++;
                dgProductosSeleccionados.Items.Refresh();
            }
            else
            {
                ProduktuakListarako.Add(new ProduktuListarako
                {
                    Izena = izena,
                    Prezioa = prezioa
                });
            }
        }

        private void AutatuaEzabatu(object sender, RoutedEventArgs e)
        {
            if (dgProductosSeleccionados.SelectedItem is ProduktuListarako aukeratua)
            {
                ProduktuakListarako.Remove(aukeratua);
                ProduktuakKargatu(); //Stocka eguneratzeko
            }
        }

        private void ImprimatuTicketa(object sender, RoutedEventArgs e)
        {
            if (ProduktuakListarako.Count == 0)
            {
                MessageBox.Show("Ez dago produktu aukeraturik.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Crear documento PDF
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Ticket TPV";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            GlobalFontSettings.UseWindowsFontsUnderWindows = true;
            XFont font = new XFont("Arial", 10, XFontStyleEx.Regular);
            double yPoint = 20;

            // Encabezado
            gfx.DrawString("=== TICKET TPV ===", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"Data: {DateTime.Now}", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString("----------------------------", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;

            decimal total = 0;
            foreach (var p in ProduktuakListarako)
            {
                gfx.DrawString($"{p.Izena} x{p.Kantitatea}  {p.Guztira:F2} €", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
                yPoint += 20;
                total += p.Guztira;
            }

            gfx.DrawString("----------------------------", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"GUZTIRA: {total:F2} €", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString("============================", font, XBrushes.Black, new XRect(20, yPoint, page.Width, page.Height), XStringFormats.TopLeft);

            // Guardar PDF
            string ruta = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TicketTPV.pdf");
            document.Save(ruta);

            // Abrir automáticamente
            Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });

            // Limpiar lista después de pagar
            ProduktuakListarako.Clear();
        }
        private void erreserbaPantaila(object sender, RoutedEventArgs e)
        {
            var erreserbaWindow = new ErreserbaWindow
            {
                Owner = this
            };
            erreserbaWindow.ShowDialog();
        }
    }
}
