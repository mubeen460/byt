﻿using System.Windows;
using System.Windows.Controls;
using Trascend.Bolet.Cliente.Contratos.TraspasosPatentes.CesionesPatentes;
using Trascend.Bolet.Cliente.Presentadores.TraspasosPatentes.CesionesPatentes;
using Trascend.Bolet.Cliente.Ayuda;
using System.Windows.Media;

namespace Trascend.Bolet.Cliente.Ventanas.TraspasosPatentes.CesionesPatentes
{
    /// <summary>
    /// Interaction logic for GestionarFusion.xaml
    /// </summary>
    public partial class GestionarCesionPatentes : Page, IGestionarCesionPatentes
    {

        private PresentadorGestionarCesionPatentes _presentador;
        private bool _cargada;
        private GridViewColumnHeader _CurSortCol = null;
        private SortAdorner _CurAdorner = null;

        #region IGestionarCesion

        public bool EstaCargada
        {
            get { return this._cargada; }
            set { this._cargada = value; }
        }

        public void BorrarCerosInternacional()
        {
            if (this._txtIdMarcaInt.Text.Equals("0"))
                this._txtIdMarcaInt.Text = ""; // cambio el texto del textbox para que no aparezca el "0"
            if (this._txtIdMarcaIntCor.Text.Equals("0"))
                this._txtIdMarcaIntCor.Text = ""; // cambio el texto del textbox para que no aparezca el "0"
        }

        public void EsPatenteNacional(bool patenteNacional)
        {
            if (patenteNacional)
            {
                this._radioExtranjero.IsChecked = !patenteNacional;
                this._radioNacional.IsChecked = patenteNacional;
            }
            else
            {
                this._radioExtranjero.IsChecked = !patenteNacional;
                this._radioNacional.IsChecked = patenteNacional;
            }
        }

        public string IdCedenteFiltrar
        {
            get { return this._txtIdCedenteFiltrar.Text; }
        }

        public string Ubicacion
        {
            get { return this._txtUbicacion.Text; }
            set { this._txtUbicacion.Text = value; }
        }

        public string Expediente
        {
            get { return this._txtExpediente.Text; }
            set { this._txtExpediente.Text = value; }
        }

        public string Tipo
        {
            get { return this._txtTipo.Text; }
            set { this._txtTipo.Text = value; }
        }

        public string NombreCedenteFiltrar
        {
            get { return this._txtNombreCedenteFiltrar.Text; }
        }

        public string IdCesionarioFiltrar
        {
            get { return this._txtIdCesionarioFiltrar.Text; }
        }

        public string NombreCesionarioFiltrar
        {
            get { return this._txtNombreCesionarioFiltrar.Text; }
        }

        public string IdPoderCesionarioFiltrar
        {
            get { return this._txtIdPoderCesionarioFiltrar.Text; }
        }

        public string FechaPoderCesionarioFiltrar
        {
            get { return this._dpkFechaPoderCesionarioFiltrar.Text; }
        }

        public string IdPoderCedenteFiltrar
        {
            get { return this._txtIdPoderCedenteFiltrar.Text; }
        }

        public string FechaPoderCedenteFiltrar
        {
            get { return this._dpkFechaPoderCedenteFiltrar.Text; }
        }

        public string IdApoderadoCesionarioFiltrar
        {
            get { return this._txtIdApoderadoCesionarioFiltrar.Text; }
        }

        public string NombreApoderadoCesionarioFiltrar
        {
            get { return this._txtNombreApoderadoCesionarioFiltrar.Text; }
        }

        public string IdApoderadoCedenteFiltrar
        {
            get { return this._txtIdApoderadoCedenteFiltrar.Text; }
        }

        public string NombreApoderadoCedenteFiltrar
        {
            get { return this._txtNombreApoderadoCedenteFiltrar.Text; }
        }

        public string IdPatenteFiltrar
        {
            get { return this._txtIdPatenteFiltrar.Text; }
        }

        public string NombrePatenteFiltrar
        {
            get { return this._txtNombrePatenteFiltrar.Text; }
        }

        public object Patente
        {
            get { return this._gridDatosPatente.DataContext; }
            set { this._gridDatosPatente.DataContext = value; }
        }

        public object InteresadoCedente
        {
            get { return this._gridDatosCedente.DataContext; }
            set { this._gridDatosCedente.DataContext = value; }
        }

        public object InteresadoCesionario
        {
            get { return this._gridDatosCesionario.DataContext; }
            set { this._gridDatosCesionario.DataContext = value; }
        }

        public object ApoderadoCedente
        {
            get { return this._gridDatosApoderadoCedente.DataContext; }
            set { this._gridDatosApoderadoCedente.DataContext = value; }
        }

        public object ApoderadoCesionario
        {
            get { return this._gridDatosApoderadoCesionario.DataContext; }
            set { this._gridDatosApoderadoCesionario.DataContext = value; }
        }

        public object PoderCedente
        {
            get { return this._gridDatosPoderCedente.DataContext; }
            set { this._gridDatosPoderCedente.DataContext = value; }
        }

        public object PoderCesionario
        {
            get { return this._gridDatosPoderCesionario.DataContext; }
            set { this._gridDatosPoderCesionario.DataContext = value; }
        }

        public object PatentesFiltradas
        {
            get { return this._lstPatentes.DataContext; }
            set { this._lstPatentes.DataContext = value; }
        }

        public object CedentesFiltrados
        {
            get { return this._lstCedentes.DataContext; }
            set { this._lstCedentes.DataContext = value; }
        }

        public object CesionariosFiltrados
        {
            get { return this._lstCesionarios.DataContext; }
            set { this._lstCesionarios.DataContext = value; }
        }

        public object ApoderadosCedenteFiltrados
        {
            get { return this._lstApoderadosCedente.DataContext; }
            set { this._lstApoderadosCedente.DataContext = value; }
        }

        public object ApoderadosCesionarioFiltrados
        {
            get { return this._lstApoderadosCesionario.DataContext; }
            set { this._lstApoderadosCesionario.DataContext = value; }
        }

        public object PoderesCedenteFiltrados
        {
            get { return this._lstPoderesCedente.DataContext; }
            set { this._lstPoderesCedente.DataContext = value; }
        }

        public object PoderesCesionarioFiltrados
        {
            get { return this._lstPoderesCesionario.DataContext; }
            set { this._lstPoderesCesionario.DataContext = value; }
        }

        public object CesionPatente
        {
            get { return this._gridDatos.DataContext; }
            set { this._gridDatos.DataContext = value; }
        }

        public object PatenteFiltrada
        {
            get { return this._lstPatentes.SelectedItem; }
            set { this._lstPatentes.SelectedItem = value; }
        }

        public object CedenteFiltrado
        {
            get { return this._lstCedentes.SelectedItem; }
            set { this._lstCedentes.SelectedItem = value; }
        }

        public object CesionarioFiltrado
        {
            get { return this._lstCesionarios.SelectedItem; }
            set { this._lstCesionarios.SelectedItem = value; }
        }

        public object ApoderadoCedenteFiltrado
        {
            get { return this._lstApoderadosCedente.SelectedItem; }
            set { this._lstApoderadosCedente.SelectedItem = value; }
        }

        public object ApoderadoCesionarioFiltrado
        {
            get { return this._lstApoderadosCesionario.SelectedItem; }
            set { this._lstApoderadosCesionario.SelectedItem = value; }
        }

        public object PoderCedenteFiltrado
        {
            get { return this._lstPoderesCedente.SelectedItem; }
            set { this._lstPoderesCedente.SelectedItem = value; }
        }

        public object PoderCesionarioFiltrado
        {
            get { return this._lstPoderesCesionario.SelectedItem; }
            set { this._lstPoderesCesionario.SelectedItem = value; }
        }

        public string Region
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public void FocoPredeterminado()
        {
            this._txtIdCesion.Focus();
        }

        public bool HabilitarCampos
        {
            set
            {
                this._txtAsociado.IsEnabled = value;
                this._txtIdAsociado.IsEnabled = value;
                this._txtExpediente.IsEnabled = value;
                this._txtIdCesion.IsEnabled = value;
                this._txtIdPatenteFiltrar.IsEnabled = value;
                this._txtNombrePatente.IsEnabled = value;
                this._txtIdPatente.IsEnabled = value;
                this._txtNombrePatenteFiltrar.IsEnabled = value;
                this._txtNumInscripcion.IsEnabled = value;
                this._txtNumRegistro.IsEnabled = value;
                this._txtTipo.IsEnabled = value;
                this._txtUbicacion.IsEnabled = value;
                this._btnConsultarPatente.IsEnabled = value;
                this._dpkFechaCesion.IsEnabled = value;

                this._txtNombreCedente.IsEnabled = value;
                this._txtIdCedente.IsEnabled = value;
                this._txtPaisCedente.IsEnabled = value;
                this._txtNacionalidadCedente.IsEnabled = value;
                this._txtIdCedenteFiltrar.IsEnabled = value;
                this._txtNombreCedenteFiltrar.IsEnabled = value;
                this._txtNombreApoderadoCedente.IsEnabled = value;
                this._txtIdApoderadoCedente.IsEnabled = value;
                this._txtNombreApoderadoCedenteFiltrar.IsEnabled = value;
                this._txtIdApoderadoCedenteFiltrar.IsEnabled = value;
                this._txtIdPoderCedente.IsEnabled = value;
                this._txtIdPoderCedenteFiltrar.IsEnabled = value;
                this._txtAnexoPoderCedente.IsEnabled = value;
                this._txtBoletinPoderCedente.IsEnabled = value;
                this._txtFacultadPoderCedente.IsEnabled = value;
                this._txtNumPoderCedente.IsEnabled = value;
                this._txtFechaPoderCedente.IsEnabled = value;

                this._txtNombreCesionario.IsEnabled = value;
                this._txtIdCesionario.IsEnabled = value;
                this._txtIdCesionarioFiltrar.IsEnabled = value;
                this._txtNombreCesionarioFiltrar.IsEnabled = value;
                this._txtNombreApoderadoCesionario.IsEnabled = value;
                this._txtIdApoderadoCesionario.IsEnabled = value;
                this._txtIdApoderadoCesionarioFiltrar.IsEnabled = value;
                this._txtNombreApoderadoCesionarioFiltrar.IsEnabled = value;
                this._txtIdPoderCesionario.IsEnabled = value;
                this._txtIdPoderCesionarioFiltrar.IsEnabled = value;

                this._txtAnexoPoderCesionario.IsEnabled = value;
                this._txtBoletinPoderCesionario.IsEnabled = value;
                this._txtFacultadPoderCesionario.IsEnabled = value;
                this._txtNumPoderCesionario.IsEnabled = value;
                this._txtFechaPoderCesionario.IsEnabled = value;
                this._txtPaisCesionario.IsEnabled = value;
                this._txtNacionalidadCesionario.IsEnabled = value;

                this._txtObservacionCesion.IsEnabled = value;
                this._txtOtrosCesion.IsEnabled = value;
                this._txtReferenciaCesion.IsEnabled = value;
                this._txtAnexoCesion.IsEnabled = value;
                this._txtComentarioCesion.IsEnabled = value;
                this._chkAsientoEnLibro.IsEnabled = value;
                this._cbxBoletin.IsEnabled = value;

                this._btnPatente.IsEnabled = value;
                this._btnCedente.IsEnabled = value;
                this._btnCesionario.IsEnabled = value;

            }
        }

        public string NombrePatente
        {
            set { this._txtNombrePatente.Text = value; }
        }

        public string IdPatente
        {
            get { return this._txtIdPatente.Text; }
            set { this._txtIdPatente.Text = value; }
        }

        public string NombreCedente
        {
            set { this._txtNombreCedente.Text = value; }
        }

        public string IdCedente
        {
            get { return this._txtIdCedente.Text; }
            set { this._txtIdCedente.Text = value; }
        }

        public string NombreApoderadoCedente
        {
            set { this._txtNombreApoderadoCedente.Text = value; }
        }

        public string IdApoderadoCedente
        {
            get { return this._txtIdApoderadoCedente.Text; }
            set { this._txtIdApoderadoCedente.Text = value; }
        }

        public string NombreApoderadoCesionario
        {
            set { this._txtNombreApoderadoCesionario.Text = value; }
        }

        public string IdApoderadoCesionario
        {
            get { return this._txtIdApoderadoCesionario.Text; }
            set { this._txtIdApoderadoCesionario.Text = value; }
        }

        public string IdPoderCedente
        {
            get { return this._txtIdPoderCedente.Text; }
            set { this._txtIdPoderCedente.Text = value; }
        }

        public string IdPoderCesionario
        {
            get { return this._txtIdPoderCesionario.Text; }
            set { this._txtIdPoderCesionario.Text = value; }
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

        public string NombreCesionario
        {
            set { this._txtNombreCesionario.Text = value; }
        }

        public string IdCesionario
        {
            get { return this._txtIdCesionario.Text; }
            set { this._txtIdCesionario.Text = value; }
        }

        public string PaisCedente
        {
            set { this._txtPaisCedente.Text = value; }
        }

        public string PaisCesionario
        {
            set { this._txtPaisCesionario.Text = value; }
        }

        public string NacionalidadCedente
        {
            set { this._txtNacionalidadCedente.Text = value; }
        }

        public string NacionalidadCesionario
        {
            set { this._txtNacionalidadCesionario.Text = value; }
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

        #endregion


        public GestionarCesionPatentes(object cesion)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorGestionarCesionPatentes(this, cesion);
        }

        /// <summary>
        /// Constructor para la consulta desde operaciones
        /// </summary>
        /// <param name="cesion">la cesion a mostrar</param>
        /// <param name="visibilidad">parametro que indica la visibilidad de los botones</param>
        public GestionarCesionPatentes(object cesion, object parametro)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorGestionarCesionPatentes(this, cesion);
            if (parametro.GetType() == typeof(System.Windows.Visibility))
            {
                this._btnModificar.Visibility = (System.Windows.Visibility)parametro;
                this._btnEliminar.Visibility = (System.Windows.Visibility)parametro;
            }
            else if (parametro.GetType() == typeof(ConsultarCesionesPatentes))
            {
                _presentador._ventanaPadre = parametro;
            }
        }




        /// <summary>
        /// Constructor para la consulta desde operaciones que recibe una ventana padre
        /// </summary>
        /// <param name="cesion">la cesion a mostrar</param>
        /// <param name="visibilidad">parametro que indica la visibilidad de los botones</param>
        /// <param name="ventanaPadre">Ventana anterior a esta ventana</param>
        public GestionarCesionPatentes(object cesion, object parametro, object ventanaPadre)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorGestionarCesionPatentes(this, cesion,ventanaPadre);
            if (parametro.GetType() == typeof(System.Windows.Visibility))
            {
                this._btnModificar.Visibility = (System.Windows.Visibility)parametro;
                this._btnEliminar.Visibility = (System.Windows.Visibility)parametro;
            }
            else if (parametro.GetType() == typeof(ConsultarCesionesPatentes))
            {
                _presentador._ventanaPadre = parametro;
            }
        }





        public void ActivarControlesAlAgregar()
        {
            this._btnEliminar.Visibility = System.Windows.Visibility.Collapsed;
            this._lblIdCesion.Visibility = System.Windows.Visibility.Collapsed;
            this._txtIdCesion.Visibility = System.Windows.Visibility.Collapsed;
            this._dpkFechaCesion.IsEnabled = true;
            this._btnAnexo.Visibility = System.Windows.Visibility.Collapsed;
            //this._btnCarpeta.Visibility = System.Windows.Visibility.Collapsed;
            this._btnPlanilla.Visibility = System.Windows.Visibility.Collapsed;
            this._btnPlanillaVan.Visibility = System.Windows.Visibility.Collapsed;
            this._btnPlanillaVienen.Visibility = System.Windows.Visibility.Collapsed;
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
            if (MessageBoxResult.Yes == MessageBox.Show(Recursos.MensajesConElUsuario.ConfirmacionEliminarCesion,
                "Eliminar CesionPatente", MessageBoxButton.YesNo, MessageBoxImage.Question))
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
            if (((Button)sender).Name.Equals("_btnConsultarPatente"))
                this._presentador.ConsultarPatentes();
            else if (((Button)sender).Name.Equals("_btnConsultarCedente"))
                this._presentador.ConsultarCedentes();
            else if (((Button)sender).Name.Equals("_btnConsultarApoderadoCedente"))
                this._presentador.ConsultarApoderadosCedente();
            else if (((Button)sender).Name.Equals("_btnConsultarPoderCedente"))
                this._presentador.ConsultarPoderesCedente();
            else if (((Button)sender).Name.Equals("_btnConsultarCesionario"))
                this._presentador.ConsultarCesionarios();
            else if (((Button)sender).Name.Equals("_btnConsultarApoderadoCesionario"))
                this._presentador.ConsultarApoderadosCesionario();
            else if (((Button)sender).Name.Equals("_btnConsultarPoderCesionario"))
                this._presentador.ConsultarPoderesCesionario();
        }


        private void _btnPlanillaVienen_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.IrImprimir(((Button)sender).Name);
        }


        private void _btnPlanillaVan_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.IrImprimir(((Button)sender).Name);
        }


        private void _btnPlanilla_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.IrImprimir(((Button)sender).Name);
        }


        private void _btnCarpeta_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.IrImprimir(((Button)sender).Name);
        }


        private void _btnAnexo_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.IrImprimir(((Button)sender).Name);
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
                color = Brushes.Red;
            }
            else color = Brushes.White;

            this._txtAsociado.Background = color;
            this._txtIdAsociado.Background = color;
        }


        public void ConvertirEnteroMinimoABlanco()
        {
            if (!this.IdPatente.Equals(""))
            {
                if (int.Parse(this.IdPatente) == int.MinValue)
                    this.IdPatente = "";
            }

            if (!this.IdCedente.Equals(""))
            {
                if (int.Parse(this.IdCedente) == int.MinValue)
                    this.IdCedente = "";
            }

            if (!this.IdCesionario.Equals(""))
            {
                if (int.Parse(this.IdCesionario) == int.MinValue)
                    this.IdCesionario = "";
            }

            if (!this.IdPoderCedente.Equals(""))
            {
                if (int.Parse(this.IdPoderCedente) == int.MinValue)
                    this.IdPoderCedente = "";
            }

            if (!this.IdPoderCesionario.Equals(""))
            {
                if (int.Parse(this.IdPoderCesionario) == int.MinValue)
                    this.IdPoderCesionario = "";
            }

        }


        public void GestionarBotonConsultarInteresados(string tipo, bool value)
        {
            if (tipo.Equals("Cedente"))
            {
                this._btnConsultarCedente.IsEnabled = value;
            }
            else if (tipo.Equals("Cesionario"))
            {
                this._btnConsultarCesionario.IsEnabled = value;
            }
        }


        public void GestionarBotonConsultarApoderados(string tipo, bool value)
        {
            if (tipo.Equals("Cedente"))
            {
                this._btnConsultarApoderadoCedente.IsEnabled = value;
            }
            else if (tipo.Equals("Cesionario"))
            {
                this._btnConsultarApoderadoCesionario.IsEnabled = value;
            }
        }


        public void GestionarBotonConsultarPoderes(string tipo, bool value)
        {
            if (tipo.Equals("Cedente"))
            {
                this._btnConsultarPoderCedente.IsEnabled = value;
            }
            else if (tipo.Equals("Cesionario"))
            {
                this._btnConsultarPoderCesionario.IsEnabled = value;
            }
        }

        #region Patente


        private void _lstPatentes_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._presentador.CambiarPatente())
            {
                GestionarVisibilidadDatosDePatente(Visibility.Visible);
                GestionarVisibilidadFiltroPatente(Visibility.Collapsed);
            }
        }


        private void _OrdenarPatentes_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstPatentes);
        }


        private void _txtNombrePatente_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GestionarVisibilidadDatosDePatente(Visibility.Collapsed);

            GestionarVisibilidadFiltroCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroPatente(Visibility.Visible);

            GestionarVisibilidadDatosDeCedente(Visibility.Visible);

            GestionarVisibilidadDatosDeCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoCedente(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDePoderCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDePoderCedente(Visibility.Visible);
        }


        private void GestionarVisibilidadDatosDePatente(object value)
        {
            this._txtNombrePatente.Visibility = (System.Windows.Visibility)value;
            this._txtIdPatente.Visibility = (System.Windows.Visibility)value;
            this._lblNoInscripcion.Visibility = (System.Windows.Visibility)value;
            this._txtNumInscripcion.Visibility = (System.Windows.Visibility)value;
            this._lblNoRegistro.Visibility = (System.Windows.Visibility)value;
            this._txtNumRegistro.Visibility = (System.Windows.Visibility)value;
            this._lblTipo.Visibility = (System.Windows.Visibility)value;
            this._txtTipo.Visibility = (System.Windows.Visibility)value;
            this._btnIrAsociados.Visibility = (System.Windows.Visibility)value;
            this._txtAsociado.Visibility = (System.Windows.Visibility)value;
            this._txtIdAsociado.Visibility = (System.Windows.Visibility)value;
            this._btnPatente.Visibility = (System.Windows.Visibility)value;
        }


        private void GestionarVisibilidadFiltroPatente(object value)
        {
            this._lblNombrePatente.Visibility = (System.Windows.Visibility)value;
            this._txtNombrePatenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblIdPatente.Visibility = (System.Windows.Visibility)value;
            this._txtIdPatenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lstPatentes.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarPatente.Visibility = (System.Windows.Visibility)value;
        }


        private void _txtPatenteFiltrar_GotFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultarPatente.IsDefault = true;
            this._btnModificar.IsDefault = false;
        }


        #endregion

        #region Cedente


        private void _lstCedentes_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._presentador.CambiarCedente())
            {
                GestionarVisibilidadDatosDeCedente(Visibility.Visible);
                GestionarVisibilidadFiltroCedente(Visibility.Collapsed);

                if (this._presentador.VerificarCambioInteresado("Cedente"))
                {
                    //this._btnConsultarPoderCedente.IsEnabled = false;
                    this._btnConsultarPoderCedente.IsEnabled = true;
                }
                else
                {
                    this._btnConsultarPoderCedente.IsEnabled = true;
                }

            }

        }


        private void _lstPoderesCedente_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._presentador.CambiarPoderCedente())
            {
                GestionarVisibilidadDatosDePoderCedente(Visibility.Visible);
                GestionarVisibilidadFiltroPoderCedente(Visibility.Collapsed);

                if (this._presentador.VerificarCambioPoder("Cedente"))
                {
                    //this._btnConsultarApoderadoCedente.IsEnabled = false;
                    //this._btnConsultarCedente.IsEnabled = false;
                    this._btnConsultarApoderadoCedente.IsEnabled = true;
                    this._btnConsultarCedente.IsEnabled = true;
                }
                else
                {
                    this._btnConsultarApoderadoCedente.IsEnabled = true;
                    this._btnConsultarCedente.IsEnabled = true;
                }
            }
        }


        private void _lstApoderadosCedente_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._presentador.CambiarApoderadoCedente())
            {
                GestionarVisibilidadDatosDeApoderadoCedente(Visibility.Visible);
                GestionarVisibilidadFiltroApoderadoCedente(Visibility.Collapsed);

                if (this._presentador.VerificarCambioAgente("Cedente"))
                {
                    //this._btnConsultarPoderCedente.IsEnabled = false;
                    this._btnConsultarPoderCedente.IsEnabled = true;
                }
                else
                {
                    this._btnConsultarPoderCedente.IsEnabled = true;
                }
            }
        }


        private void _OrdenarCedentes_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstCedentes);
        }


        private void _OrdenarApoderadosCedente_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstApoderadosCedente);
        }


        private void _OrdenarPoderesCedente_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstPoderesCedente);
        }


        private void _txtNombreCedente_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GestionarVisibilidadDatosDeCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroPatente(Visibility.Collapsed);

            GestionarVisibilidadFiltroCedente(Visibility.Visible);

            GestionarVisibilidadDatosDePatente(Visibility.Visible);

            GestionarVisibilidadDatosDeCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoCedente(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDePoderCedente(Visibility.Visible);

            GestionarVisibilidadDatosDePoderCesionario(Visibility.Visible);
        }


        private void _txtNombreApoderadoCedente_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GestionarVisibilidadDatosDeApoderadoCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroPatente(Visibility.Collapsed);

            GestionarVisibilidadFiltroCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoCedente(Visibility.Visible);

            GestionarVisibilidadDatosDePatente(Visibility.Visible);

            GestionarVisibilidadDatosDeCedente(Visibility.Visible);

            GestionarVisibilidadDatosDeCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDePoderCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDePoderCedente(Visibility.Visible);
        }


        private void _txtIdPoderCedente_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GestionarVisibilidadDatosDePoderCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroPatente(Visibility.Collapsed);

            GestionarVisibilidadFiltroCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderCedente(Visibility.Visible);

            GestionarVisibilidadDatosDePatente(Visibility.Visible);

            GestionarVisibilidadDatosDeCedente(Visibility.Visible);

            GestionarVisibilidadDatosDeCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoCedente(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDePoderCesionario(Visibility.Visible);
        }


        private void _txtCedenteFiltrar_GotFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultarCedente.IsDefault = true;
            this._btnModificar.IsDefault = false;
        }


        private void _txtApoderadoCedenteFiltrar_GotFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultarApoderadoCedente.IsDefault = true;
            this._btnModificar.IsDefault = false;
        }


        private void _txtPoderCedenteFiltrar_GotFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultarPoderCedente.IsDefault = true;
            this._btnModificar.IsDefault = false;
        }


        private void GestionarVisibilidadDatosDeCedente(object value)
        {
            //this._lblNombreCedente.Visibility = (System.Windows.Visibility)value;
            this._btnCedente.Visibility = (System.Windows.Visibility)value;
            this._txtNombreCedente.Visibility = (System.Windows.Visibility)value;
            this._txtIdCedente.Visibility = (System.Windows.Visibility)value;
            this._txtPaisCedente.Visibility = (System.Windows.Visibility)value;
            this._txtNacionalidadCedente.Visibility = (System.Windows.Visibility)value;
        }


        private void GestionarVisibilidadDatosDeApoderadoCedente(object value)
        {
            this._lblNombreApoderadoCedente.Visibility = (System.Windows.Visibility)value;
            this._txtNombreApoderadoCedente.Visibility = (System.Windows.Visibility)value;
            this._txtIdApoderadoCedente.Visibility = (System.Windows.Visibility)value;
        }


        private void GestionarVisibilidadDatosDePoderCedente(object value)
        {
            this._lblIdPoderCedente.Visibility = (System.Windows.Visibility)value;
            this._txtIdPoderCedente.Visibility = (System.Windows.Visibility)value;
            this._lblFomentoCedente.Visibility = (System.Windows.Visibility)value;
            this._lblFechaPoderCedente.Visibility = (System.Windows.Visibility)value;
            this._lblBoletinPoderCedente.Visibility = (System.Windows.Visibility)value;
            this._lblAnexoPoderCedente.Visibility = (System.Windows.Visibility)value;
            this._lblFacultadPoderCedente.Visibility = (System.Windows.Visibility)value;
            this._txtNumPoderCedente.Visibility = (System.Windows.Visibility)value;
            this._txtFechaPoderCedente.Visibility = (System.Windows.Visibility)value;
            this._txtBoletinPoderCedente.Visibility = (System.Windows.Visibility)value;
            this._txtAnexoPoderCedente.Visibility = (System.Windows.Visibility)value;
            this._txtFacultadPoderCedente.Visibility = (System.Windows.Visibility)value;
        }


        private void GestionarVisibilidadFiltroCedente(object value)
        {
            this._lblCedenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblNombreCedenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtNombreCedenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblIdCedenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtIdCedenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lstCedentes.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarCedente.Visibility = (System.Windows.Visibility)value;

        }


        private void GestionarVisibilidadFiltroApoderadoCedente(object value)
        {
            this._lblApoderadoCedenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblNombreApoderadoCedenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtNombreApoderadoCedenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblIdApoderadoCedenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtIdApoderadoCedenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lstApoderadosCedente.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarApoderadoCedente.Visibility = (System.Windows.Visibility)value;

        }


        private void GestionarVisibilidadFiltroPoderCedente(object value)
        {
            this._lblPoderCedenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._dpkFechaPoderCedenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblIdPoderCedenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtIdPoderCedenteFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lstPoderesCedente.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarPoderCedente.Visibility = (System.Windows.Visibility)value;

        }

        #endregion

        #region Cesionario


        private void _lstCesionarios_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._presentador.CambiarCesionario())
            {
                GestionarVisibilidadDatosDeCesionario(Visibility.Visible);
                GestionarVisibilidadFiltroCesionario(Visibility.Collapsed);

                if (this._presentador.VerificarCambioInteresado("Cesionario"))
                {
                    //this._btnConsultarPoderCesionario.IsEnabled = false;
                    this._btnConsultarPoderCesionario.IsEnabled = true;
                }
                else
                {
                    this._btnConsultarPoderCesionario.IsEnabled = true;
                }
            }
        }


        private void _lstApoderadosCesionario_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._presentador.CambiarApoderadoCesionario())
            {
                GestionarVisibilidadDatosDeApoderadoCesionario(Visibility.Visible);
                GestionarVisibilidadFiltroApoderadoCesionario(Visibility.Collapsed);

                if (this._presentador.VerificarCambioAgente("Cesionario"))
                {
                    //this._btnConsultarPoderCesionario.IsEnabled = false;
                    this._btnConsultarPoderCesionario.IsEnabled = true;
                }
                else
                {
                    this._btnConsultarPoderCesionario.IsEnabled = true;
                }
            }
        }


        private void _lstPoderesCesionario_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._presentador.CambiarPoderCesionario())
            {
                GestionarVisibilidadDatosDePoderCesionario(Visibility.Visible);
                GestionarVisibilidadFiltroPoderCesionario(Visibility.Collapsed);

                if (this._presentador.VerificarCambioPoder("Cesionario"))
                {
                    //this._btnConsultarApoderadoCesionario.IsEnabled = false;
                    //this._btnConsultarCesionario.IsEnabled = false;
                    this._btnConsultarApoderadoCesionario.IsEnabled = true;
                    this._btnConsultarCesionario.IsEnabled = true;
                }
                else
                {
                    this._btnConsultarApoderadoCesionario.IsEnabled = true;
                    this._btnConsultarCesionario.IsEnabled = true;
                }
            }
        }


        private void _OrdenarCesionarios_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstCesionarios);
        }


        private void _OrdenarApoderadosCesionario_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstApoderadosCesionario);
        }


        private void _OrdenarPoderesCesionario_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstPoderesCesionario);
        }


        private void _txtNombreCesionario_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GestionarVisibilidadDatosDeCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroPatente(Visibility.Collapsed);

            GestionarVisibilidadFiltroCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDePatente(Visibility.Visible);

            GestionarVisibilidadDatosDeCedente(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoCedente(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDePoderCedente(Visibility.Visible);

            GestionarVisibilidadDatosDePoderCesionario(Visibility.Visible);
        }


        private void _txtNombreApoderadoCesionario_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GestionarVisibilidadDatosDeApoderadoCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroPatente(Visibility.Collapsed);

            GestionarVisibilidadFiltroCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDePatente(Visibility.Visible);

            GestionarVisibilidadDatosDeCedente(Visibility.Visible);

            GestionarVisibilidadDatosDeCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoCedente(Visibility.Visible);

            GestionarVisibilidadDatosDePoderCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDePoderCedente(Visibility.Visible);
        }


        private void _txtIdPoderCesionario_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            GestionarVisibilidadDatosDePoderCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroPatente(Visibility.Collapsed);

            GestionarVisibilidadFiltroCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderCedente(Visibility.Collapsed);

            GestionarVisibilidadFiltroCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroApoderadoCesionario(Visibility.Collapsed);

            GestionarVisibilidadFiltroPoderCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDePatente(Visibility.Visible);

            GestionarVisibilidadDatosDeCedente(Visibility.Visible);

            GestionarVisibilidadDatosDeCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoCedente(Visibility.Visible);

            GestionarVisibilidadDatosDeApoderadoCesionario(Visibility.Visible);

            GestionarVisibilidadDatosDePoderCedente(Visibility.Visible);
        }


        private void GestionarVisibilidadDatosDeCesionario(object value)
        {
            //this._lblNombreCesionario.Visibility = (System.Windows.Visibility)value;
            this._btnCesionario.Visibility = (System.Windows.Visibility)value;
            this._txtNombreCesionario.Visibility = (System.Windows.Visibility)value;
            this._txtIdCesionario.Visibility = (System.Windows.Visibility)value;
            this._txtPaisCesionario.Visibility = (System.Windows.Visibility)value;
            this._txtNacionalidadCesionario.Visibility = (System.Windows.Visibility)value;
        }


        private void GestionarVisibilidadDatosDeApoderadoCesionario(object value)
        {
            this._lblNombreApoderadoCesionario.Visibility = (System.Windows.Visibility)value;
            this._txtNombreApoderadoCesionario.Visibility = (System.Windows.Visibility)value;
            this._txtIdApoderadoCesionario.Visibility = (System.Windows.Visibility)value;
        }


        private void GestionarVisibilidadDatosDePoderCesionario(object value)
        {
            this._lblIdPoderCesionario.Visibility = (System.Windows.Visibility)value;
            this._txtIdPoderCesionario.Visibility = (System.Windows.Visibility)value;
            this._lblFomentoCesionario.Visibility = (System.Windows.Visibility)value;
            this._lblFechaPoderCesionario.Visibility = (System.Windows.Visibility)value;
            this._lblBoletinPoderCesionario.Visibility = (System.Windows.Visibility)value;
            this._lblAnexoPoderCesionario.Visibility = (System.Windows.Visibility)value;
            this._lblFacultadPoderCesionario.Visibility = (System.Windows.Visibility)value;
            this._txtNumPoderCesionario.Visibility = (System.Windows.Visibility)value;
            this._txtFechaPoderCesionario.Visibility = (System.Windows.Visibility)value;
            this._txtBoletinPoderCesionario.Visibility = (System.Windows.Visibility)value;
            this._txtAnexoPoderCesionario.Visibility = (System.Windows.Visibility)value;
            this._txtFacultadPoderCesionario.Visibility = (System.Windows.Visibility)value;
        }


        private void GestionarVisibilidadFiltroCesionario(object value)
        {
            this._lblCesionarioFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblNombreCesionarioFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtNombreCesionarioFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblIdCesionario.Visibility = (System.Windows.Visibility)value;
            this._txtIdCesionarioFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lstCesionarios.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarCesionario.Visibility = (System.Windows.Visibility)value;
        }


        private void GestionarVisibilidadFiltroApoderadoCesionario(object value)
        {
            this._lblApoderadoCesionarioFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblNombreApoderadoCesionarioFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtNombreApoderadoCesionarioFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblIdApoderadoCesionarioFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtIdApoderadoCesionarioFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lstApoderadosCesionario.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarApoderadoCesionario.Visibility = (System.Windows.Visibility)value;

        }


        private void GestionarVisibilidadFiltroPoderCesionario(object value)
        {
            this._lblPoderCesionarioFiltrar.Visibility = (System.Windows.Visibility)value;
            this._dpkFechaPoderCesionarioFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblIdPoderCesionarioFiltrar.Visibility = (System.Windows.Visibility)value;
            this._txtIdPoderCesionarioFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lstPoderesCesionario.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarPoderCesionario.Visibility = (System.Windows.Visibility)value;

        }


        private void _txtCesionarioFiltrar_GotFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultarCesionario.IsDefault = true;
            this._btnModificar.IsDefault = false;
        }


        private void _txtApoderadoCesionarioFiltrar_GotFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultarApoderadoCesionario.IsDefault = true;
            this._btnModificar.IsDefault = false;
        }


        private void _txtPoderCesionarioFiltrar_GotFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultarPoderCesionario.IsDefault = true;
            this._btnModificar.IsDefault = false;
        }

        #endregion


        private void _btnIrAsociados_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.IrVentanaAsociado();
        }

        private void _btnConsultarInteresadosCesion_Click(object sender, RoutedEventArgs e)
        {
            Button botonPresionado = (Button)sender;
            string nombreBoton = botonPresionado.Name;
            this._presentador.ConsultarInteresadosDeCesion(nombreBoton);
        }

        private void _btnPatente_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.ConsultarPatenteDeCesion();
        }

    }
}
