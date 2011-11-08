﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trascend.Bolet.Cliente.Ayuda;
using Trascend.Bolet.Cliente.Contratos.Cartas;
using Trascend.Bolet.Cliente.Presentadores.CartasOuts;

namespace Trascend.Bolet.Cliente.Ventanas.CartasOuts
{
    /// <summary>
    /// Interaction logic for ConsultarTodosPoder.xaml
    /// </summary>
    public partial class ConsultarCartasOuts : Page, IConsultarCartas
    {
        private GridViewColumnHeader _CurSortCol = null;
        private SortAdorner _CurAdorner = null;
        private PresentadorConsultarCartasOuts _presentador;
        private bool _cargada;

        #region IConsultarCartas



        public object Resultados
        {
            get { return this._lstResultados.DataContext; }
            set { this._lstResultados.DataContext = value; }
        }

        public string Id
        {
            get { return this._txtId.Text; }
        }


        public object CartaSeleccionado
        {
            get { return this._lstResultados.SelectedItem; }
        }

        public object CartaFiltrar
        {
            get { return this._splFiltro.DataContext; }
            set { this._splFiltro.DataContext = value; }
        }

        public string IdAsociadoFiltrar
        {
            get { return this._txtIdAsociado.Text; }
        }

        public string NombreAsociadoFiltrar
        {
            get { return this._txtNombreAsociado.Text; }
        }

        public string ResumenFiltrar
        {
            get { return this._txtResumen.Text; }
        }

        public string Fecha
        {
            get { return this._dpkFecha.SelectedDate.ToString(); }
        }

        public bool EstaCargada
        {
            get { return this._cargada; }
            set { this._cargada = value; }
        }

        public void FocoPredeterminado()
        {
            this._txtId.Focus();
        }

        public object PoderSeleccionado
        {
            get { return this._lstResultados.SelectedItem; }
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

        public ListView ListaResultados
        {
            get { return this._lstResultados; }
            set { this._lstResultados = value; }
        }




        public object Asociados
        {
            get { return this._lstAsociados.DataContext; }
            set { this._lstAsociados.DataContext = value; }
        }

        public object Asociado
        {
            get { return this._lstAsociados.SelectedItem; }
            set { this._lstAsociados.SelectedItem = value; }
        }
        
        public void Mensaje(string mensaje)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        public ConsultarCartasOuts()
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorConsultarCartasOuts(this);
        }

        private void _btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.Cancelar();
        }

        private void _btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            this._btnConsultar.Focus();
            this._presentador.Consultar();
            validarCamposVacios();
        }

        private void _lstResultados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //this._presentador.IrConsultarPoder();
        }

        private void _Ordenar_Click(object sender, RoutedEventArgs e)
        {
            //this._presentador.OrdenarColumna(sender as GridViewColumnHeader);
        }

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

        private void _btnConsultarAsociado_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.BuscarAsociado();
        }

        private void _btnConsultarInteresadoFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultar.IsDefault = false;
            this._btnConsultarAsociado.IsDefault = true;
        }

        private void _btnConsultarFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultar.IsDefault = true;
            this._btnConsultarAsociado.IsDefault = false;
        }

        /// <summary>
        /// Método que se encarga de posicionar el cursor en los campos del filto
        /// </summary>
        private void validarCamposVacios()
        {
            bool todosCamposVacios = true;
            if (!this._txtId.Text.Equals(""))
            {
                todosCamposVacios = false;
                this._txtId.Focus();
            }

            if (!this._txtResumen.Text.Equals(""))
            {
                todosCamposVacios = false;
                this._txtResumen.Focus();
            }

            if (!this._dpkFecha.Text.Equals(""))
            {
                todosCamposVacios = false;
                this._dpkFecha.Focus();
            }

            if (todosCamposVacios)
                this._txtId.Focus();
        }

        private void _dpkFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void _btnTransferir_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
