﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trascend.Bolet.Cliente.Ayuda;
using Trascend.Bolet.Cliente.Contratos.Internacionales;
using Trascend.Bolet.Cliente.Presentadores.Internacionales;

namespace Trascend.Bolet.Cliente.Ventanas.Internacionales
{
    /// <summary>
    /// Interaction logic for ConsultarInternacionales.xaml
    /// </summary>
    public partial class ConsultarInternacionales : Page, IConsultarInternacionales
    {
        private GridViewColumnHeader _CurSortCol = null;
        private SortAdorner _CurAdorner = null;
        private PresentadorConsultarInternacionales _presentador;
        private bool _cargada;

        #region IConsultarInternacionales

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

        public bool EstaCargada
        {
            get { return this._cargada; }
            set { this._cargada = value; }
        }

        public void FocoPredeterminado()
        {
            this._txtId.Focus();
        }

        public object InternacionalSeleccionado
        {
            get { return this._lstResultados.SelectedItem; }

        }

        public string Id
        {
            get { return this._txtId.Text; }
        }

        public string Descripcion
        {
            get { return this._txtDescripcion.Text; }
        }

        public object Resultados
        {
            get { return this._lstResultados.DataContext; }
            set { this._lstResultados.DataContext = value; }
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

        public ConsultarInternacionales()
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorConsultarInternacionales(this);

        }

        private void _btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.Cancelar();
        }

        private void _btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            this._btnConsultar.Focus();
            this._presentador.Consultar();
        }

        private void _lstResultados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this._presentador.IrConsultarInternacional();
        }

        private void _Ordenar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader);
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

    }
}
