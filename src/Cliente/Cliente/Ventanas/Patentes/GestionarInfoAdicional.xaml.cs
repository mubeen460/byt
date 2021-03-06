﻿using System.Windows;
using System.Windows.Controls;
using Trascend.Bolet.Cliente.Contratos.Patentes;
using Trascend.Bolet.Cliente.Presentadores.Patentes;
using System;
using System.Windows.Threading;
using System.ComponentModel;
using System.Threading;
using System.Windows.Media;

namespace Trascend.Bolet.Cliente.Ventanas.Patentes
{
    /// <summary>
    /// Interaction logic for AgregarObjeto.xaml
    /// </summary>
    public partial class GestionarInfoAdicional : Page, IGestionarInfoAdicional
    {
        private PresentadorGestionarInfoAdicional _presentador;
        private bool _cargada;
        BackgroundWorker _bgw = new BackgroundWorker();
        private string _tab;

        #region IAgregarInfoAdicional

        public void OculatarControlesAlAgregar()
        {
            this._btnAuditoria.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void FocoPredeterminado()
        {
            this._txtNombre.Focus();
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
                this._txtNombre.IsEnabled = value;
                this._txtEmail.IsEnabled = value;
                this._txtInfo.IsEnabled = value;
            }
        }

        public bool EstaCargada
        {
            get { return this._cargada; }
            set { this._cargada = value; }
        }

        public object InfoAdicional
        {
            get { return this._gridDatos.DataContext; }
            set { this._gridDatos.DataContext = value; }
        }

        public void PintarAuditoria()
        {
            this._btnAuditoria.Background = Brushes.LightGreen;
        }

        public string Tab
        {
            get { return this._tab; }
        }

        #endregion

        /// <summary>
        /// Constructor por defecto que recibe una patente
        /// </summary>
        /// <param name="patente">Patente consultada</param>
        public GestionarInfoAdicional(object patente)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorGestionarInfoAdicional(this, patente,null);

            _bgw.WorkerReportsProgress = true;
            _bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_DoWork);
            _bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            _bgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bgw_ProgressChanged);
        }

        /// <summary>
        /// Constructor por defecto que recibe una patente y la ventana ConsultarPatentes
        /// </summary>
        /// <param name="patente">Patente consultada</param>
        /// <param name="ventanaPadreConsultarPatentes">Ventana padre ConsultarPatentes</param>
        public GestionarInfoAdicional(object patente, object ventanaPadreConsultarPatentes)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorGestionarInfoAdicional(this, patente,ventanaPadreConsultarPatentes);

            _bgw.WorkerReportsProgress = true;
            _bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_DoWork);
            _bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            _bgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bgw_ProgressChanged);
        }


        /// <summary>
        /// REferencia a constructor por defecto que recibe una patente como parametro
        /// </summary>
        /// <param name="patente">Patente consultada</param>
        /// <param name="tab">Nombre del tab de la ventana anterior</param>
        public GestionarInfoAdicional(object patente, string tab)
            : this(patente)
        {
            this._tab = tab;
        }


        public GestionarInfoAdicional(object patente, string tab, object ventanaPadreConsultarPatentes)
            : this(patente, ventanaPadreConsultarPatentes)
        {
            this._tab = tab;
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
            this._presentador.IrGestionarPatente();
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

        private void _btnAuditoria_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.Auditoria();
        }
    }
}
