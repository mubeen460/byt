﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.ComponentModel;
using System.Threading;
using Trascend.Bolet.Cliente.Contratos.Marcas;
using Trascend.Bolet.Cliente.Presentadores.Marcas;

namespace Trascend.Bolet.Cliente.Ventanas.Marcas
{
    /// <summary>
    /// Lógica de interacción para GestionarArchivoDeMarca.xaml
    /// </summary>
    public partial class GestionarArchivoDeMarca : Page, IGestionarArchivoDeMarca
    {

        private PresentadorGestionarArchivoDeMarca _presentador;
        private bool _cargada;
        BackgroundWorker _bgw = new BackgroundWorker();
        private bool _camposHabilitados = true;


        #region IGestionarArchivoDeMarca

        public bool EstaCargada
        {
            get { return this._cargada; }
            set { this._cargada = value; }
        }

        public object Archivo
        {
            get { return this._gridDatos.DataContext; }
            set { this._gridDatos.DataContext = value; }
        }

        public void FocoPredeterminado()
        {
            this._txtIdArchivo.Focus();
        }


        public string IdMarcaArchivo
        {
            get { return this._txtExpediente.Text; }
            set { this._txtExpediente.Text = value; }
        }

        public string IdArchivo
        {
            get { return this._txtIdArchivo.Text; }
            set { this._txtIdArchivo.Text = value; }
        }

        public string AuxArchivo
        {
            get { return this._txtAuxArchivo.Text; }
            set { this._txtAuxArchivo.Text = value; }
        }

        public object Documentos
        {
            get { return this._cbxDocumentoArchivo.DataContext; }
            set { this._cbxDocumentoArchivo.DataContext = value; }
        }

        public object Documento
        {
            get { return this._cbxDocumentoArchivo.SelectedItem; }
            set { this._cbxDocumentoArchivo.SelectedItem = value; }
        }

        public object TipoDocumentos
        {
            get { return this._cbxTipoDocumentoArchivo.DataContext; }
            set { this._cbxTipoDocumentoArchivo.DataContext = value; }
        }

        public object TipoDocumento
        {
            get { return this._cbxTipoDocumentoArchivo.SelectedItem; }
            set { this._cbxTipoDocumentoArchivo.SelectedItem = value; }
        }

        public object TipoCajas
        {
            get { return this._cbxTipoCajaArchivo.DataContext; }
            set { this._cbxTipoCajaArchivo.DataContext = value; }
        }

        public object TipoCaja
        {
            get { return this._cbxTipoCajaArchivo.SelectedItem; }
            set { this._cbxTipoCajaArchivo.SelectedItem = value; }
        }

        public object Cajas
        {
            get { return this._cbxCajaArchivo.DataContext; }
            set { this._cbxCajaArchivo.DataContext = value; }
        }

        public object Caja
        {
            get { return this._cbxCajaArchivo.SelectedItem; }
            set { this._cbxCajaArchivo.SelectedItem = value; }
        }

        public object Almacenes
        {
            get { return this._cbxAlmacenArchivo.DataContext; }
            set { this._cbxAlmacenArchivo.DataContext = value; }
        }

        public object Almacen
        {
            get { return this._cbxAlmacenArchivo.SelectedItem; }
            set { this._cbxAlmacenArchivo.SelectedItem = value; }
        }

        public object Usuarios
        {
            get { return this._cbxUsuarioArchivo.DataContext; }
            set { this._cbxUsuarioArchivo.DataContext = value; }
        }

        public object Usuario
        {
            get { return this._cbxUsuarioArchivo.SelectedItem; }
            set { this._cbxUsuarioArchivo.SelectedItem = value; }
        }

        public string UbicacionMarca
        {
            get { return this._txtUbicacionMarcaArchivo.Text; }
            set { this._txtUbicacionMarcaArchivo.Text = value; }
        }


        #endregion




        #region Constructores

        public GestionarArchivoDeMarca(object archivo, object marca)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorGestionarArchivoDeMarca(this, archivo, marca, null);

            _bgw.WorkerReportsProgress = true;
            _bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_DoWork);
            _bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            _bgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bgw_ProgressChanged);
        }


        public GestionarArchivoDeMarca(object archivo, object marca, object ventanaPadre)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorGestionarArchivoDeMarca(this, archivo, marca, ventanaPadre);
            _bgw.WorkerReportsProgress = true;
            _bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_DoWork);
            _bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            _bgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bgw_ProgressChanged);
        }


        void bgw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            _bgw.ReportProgress(1);
            Thread.Sleep(2000);
        }

        void bgw_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            this._txtMensaje.Text = "Archivo de Marca modificado exitosamente.";
        }

        void bgw_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //this._presentador.IrConsultarMarca();
        }

        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!EstaCargada)
            {
                this._presentador.CargarPagina();
                EstaCargada = true;
            }
            else
                this._presentador.ActualizarTitulo();
        }

        public void Mensaje(string mensaje, int opcion)
        {
            if (opcion == 0)
                MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show(mensaje, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void _btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.RegresarVentanaPadre();
        }

        private void _btnModificar_Click(object sender, RoutedEventArgs e)
        {
            
            if (MessageBoxResult.Yes == MessageBox.Show(string.Format(Recursos.MensajesConElUsuario.ConfirmarModificarArchivoMarca,
                _presentador.ObtenerIdMarca()),
                "Modificar Archivo de Marca", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                if (this._presentador.Modificar())
                {
                    _bgw.RunWorkerAsync();
                }

            }


           
        }


        public void ActivarBotonModificar(bool activarArchivo)
        {
            if (activarArchivo)
                this._btnModificar.IsEnabled = true;
            else
                this._btnModificar.IsEnabled = false;
        }


        public void MostarMensajeCompletadoConExito()
        {
            MessageBox.Show("Actualizacion del Archivo con Exito", "Archivo de Marca", MessageBoxButton.OK);
        }

        
    }
}
