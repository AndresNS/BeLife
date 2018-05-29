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
    /// Lógica de interacción para GestionContratos.xaml
    /// </summary>
    public partial class GestionContratos : Window
    {
        Datos.BeLifeEntities conector;
        public GestionContratos()
        {
            InitializeComponent();
            if (conector == null)
            {
                conector = new BeLifeEntities();
            }
            ActualizarListadoContratos();
            cbxRegContratoPlan.ItemsSource = conector.Plans.ToList();
            cbxModContratoNumero.ItemsSource = conector.Contratoes.ToList();
            dpRegContratoInicioVigencia.SelectedDate = DateTime.Now;
            cbxRegContratoRut.ItemsSource = conector.Clientes.ToList();

            cbxFilterConRut.ItemsSource = conector.Clientes.ToList();
            cbxFilterNC.ItemsSource = conector.Contratoes.ToList();
            cbxFilterNP.ItemsSource = conector.Plans.ToList();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rbRegistroIngRut.IsChecked = true;
            cbxRegContratoRut.Visibility = Visibility.Hidden;
        }

        private void ActualizarListadoContratos()
        {
            dgListadoContratos.ItemsSource = null;
            dgListadoContratos.ItemsSource = conector.Contratoes.ToList();
        }

        private void btnRegContratoRegistrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((rbRegContratoSaludSi.IsChecked == false && rbRegContratoSaludNo.IsChecked == false) ||
                        txtRegContratoRut.Text == "" || cbxRegContratoPlan.SelectedIndex < 0 ||
                        txtRegContratoObservaciones.Text == "")
                {
                    MessageBox.Show("Debe llenar todos los campos.");
                }
                else
                {
                    DateTime fecVig = (DateTime)dpRegContratoInicioVigencia.SelectedDate;
                    if (fecVig < DateTime.Now || fecVig > DateTime.Now.AddMonths(1))
                    {
                        MessageBox.Show("La fecha de vigencia no puede ser anterior a la fecha actual ni exceder de 1 mes.");
                    }
                    else
                    {
                        
                        Contrato c = new Contrato();
                        string numContrato = DateTime.Now.ToString("yyyyMMddHHmmss");
                        c.Numero = numContrato;
                        c.FechaCreacion = DateTime.Now;
                        string rutCli = txtRegContratoRut.Text;
                        int contadorRut = conector.Clientes.Count(client => client.RutCliente == rutCli);
                        if (contadorRut > 0)
                        {

                            c.RutCliente = rutCli;
                            DateTime InicioVigencia = (DateTime)dpRegContratoInicioVigencia.SelectedDate;
                            c.FechaInicioVigencia = InicioVigencia;
                            c.FechaFinVigencia = InicioVigencia.AddYears(1);
                            c.Vigente = true;
                            c.Observaciones = txtRegContratoObservaciones.Text;
                            c.FechaTermino = InicioVigencia.AddYears(1);
                            int planint = cbxRegContratoPlan.SelectedIndex;
                            string plan = "";
                            switch (planint)
                            {
                                case 0:
                                    plan = "VID01";
                                    break;
                                case 1:
                                    plan = "VID02";
                                    break;
                                case 2:
                                    plan = "VID03";
                                    break;
                                case 3:
                                    plan = "VID04";
                                    break;
                                case 4:
                                    plan = "VID05";
                                    break;
                            }

                            c.CodigoPlan = plan;

                            /* INICIO CALCULO PRIMAS */

                            Cliente cli = conector.Clientes.First(clie => clie.RutCliente == rutCli);

                            string fecNac = cli.FechaNacimiento;
                            int sex = cli.IdSexo;
                            int eCivil = cli.IdEstadoCivil;

                            DateTime hoy = DateTime.Today;
                            DateTime fecNacDT = DateTime.Parse(fecNac);
                            int edad = hoy.Year - fecNacDT.Year;
                            if (fecNacDT > hoy.AddYears(-edad))
                            {
                                edad--;
                            }

                            double recargo = CalculoRecargo(edad, sex, eCivil);

                            Plan p = conector.Plans.First(pla => pla.IdPlan == plan);

                            double primaAnual = 0.0;
                            primaAnual = p.PrimaBase + recargo;

                            double primaMensual = primaAnual / 12;

                            c.PrimaAnual = primaAnual;
                            c.PrimaMensual = primaMensual;

                            /* FIN CALCULO PRIMAS */

                            if (rbRegContratoSaludSi.IsChecked == true)
                            {
                                c.DeclaracionSalud = true;
                            }
                            else
                            {
                                c.DeclaracionSalud = false;
                            }
                            conector.Contratoes.Add(c);
                            conector.SaveChanges();
                            MessageBox.Show("Se ha ingresado un contrato.");
                            ActualizarListadoContratos();

                            txtRegContratoRut.Text = "";
                            dpRegContratoInicioVigencia.SelectedDate = DateTime.Now;
                            rbRegContratoSaludSi.IsChecked = false;
                            rbRegContratoSaludNo.IsChecked = false;
                            txtRegContratoObservaciones.Text = "";
                            cbxRegContratoPlan.SelectedIndex = -1;
                        } else
                        {
                            MessageBox.Show("El rut ingresado no existe.");
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void btnModContratoIngNumero_Click(object sender, RoutedEventArgs e)
        {
            txtModContratoNumero.IsEnabled = true;
            cbxModContratoNumero.IsEnabled = false;
            btnModContratoBuscar.IsEnabled = true;

            cbxModContratoNumero.SelectedIndex = -1;
        }

        private void btnModContratoSelNumero_Click(object sender, RoutedEventArgs e)
        {
            cbxModContratoNumero.IsEnabled = true;
            txtModContratoNumero.IsEnabled = false;
            btnModContratoBuscar.IsEnabled = true;

            txtModContratoNumero.Text = "";

        }

        private void btnModContratoBuscar_Click(object sender, RoutedEventArgs e)
        {
            string num = "";

            if(txtModContratoNumero.IsEnabled == true)
            {
                num = txtModContratoNumero.Text;
                txtModContratoNumero.IsEnabled = false;
                cbxModContratoNumero.SelectedIndex = -1;
            }
            else
            {
                num = cbxModContratoNumero.SelectedValue.ToString();
                cbxModContratoNumero.IsEnabled = false;
                txtModContratoNumero.Text = num;
            }

            int cantContratos = conector.Contratoes.Count(con => con.Numero == num);
            if (cantContratos > 0)
            {
                Contrato c = conector.Contratoes.First(con => con.Numero == num);
                txtModContratoNumero.Text = c.Numero;

                txtModContratoRut.Text = c.RutCliente;
                lblModContratoFechaCreacion.Content = c.FechaCreacion;
                lblModContratoFechaTermino.Content = c.FechaTermino;
                lblModContratoInicioVigencia.Content = c.FechaInicioVigencia;
                lblModContratoFinVigencia.Content = c.FechaFinVigencia;

                if (c.FechaInicioVigencia > DateTime.Now && c.FechaFinVigencia < DateTime.Now)
                {
                    chbxModContratoVigente.IsChecked = false;
                }
                else
                {
                    chbxModContratoVigente.IsChecked = true;
                }


                cbxModContratoPlan.ItemsSource = conector.Plans.ToList();
                cbxModContratoPlan.SelectedValue = c.Plan;


                lblModContratoPrimaAnual.Content = c.PrimaAnual;
                lblModContratoPrimaMensual.Content = c.PrimaMensual;

                txtModContratoObservaciones.Text = c.Observaciones;

                rbModContratoSaludSi.IsEnabled = true;
                rbModContratoSaludNo.IsEnabled = true;

                if (c.DeclaracionSalud == true)
                {
                    rbModContratoSaludSi.IsChecked = true;
                }
                else
                {
                    rbModContratoSaludNo.IsChecked = true;
                }
                if (c.FechaTermino <= DateTime.Now)
                {
                    rbModContratoSaludSi.IsEnabled = false;
                    rbModContratoSaludNo.IsEnabled = false;
                    txtModContratoObservaciones.IsEnabled = false;

                    MessageBox.Show("Este contrato ya ha finalizado. No podrá realizar modificaciones.");
                }
                else
                {
                    txtModContratoObservaciones.IsEnabled = true;
                    btnModContratoGuardar.IsEnabled = true;
                    btnModContratoTerminar.IsEnabled = true;
                }

            } else
            {
                MessageBox.Show("No existen contratos con ese Número de contrato");
                tabRegistroContrato.Focus();
            }

            
        }

        private void btnModContratoTerminar_Click(object sender, RoutedEventArgs e)
        {
            string num = txtModContratoNumero.Text;
            Contrato c = conector.Contratoes.First(con => con.Numero == num);

            c.FechaTermino = DateTime.Now;
            c.FechaFinVigencia = DateTime.Now;
            c.Vigente = false;

            conector.SaveChanges();
            MessageBox.Show("Se ha dado término al contrato.");
            ActualizarListadoContratos();

        }

        private void btnModContratoGuardar_Click(object sender, RoutedEventArgs e)
        {
            string num = txtModContratoNumero.Text;

            Contrato c = conector.Contratoes.First(con => con.Numero == num);

            if (rbModContratoSaludSi.IsChecked == true)
            {
                c.DeclaracionSalud = true;
            }
            else
            {
                c.DeclaracionSalud = false;
            }

            c.Observaciones = txtModContratoObservaciones.Text;

            conector.SaveChanges();
            MessageBox.Show("Se ha actualizado el contrato.");
            ActualizarListadoContratos();
        }

        private void cbxRegContratoRut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbxRegContratoRut.SelectedIndex != -1)
            {
                string rut = cbxRegContratoRut.SelectedValue.ToString();
                txtRegContratoRut.Text = rut;
            }
            
        }

        private void cbxRegContratoPlan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxRegContratoPlan.SelectedIndex != -1)
            {
                string rutCli = txtRegContratoRut.Text;
                int planint = cbxRegContratoPlan.SelectedIndex;
                string plan = "";
                switch (planint)
                {
                    case 0:
                        plan = "VID01";
                        break;
                    case 1:
                        plan = "VID02";
                        break;
                    case 2:
                        plan = "VID03";
                        break;
                    case 3:
                        plan = "VID04";
                        break;
                    case 4:
                        plan = "VID05";
                        break;
                }

                Plan p = conector.Plans.First(pla => pla.IdPlan == plan);
                string poliza = p.PolizaActual;
                lblRegContratoPoliza.Content = poliza;

                /* INICIO CALCULO PRIMAS */

                Cliente cli = conector.Clientes.First(clie => clie.RutCliente == rutCli);

                string fecNac = cli.FechaNacimiento;
                int sex = cli.IdSexo;
                int eCivil = cli.IdEstadoCivil;

                DateTime hoy = DateTime.Today;
                DateTime fecNacDT = DateTime.Parse(fecNac);
                int edad = hoy.Year - fecNacDT.Year;
                if (fecNacDT > hoy.AddYears(-edad))
                {
                    edad--;
                }

                double recargo = CalculoRecargo(edad, sex, eCivil);

                Plan pl = conector.Plans.First(pla => pla.IdPlan == plan);

                double primaAnual = 0.0;
                primaAnual = pl.PrimaBase + recargo;

                double primaMensual = primaAnual / 12;

                /* FIN CALCULO PRIMAS */
                lblRegContratoPrimaAnual.Content = primaAnual.ToString();
                lblRegContratoPrimaMensual.Content = primaMensual.ToString();

            }
        }

        public double CalculoRecargo(int edad, int sex, int eCivil)
        {
            double recargo = 0.0;

            if (edad < 26)
            {
                recargo = recargo + 3.6;
                switch (sex)
                {
                    case 1:
                        recargo = recargo + 2.4;
                        break;
                    case 2:
                        recargo = recargo + 1.2;
                        break;
                }

                switch (eCivil)
                {
                    case 1:
                        recargo = recargo + 4.8;
                        break;
                    case 2:
                        recargo = recargo + 2.4;
                        break;
                    default:
                        recargo = recargo + 3.6;
                        break;
                }
            }
            else if (edad < 46)
            {
                recargo = recargo + 2.4;
                switch (sex)
                {
                    case 1:
                        recargo = recargo + 2.4;
                        break;
                    case 2:
                        recargo = recargo + 1.2;
                        break;
                }

                switch (eCivil)
                {
                    case 1:
                        recargo = recargo + 4.8;
                        break;
                    case 2:
                        recargo = recargo + 2.4;
                        break;
                    default:
                        recargo = recargo + 3.6;
                        break;
                }
            }
            else
            {
                recargo = recargo + 6.0;
                switch (sex)
                {
                    case 1:
                        recargo = recargo + 2.4;
                        break;
                    case 2:
                        recargo = recargo + 1.2;
                        break;
                }

                switch (eCivil)
                {
                    case 1:
                        recargo = recargo + 4.8;
                        break;
                    case 2:
                        recargo = recargo + 2.4;
                        break;
                    default:
                        recargo = recargo + 3.6;
                        break;
                }
            }

            return recargo;
        }

        //filtros
        //filtros
        private void btnFilterContract_Click(object sender, RoutedEventArgs e)
        {
            if (cbxFilterConRut.SelectedValue != null)
            {
                dgListadoContratos.ItemsSource = conector.Contratoes.ToList().Where(
                    i => i.RutCliente == cbxFilterConRut.SelectedItem.ToString()
                );
            }
            else if (cbxFilterNC.SelectedValue != null)
            {
                dgListadoContratos.ItemsSource = conector.Contratoes.ToList().Where(
                    i => i.Numero == cbxFilterNC.SelectedItem.ToString()
                );
            }
            else if (cbxFilterNP.SelectedValue != null)
            {
                string plan = cbxFilterNP.SelectedValue.ToString();
                Plan p = conector.Plans.First(pla => pla.Nombre == plan);
                string poliza = p.PolizaActual;

                dgListadoContratos.ItemsSource = conector.Contratoes.ToList().Where(
                    i => i.Plan.PolizaActual == poliza);
            }
        }

        private void btnFilterReset_Click(object sender, RoutedEventArgs e)
        {
            cbxFilterConRut.SelectedIndex = -1;
            cbxFilterNC.SelectedIndex = -1;
            cbxFilterNP.SelectedIndex = -1;
            ActualizarListadoContratos();
        }

        private void btnRegBuscarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                String rut = txtRegContratoRut.Text;
                int contador = conector.Clientes.Count(cli => cli.RutCliente == rut);
                if (contador == 0)
                {
                    MessageBox.Show("No hay clientes con ese Rut.");

                    cbxRegContratoPlan.SelectedIndex = -1;
                    dpRegContratoInicioVigencia.SelectedDate = null;
                    rbRegContratoSaludSi.IsChecked = false;
                    rbRegContratoSaludNo.IsChecked = false;
                    txtRegContratoObservaciones.Text = string.Empty;

                    cbxRegContratoPlan.IsEnabled = false;
                    dpRegContratoInicioVigencia.IsEnabled = false;
                    rbRegContratoSaludSi.IsEnabled = false;
                    rbRegContratoSaludNo.IsEnabled = false;
                    txtRegContratoObservaciones.IsEnabled = false;
                    btnRegContratoRegistrar.IsEnabled = false;
                }
                else
                {
                    Cliente c = conector.Clientes.First(cli => cli.RutCliente == rut);
                    cbxRegContratoPlan.IsEnabled = true;
                    dpRegContratoInicioVigencia.IsEnabled = true;
                    rbRegContratoSaludSi.IsEnabled = true;
                    rbRegContratoSaludNo.IsEnabled = true;
                    txtRegContratoObservaciones.IsEnabled = true;
                    btnRegContratoRegistrar.IsEnabled = true;

                    txtRegContratoRut.Text = c.RutCliente;
                    //lblRegContratoRut.Content = c.RutCliente;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                tabRegistroContrato.Focus();
            }
        }

        private void rbRegistroBuscarRut_Checked(object sender, RoutedEventArgs e)
        {
            if (rbRegistroIngRut.IsChecked == true)
            {
                cbxRegContratoRut.Visibility = Visibility.Hidden;
                txtRegContratoRut.Visibility = Visibility.Visible;
            }
            else
            {
                cbxRegContratoRut.Visibility = Visibility.Visible;
                cbxRegContratoRut.SelectedIndex = 0;
                txtRegContratoRut.Visibility = Visibility.Hidden;

            }
        }
    }
}
