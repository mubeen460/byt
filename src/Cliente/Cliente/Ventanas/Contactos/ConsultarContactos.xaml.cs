﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trascend.Bolet.Cliente.Ayuda;
using Trascend.Bolet.Cliente.Contratos.Contactos;
using Trascend.Bolet.Cliente.Presentadores.Contactos;

namespace Trascend.Bolet.Cliente.Ventanas.Contactos
{
    /// <summary>
    /// Interaction logic for ConsultarTodosPoder.xaml
    /// </summary>
    public partial class ConsultarContactos : Page, IConsultarContactos
    {
        private GridViewColumnHeader _CurSortCol = null;
        private SortAdorner _CurAdorner = null;
        private PresentadorConsultarContactos _presentador;
        private bool _cargada;
        private bool _precargada = false;

        #region IConsultarCartas



        public object Resultados
        {
            get { return this._lstResultados.DataContext; }
            set { this._lstResultados.DataContext = value; }
        }

        public string Id
        {
            get { return this._txtId.Text; }
            set { this._txtId.Text = value; }
        }

        public object Responsable
        {
            get { return this._cbxResponsable.SelectedItem; }
            set { this._cbxResponsable.SelectedItem = value; }
        }

        public object Responsables
        {
            get { return this._cbxResponsable.DataContext; }
            set { this._cbxResponsable.DataContext = value; }
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
            set { this._txtIdAsociado.Text = value; }
        }


        public string NombreAsociadoFiltrar
        {
            get { return this._txtNombreAsociado.Text; }
            set { this._txtNombreAsociado.Text = value; }
        }

        public string ResumenFiltrar
        {
            get { return this._txtResumen.Text; }
            set { this._txtResumen.Text = value; }
        }

        public string ReferenciaFiltrar
        {
            get { return this._txtReferencia.Text; }
            set { this._txtReferencia.Text = value; }
        }

        public string Fecha
        {
            get { return this._dpkFecha.SelectedDate.ToString(); }
            set { this._dpkFecha.Text = value; }
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

        public void Mensaje(string mensaje, int opcion)
        {
            if (opcion == 0)
                MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show(mensaje, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public string TotalHits
        {
            set { this._lblHits.Text = value; }
        }

        public string NombreAsociado
        {
            set { this._txtAsociado.Text = value; }
        }
        #endregion

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        public ConsultarContactos()
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorConsultarContactos(this, null, null);
        }


        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        public ConsultarContactos(object ventana, object asociado)
            : this()
        {
            this._precargada = true;
            this._presentador = new PresentadorConsultarContactos(this, asociado, ventana);

        }


        /// <summary>
        /// Constructor Con Que recive la lista de cartas anteriormente consultada
        /// </summary>
        public ConsultarContactos(object listaCartas)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorConsultarContactos(this, null, null, listaCartas);
        }


        private void _btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (_precargada)
                this._presentador.RegresarVentanaPadre();
            else
                this._presentador.Cancelar();
        }


        private void _btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            this._btnConsultar.Focus();
            this._presentador.Consultar();
            this._dpkFecha.Text = string.Empty;
            validarCamposVacios();
        }


        private void _lstResultados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this._presentador.IrConsultarCarta();
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


        private void _btnLimpiarCampos_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.Navegar(new ConsultarContactos());
        }


        private void _cbxResponsable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void _txtAsociado_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MostrarCamposAsociadosFiltrar(Visibility.Visible);
            MostrarCampoAsociado(Visibility.Collapsed);
        }


        private void MostrarCamposAsociadosFiltrar(object visibilidad)
        {
            this._lblIdAsociado.Visibility = (Visibility)visibilidad;
            this._txtIdAsociado.Visibility = (Visibility)visibilidad;
            this._lblNombreAsociado.Visibility = (Visibility)visibilidad;
            this._txtNombreAsociado.Visibility = (Visibility)visibilidad;
            this._btnConsultarAsociado.Visibility = (Visibility)visibilidad;
            this._lstAsociados.Visibility = (Visibility)visibilidad;
        }


        private void MostrarCampoAsociado(object visibilidad)
        {
            //this._lblAsociado.Visibility = (Visibility)visibilidad;
            this._txtAsociado.Visibility = (Visibility)visibilidad;
        }


        private void _lstAsociados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this._presentador.CambiarAsociado())
            {
                MostrarCamposAsociadosFiltrar(Visibility.Collapsed);
                MostrarCampoAsociado(Visibility.Visible);
            }
        }

    }
}
