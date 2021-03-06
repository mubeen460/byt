﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.Cliente.Ayuda;
using Trascend.Bolet.Cliente.Contratos.Asociados;
using Trascend.Bolet.Cliente.Presentadores.Asociados;

namespace Trascend.Bolet.Cliente.Ventanas.Asociados
{
    /// <summary>
    /// Interaction logic for ListaAuditorias.xaml
    /// </summary>
    public partial class ListaContactos : Page, IListaContactos
    {
        
        private GridViewColumnHeader _CurSortCol = null;
        private SortAdorner _CurAdorner = null;
        private PresentadorListaContactos _presentador;
        private bool _cargada;


        #region IListaContactos

        public bool EstaCargada
        {
            get { return this._cargada; }
            set { this._cargada = value; }
        }

        public object Contactos
        {
            get { return this._lstResultados.DataContext; }
            set { this._lstResultados.DataContext = value; }
        }

        public object ContactoSeleccionado
        {
            get { return this._lstResultados.SelectedItem; }
        }

        public void FocoPredeterminado()
        {
            this._btnRegresar.Focus();
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

        public ListaContactos(object asociado,object ventanaPadre)
        {
            InitializeComponent();
            this._cargada= false;
            this._presentador = new PresentadorListaContactos(this, asociado,ventanaPadre);

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!EstaCargada)
            {
                this._presentador.CargarPagina();
                EstaCargada = true;
            }
        }

        private void _Ordenar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader);
        }

        private void _btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.RegresarVentanaPadre();
        }

        private void _btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.IrAgregarContacto();
        }

        private void _lstResultados_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this._presentador.IrConsultarContacto();
        }



        private void _btnVerEnviada_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.ConsultarUltimaCorrespondenciaEnviada();
        }

        private void _btnVerCreacion_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.ConsultarCorrespondenciaCreacion();
        }

        private void _btnVerEntrada_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.ConsultarUltimaCorrespondenciaEntrada();
        }

        private void _btnRegistrarCxP_Click(object sender, RoutedEventArgs e)
        {
            if (this._lstResultados.SelectedItem != null)
            {
                if (MessageBoxResult.Yes == System.Windows.MessageBox.Show(string.Format(Recursos.MensajesConElUsuario.ConfirmarRegistroContactoCxp),
                    "Registro de Contacto CxP", MessageBoxButton.YesNo, MessageBoxImage.Question))
                {
                    this._presentador.IrRegistrarContactoCxP();
                }
            }
            else
                Mensaje("Debe seleccionar un contacto para registrarlo como Contacto CxP", 0);
                
        }

        public void Mensaje(string mensaje, int opcion)
        {
            if (opcion == 0)
                MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (opcion == 1)
                MessageBox.Show(mensaje, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
                MessageBox.Show(mensaje, "Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void _btnVerContactosCxC_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.IrListaContactosCxC();
        }

        public void PintarBotonListaContactosCxC()
        {
            this._btnVerContactosCxC.Background = Brushes.LightGreen;
        }

        //private void _btnSeleccionar_Click(object sender, RoutedEventArgs e)
        //{
        //    this._presentador.SeleccionarContacto();
        //}
    }
}
