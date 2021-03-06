﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Trascend.Bolet.Cliente.Ayuda;
using Trascend.Bolet.Cliente.Contratos.ReportesMaestro;
using Trascend.Bolet.Cliente.Presentadores.ReportesMaestro;

namespace Trascend.Bolet.Cliente.Ventanas.ReportesMaestro
{
    /// <summary>
    /// Lógica de interacción para GestionarValoresFiltrosDeReporte.xaml
    /// </summary>
    public partial class GestionarValoresFiltrosDeReporte : Page, IGestionarValoresFiltrosDeReporte
    {

        private bool _cargada;
        private PresentadorGestionarValoresFiltrosDeReporte _presentador;


        public GestionarValoresFiltrosDeReporte(object reporte, object ventanaPadre)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorGestionarValoresFiltrosDeReporte(this, reporte, ventanaPadre);
        }


        #region IGestionarValoresFiltrosDeReporte

        public bool EstaCargada
        {
            get { return this._cargada; }
            set { this._cargada = value; }
        }

        public void FocoPredeterminado()
        {
            this._btnAceptar.Focus();
        }

        public object Reporte
        {
            get { return this._gridReporte.DataContext; }
            set { this._gridReporte.DataContext = value; }
        }

        public string TituloReporte
        {
            get { return this._txtTituloReporte.Text; }
            set { this._txtTituloReporte.Text = value; }
        }

        public string Usuario
        {
            get { return this._txtUsuarioReporte.Text; }
            set { this._txtUsuarioReporte.Text = value; }
        }

        public object Filtros
        {
            get { return this._grdFiltrosReporte.DataContext; }
            set { this._grdFiltrosReporte.DataContext = value; }
        }

        public object CamposDelReporte
        {
            get { return this._cbxCamposReporte.DataContext; }
            set { this._cbxCamposReporte.DataContext = value; }
        }

        public object CampoDelReporte
        {
            get { return this._cbxCamposReporte.SelectedItem; }
            set { this._cbxCamposReporte.SelectedItem = value; }
        }

        public object TiposDeOrdenamiento
        {
            get { return this._cbxTipoOrdenamientoReporte.DataContext; }
            set { this._cbxTipoOrdenamientoReporte.DataContext = value; }
        }

        public object TipoDeOrdenamiento
        {
            get { return this._cbxTipoOrdenamientoReporte.SelectedItem; }
            set { this._cbxTipoOrdenamientoReporte.SelectedItem = value; }
        }

        public object OrdenamientosReporte
        {
            get { return this._lstOrdenReporte.DataContext; }
            set { this._lstOrdenReporte.DataContext = value; }
        }

        public object OrdenamientoReporte
        {
            get { return this._lstOrdenReporte.SelectedItem; }
            set { this._lstOrdenReporte.SelectedItem = value; }
        }


        #endregion

        #region Eventos

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

        private void _btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.EjecutarReporte();
        }

        private void _btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.RegresarVentanaPadre();
        }

        private void _btnAgregarOrdenamiento_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.AgregarOrdenamientoAReporte();
        }

        private void _btnQuitarOrdenamiento_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.QuitarOrdenamientoAReporte();
        }


        #endregion

        #region Metodos

        public void Mensaje(string mensaje, int opcion)
        {
            if (opcion == 0)
                MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show(mensaje, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        #endregion

        

    }
}
