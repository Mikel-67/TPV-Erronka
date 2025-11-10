using Erronka1.Modeloak;
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

namespace Erronka1.Bistak.Views
{
    /// <summary>
    /// Interaction logic for ProduktuakDialog.xaml
    /// </summary>
    public partial class ProduktuakDialog : Window
    {
        public Produktua NuevoProducto { get; private set; }

        public ProduktuakDialog()
        {
            InitializeComponent();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string izena = tbIzena.Text;
                decimal prezioa = decimal.Parse(tbPrezioa.Text);
                int stock = int.Parse(tbStocka.Text);

                NuevoProducto = new Produktua(0, izena, prezioa, stock);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
