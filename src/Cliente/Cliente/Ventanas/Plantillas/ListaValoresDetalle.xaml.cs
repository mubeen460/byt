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
using Trascend.Bolet.Cliente.Contratos.Plantillas;
using Trascend.Bolet.Cliente.Presentadores.Plantillas;

namespace Trascend.Bolet.Cliente.Ventanas.Plantillas
{
    /// <summary>
    /// Lógica de interacción para ListaValoresDetalle.xaml
    /// </summary>
    public partial class ListaValoresDetalle : Page, IListaValoresDetalle
    {
        private bool _cargada;
        private PresentadorListaValoresDetalle _presentador;


        /// <summary>
        /// Constructor por defecto que recibe la plantilla y la ventana que precede a esta ventana
        /// </summary>
        /// <param name="maestroPlantilla"></param>
        /// <param name="ventanaPadre">Ventana GestionarMaestroPlantilla</param>
        /// <param name="ventanaPadreConsultarMaestroPlantilla">Ventana ConsultarMaestrosPlantillas</param>
        public ListaValoresDetalle(object maestroPlantilla, object ventanaPadre, object ventanaPadreConsultarMaestroPlantilla)
        {
            InitializeComponent();
            this._cargada = false;
            this._presentador = new PresentadorListaValoresDetalle(this, maestroPlantilla, ventanaPadre, ventanaPadreConsultarMaestroPlantilla);
        }


        #region IListaValoresDetalle

        public bool EstaCargada
        {
            get { return this._cargada; }
            set { this._cargada = value; }
        }

        public void FocoPredeterminado()
        {
            this._btnRegresar.Focus();
        }

        public object FiltrosEncabezado
        {
            get { return this._lstResultados.DataContext; }
            set { this._lstResultados.DataContext = value; }
        }

        public object FiltroEncabezado
        {
            get { return this._lstResultados.SelectedItem; }
            set { this._lstResultados.SelectedItem = value; }
        }


        public string TotalHits
        {
            get { return this._lblHits.Text; }
            set { this._lblHits.Text = value; }
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


        private void _btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            //this._presentador.RegresarVentanaPadre();
            this._presentador.IrCargarMaestroDePlantilla();
        }


        private void EventoIrGestionarFiltroPlantilla(object sender, EventArgs e)
        {
            if (sender.GetType().ToString().Equals("System.Windows.Controls.Button"))
                this._presentador.IrGestionarFiltroEncabezado(true);
            else
                this._presentador.IrGestionarFiltroEncabezado(false);
        }

        #endregion

        
    }
}
