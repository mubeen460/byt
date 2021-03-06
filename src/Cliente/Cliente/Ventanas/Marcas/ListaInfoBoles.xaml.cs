﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.Cliente.Ayuda;
using Trascend.Bolet.Cliente.Contratos.Marcas;
using Trascend.Bolet.Cliente.Presentadores.Marcas;
using System;

namespace Trascend.Bolet.Cliente.Ventanas.Marcas
{
    /// <summary>
    /// Interaction logic for ListaAuditorias.xaml
    /// </summary>
    public partial class ListaInfoBoles : Page, IListaInfoBoles
    {
        
        private GridViewColumnHeader _CurSortCol = null;
        private SortAdorner _CurAdorner = null;
        private PresentadorListaInfoBoles _presentador;
        private bool _cargada;


        #region IListaContactos

        public bool EstaCargada
        {
            get { return this._cargada; }
            set { this._cargada = value; }
        }

        public object InfoBoles
        {
            get { return this._lstResultados.DataContext; }
            set { this._lstResultados.DataContext = value; }
        }

        public object InfoBolSeleccionado
        {
            get { return this._lstResultados.SelectedItem; }
            set { this._lstResultados.SelectedItem = value; }
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

        /// <summary>
        /// Constructor por defecto de la ventana ListaInfoboles que no recibe una ventana padre por parametro
        /// </summary>
        /// <param name="marca"></param>
        public ListaInfoBoles(object marca)
        {
            InitializeComponent();
            this._cargada= false;
            this._presentador = new PresentadorListaInfoBoles(this, marca);

        }


        /// <summary>
        /// Constructor por defecto de la ventana ListaInfoboles que recibe una ventana padre por parametro
        /// </summary>
        /// <param name="marca"></param>
        public ListaInfoBoles(object marca, object ventanaPadre)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorListaInfoBoles(this, marca, ventanaPadre);

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
            //this._presentador.IrConsultarMarca();
            this._presentador.RegresarVentanaPadre();
        }

        private void EventoIrGestionarInfoBol(object sender, EventArgs e)
        {
            if (sender.GetType().ToString().Equals("System.Windows.Controls.Button"))
                this._presentador.IrGestionarInfoBol(true);
            else
                this._presentador.IrGestionarInfoBol(false);
        }
    }
}
