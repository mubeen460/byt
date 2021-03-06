﻿using System.Windows;
using System.Windows.Controls;
using Trascend.Bolet.Cliente.Contratos.MarcasTercero;
using Trascend.Bolet.Cliente.Presentadores.MarcasTercero;
using System.ComponentModel;
using System.Threading;
using Trascend.Bolet.Cliente.Ayuda;

namespace Trascend.Bolet.Cliente.Ventanas.MarcasTercero
{
    /// <summary>
    /// Interaction logic for AgregarObjeto.xaml
    /// </summary>
    public partial class GestionarInfoBolMarcaTer : Page, IGestionarInfoBolMarcaTer
    {
        private GridViewColumnHeader _CurSortCol = null;
        private SortAdorner _CurAdorner = null;
        private PresentadorGestionarInfoBolMarcaTer _presentador;
        private bool _cargada;
        BackgroundWorker _bgw = new BackgroundWorker();

        #region IAgregarInfoBolMarcaTer

        public void OculatarControlesAlAgregar()
        {
            this._btnEliminar.Visibility = System.Windows.Visibility.Collapsed;
            this._cbxTipo.IsEnabled = true;
        }

        public void FocoPredeterminado()
        {
            this._cbxTipo.Focus();
        }

        public string TextoBotonModificar
        {
            get { return this._txbAceptar.Text; }
            set { this._txbAceptar.Text = value; }
        }

        public bool HabilitarCampos
        {
            set
            {
                this._cbxBoletin.IsEnabled = value;
                this._txtCambio.IsEnabled = value;
                this._txtComentario.IsEnabled = value;
                this._txtPagina.IsEnabled = value;
                this._txtResolucion.IsEnabled = value;
                this._cbxTomo.IsEnabled = value;
                this._dpkFecha.IsEnabled = value;
            }
        }

        public bool EstaCargada
        {
            get { return this._cargada; }
            set { this._cargada = value; }
        }

        public object InfoBolMarcaTer
        {
            get { return this._gridDatos.DataContext; }
            set { this._gridDatos.DataContext = value; }
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

        public object Cambios
        {
            get { return this._lstCambios.DataContext; }
            set { this._lstCambios.DataContext = value; }
        }

        public object Cambio
        {
            get { return this._lstCambios.SelectedItem; }
            set { this._lstCambios.SelectedItem = value; }
        }

        public object Tomos
        {
            get { return this._cbxTomo.DataContext; }
            set { this._cbxTomo.DataContext = value; }
        }

        public object Tomo
        {
            get { return this._cbxTomo.SelectedItem; }
            set { this._cbxTomo.SelectedItem = value; }
        }

        public object Tipos
        {
            get { return this._cbxTipo.DataContext; }
            set { this._cbxTipo.DataContext = value; }
        }

        public object Tipo
        {
            get { return this._cbxTipo.SelectedItem; }
            set { this._cbxTipo.SelectedItem = value; }
        }

        public string TextoCambio
        {
            get { return this._txtCambio.Text; }
            set { this._txtCambio.Text = value; }
        }

        public void BorrarTextoCambio()
        {
            this._txtCambio.Text = "";
        }

        public bool Mensaje(string mensaje)
        {
            this._txtMensaje.Text = mensaje;
            return true;
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

        #endregion

        /// <summary>
        /// Constructor por defecto que recibe un infobol
        /// </summary>
        /// <param name="infoBol"></param>
        public GestionarInfoBolMarcaTer(object infoBol)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorGestionarInfoBolMarcaTer(this, infoBol);

            _bgw.WorkerReportsProgress = true;
            _bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_DoWork);
            _bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            _bgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bgw_ProgressChanged);
        }


        public GestionarInfoBolMarcaTer(object infoBol, object ventanaPadre)
        {
            InitializeComponent();
            this._cargada = false;
            //this._presentador = new PresentadorGestionarInfoBolMarcaTer(this, infoBol);
            this._presentador = new PresentadorGestionarInfoBolMarcaTer(this, infoBol, ventanaPadre, null);

            _bgw.WorkerReportsProgress = true;
            _bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_DoWork);
            _bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            _bgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bgw_ProgressChanged);
        }


        public GestionarInfoBolMarcaTer(object infoBol, object ventanaPadre, object ventanaPadreListaInfoboles)
        {
            InitializeComponent();
            this._cargada = false;
            //this._presentador = new PresentadorGestionarInfoBolMarcaTer(this, infoBol);
            this._presentador = new PresentadorGestionarInfoBolMarcaTer(this, infoBol, ventanaPadre, ventanaPadreListaInfoboles);
            _bgw.WorkerReportsProgress = true;
            _bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_DoWork);
            _bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            _bgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bgw_ProgressChanged);
        }

        private void _btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (this._presentador.Aceptar())
            {
                _bgw.RunWorkerAsync();
            }
        }

        void bgw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            _bgw.ReportProgress(1);
            Thread.Sleep(2000);
        }

        void bgw_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            this._txtMensaje.Text = "Operación realizada exitósamente.";
        }

        void bgw_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this._presentador.irListaInfoBolMarcaTer();
        }

        private void _btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.Regresar();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!EstaCargada)
            {
                this._presentador.CargarPagina();
                EstaCargada = true;
            }
        }

        private void _btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show(Recursos.MensajesConElUsuario.ConfirmacionEliminarInfoBol,
                "Eliminar InfoBolMarcaTer", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                if (this._presentador.Eliminar())
                {
                    _bgw.RunWorkerAsync();
                }
            }
        }


        private void mostrarLstCambio()
        {
            this._txtCambio.Visibility = System.Windows.Visibility.Collapsed;
            this._lstCambios.Visibility = System.Windows.Visibility.Visible;
            this._lstCambios.IsEnabled = true;
        }

        private void ocultarLstCambio()
        {
            this._lstCambios.Visibility = System.Windows.Visibility.Collapsed;
            this._txtCambio.Visibility = System.Windows.Visibility.Visible;
        }

        private void _lstCambios_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this._presentador.CambiarCambio();
            ocultarLstCambio();
        }

        private void _txtCambio_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this._presentador.TieneElementosListaCambio())
            {
                mostrarLstCambio();
            }
            else
            {
                MessageBox.Show("No existen cambios para el tipo seleccionado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void _OrdenarCambio_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstCambios);
        }

        private void _cbxTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BorrarTextoCambio();
            ocultarLstCambio();
            this._presentador.CargarCambio();
        }

    }
}
