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

namespace Aplicacion.Presentacion
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

        

        private void btnGestionClientes_Click(object sender, RoutedEventArgs e)
        {
            GestionClientes gClientes = new GestionClientes();
            gClientes.Show();
        }

        private void btnGestionContratos_Click(object sender, RoutedEventArgs e)
        {
            GestionContratos gContratos = new GestionContratos();
            gContratos.Show();
        }
    }
}
