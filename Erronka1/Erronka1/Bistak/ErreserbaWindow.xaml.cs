using Erronka1.Modeloak;
using Erronka1.Repositories;
using Erronka1.Bistak.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Erronka1.Bistak.Views
{
    public partial class ErreserbaWindow : Window
    {
        private readonly string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=tpvdb";
        private readonly MahaiRepository mahaiaRepo;
        private readonly ErreserbaRepository erreserbaRepo;
        private Mahaiak _mahaiaSeleccionada;

        public ErreserbaWindow()
        {
            InitializeComponent();

            mahaiaRepo = new MahaiRepository();
            erreserbaRepo = new ErreserbaRepository(connectionString);

            dpData.SelectedDate = DateTime.Today;
            dpData.SelectedDateChanged += (s, e) => _ = LortuMahaiakAsync(dpData.SelectedDate.Value);

            _ = LortuMahaiakAsync(dpData.SelectedDate.Value);
        }

        private async Task LortuMahaiakAsync(DateTime data)
        {
            wpMahaiak.Children.Clear();
            var mahaiaList = await mahaiaRepo.LortuMahaiakLibreAsync(data);

            foreach (var m in mahaiaList)
            {
                var mahaiaControl = new MahaiaControl(m) { Margin = new Thickness(5) };
                mahaiaControl.MouseLeftButtonUp += (s, e) =>
                {
                    // Deselecciona la mesa anterior
                    if (_mahaiaSeleccionada != null && _mahaiaSeleccionada != m)
                    {
                        foreach (MahaiaControl mc in wpMahaiak.Children)
                        {
                            if (mc.mesita == _mahaiaSeleccionada)
                                mc.Ocupada(false);
                        }
                    }

                    _mahaiaSeleccionada = m;
                    mahaiaControl.Ocupada(true);
                };

                wpMahaiak.Children.Add(mahaiaControl);
            }

            // Mensaje de info
            tbInfo.Text = mahaiaList.Count == 0 ? "Ez dago mahairik libre." : $"Mahai libreak: {mahaiaList.Count}";
        }

        private async void BtnErreserbatu_Click(object sender, RoutedEventArgs e)
        {
            if (_mahaiaSeleccionada == null)
            {
                MessageBox.Show("Hautatu mahaia.");
                return;
            }

            DateTime data = dpData.SelectedDate ?? DateTime.Today;

            var erreserba = new Erreserba(0, _mahaiaSeleccionada, data);
            await erreserbaRepo.GordeErreserbaAsync(erreserba);

            MessageBox.Show($"Mahai {_mahaiaSeleccionada.Zenbakia} erreserbatuta.");
            _mahaiaSeleccionada = null;

            await LortuMahaiakAsync(data);
        }

        private void BtnEzeztatu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
