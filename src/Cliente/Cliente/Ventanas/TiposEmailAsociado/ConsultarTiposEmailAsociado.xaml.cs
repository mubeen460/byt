﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trascend.Bolet.Cliente.Ayuda;
using Trascend.Bolet.Cliente.Contratos.TiposEmailAsociado;
using Trascend.Bolet.Cliente.Presentadores.TiposEmailAsociado;

namespace Trascend.Bolet.Cliente.Ventanas.TiposEmailAsociado
{
    /// <summary>
    /// Interaction logic for ConsultarObjetos.xaml
    /// </summary>
    public partial class ConsultarTiposEmailAsociado : Page, IConsultarTiposEmailAsociado
    {
        private GridViewColumnHeader _CurSortCol = null;
        private SortAdorner _CurAdorner = null;
        private PresentadorConsultarTiposEmailAsociado _presentador;
        private bool _cargada;

        #region IConsultarPaises

        public bool EstaCargada
        {
            get { return this._cargada; }
            set { this._cargada = value; }
        }

        public void FocoPredeterminado()
        {
        }

        public object TipoEmailAsociadoSeleccionado
        {
            get { return this._lstResultados.SelectedItem; }
            set { this._lstResultados.SelectedItem = value; }

        }

        public object Resultados
        {
            get { return this._lstResultados.DataContext; }
            set { this._lstResultados.DataContext = value; }
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

        public string TotalHits
        {
            set { this._lblHits.Text = value; }
        }

        #endregion

        public ConsultarTiposEmailAsociado()
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorConsultarTiposEmailAsociado(this);

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

        private void _btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.Cancelar();
        }

        private void _btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            //this._btnConsultar.Focus();
            //this._presentador.Consultar();
            //validarCamposVacios();
        }

        private void _lstResultados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this._presentador.IrConsultarTipoEmailAsociado();
        }

        private void _Ordenar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader);
        }

        private void validarCamposVacios()
        {
            //bool todosCamposVacios = true;
            //if (!this._txtId.Text.Equals(""))
            //{
            //    todosCamposVacios = false;
            //    this._txtId.Focus();
            //}

            //if (!this._txtCodigo.Text.Equals(""))
            //{
            //    todosCamposVacios = false;
            //    this._txtCodigo.Focus();
            //}

            //if (!this._txtNombreIngles.Text.Equals(""))
            //{
            //    todosCamposVacios = false;
            //    this._txtNombreIngles.Focus();
            //}

            //if (!this._txtNombreEspanol.Text.Equals(""))
            //{
            //    todosCamposVacios = false;
            //    this._txtNombreEspanol.Focus();
            //}

            //if (!this._txtNacionalidad.Text.Equals(""))
            //{
            //    todosCamposVacios = false;
            //    this._txtNacionalidad.Focus();
            //}

            //if (!this._txtNacionalidad.Text.Equals(""))
            //{
            //    todosCamposVacios = false;
            //    this._txtNacionalidad.Focus();
            //}

            //if ((this._cbxRegion.SelectedIndex != 0) && (this._cbxRegion.SelectedIndex != -1))
            //{
            //    todosCamposVacios = false;
            //    this._cbxRegion.Focus();
            //}

            //if (todosCamposVacios)
            //    this._txtId.Focus();
        }
    }
}
