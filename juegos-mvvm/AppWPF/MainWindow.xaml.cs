using AppWPF.MVVM.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppWPF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            ActivarText();
            Activarbtns();
        }
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            Activarbtns();
            ActivarText();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Desactivarbtns();
            DesactivarText();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Desactivarbtns();
            DesactivarText();
        }
        private void Activarbtns()
        {
            btnPanelone.Visibility = Visibility.Hidden;
            btnPaneltwo.Visibility = Visibility.Visible;
        }
        private void Desactivarbtns()
        {
            btnPaneltwo.Visibility = Visibility.Hidden;
            btnPanelone.Visibility = Visibility.Visible;
        }
        private void ActivarText()
        {
            Year_lanzamiento.IsEnabled = true;
            cmbConsola.IsEnabled = true;
            txtTitulo.IsEnabled = true;
            txtDesarrolladora.IsEnabled = true;
            txtPais.IsEnabled = true;
            lvGames.IsEnabled = false;
        }
        private void DesactivarText()
        {
            txtTitulo.IsEnabled = false;
            txtDesarrolladora.IsEnabled = false;
            txtPais.IsEnabled = false;
            cmbConsola.IsEnabled = false;
            Year_lanzamiento.IsEnabled = false;
            lvGames.IsEnabled = true;
        }
    }
}
