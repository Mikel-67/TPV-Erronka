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
    /// Interaction logic for UserrakDialog.xaml
    /// </summary>
    public partial class UserrakDialog : Window
    {
        public Userrak NuevoUser { get; private set; }
        public UserrakDialog()
        {
            InitializeComponent();
        }
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string izena = tbIzena.Text;
                string pasahitza = tbPrezioa.Text;
                bool admin;

                if (cbAdmin.IsChecked == true)
                {
                    admin = true;
                }
                else
                {
                    admin = false;
                }

                NuevoUser = new Userrak(0, izena, pasahitza, admin);
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
