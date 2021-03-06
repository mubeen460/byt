﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.Cliente.Ayuda;
using Trascend.Bolet.Cliente.Contratos.Patentes;
using Trascend.Bolet.Cliente.Presentadores.Patentes;
using System;

namespace Trascend.Bolet.Cliente.Ventanas.Patentes
{
    /// <summary>
    /// Interaction logic for ListaAuditorias.xaml
    /// </summary>
    public partial class ListaOperaciones : Page, IListaOperaciones
    {

        private GridViewColumnHeader _CurSortCol = null;
        private SortAdorner _CurAdorner = null;
        private PresentadorListaOperaciones _presentador;
        private bool _cargada;


        #region IListaOperaciones

        public bool EstaCargada
        {
            get { return this._cargada; }
            set { this._cargada = value; }
        }

        public object Operaciones
        {
            get { return this._lstResultados.DataContext; }
            set { this._lstResultados.DataContext = value; }
        }

        public object OperacionSeleccionado
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

        public ListaOperaciones(object patente)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorListaOperaciones(this, patente);

        }


        public ListaOperaciones(object patente, object ventanaPadre)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorListaOperaciones(this, patente,ventanaPadre);

        }

        /// <summary>
        /// Constructor predeterminado que recibe una lista de operaciones 
        /// </summary>
        /// <param name="operaciones">Lista de Operaciones</param>
        /// <param name="ventanaPadre">Ventana padre</param>
        /// <param name="usarOperaciones">Bandera para saber si se usa la lista de operaciones o la Patente</param>
        public ListaOperaciones(object operaciones, object ventanaPadre, bool usarOperaciones)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorListaOperaciones(this, operaciones, ventanaPadre, usarOperaciones);
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
            //this._presentador.Regresar();
            this._presentador.RegresarVentanaPadre();
        }

        private void EventoIrGestionarOperacion(object sender, EventArgs e)
        {
            this._presentador.IrGestionarOperacion();
        }
    }
}
