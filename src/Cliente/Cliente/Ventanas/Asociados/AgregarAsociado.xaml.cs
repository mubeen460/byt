﻿using System.Windows.Controls;
using Trascend.Bolet.Cliente.Contratos.Asociados;
using Trascend.Bolet.Cliente.Presentadores.Asociados;
using System.Windows;

namespace Trascend.Bolet.Cliente.Ventanas.Asociados
{
    /// <summary>
    /// Interaction logic for AgregarAsociado.xaml
    /// </summary>
    public partial class AgregarAsociado : Page, IAgregarAsociado
    {
        private PresentadorAgregarAsociado _presentador;
        private bool _cargada;

        #region IAgregarAsociado

        public object Asociado
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public object Pais
        {
            get { return this._cbxPaisDatos.SelectedItem; }
            set { this._cbxPaisDatos.SelectedItem = value; }
        }

        public object Paises
        {
            get { return this._cbxPaisDatos.DataContext; }
            set { this._cbxPaisDatos.DataContext = value; }
        }

        public object Idioma
        {
            get { return this._cbxIdiomaDatos.SelectedItem; }
            set { this._cbxIdiomaDatos.SelectedItem = value; }
        }

        public object Idiomas
        {
            get { return this._cbxIdiomaDatos.DataContext; }
            set { this._cbxIdiomaDatos.DataContext = value; }
        }

        public object Moneda
        {
            get { return this._cbxMonedaDatos.SelectedItem; }
            set { this._cbxMonedaDatos.SelectedItem = value; }
        }

        public object Monedas
        {
            get { return this._cbxMonedaDatos.DataContext; }
            set { this._cbxMonedaDatos.DataContext = value; }
        }

        public object Descuento
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public object Descuentos
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public object TipoCliente
        {
            get { return this._cbxTipoClienteAdministracion.SelectedItem; }
            set { this._cbxTipoClienteAdministracion.SelectedItem = value; }
        }

        public object TiposClientes
        {
            get { return this._cbxTipoClienteAdministracion.DataContext; }
            set { this._cbxTipoClienteAdministracion.DataContext = value; }
        }

        public object Etiqueta
        {
            get { return this._cbxEtiquetaAdministracion.SelectedItem; }
            set { this._cbxEtiquetaAdministracion.SelectedItem = value; }
        }

        public object Etiquetas
        {
            get { return this._cbxEtiquetaAdministracion.DataContext; }
            set { this._cbxEtiquetaAdministracion.DataContext = value; }
        }

        public object DetallePago
        {
            get { return this._cbxDetallePagoAdministracion.SelectedItem; }
            set { this._cbxDetallePagoAdministracion.SelectedItem = value; }
        }

        public object DetallesPagos
        {
            get { return this._cbxDetallePagoAdministracion.DataContext; }
            set { this._cbxDetallePagoAdministracion.DataContext = value; }
        }

        public object Tarifa
        {
            get { return this._cbxTarifaAdministracion.SelectedItem; }
            set { this._cbxTarifaAdministracion.SelectedItem = value; }
        }

        public object Tarifas
        {
            get { return this._cbxTarifaAdministracion.DataContext; }
            set { this._cbxTarifaAdministracion.DataContext = value; }
        }

        public char TipoPersona
        {
            get
            {
                if (!string.IsNullOrEmpty(this._cbxTipoPersonaDatos.Text))
                    return ((string)this._cbxTipoPersonaDatos.Text)[0];
                else
                    return ' ';
            }
        }

        public bool EstaCargada
        {
            get { return this._cargada; }
            set { this._cargada = value; }
        }

        public void FocoPredeterminado()
        {
            this._txtNombreDatos.Focus();
        }

        #endregion

        public AgregarAsociado()
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorAgregarAsociado(this);
        }

        private void _btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.Cancelar();
        }

        private void _btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.Aceptar();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!EstaCargada)
            {
                this._presentador.CargarPagina();
                EstaCargada = true;
            }
        }
    }
}
