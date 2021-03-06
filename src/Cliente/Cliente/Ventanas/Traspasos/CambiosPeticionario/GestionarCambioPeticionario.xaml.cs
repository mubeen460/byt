﻿using System.Windows;
using System.Windows.Controls;
using Trascend.Bolet.Cliente.Contratos.Traspasos.CambiosDePeticionario;
using Trascend.Bolet.Cliente.Presentadores.Traspasos.CambiosDePeticionario;
using Trascend.Bolet.Cliente.Ayuda;
using System.Windows.Media;
using Trascend.Bolet.Cliente.Presentadores.Traspasos.Licencias;

namespace Trascend.Bolet.Cliente.Ventanas.Traspasos.CambiosPeticionario
{
    /// <summary>
    /// Interaction logic for GestionarFusion.xaml
    /// </summary>
    public partial class GestionarCambioPeticionario : Page, IGestionarCambioDePeticionario
    {

        private PresentadorGestionarCambioDePeticionario _presentador;
        private bool _cargada;
        private GridViewColumnHeader _CurSortCol = null;
        private SortAdorner _CurAdorner = null;

        #region IGestionarCesion

        public void EsMarcaNacional(bool marcaNacional)
        {
            if (marcaNacional)
            {
                this._radioExtranjero.IsChecked = !marcaNacional;
                this._radioNacional.IsChecked = marcaNacional;
            }
            else
            {
                this._radioExtranjero.IsChecked = !marcaNacional;
                this._radioNacional.IsChecked = marcaNacional;
            }
        }

        public void BorrarCerosInternacional()
        {
            if (this._txtIdMarcaInt.Text.Equals("0"))
                this._txtIdMarcaInt.Text = ""; // cambio el texto del textbox para que no aparezca el "0"
            if (this._txtIdMarcaIntCor.Text.Equals("0"))
                this._txtIdMarcaIntCor.Text = ""; // cambio el texto del textbox para que no aparezca el "0"
        }

        public string TipoClase
        {
            set { this._txtClasificacionInt.Text = value; }
        }

        public bool EstaCargada
        {
            get { return this._cargada; }
            set { this._cargada = value; }
        }

        public string IdAnteriorFiltrar
        {
            get { return this._txtIdAnteriorFiltrar.Text; }
        }

        public string NombreAnteriorFiltrar
        {
            get { return this._txtNombreAnteriorFiltrar.Text; }
        }

        public string IdActualFiltrar
        {
            get { return this._txtIdActualFiltrar.Text; }
        }

        public string NombreActualFiltrar
        {
            get { return this._txtNombreActualFiltrar.Text; }
        }

        public string IdPoderActualFiltrar
        {
            get { return this._txtIdPoderActualFiltrar.Text; }
        }

        public string FechaPoderActualFiltrar
        {
            get { return this._dpkFechaPoderActualFiltrar.Text; }
        }

        public string IdPoderAnteriorFiltrar
        {
            get { return this._txtIdPoderAnteriorFiltrar.Text; }
        }

        public string FechaPoderAnteriorFiltrar
        {
            get { return this._dpkFechaPoderAnteriorFiltrar.Text; }
        }

        public string IdApoderadoActualFiltrar
        {
            get { return this._txtIdApoderadoActualFiltrar.Text; }
        }

        public string NombreApoderadoActualFiltrar
        {
            get { return this._txtNombreApoderadoActualFiltrar.Text; }
        }

        public string IdApoderadoAnteriorFiltrar
        {
            get { return this._txtIdApoderadoAnteriorFiltrar.Text; }
        }

        public string NombreApoderadoAnteriorFiltrar
        {
            get { return this._txtNombreApoderadoAnteriorFiltrar.Text; }
        }

        public string IdMarcaFiltrar
        {
            get { return this._txtIdMarcaFiltrar.Text; }
        }

        public string NombreMarcaFiltrar
        {
            get { return this._txtNombreMarcaFiltrar.Text; }
        }

        public string TipoClaseNacional
        {
            get { return this._txtTipo.Text; }
            set { this._txtTipo.Text = value; }
        }

        public object Marca
        {
            get { return this._gridDatosMarca.DataContext; }
            set { this._gridDatosMarca.DataContext = value; }
        }

        public object InteresadoAnterior
        {
            get { return this._gridDatosAnterior.DataContext; }
            set { this._gridDatosAnterior.DataContext = value; }
        }

        public object InteresadoActual
        {
            get { return this._gridDatosActual.DataContext; }
            set { this._gridDatosActual.DataContext = value; }
        }

        public object ApoderadoAnterior
        {
            get { return this._gridDatosApoderadoAnterior.DataContext; }
            set { this._gridDatosApoderadoAnterior.DataContext = value; }
        }

        public object ApoderadoActual
        {
            get { return this._gridDatosApoderadoActual.DataContext; }
            set { this._gridDatosApoderadoActual.DataContext = value; }
        }

        public object PoderAnterior
        {
            get { return this._gridDatosPoderAnterior.DataContext; }
            set { this._gridDatosPoderAnterior.DataContext = value; }
        }

        public object PoderActual
        {
            get { return this._gridDatosPoderActual.DataContext; }
            set { this._gridDatosPoderActual.DataContext = value; }
        }

        public object MarcasFiltradas
        {
            get { return this._lstMarcas.DataContext; }
            set { this._lstMarcas.DataContext = value; }
        }

        public object AnteriorsFiltrados
        {
            get { return this._lstAnteriors.DataContext; }
            set { this._lstAnteriors.DataContext = value; }
        }

        public object ActualsFiltrados
        {
            get { return this._lstActuals.DataContext; }
            set { this._lstActuals.DataContext = value; }
        }

        public object ApoderadosAnteriorFiltrados
        {
            get { return this._lstApoderadosAnterior.DataContext; }
            set { this._lstApoderadosAnterior.DataContext = value; }
        }

        public object ApoderadosActualFiltrados
        {
            get { return this._lstApoderadosActual.DataContext; }
            set { this._lstApoderadosActual.DataContext = value; }
        }

        public object PoderesAnteriorFiltrados
        {
            get { return this._lstPoderesAnterior.DataContext; }
            set { this._lstPoderesAnterior.DataContext = value; }
        }

        public object PoderesActualFiltrados
        {
            get { return this._lstPoderesActual.DataContext; }
            set { this._lstPoderesActual.DataContext = value; }
        }

        public object CambioPeticionario
        {
            get { return this._gridDatos.DataContext; }
            set { this._gridDatos.DataContext = value; }
        }

        public object MarcaFiltrada
        {
            get { return this._lstMarcas.SelectedItem; }
            set { this._lstMarcas.SelectedItem = value; }
        }

        public object AnteriorFiltrado
        {
            get { return this._lstAnteriors.SelectedItem; }
            set { this._lstAnteriors.SelectedItem = value; }
        }

        public object ActualFiltrado
        {
            get { return this._lstActuals.SelectedItem; }
            set { this._lstActuals.SelectedItem = value; }
        }

        public object ApoderadoAnteriorFiltrado
        {
            get { return this._lstApoderadosAnterior.SelectedItem; }
            set { this._lstApoderadosAnterior.SelectedItem = value; }
        }

        public object ApoderadoActualFiltrado
        {
            get { return this._lstApoderadosActual.SelectedItem; }
            set { this._lstApoderadosActual.SelectedItem = value; }
        }

        public object PoderAnteriorFiltrado
        {
            get { return this._lstPoderesAnterior.SelectedItem; }
            set { this._lstPoderesAnterior.SelectedItem = value; }
        }

        public object PoderActualFiltrado
        {
            get { return this._lstPoderesActual.SelectedItem; }
            set { this._lstPoderesActual.SelectedItem = value; }
        }

        public string Region
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public void FocoPredeterminado()
        {
            this._txtIdCambioPeticionario.Focus();
        }

        public bool HabilitarCampos
        {
            set
            {
                this._txtAsociado.IsEnabled = value;
                this._txtIdAsociado.IsEnabled = value;
                this._txtClaseInternacional.IsEnabled = value;
                this._txtClaseNacional.IsEnabled = value;
                this._txtExpediente.IsEnabled = value;
                this._txtIdCambioPeticionario.IsEnabled = value;
                this._txtIdMarcaFiltrar.IsEnabled = value;
                this._txtNombreMarca.IsEnabled = value;
                this._txtIdMarca.IsEnabled = value;
                this._txtNombreMarcaFiltrar.IsEnabled = value;
                this._txtNumInscripcion.IsEnabled = value;
                this._txtNumRegistro.IsEnabled = value;
                this._chkEtiqueta.IsEnabled = value;
                this._txtTipo.IsEnabled = value;
                this._txtUbicacion.IsEnabled = value;
                this._btnConsultarMarca.IsEnabled = value;
                this._dpkFechaCambioPeticionario.IsEnabled = value;

                this._txtNombreAnterior.IsEnabled = value;
                this._txtIdAnterior.IsEnabled = value;
                this._txtPaisAnterior.IsEnabled = value;
                this._txtNacionalidadAnterior.IsEnabled = value;
                this._txtIdAnteriorFiltrar.IsEnabled = value;
                this._txtNombreAnteriorFiltrar.IsEnabled = value;
                this._txtNombreApoderadoAnterior.IsEnabled = value;
                this._txtIdApoderadoAnterior.IsEnabled = value;
                this._txtNombreApoderadoAnteriorFiltrar.IsEnabled = value;
                this._txtIdApoderadoAnteriorFiltrar.IsEnabled = value;
                this._txtIdPoderAnterior.IsEnabled = value;
                this._txtIdPoderAnteriorFiltrar.IsEnabled = value;
                this._txtAnexoPoderAnterior.IsEnabled = value;
                this._txtBoletinPoderAnterior.IsEnabled = value;
                this._txtFacultadPoderAnterior.IsEnabled = value;
                this._txtNumPoderAnterior.IsEnabled = value;
                this._txtFechaPoderAnterior.IsEnabled = value;

                this._txtNombreActual.IsEnabled = value;
                this._txtIdActual.IsEnabled = value;
                this._txtIdActualFiltrar.IsEnabled = value;
                this._txtNombreActualFiltrar.IsEnabled = value;
                this._txtIdActualFiltrar.IsEnabled = value;
                this._txtNombreApoderadoActual.IsEnabled = value;
                this._txtIdApoderadoActual.IsEnabled = value;
                this._txtIdApoderadoActualFiltrar.IsEnabled = value;
                this._txtNombreApoderadoActualFiltrar.IsEnabled = value;
                this._txtIdPoderActual.IsEnabled = value;
                this._txtIdPoderActualFiltrar.IsEnabled = value;

                this._txtAnexoPoderActual.IsEnabled = value;
                this._txtBoletinPoderActual.IsEnabled = value;
                this._txtFacultadPoderActual.IsEnabled = value;
                this._txtNumPoderActual.IsEnabled = value;
                this._txtFechaPoderActual.IsEnabled = value;
                this._txtPaisActual.IsEnabled = value;
                this._txtNacionalidadActual.IsEnabled = value;

                this._txtObservacionCambioPeticionario.IsEnabled = value;
                this._txtReferenciaCambioPeticionario.IsEnabled = value;
                this._txtAnexoCambioPeticionario.IsEnabled = value;
                this._txtComentarioCambioPeticionario.IsEnabled = value;
                this._txtObservacionCambioPeticionario.IsEnabled = value;
                this._txtReferenciaCambioPeticionario.IsEnabled = value;
                this._txtAnexoCambioPeticionario.IsEnabled = value;
                this._txtComentarioCambioPeticionario.IsEnabled = value;
                this._chkAsientoEnLibro.IsEnabled = value;
                this._cbxBoletin.IsEnabled = value;

                this._btnIrMarcas.IsEnabled = value;
                this._btnConsultarCadenaCambios.IsEnabled = value;
                this._txtCadenaCambios.IsEnabled = value;
                this._btnIrAsociados.IsEnabled = value;

                this._btnVerInteresadoAnterior.IsEnabled = value;
                this._btnVerInteresadoActual.IsEnabled = value;

                #region Internacional

                this._txtIdMarcaInt.IsEnabled = value;
                this._txtIdMarcaIntCor.IsEnabled = value;
                this._txtPaisInt.IsEnabled = value;
                this._txtClaseInternacionalSolicitud.IsEnabled = value;
                this._txtClasificacionInt.IsEnabled = value;

                #endregion
            }
        }

        public string NombreMarca
        {
            set { this._txtNombreMarca.Text = value; }
        }

        public string IdMarca
        {
            get { return this._txtIdMarca.Text; }
            set { this._txtIdMarca.Text = value; }
        }

        public string NombreAnterior
        {
            set { this._txtNombreAnterior.Text = value; }
        }

        public string IdAnterior
        {

            get { return this._txtIdAnterior.Text; }
            set { this._txtIdAnterior.Text = value; }
        }

        public string NombreApoderadoAnterior
        {
            set { this._txtNombreApoderadoAnterior.Text = value; }
        }

        public string IdApoderadoAnterior
        {
            get { return this._txtIdApoderadoAnterior.Text; }
            set { this._txtIdApoderadoAnterior.Text = value; }
        }

        public string NombreApoderadoActual
        {
            set { this._txtNombreApoderadoActual.Text = value; }
        }

        public string IdApoderadoActual
        {
            get { return this._txtIdApoderadoActual.Text; }
            set { this._txtIdApoderadoActual.Text = value; }
        }

        public string IdPoderAnterior
        {
            get { return this._txtIdPoderAnterior.Text; }
            set { this._txtIdPoderAnterior.Text = value; }
        }

        public object Boletines
        {
            get { return this._cbxBoletin.DataContext; }
            set { this._cbxBoletin.DataContext = value; }
        }

        public object Boletin
        {
            get { return this._cbxBoletin.SelectedItem; }
            set { this._cbxBoletin.SelectedItem = value; }
        }

        public string IdPoderActual
        {
            get { return this._txtIdPoderActual.Text; }
            set { this._txtIdPoderActual.Text = value; }
        }

        public string NombreActual
        {
            set { this._txtNombreActual.Text = value; }
        }

        public string IdActual
        {
            get { return this._txtIdActual.Text; }
            set { this._txtIdActual.Text = value; }
        }

        public string PaisAnterior
        {
            set { this._txtPaisAnterior.Text = value; }
        }

        public string PaisActual
        {
            set { this._txtPaisActual.Text = value; }
        }

        public string NacionalidadAnterior
        {
            set { this._txtNacionalidadAnterior.Text = value; }
        }

        public string NacionalidadActual
        {
            set { this._txtNacionalidadActual.Text = value; }
        }

        public string TextoBotonModificar
        {
            get { return this._txbModificar.Text; }
            set { this._txbModificar.Text = value; }
        }

        public string TextoBotonRegresar
        {
            get { return this._txbRegresar.Text; }
            set { this._txbRegresar.Text = value; }
        }

        public GridViewColumnHeader CurSortCol
        {
            get { return _CurSortCol; }
            set { _CurSortCol = value; }
        }

        public SortAdorner CurAdorner
        {
            get { return _CurAdorner; }
            set { _CurAdorner = value; }
        }

        public void Mensaje(string mensaje, int opcion)
        {
            if (opcion == 0)
                MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show(mensaje, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public string IdCadenaDeCambios
        {
            get { return this._txtCadenaCambios.Text; }
            set { this._txtCadenaCambios.Text = value; }
        }

        #endregion

        public GestionarCambioPeticionario(object cambioPeticionario)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorGestionarCambioDePeticionario(this, cambioPeticionario);
        }

        /// <summary>
        /// Constructor para la consulta desde operaciones
        /// </summary>
        /// <param name="cambioPeticionario">la cambioPeticionario a mostrar</param>
        /// <param name="visibilidad">parametro que indica la visibilidad de los botones</param>
        public GestionarCambioPeticionario(object cambioPeticionario, object parametro)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorGestionarCambioDePeticionario(this, cambioPeticionario);


            if (parametro.GetType() == typeof(System.Windows.Visibility))
            {
                this._btnModificar.Visibility = (System.Windows.Visibility)parametro;
                this._btnEliminar.Visibility = (System.Windows.Visibility)parametro;
            }
            else if (parametro.GetType() == typeof(ConsultarCambiosPeticionario))
            {
                _presentador._ventanaPadre = parametro;
            }
        }


        /// <summary>
        /// Constructor para la consulta desde operaciones que admite una ventana Padre
        /// </summary>
        /// <param name="cambioPeticionario">la cambioPeticionario a mostrar</param>
        /// <param name="parametro">Visibilidad de los botones</param>
        /// <param name="ventanaPadre">Ventana desde donde se llama a esta ventana</param>
        public GestionarCambioPeticionario(object cambioPeticionario, object parametro, object ventanaPadre)
        {
            InitializeComponent();
            this._cargada = false;
        
            //this._presentador = new PresentadorGestionarCambioDePeticionario(this, cambioPeticionario);
            this._presentador = new PresentadorGestionarCambioDePeticionario(this, cambioPeticionario,ventanaPadre);

            if (parametro.GetType() == typeof(System.Windows.Visibility))
            {
                this._btnModificar.Visibility = (System.Windows.Visibility)parametro;
                this._btnEliminar.Visibility = (System.Windows.Visibility)parametro;
            }
            else if (parametro.GetType() == typeof(ConsultarCambiosPeticionario))
            {
                _presentador._ventanaPadre = parametro;
            }
        }




        public void PintarAsociado(string tipo)
        {
            SolidColorBrush color;

            if (tipo.Equals("1"))
            {
                color = Brushes.LightGreen;
            }
            else if (tipo.Equals("2"))
            {
                color = Brushes.LightBlue;
            }
            else if (tipo.Equals("3"))
            {
                color = Brushes.LightYellow;
            }
            else if (tipo.Equals("4"))
            {
                color = Brushes.Pink;
            }
            else color = Brushes.White;

            this._txtAsociado.Background = color;
            this._txtIdAsociado.Background = color;

        }


        public void ActivarControlesAlAgregar()
        {
            this._btnEliminar.Visibility = System.Windows.Visibility.Collapsed;
            this._lblIdCambioPeticionario.Visibility = System.Windows.Visibility.Collapsed;
            this._txtIdCambioPeticionario.Visibility = System.Windows.Visibility.Collapsed;
            this._dpkFechaCambioPeticionario.IsEnabled = true;
        }

        private void _btnModificar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.Modificar();
        }

        private void _btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            if (this.TextoBotonRegresar == Recursos.Etiquetas.btnRegresar)
                this._presentador.RegresarVentanaPadre();
            else if (this.TextoBotonRegresar == Recursos.Etiquetas.btnCancelar)
                this._presentador.Cancelar();
        }

        private void _btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show(Recursos.MensajesConElUsuario.ConfirmacionEliminarCambioPeticionario,
               "Eliminar Cambio peticionario", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                this._presentador.Eliminar();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!EstaCargada)
            {
                this._presentador.CargarPagina();
                EstaCargada = true;
            }
        }

        private void _btnConsultar(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name.Equals("_btnConsultarMarca"))
                this._presentador.ConsultarMarcas();
            else if (((Button)sender).Name.Equals("_btnConsultarAnterior"))
                this._presentador.ConsultarAnteriors();
            else if (((Button)sender).Name.Equals("_btnConsultarApoderadoAnterior"))
                this._presentador.ConsultarApoderadosAnterior();
            else if (((Button)sender).Name.Equals("_btnConsultarPoderAnterior"))
                this._presentador.ConsultarPoderesAnterior();
            else if (((Button)sender).Name.Equals("_btnConsultarActual"))
                this._presentador.ConsultarActuals();
            else if (((Button)sender).Name.Equals("_btnConsultarApoderadoActual"))
                this._presentador.ConsultarApoderadosActual();
            else if (((Button)sender).Name.Equals("_btnConsultarPoderActual"))
                this._presentador.ConsultarPoderesActual();
        }

        //private void _btnPlanillaVienen_Click(object sender, RoutedEventArgs e)
        //{
        //    this._presentador.IrImprimir(((Button)sender).Name);
        //}

        //private void _btnPlanillaVan_Click(object sender, RoutedEventArgs e)
        //{
        //    this._presentador.IrImprimir(((Button)sender).Name);
        //}

        //private void _btnPlanilla_Click(object sender, RoutedEventArgs e)
        //{
        //    this._presentador.IrImprimir(((Button)sender).Name);
        //}

        //private void _btnCarpeta_Click(object sender, RoutedEventArgs e)
        //{
        //    this._presentador.IrImprimir(((Button)sender).Name);
        //}

        //private void _btnAnexo_Click(object sender, RoutedEventArgs e)
        //{
        //    this._presentador.IrImprimir(((Button)sender).Name);
        //}

        public void ConvertirEnteroMinimoABlanco()
        {

            if (!this.IdMarca.Equals(""))
            {
                if (int.Parse(this.IdMarca) == int.MinValue)
                    this.IdMarca = "";
            }

            if (!this.IdAnterior.Equals(""))
            {
                if (int.Parse(this.IdAnterior) == int.MinValue)
                    this.IdAnterior = "";
            }

            if (!this.IdPoderAnterior.Equals(""))
            {
                if (int.Parse(this.IdPoderAnterior) == int.MinValue)
                    this.IdPoderAnterior = "";
            }

            if (!this.IdActual.Equals(""))
            {
                if (int.Parse(this.IdActual) == int.MinValue)
                    this.IdActual = "";
            }

            if (!this.IdPoderActual.Equals(""))
            {
                if (int.Parse(this.IdPoderActual) == int.MinValue)
                    this.IdPoderActual = "";
            }

        }

        public void GestionarBotonConsultarInteresados(string tipo, bool value)
        {
            if (tipo.Equals("Anterior"))
            {
                this._btnConsultarAnterior.IsEnabled = value;
            }
            else if (tipo.Equals("Actual"))
            {
                this._btnConsultarActual.IsEnabled = value;
            }
        }

        public void GestionarBotonConsultarApoderados(string tipo, bool value)
        {
            if (tipo.Equals("Anterior"))
            {
                this._btnConsultarApoderadoAnterior.IsEnabled = value;
            }
            else if (tipo.Equals("Actual"))
            {
                this._btnConsultarApoderadoActual.IsEnabled = value;
            }
        }

        public void GestionarBotonConsultarPoderes(string tipo, bool value)
        {
            if (tipo.Equals("Anterior"))
            {
                this._btnConsultarPoderAnterior.IsEnabled = value;
            }
            else if (tipo.Equals("Actual"))
            {
                this._btnConsultarPoderActual.IsEnabled = value;
            }
        }


        private void _btnIrAsociados_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.IrVentanaAsociado();
        }

        #region Marca

        private void _lstMarcas_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._presentador.CambiarMarca())
            {
                GestionarVisibilidadDatosDeMarca(Visibility.Visible);
                GestionarVisibilidadFiltroMarca(Visibility.Collapsed);
            }
        }

        private void _OrdenarMarcas_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstMarcas);
        }

        private void _txtNombreMarca_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GestionarVisibilidadDatosDeMarca(Visibility.Collapsed);

            GestionarVisibilidadFiltroAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroMarca(Visibility.Visible);

            GestionarVisibilidadDatosDeAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDeActual(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoActual(Visibility.Visible);

            GestionarVisibilidadDatosDePoderActual(Visibility.Visible);

            GestionarVisibilidadDatosDePoderAnterior(Visibility.Visible);
        }

        private void GestionarVisibilidadDatosDeMarca(object value)
        {
            this._txtNombreMarca.Visibility = (System.Windows.Visibility)value;
            this._txtIdMarca.Visibility = (System.Windows.Visibility)value;
            this._chkEtiqueta.Visibility = (System.Windows.Visibility)value;
            this._lblNoInscripcion.Visibility = (System.Windows.Visibility)value;
            this._txtNumInscripcion.Visibility = (System.Windows.Visibility)value;
            this._lblNoRegistro.Visibility = (System.Windows.Visibility)value;
            this._txtNumRegistro.Visibility = (System.Windows.Visibility)value;
            this._lblTipo.Visibility = (System.Windows.Visibility)value;
            this._txtTipo.Visibility = (System.Windows.Visibility)value;
            this._lblClaseNacional.Visibility = (System.Windows.Visibility)value;
            this._txtClaseNacional.Visibility = (System.Windows.Visibility)value;
            this._lblClaseInternacional.Visibility = (System.Windows.Visibility)value;
            this._txtClaseInternacional.Visibility = (System.Windows.Visibility)value;
            this._lblAsociado.Visibility = (System.Windows.Visibility)value;
            this._txtAsociado.Visibility = (System.Windows.Visibility)value;
            this._txtIdAsociado.Visibility = (System.Windows.Visibility)value;
            this._btnIrMarcas.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarCadenaCambios.Visibility = (System.Windows.Visibility)value;
            this._txtCadenaCambios.Visibility = (System.Windows.Visibility)value;
            this._btnIrAsociados.Visibility = (System.Windows.Visibility)value;
        }

        private void GestionarVisibilidadFiltroMarca(object value)
        {
            this._lblNombreMarca.Visibility = (System.Windows.Visibility)value;
            this._txtNombreMarcaFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblIdMarca.Visibility = (System.Windows.Visibility)value;
            this._txtIdMarcaFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lstMarcas.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarMarca.Visibility = (System.Windows.Visibility)value;
            //this._btnIrAsociados.Visibility = (System.Windows.Visibility)value;
        }

        private void _txtMarcaFiltrar_GotFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultarMarca.IsDefault = true;
            this._btnModificar.IsDefault = false;
        }

        #endregion

        #region Anterior

        private void _lstAnterior_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._presentador.CambiarAnterior())
            {
                GestionarVisibilidadDatosDeAnterior(Visibility.Visible);
                GestionarVisibilidadFiltroAnterior(Visibility.Collapsed);

                if (this._presentador.VerificarCambioInteresado("Anterior"))
                {
                    //this._btnConsultarPoderAnterior.IsEnabled = false;
                    this._btnConsultarPoderAnterior.IsEnabled = true;
                }
                else
                {
                    this._btnConsultarPoderAnterior.IsEnabled = true;
                }

            }

        }

        private void _OrdenarAnterior_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstAnteriors);
        }

        private void _txtNombreAnterior_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GestionarVisibilidadDatosDeAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroMarca(Visibility.Collapsed);

            GestionarVisibilidadFiltroAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDeMarca(Visibility.Visible);

            GestionarVisibilidadDatosDeActual(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoActual(Visibility.Visible);

            GestionarVisibilidadDatosDePoderAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDePoderActual(Visibility.Visible);
        }

        private void GestionarVisibilidadDatosDeAnterior(object value)
        {
            //this._lblNombreAnterior.Visibility = (System.Windows.Visibility)value;
            this._btnVerInteresadoAnterior.Visibility = (System.Windows.Visibility)value;
            this._txtNombreAnterior.Visibility = (System.Windows.Visibility)value;
            this._txtIdAnterior.Visibility = (System.Windows.Visibility)value;
            this._txtPaisAnterior.Visibility = (System.Windows.Visibility)value;
            this._txtNacionalidadAnterior.Visibility = (System.Windows.Visibility)value;
        }

        private void GestionarVisibilidadFiltroAnterior(object value)
        {
            this._lblAnteriorFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblNombreAnteriorFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtNombreAnteriorFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblIdAnteriorFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtIdAnteriorFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lstAnteriors.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarAnterior.Visibility = (System.Windows.Visibility)value;

        }

        private void _txtAnteriorFiltrar_GotFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultarAnterior.IsDefault = true;
            this._btnModificar.IsDefault = false;
        }

        #endregion

        #region Apoderado Anterior

        private void _lstApoderadosAnterior_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._presentador.CambiarApoderadoAnterior())
            {
                GestionarVisibilidadDatosDeApoderadoAnterior(Visibility.Visible);
                GestionarVisibilidadFiltroApoderadoAnterior(Visibility.Collapsed);
                this._txtNombreApoderadoAnteriorFiltrar.Text = string.Empty;
                this._txtIdApoderadoAnteriorFiltrar.Text = string.Empty;

                if (this._presentador.VerificarCambioAgente("Anterior"))
                {
                    //this._btnConsultarPoderAnterior.IsEnabled = false;
                    this._btnConsultarPoderAnterior.IsEnabled = true;
                }
                else
                {
                    this._btnConsultarPoderAnterior.IsEnabled = true;
                }
            }
        }

        private void _OrdenarApoderadosAnterior_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstApoderadosAnterior);
        }

        private void _txtNombreApoderadoAnterior_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GestionarVisibilidadDatosDeApoderadoAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroMarca(Visibility.Collapsed);

            GestionarVisibilidadFiltroAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDeMarca(Visibility.Visible);

            GestionarVisibilidadDatosDeAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDeActual(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoActual(Visibility.Visible);

            GestionarVisibilidadDatosDePoderActual(Visibility.Visible);

            GestionarVisibilidadDatosDePoderAnterior(Visibility.Visible);
        }

        private void GestionarVisibilidadDatosDeApoderadoAnterior(object value)
        {
            this._lblNombreApoderadoAnterior.Visibility = (System.Windows.Visibility)value;
            this._txtNombreApoderadoAnterior.Visibility = (System.Windows.Visibility)value;
            this._txtIdApoderadoAnterior.Visibility = (System.Windows.Visibility)value;
        }

        private void GestionarVisibilidadFiltroApoderadoAnterior(object value)
        {
            this._lblApoderadoAnteriorFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblNombreApoderadoAnteriorFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtNombreApoderadoAnteriorFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblIdApoderadoAnteriorFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtIdApoderadoAnteriorFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lstApoderadosAnterior.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarApoderadoAnterior.Visibility = (System.Windows.Visibility)value;

        }

        private void _txtApoderadoAnteriorFiltrar_GotFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultarApoderadoAnterior.IsDefault = true;
            this._btnModificar.IsDefault = false;
        }

        #endregion

        #region Poder Anterior

        private void _lstPoderesAnterior_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._presentador.CambiarPoderAnterior())
            {
                GestionarVisibilidadDatosDePoderAnterior(Visibility.Visible);
                GestionarVisibilidadFiltroPoderAnterior(Visibility.Collapsed);
                this._txtIdPoderAnteriorFiltrar.Text = string.Empty;
                this._dpkFechaPoderAnteriorFiltrar.Text = string.Empty;

                if (this._presentador.VerificarCambioPoder("Anterior"))
                {
                    //this._btnConsultarApoderadoAnterior.IsEnabled = false;
                    //this._btnConsultarAnterior.IsEnabled = false;
                    this._btnConsultarApoderadoAnterior.IsEnabled = true;
                    this._btnConsultarAnterior.IsEnabled = true;
                }
                else
                {
                    this._btnConsultarApoderadoAnterior.IsEnabled = true;
                    this._btnConsultarAnterior.IsEnabled = true;
                }
            }
        }

        private void _OrdenarPoderesAnterior_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstPoderesAnterior);
        }

        private void _txtIdPoderAnterior_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GestionarVisibilidadDatosDePoderAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroMarca(Visibility.Collapsed);

            GestionarVisibilidadFiltroAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDeMarca(Visibility.Visible);

            GestionarVisibilidadDatosDeAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDeActual(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoActual(Visibility.Visible);

            GestionarVisibilidadDatosDePoderActual(Visibility.Visible);
        }

        private void GestionarVisibilidadDatosDePoderAnterior(object value)
        {
            this._lblIdPoderAnterior.Visibility = (System.Windows.Visibility)value;
            this._txtIdPoderAnterior.Visibility = (System.Windows.Visibility)value;
            this._lblFomentoAnterior.Visibility = (System.Windows.Visibility)value;
            this._lblFechaPoderAnterior.Visibility = (System.Windows.Visibility)value;
            this._lblBoletinPoderAnterior.Visibility = (System.Windows.Visibility)value;
            this._lblAnexoPoderAnterior.Visibility = (System.Windows.Visibility)value;
            this._lblFacultadPoderAnterior.Visibility = (System.Windows.Visibility)value;
            this._txtNumPoderAnterior.Visibility = (System.Windows.Visibility)value;
            this._txtFechaPoderAnterior.Visibility = (System.Windows.Visibility)value;
            this._txtBoletinPoderAnterior.Visibility = (System.Windows.Visibility)value;
            this._txtAnexoPoderAnterior.Visibility = (System.Windows.Visibility)value;
            this._txtFacultadPoderAnterior.Visibility = (System.Windows.Visibility)value;
        }

        private void GestionarVisibilidadFiltroPoderAnterior(object value)
        {
            this._lblPoderAnteriorFiltrar.Visibility = (System.Windows.Visibility)value;
            this._dpkFechaPoderAnteriorFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblIdPoderAnteriorFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtIdPoderAnteriorFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lstPoderesAnterior.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarPoderAnterior.Visibility = (System.Windows.Visibility)value;

        }

        private void _txtPoderAnteriorFiltrar_GotFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultarPoderAnterior.IsDefault = true;
            this._btnModificar.IsDefault = false;
        }

        #endregion

        #region Actual

        private void _lstActual_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._presentador.CambiarActual())
            {
                GestionarVisibilidadDatosDeActual(Visibility.Visible);
                GestionarVisibilidadFiltroActual(Visibility.Collapsed);

                if (this._presentador.VerificarCambioInteresado("Actual"))
                {
                    //this._btnConsultarPoderActual.IsEnabled = false;
                    this._btnConsultarPoderActual.IsEnabled = true;
                }
                else
                {
                    this._btnConsultarPoderActual.IsEnabled = true;
                }
            }
        }

        private void _OrdenarActual_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstActuals);
        }

        private void _txtNombreActual_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GestionarVisibilidadDatosDeActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroMarca(Visibility.Collapsed);

            GestionarVisibilidadFiltroAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroActual(Visibility.Visible);

            GestionarVisibilidadDatosDeMarca(Visibility.Visible);

            GestionarVisibilidadDatosDeAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoActual(Visibility.Visible);

            GestionarVisibilidadDatosDePoderAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDePoderActual(Visibility.Visible);
        }

        private void GestionarVisibilidadDatosDeActual(object value)
        {
            //this._lblNombreActual.Visibility = (System.Windows.Visibility)value;
            this._btnVerInteresadoActual.Visibility = (System.Windows.Visibility)value;
            this._txtNombreActual.Visibility = (System.Windows.Visibility)value;
            this._txtIdActual.Visibility = (System.Windows.Visibility)value;
            this._txtPaisActual.Visibility = (System.Windows.Visibility)value;
            this._txtNacionalidadActual.Visibility = (System.Windows.Visibility)value;
        }

        private void GestionarVisibilidadFiltroActual(object value)
        {
            this._lblActualFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblNombreActualFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtNombreActualFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblIdActual.Visibility = (System.Windows.Visibility)value;
            this._txtIdActualFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lstActuals.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarActual.Visibility = (System.Windows.Visibility)value;
        }

        private void _txtActualFiltrar_GotFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultarActual.IsDefault = true;
            this._btnModificar.IsDefault = false;
        }

        #endregion

        #region Apoderado Actual

        private void _lstApoderadosActual_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._presentador.CambiarApoderadoActual())
            {
                GestionarVisibilidadDatosDeApoderadoActual(Visibility.Visible);
                GestionarVisibilidadFiltroApoderadoActual(Visibility.Collapsed);
                this._txtNombreApoderadoActualFiltrar.Text = string.Empty;
                this._txtIdApoderadoActualFiltrar.Text = string.Empty;

                if (this._presentador.VerificarCambioAgente("Actual"))
                {
                    //this._btnConsultarPoderActual.IsEnabled = false;
                    this._btnConsultarPoderActual.IsEnabled = true;
                }
                else
                {
                    this._btnConsultarPoderActual.IsEnabled = true;
                }
            }
        }

        private void _OrdenarApoderadosActual_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstApoderadosActual);
        }

        private void _txtNombreApoderadoActual_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GestionarVisibilidadDatosDeApoderadoActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroMarca(Visibility.Collapsed);

            GestionarVisibilidadFiltroActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoActual(Visibility.Visible);

            GestionarVisibilidadDatosDeMarca(Visibility.Visible);

            GestionarVisibilidadDatosDeAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDeActual(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDePoderActual(Visibility.Visible);

            GestionarVisibilidadDatosDePoderAnterior(Visibility.Visible);
        }

        private void GestionarVisibilidadDatosDeApoderadoActual(object value)
        {
            this._lblNombreApoderadoActual.Visibility = (System.Windows.Visibility)value;
            this._txtNombreApoderadoActual.Visibility = (System.Windows.Visibility)value;
            this._txtIdApoderadoActual.Visibility = (System.Windows.Visibility)value;
        }

        private void GestionarVisibilidadFiltroApoderadoActual(object value)
        {
            this._lblApoderadoActualFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblNombreApoderadoActualFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtNombreApoderadoActualFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblIdApoderadoActualFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtIdApoderadoActualFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lstApoderadosActual.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarApoderadoActual.Visibility = (System.Windows.Visibility)value;

        }

        private void _txtApoderadoActualFiltrar_GotFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultarApoderadoActual.IsDefault = true;
            this._btnModificar.IsDefault = false;
        }

        #endregion

        #region Poder Actual

        private void _lstPoderesActual_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._presentador.CambiarPoderActual())
            {
                GestionarVisibilidadDatosDePoderActual(Visibility.Visible);
                GestionarVisibilidadFiltroPoderActual(Visibility.Collapsed);

                if (this._presentador.VerificarCambioPoder("Actual"))
                {
                    //this._btnConsultarApoderadoActual.IsEnabled = false;
                    //this._btnConsultarActual.IsEnabled = false;
                    this._btnConsultarApoderadoActual.IsEnabled = true;
                    this._btnConsultarActual.IsEnabled = true;
                }
                else
                {
                    this._btnConsultarApoderadoActual.IsEnabled = true;
                    this._btnConsultarActual.IsEnabled = true;
                }
            }
        }

        private void _OrdenarPoderesActual_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstPoderesActual);
        }

        private void _txtIdPoderActual_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GestionarVisibilidadDatosDePoderActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroMarca(Visibility.Collapsed);

            GestionarVisibilidadFiltroAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderAnterior(Visibility.Collapsed);

            GestionarVisibilidadFiltroActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoActual(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderActual(Visibility.Visible);

            GestionarVisibilidadDatosDeMarca(Visibility.Visible);

            GestionarVisibilidadDatosDeAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDeActual(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoAnterior(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoActual(Visibility.Visible);

            GestionarVisibilidadDatosDePoderAnterior(Visibility.Visible);
        }

        private void GestionarVisibilidadDatosDePoderActual(object value)
        {
            this._lblIdPoderActual.Visibility = (System.Windows.Visibility)value;
            this._txtIdPoderActual.Visibility = (System.Windows.Visibility)value;
            this._lblFomentoActual.Visibility = (System.Windows.Visibility)value;
            this._lblFechaPoderActual.Visibility = (System.Windows.Visibility)value;
            this._lblBoletinPoderActual.Visibility = (System.Windows.Visibility)value;
            this._lblAnexoPoderActual.Visibility = (System.Windows.Visibility)value;
            this._lblFacultadPoderActual.Visibility = (System.Windows.Visibility)value;
            this._txtNumPoderActual.Visibility = (System.Windows.Visibility)value;
            this._txtFechaPoderActual.Visibility = (System.Windows.Visibility)value;
            this._txtBoletinPoderActual.Visibility = (System.Windows.Visibility)value;
            this._txtAnexoPoderActual.Visibility = (System.Windows.Visibility)value;
            this._txtFacultadPoderActual.Visibility = (System.Windows.Visibility)value;
        }

        private void GestionarVisibilidadFiltroPoderActual(object value)
        {
            this._lblPoderActualFiltrar.Visibility = (System.Windows.Visibility)value;
            this._dpkFechaPoderActualFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblIdPoderActualFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtIdPoderActualFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lstPoderesActual.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarPoderActual.Visibility = (System.Windows.Visibility)value;

        }

        private void _txtPoderActualFiltrar_GotFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultarPoderActual.IsDefault = true;
            this._btnModificar.IsDefault = false;
        }

        #endregion

        private void _btnConsultarCadenaCambios_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.IrConsultarCadenaDeCambios();
        }

        private void _btnIrMarcas_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.IrConsultarMarca();
        }

        private void _btnVerInteresadosAnterior_y_Actual_Click(object sender, RoutedEventArgs e)
        {
            Button botonPresionado = (Button)sender;
            string nombreBoton = botonPresionado.Name;
            this._presentador.ConsultarInteresadosCambioPeticionario(nombreBoton);
        }


    }
}
