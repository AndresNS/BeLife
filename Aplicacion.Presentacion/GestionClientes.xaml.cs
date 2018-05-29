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
using Aplicacion.Datos;

namespace Aplicacion.Presentacion
{
    /// <summary>
    /// Lógica de interacción para GestionClientes.xaml
    /// </summary>
    public partial class GestionClientes : Window
    {
        Datos.BeLifeEntities conector;
        public GestionClientes()
        {
            InitializeComponent();
            if (conector == null)
            {
                conector = new BeLifeEntities();
            }
            ActualizarListado();

            cbxIngresarSexo.ItemsSource = conector.Sexoes.ToList();
            cbxIngresarEstadoCivil.ItemsSource = conector.EstadoCivils.ToList();
            cbxModificarSexo.ItemsSource = conector.Sexoes.ToList();
            cbxModificarEstadoCivil.ItemsSource = conector.EstadoCivils.ToList();
            cbxModificarRut.ItemsSource = conector.Clientes.ToList();

            //filtros
            cbxFilterSexo.ItemsSource = conector.Sexoes.ToList();
            cbxFilterCivil.ItemsSource = conector.EstadoCivils.ToList();
            cbxFilterRut.ItemsSource = conector.Clientes.ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rbModificarIngRut.IsChecked = true;
            cbxModificarRut.Visibility = Visibility.Hidden;
        }

        private void rbModificarBuscarRut_Checked(object sender, RoutedEventArgs e)
        {
            if (rbModificarIngRut.IsChecked == true)
            {
                cbxModificarRut.Visibility = Visibility.Hidden;
                txtModificarRut.Visibility = Visibility.Visible;
            }
            else
            {
                cbxModificarRut.Visibility = Visibility.Visible;
                cbxModificarRut.SelectedIndex = 0;
                txtModificarRut.Visibility = Visibility.Hidden;

            }
        }

        private void ActualizarListado()
        {
            dgListadoClientes.ItemsSource = null;
            cbxModificarRut.ItemsSource = null;
            cbxFilterSexo.ItemsSource = null;
            cbxFilterCivil.ItemsSource = null;
            cbxFilterRut.ItemsSource = null;

            dgListadoClientes.ItemsSource = conector.Clientes.ToList();
            cbxModificarRut.ItemsSource = conector.Clientes.ToList();
            cbxFilterSexo.ItemsSource = conector.Sexoes.ToList();
            cbxFilterCivil.ItemsSource = conector.EstadoCivils.ToList();
            cbxFilterRut.ItemsSource = conector.Clientes.ToList();
        
        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            if (txtIngresarRut.Text == "" || txtIngresarNombres.Text == "" || txtIngresarApellidos.Text == "" || cbxIngresarSexo.SelectedValue == null || cbxIngresarEstadoCivil.SelectedValue == null)
            {
                MessageBox.Show("Todos los campos son oligatorios");
            }
            else
            {
                try
                {
                    Cliente c = new Cliente();
                    c.RutCliente = txtIngresarRut.Text;
                    c.Nombres = txtIngresarNombres.Text;
                    c.Apellidos = txtIngresarApellidos.Text;
                    string fecNac = dpIngresarFechaNacimiento.SelectedDate.ToString();

                    DateTime hoy = DateTime.Today;
                    DateTime fecNacDT = DateTime.Parse(fecNac);
                    int edad = hoy.Year - fecNacDT.Year;
                    if (fecNacDT > hoy.AddYears(-edad))
                    {
                        edad--;
                    }

                    if (edad >= 18)
                    {
                        c.FechaNacimiento = fecNac;

                        c.IdSexo = cbxIngresarSexo.SelectedIndex + 1;
                        c.IdEstadoCivil = cbxIngresarEstadoCivil.SelectedIndex + 1;

                        conector.Clientes.Add(c);
                        conector.SaveChanges();

                        MessageBox.Show("Se ha ingresado un cliente");
                        txtIngresarRut.Text = string.Empty;
                        txtIngresarNombres.Text = string.Empty;
                        txtIngresarApellidos.Text = string.Empty;
                        dpIngresarFechaNacimiento.SelectedDate = DateTime.Now;
                        cbxIngresarSexo.SelectedIndex = -1;
                        cbxIngresarEstadoCivil.SelectedIndex = -1;

                        ActualizarListado();
                    }
                    else
                    {
                        MessageBox.Show("El cliente debe ser mayor de 18 años.");
                        dpIngresarFechaNacimiento.Focus();
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    txtIngresarRut.Text = string.Empty;
                    txtIngresarNombres.Text = string.Empty;
                    txtIngresarApellidos.Text = string.Empty;
                    dpIngresarFechaNacimiento.SelectedDate = DateTime.Now;
                    cbxIngresarSexo.SelectedIndex = 0;
                    cbxIngresarEstadoCivil.SelectedIndex = 0;
                }
            }

        }

        private void btnBuscarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                String rut = txtModificarRut.Text;
                int contador = conector.Clientes.Count(cli => cli.RutCliente == rut);
                if(contador == 0)
                {
                    MessageBox.Show("No hay clientes con ese Rut.");
                    txtModificarRut.Text = "";
                    txtModificarNombres.Text = "";
                    txtModificarApellidos.Text = "";
                    lblModificarRutSeleccionado.Content = "";
                    dpModificarFechaNacimiento.SelectedDate = null;
                    cbxModificarEstadoCivil.SelectedIndex = -1;
                    cbxModificarSexo.SelectedIndex = -1;
                    tabRegistro.Focus();
                }
                else
                {
                    Cliente c = conector.Clientes.First(cli => cli.RutCliente == rut);
                    btnModificarCliente.IsEnabled = true;
                    txtModificarRut.IsEnabled = false;
                    txtModificarNombres.IsEnabled = true;
                    txtModificarApellidos.IsEnabled = true;
                    dpModificarFechaNacimiento.IsEnabled = true;
                    cbxModificarSexo.IsEnabled = true;
                    cbxModificarEstadoCivil.IsEnabled = true;

                    txtModificarRut.Text = string.Empty;
                    lblModificarRutSeleccionado.Content = c.RutCliente;
                    txtModificarNombres.Text = c.Nombres;
                    txtModificarApellidos.Text = c.Apellidos;
                    dpModificarFechaNacimiento.SelectedDate = DateTime.Parse(c.FechaNacimiento);
                    cbxModificarSexo.SelectedIndex = c.IdSexo-1;
                    cbxModificarEstadoCivil.SelectedIndex = c.IdEstadoCivil-1;

                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tabRegistro.Focus();
            }
        }

        private void btnModificarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string rut = lblModificarRutSeleccionado.Content.ToString();
                Cliente c = conector.Clientes.First(cli => cli.RutCliente == rut);

                c.Nombres = txtModificarNombres.Text;
                c.Apellidos = txtModificarApellidos.Text;
                c.FechaNacimiento = dpModificarFechaNacimiento.SelectedDate.ToString();
                c.IdSexo = cbxModificarSexo.SelectedIndex + 1;
                c.IdEstadoCivil = cbxModificarEstadoCivil.SelectedIndex + 1;

                conector.SaveChanges();
                MessageBox.Show("Se ha modificado el cliente.");
                ActualizarListado();

                lblModificarRutSeleccionado.Content = string.Empty;
                txtModificarNombres.Text = string.Empty;
                txtModificarApellidos.Text = string.Empty;
                dpModificarFechaNacimiento.SelectedDate = DateTime.Now;
                cbxModificarSexo.SelectedIndex = -1;
                cbxModificarEstadoCivil.SelectedIndex = -1;
                btnModificarCliente.IsEnabled = false;

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tabRegistro.Focus();
            }
        }

        private void btnEliminarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string rut = txtModificarRut.Text;
                Cliente client = conector.Clientes.First(cli => cli.RutCliente == rut);
                int contratos = conector.Contratoes.Count(con => con.RutCliente == rut);

                if (contratos > 0)
                {
                    MessageBox.Show("El cliente no puede ser eliminado, porque tiene un contrato asociado.");
                } else
                {
                    conector.Clientes.Remove(client);
                    conector.SaveChanges();

                    MessageBox.Show("El cliente ha sido eliminado");
                    txtModificarRut.Text = string.Empty;
                    ActualizarListado();

                }

                tabRegistro.Focus();
                

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //filtros

        private void cbxFilterRut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //cbxFilterCivil.SelectedIndex = -1; ;
            //cbxFilterSexo.SelectedIndex = -1; ;
            cbxFilterCivil.IsEnabled = false;
            cbxFilterSexo.IsEnabled = false;
        }
        private void cbxFilterCivil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbxFilterRut.SelectedIndex = -1;
            cbxFilterRut.IsEnabled = false;
        }

        private void cbxFilterSexo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbxFilterRut.SelectedIndex = -1;
            cbxFilterRut.IsEnabled = false;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            if (cbxFilterRut.SelectedValue != null)
            {
                Cliente cli = (Cliente)cbxFilterRut.SelectedValue;
                List<Cliente> lstCliente = new List<Cliente>();
                lstCliente.Add(cli);
                dgListadoClientes.ItemsSource = null;
                dgListadoClientes.ItemsSource = lstCliente;
            }
            else if (cbxFilterSexo.SelectedValue != null & cbxFilterCivil.SelectedValue != null)
            {
                Sexo sex = (Sexo)cbxFilterSexo.SelectedValue;
                EstadoCivil estcivil = (EstadoCivil)cbxFilterCivil.SelectedValue;
                List<Cliente> lstCliente = conector.Clientes.Where(cli => cli.IdSexo == sex.IdSexo & cli.IdEstadoCivil == estcivil.IdEstadoCivil).ToList();
                dgListadoClientes.ItemsSource = null;
                dgListadoClientes.ItemsSource = lstCliente;

            }
            else if (cbxFilterSexo.SelectedValue != null)
            {
                Sexo sex = (Sexo)cbxFilterSexo.SelectedValue;
                List<Cliente> lstCliente = conector.Clientes.Where(cli => cli.IdSexo == sex.IdSexo).ToList();
                dgListadoClientes.ItemsSource = null;
                dgListadoClientes.ItemsSource = lstCliente;

            }
            else if (cbxFilterCivil.SelectedValue != null)
            {
                EstadoCivil estcivil = (EstadoCivil)cbxFilterCivil.SelectedValue;
                List<Cliente> lstCliente = conector.Clientes.Where(cli => cli.IdEstadoCivil == estcivil.IdEstadoCivil).ToList();
                dgListadoClientes.ItemsSource = null;
                dgListadoClientes.ItemsSource = lstCliente;

            }
            else
            {
                MessageBox.Show("Tiene que seleccionar un filtro");
            }


        }

        private void BtnFilterReset_Click(object sender, RoutedEventArgs e)
        {
            cbxFilterCivil.SelectedIndex = -1;
            cbxFilterCivil.IsEnabled = true;
            cbxFilterRut.SelectedIndex = -1;
            cbxFilterRut.IsEnabled = true;
            cbxFilterSexo.SelectedIndex = -1;
            cbxFilterSexo.IsEnabled = true;
            ActualizarListado();
        }

        private void cbxModificarRut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxModificarRut.SelectedIndex != -1)
            {
                string rut = cbxModificarRut.SelectedValue.ToString();
                txtModificarRut.Text = rut;
            }
        }
    }
}
