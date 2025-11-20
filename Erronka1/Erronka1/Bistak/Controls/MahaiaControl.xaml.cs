using Erronka1.Modeloak;
using System.Windows.Controls;
using System.Windows.Media;

namespace Erronka1.Bistak.Controls
{
    public partial class MahaiaControl : UserControl
    {
        public Mahaiak mesita { get; private set; }

        public MahaiaControl(Mahaiak mahaia)
        {
            InitializeComponent();
            mesita = mahaia;
            txtZenbakia.Text = mahaia.Zenbakia;
            KoloreaAldatu();
        }

        public void Ocupada(bool valor)
        {
            mesita.Okupatuta = valor;
            KoloreaAldatu();
        }
        private void KoloreaAldatu()
        {
            border.Background = mesita.Okupatuta ? Brushes.Red : Brushes.LightGreen;
        }
    }

}
