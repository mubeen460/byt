﻿using System.Windows.Controls;
using Trascend.Bolet.Cliente.Contratos.MarcasTercero;
using Trascend.Bolet.Cliente.Presentadores.MarcasTercero;
using System.Windows;
using System.Windows.Input;
using Trascend.Bolet.Cliente.Ayuda;
using System.Windows.Media;
using System;

namespace Trascend.Bolet.Cliente.Ventanas.MarcasTercero
{
    /// <summary>
    /// Interaction logic for AgregarMarcasTercer.xaml
    /// </summary>
    public partial class ConsultarMarcaTercero : Page, IConsultarMarcaTercero
    {
        private GridViewColumnHeader _CurSortCol = null;
        private SortAdorner _CurAdorner = null;
        private PresentadorConsultarMarcaTercero _presentador;
        private bool _cargada;
        private bool _agregar = false;

        private bool _asociadosCargados;
        private bool _interesadosCargados;
        private bool _corresponsalesCargados;
        private bool _poderesCargados;
        private bool _byt;


        #region IConsultarMarcaTercero

        public object MarcaTercero
        {
            get { return this._gridDatos.DataContext; }
            set { this._gridDatos.DataContext = value; }
        }

        //public string NumPoderDatos
        //{
        //    get { return this._txtPoderDatos.Text; }
        //    set { this._txtPoderDatos.Text = value; }
        //}

        public string NumPoderSolicitud
        {
            get { return this._txtPoderSolicitud.Text; }
            set { this._txtPoderSolicitud.Text = value; }
        }


        public object PoderesSolicitud
        {
            get { return this._lstPoderesSolicitud.DataContext; }
            set { this._lstPoderesSolicitud.DataContext = value; }
        }

        public object PoderSolicitud
        {
            get { return this._lstPoderesSolicitud.SelectedItem; }
            set { this._lstPoderesSolicitud.SelectedItem = value; }
        }

        public object MarcasFiltradas
        {
            get { return this._lstMarcas.DataContext; }
            set { this._lstMarcas.DataContext = value; }
        }
        //public object PoderesDatos
        //{
        //    get { return this._lstPoderesDatos.DataContext; }
        //    set { this._lstPoderesDatos.DataContext = value; }
        //}

        //public object PoderDatos
        //{
        //    get { return this._lstPoderesDatos.SelectedItem; }
        //    set { this._lstPoderesDatos.SelectedItem = value; }
        //}

        public object Agentes
        {
            get { return this._cbxAgente.DataContext; }
            set { this._cbxAgente.DataContext = value; }
        }

        public object TipoCbx
        {
            get { return this._cbxTipo.DataContext; }
            set { this._cbxTipo.DataContext = value; }
        }

        public object TiposCbx
        {
            get { return this._cbxTipo.SelectedItem; }
            set { this._cbxTipo.SelectedItem = value; }
        }

        public object Agente
        {
            get { return this._cbxAgente.SelectedItem; }
            set { this._cbxAgente.SelectedItem = value; }
        }

        public object BoletinesOrdenPublicacion
        {
            get { return this._cbxOrdenPublicacion.DataContext; }
            set { this._cbxOrdenPublicacion.DataContext = value; }
        }

        public object BoletinOrdenPublicacion
        {
            get { return this._cbxOrdenPublicacion.SelectedItem; }
            set { this._cbxOrdenPublicacion.SelectedItem = value; }
        }

        public object MarcaByt
        {
            get { return this._lstMarcasB.SelectedItem; }
            set { this._lstMarcasB.SelectedItem = value; }
        }

        public object MarcasByt
        {
            get { return this._lstMarcasB.DataContext; }
            set { this._lstMarcasB.DataContext = value; }
        }

        public object BoletinesPublicacion
        {
            get { return this._cbxBoletinPublicacion.DataContext; }
            set { this._cbxBoletinPublicacion.DataContext = value; }
        }

        public object BoletinPublicacion
        {
            get { return this._cbxBoletinPublicacion.SelectedItem; }
            set { this._cbxBoletinPublicacion.SelectedItem = value; }
        }
        
        public object BoletinesConcesion
        {
            get { return this._cbxBoletinConcesion.DataContext; }
            set { this._cbxBoletinConcesion.DataContext = value; }
        }

        public object BoletinConcesion
        {
            get { return this._cbxBoletinConcesion.SelectedItem; }
            set { this._cbxBoletinConcesion.SelectedItem = value; }
        }

        public object Situaciones
        {
            get { return this._cbxSituacion.DataContext; }
            set { this._cbxSituacion.DataContext = value; }
        }

        public object Situacion
        {
            get { return this._cbxSituacion.SelectedItem; }
            set { this._cbxSituacion.SelectedItem = value; }
        }

        public object Detalles
        {
            get { return this._cbxDetalleDatos.DataContext; }
            set { this._cbxDetalleDatos.DataContext = value; }
        }

        public object Detalle
        {
            get { return this._cbxDetalleDatos.SelectedItem; }
            set { this._cbxDetalleDatos.SelectedItem = value; }
        }

        public object MarcaFiltrada
        {
            get { return this._lstMarcas.SelectedItem; }
            set { this._lstMarcas.SelectedItem = value; }
        }

        //public object Condicion
        //{
        //    get { return this._cbxCondiciones.SelectedItem; }
        //    set { this._cbxCondiciones.SelectedItem = value; }
        //}

        //public object Condiciones
        //{
        //    get { return this._cbxCondiciones.DataContext; }
        //    set { this._cbxCondiciones.DataContext = value; }
        //}

        public object PaisSolicitud
        {
            get { return this._cbxPaisPrioridad.SelectedItem; }
            set { this._cbxPaisPrioridad.SelectedItem = value; }
        }
        
        public CheckBox Byt
        {
            get { return this._chkByt; }
            //set { this._chkByt = value; }
        }

        public string IdInternacionalByt
        {
            get { return this._txtClaseInternacionalByt.Text; }
            set { this._txtClaseInternacionalByt.Text = value; }
        }

        public string IdNacionalByt
        {
            get { return this._txtClaseNacionalByt.Text; }
            set { this._txtClaseNacionalByt.Text = value; }
        }

        public object EstadoMarcaSolicitud
        {
            get { return this._cbxEstadoMarca.SelectedItem; }
            set { this._cbxEstadoMarca.SelectedItem = value; }
        }

        public object TipoBaseSolicitud
        {
            get { return this._cbxTipoBase.SelectedItem; }
            set { this._cbxTipoBase.SelectedItem = value; }
        }

        public string IdMarcaFiltrar
        {
            get { return this._txtIdMarcaFiltrar.Text; }
        }

        public string NombreMarcaFiltrar
        {
            get { return this._txtNombreMarcaFiltrar.Text; }
        }

        public object Marca
        {
            get { return this._gridDatosMarca.DataContext; }
            set { this._gridDatosMarca.DataContext = value; }
        }

        public object GridByt
        {
            get { return this._gridByt.DataContext; }
            set { this._gridByt.DataContext = value; }
        }

        public object PaisesSolicitud
        {       
            get { return this._cbxPaisPrioridad.DataContext; }
            set { this._cbxPaisPrioridad.DataContext = value; }
        }

        public object EstadosMarcaSolicitud
        {
            get { return this._cbxEstadoMarca.DataContext; }
            set { this._cbxEstadoMarca.DataContext = value; }
        }

        public object TiposBaseSolicitud
        {
            get { return this._cbxTipoBase.DataContext; }
            set { this._cbxTipoBase.DataContext = value; }
        }

        //public object TipoMarcasTerceroSolicitud
        //{
        //    get { return this._cbxTipoMarcaTerceroSolicitud.DataContext; }
        //    set { this._cbxTipoMarcaTerceroSolicitud.DataContext = value; }
        //}

        //public object TipoMarcaTerceroSolicitud
        //{
        //    get { return this._cbxTipoMarcaTerceroSolicitud.SelectedItem; }
        //    set { this._cbxTipoMarcaTerceroSolicitud.SelectedItem = value; }
        //}

        //public object TipoMarcasTerceroDatos
        //{
        //    get { return this._cbxTipoMarcaTerceroDatos.DataContext; }
        //    set { this._cbxTipoMarcaTerceroDatos.DataContext = value; }
        //}

        //public object TipoMarcaTerceroDatos
        //{
        //    get { return this._cbxTipoMarcaTerceroDatos.SelectedItem; }
        //    set { this._cbxTipoMarcaTerceroDatos.SelectedItem = value; }
        //}


        public bool MensajeAlerta(string mensaje)
        {
            bool retorno = false;

            if (MessageBoxResult.Yes == MessageBox.Show(mensaje,
                "Alerta", MessageBoxButton.YesNo, MessageBoxImage.Question))
                retorno = true;

            return retorno;
        }

        public bool EstaCargada
        {
            get { return this._cargada; }
            set { this._cargada = value; }
        }

        public void FocoPredeterminado()
        {
            this._txtDescripcionSolicitud.Focus();
        }

        public string TextoBotonModificar
        {
            get { return this._txbAceptar.Text; }
            set { this._txbAceptar.Text = value; }
        }

        public string IdInternacional
        {
            get { return this._txtClaseInternacional.Text; }
            set { this._txtClaseInternacional.Text = value; }
        }

        public string ComentarioClienteEspanol
        {
            get { return this._txtComencliEsp.Text; }
            set { this._txtComencliEsp.Text = value; }
        }

        public string ComentarioClienteIngles
        {
            get { return this._txtComencliIng.Text; }
            set { this._txtComencliIng.Text = value; }
        }

        //public string IdNacional
        //{
        //    get { return this._txtClaseNacionalDatos.Text; }
        //    set { this._txtClaseNacionalDatos.Text = value; }
        //}

        public bool HabilitarCampos
        {
            set
            {
                #region TextBoxs

                //this._txtAsociadoDatos.IsEnabled = value;
                this._txtAsociadoSolicitud.IsEnabled = value;
                //this._txtBusqueda.IsEnabled = value;
                this._txtClaseInternacional.IsEnabled = value;
                //this._txtClaseInternacionalDatos.IsEnabled = value;
                this._txtClaseNacional.IsEnabled = value;
                //this._txtClaseNacionalDatos.IsEnabled = value;
                this._txtCod.IsEnabled = value;
                this._txtNombreMarca.IsEnabled = value;
                this._txtNombreMarcaFiltrar.IsEnabled = value;
                this._txtIdMarcaFiltrar.IsEnabled = value;
                this._btnConsultarMarca.IsEnabled = value;
                this._txtCodigoInscripcion.IsEnabled = value;
                this._txtCodigoInscripcionSolicitud.IsEnabled = value;
               // this._txtCodigoPrioridad.IsEnabled = value;
                this._txtCodigoRegistro.IsEnabled = value;
                this._txtComencliEsp.IsEnabled = value;
                this._txtComencliIng.IsEnabled = value;
                this._txtClaseInternacionalByt.IsEnabled = value;
                this._txtClaseNacionalByt.IsEnabled = value;
               // this._txtAnexo.IsEnabled = value; m
                //this._txtCodIntlDatos.IsEnabled = value;
                //this._txtComentarioDatos.IsEnabled = value;
                //this._txtConflictoDatos.IsEnabled = value;
                //this._txtCorresponsalDatos.IsEnabled = value;
                //this._txtCorresponsalSolicitud.IsEnabled = value;
                //this._txtCorresponsalDatos.IsEnabled = value;
                //this._txtDescripcionDatos.IsEnabled = value;
                this._txtDescripcionSolicitud.IsEnabled = value;
                this._txtDistingue.IsEnabled = value;
                //this._txtDistingueDatos.IsEnabled = value;
                //this._txtDistingueInglesDatos.IsEnabled = value;
                this._txtEtiqueta.IsEnabled = value;
                //this._txtEtiquetaDatos.IsEnabled = value;
                //this._txtExptyr.IsEnabled = value;
                this._txtFechaInscripcion.IsEnabled = value;
                this._txtFechaRegistro.IsEnabled = value;
                this._txtFechaRenovacion.IsEnabled = value;
                //this._txtIdAsociadoDatos.IsEnabled = value;
                this._txtIdAsociadoSolicitud.IsEnabled = value;
                //this._txtIdDatos.IsEnabled = value;
                //this._txtIdInteresadoDatos.IsEnabled = value;
                this._txtIdInteresadoSolicitud.IsEnabled = value;
                //this._txtIdSolicitud.IsEnabled = value;
                //this._txtInteresadoDatos.IsEnabled = value;
                this._txtInteresadoSolicitud.IsEnabled = value;
                this._txtLocalidad.IsEnabled = value;
                //this._txtLocalidadDatos.IsEnabled = value;
                //this._txtNombreAsociadoDatos.IsEnabled = value;
                this._txtNombreAsociadoSolicitud.IsEnabled = value;
                //this._txtNombreInteresadoDatos.IsEnabled = value;
                this._txtNombreInteresadoSolicitud.IsEnabled = value;
                this._txtNum.IsEnabled = value;
                //this._txtNumIntlDatos.IsEnabled = value;
                this._txtNumSapi.IsEnabled = value;
                this._txtOtrosImp.IsEnabled = value;
                this._txtPoderSolicitud.IsEnabled = value;
                //this._txtPoderDatos.IsEnabled = value;
                //this._txtPrimeraReferenciaDatos.IsEnabled = value;
                //this._txtReclasificacionDatos.IsEnabled = value;
                this._txtReferencia.IsEnabled = value;
                this._txtReferenciaAsocInt.IsEnabled = value;
                //this._txtReferenciaDatos.IsEnabled = value;
                //this._txtSaldoPorVencer.IsEnabled = value;
                //this._txtSaldoVencido.IsEnabled = value;
                this._txtTipoClaseNacional.IsEnabled = value;
                //this._txtTotalDeuda.IsEnabled = value;

                #endregion

                #region ComboBoxs

                this._cbxAgente.IsEnabled = value;
                //this._cbxAsociadoInteresadoDatos.IsEnabled = value;
                this._cbxAsocInt.IsEnabled = value;
                this._cbxBoletinConcesion.IsEnabled = value;
                this._cbxBoletinPublicacion.IsEnabled = value;
                this._cbxTipo.IsEnabled = value;
                //this._cbxCondiciones.IsEnabled = value;
                this._cbxConflicto.IsEnabled = value;
                this._cbxDetalleDatos.IsEnabled = value;
                //this._cbxEstadoDatos.IsEnabled = value;
                //this._cbxIdiomaDatos.IsEnabled = value;
                //this._cbxMarcaTerceroOrigen.IsEnabled = value;
                this._cbxMarcaTerceroOrigenSolicitud.IsEnabled = value;
                this._cbxOrdenPublicacion.IsEnabled = value;
                this._cbxPais.IsEnabled = value;
                //this._cbxPaisDatos.IsEnabled = value;
                this._cbxPaisPrioridad.IsEnabled = value;
                //this._cbxSector.IsEnabled = value;
                this._cbxSituacion.IsEnabled = value;
                this._cbxTipoBase.IsEnabled = value;
                this._cbxEstadoMarca.IsEnabled = value;
                //this._cbxTipoMarcaTerceroSolicitud.IsEnabled = value;
                //this._cbxTipoMarcaTerceroDatos.IsEnabled = value;
                //this._cbxTipoReproduccion.IsEnabled = value;
                //this._chkFacturacionDatos.IsEnabled = value;
                //this._chkDescuentoDatos.IsEnabled = value;
                //this._chkCorrespondenciaDatos.IsEnabled = value;

                this._cbxBoletinConcesion.IsEnabled = value;
                this._cbxBoletinPublicacion.IsEnabled = value;
                this._cbxOrdenPublicacion.IsEnabled = value;

                #endregion

                #region CheckBox

                this._chkByt.IsEnabled = value;
                //this._checkBoxInstruccionesRenovacion.IsEnabled = value;
                //this._checkBoxRenovacionTramitente.IsEnabled = value;
                //this._chkConflicto.IsEnabled = value;
                this._chkEtiquetaSolicitud.IsEnabled = value;
                //this._chkEtiquetaDatos.IsEnabled = value;
                //this._chkOtraInf.IsEnabled = value;
                //this._chkPoder.IsEnabled = value;
                //this._chkPoderYPrioridad.IsEnabled = value;
                //this._chkPrioridad.IsEnabled = value;
                //this._chkReclasificacionNacional.IsEnabled = value;
                //this._chkOtroDatos.IsEnabled = value;

                #endregion

                #region Botones

                this._btnAceptar.IsEnabled = value;
                //this._btnAnaqua.IsEnabled = value;
                //this._btnAnexoFM02.IsEnabled = value;
                //this._btnAuditoria.IsEnabled = value;
                this._btnBusquedaDatos.IsEnabled = value;
                //this._btnBusquedaSolicitud.IsEnabled = value;
                this._btnCancelar.IsEnabled = value;
                //this._btnCarpeta.IsEnabled = value;
                this._btnCertificados.IsEnabled = value;
                this._btnClaseCompleta.IsEnabled = value;
                //this._btnConflicto.IsEnabled = value;
                //this._btnConflictoELI.IsEnabled = value;
                //this._btnConflictoINC.IsEnabled = value;
                //this._btnConsultarAsociadoDatos.IsEnabled = value;
                this._btnConsultarAsociadoSolicitud.IsEnabled = value;
                //this._btnConsultarInteresadoDatos.IsEnabled = value;
                this._btnConsultarInteresadoSolicitud.IsEnabled = value;
                //this._btnDuplicar.IsEnabled = value;
                //this._btnEnviarRecordatorios.IsEnabled = value;
                //this._btnFacturacionDatos.IsEnabled = value;
                //this._btnFM02.IsEnabled = value;
                //this._btnFM02Venen.IsEnabled = value;
                //this._btnGenCartel.IsEnabled = value;
            //    this._btnImprimirEdoCuenta.IsEnabled = value;
                this._btnInfoAdicional.IsEnabled = value;
                //this._btnInfoAdicionalSolicitud.IsEnabled = value;
                this._btnInfobol.IsEnabled = value;
                this._btnIngles.IsEnabled = value;
                //this._btnIntRenovacion.IsEnabled = value;
                this._btnIrReclasificar.IsEnabled = value;
                //this._btnLAnexoFM02.IsEnabled = value;
                //this._btnLFM02.IsEnabled = value;
                //this._btnLFM02Venen.IsEnabled = value;
                //this._btnLista.IsEnabled = value;
                this._btnNoRegistro.IsEnabled = value;
                this._btnNoSolicitud.IsEnabled = value;
                //this._btnOperacionesDatos.IsEnabled = value;
                //this._btnOtraInf.IsEnabled = value;
                //this._btnOtraInfELI.IsEnabled = value;
                //this._btnOtraInfINC.IsEnabled = value;
                //this._btnPoder.IsEnabled = value;
                //this._btnPoderELI.IsEnabled = value;
                //this._btnPoderINC.IsEnabled = value;
                //this._btnPoderYPrioridad.IsEnabled = value;
                //this._btnPoderYPrioridadELI.IsEnabled = value;
                //this._btnPoderYPrioridadINC.IsEnabled = value;
                //this._btnPrioridad.IsEnabled = value;
                //this._btnPrioridadELI.IsEnabled = value;
                //this._btnPrioridadINC.IsEnabled = value;
                //this._btnReclasificacionNacional.IsEnabled = value;
                //this._btnReclasificacionNacionalELI.IsEnabled = value;
                //this._btnReclasificacionNacionalINC.IsEnabled = value;
                //this._btnRenovacion.IsEnabled = value;
                //this._btnRevisarWeb.IsEnabled = value;
                //this._btnSaldo.IsEnabled = value;
                //this._btnVerDocDatos.IsEnabled = value;
                this._lstMarcasB.IsEnabled = value;
                this._btnMas.IsEnabled = value;
                this._btnMenos.IsEnabled = value;
                #endregion


                #region DatePicker

                this._dpkFecha.IsEnabled = value;
               // this._dpkFechaPrioridad.IsEnabled = value;
                //this._dpkFechaRequeridaConflicto.IsEnabled = value;
                //this._dpkFechaRequeridaOtraInf.IsEnabled = value;
                //this._dpkFechaRequeridaPoder.IsEnabled = value;
                //this._dpkFechaRequeridaPoderYPrioridad.IsEnabled = value;
                //this._dpkFechaRequeridaPrioridad.IsEnabled = value;
                //this._dpkFechaRequeridaReclasificacionNacional.IsEnabled = value;

                #endregion
            }
        }

        public string NombreMarca
        {
            set { this._txtNombreMarca.Text = value; }
            get { return this._txtNombreMarca.Text; }
        }

        public string Anexo
        {
            set { this._txtAnexo.Text = value; }
            get { return this._txtAnexo.Text; }
        }

   

        public string IdAsociadoSolicitudFiltrar
        {
            get { return this._txtIdAsociadoSolicitud.Text; }
        }

        //public string IdAsociadoDatosFiltrar
        //{
        //    get { return this._txtIdAsociadoDatos.Text; }
        //}

        public string NombreAsociadoSolicitudFiltrar
        {
            get { return this._txtNombreAsociadoSolicitud.Text; }
        }

        //public string NombreAsociadoDatosFiltrar
        //{
        //    get { return this._txtNombreAsociadoDatos.Text; }
        //}

        public string NombreAsociadoSolicitud
        {
            get { return this._txtAsociadoSolicitud.Text; }
            set { this._txtAsociadoSolicitud.Text = value; }
        }

        //public string NombreAsociadoDatos
        //{
        //    get { return this._txtAsociadoDatos.Text; }
        //    set { this._txtAsociadoDatos.Text = value; }
        //}

        public object AsociadosSolicitud
        {
            get { return this._lstAsociadosSolicitud.DataContext; }
            set { this._lstAsociadosSolicitud.DataContext = value; }
        }

        public object AsociadoSolicitud
        {
            get { return this._lstAsociadosSolicitud.SelectedItem; }
            set
            {
                this._lstAsociadosSolicitud.SelectedItem = value;
                //this._lstAsociadosSolicitud.ScrollIntoView(value);
            }
        }

        //public object AsociadosDatos
        //{
        //    get { return this._lstAsociadosDatos.DataContext; }
        //    set { this._lstAsociadosDatos.DataContext = value; }
        //}

        //public object AsociadoDatos
        //{
        //    get { return this._lstAsociadosDatos.SelectedItem; }
        //    set
        //    {
        //        this._lstAsociadosDatos.SelectedItem = value;
        //        //this._lstAsociadosDatos.ScrollIntoView(value);
        //    }
        //}

        public string IdInteresadoSolicitudFiltrar
        {
            get { return this._txtIdInteresadoSolicitud.Text; }
        }

        //public string IdInteresadoDatosFiltrar
        //{
        //    get { return this._txtIdInteresadoDatos.Text; }
        //}

        public string InteresadoPaisSolicitud
        {
            get { return this._txtPaisSolicitud.Text; }
            set { this._txtPaisSolicitud.Text = value; }
        }

        public string InteresadoCiudadSolicitud
        {
            get { return this._txtCiudadSolicitud.Text; }
            set { this._txtCiudadSolicitud.Text = value; }
        }

        public string NombreInteresadoSolicitudFiltrar
        {
            get { return this._txtNombreInteresadoSolicitud.Text; }
        }

        public string NombreInteresadoDatosFiltrar
        {
            get { return this._txtNombreInteresadoSolicitud.Text; }
        }

        public string NombreInteresadoSolicitud
        {
            get { return this._txtInteresadoSolicitud.Text; }
            set { this._txtInteresadoSolicitud.Text = value; }
        }

        //public string NombreInteresadoDatos
        //{
        //    get { return this._txtInteresadoDatos.Text; }
        //    set { this._txtInteresadoDatos.Text = value; }
        //}

        public object InteresadosSolicitud
        {
            get { return this._lstInteresadosSolicitud.DataContext; }
            set { this._lstInteresadosSolicitud.DataContext = value; }
        }

        public object InteresadoSolicitud
        {
            get { return this._lstInteresadosSolicitud.SelectedItem; }
            set
            {
                this._lstInteresadosSolicitud.SelectedItem = value;
                //this._lstInteresadosSolicitud.ScrollIntoView(value);
            }
        }

        //public object InteresadosDatos
        //{
        //    get { return this._lstInteresadosDatos.DataContext; }
        //    set { this._lstInteresadosDatos.DataContext = value; }
        //}

        //public object InteresadoDatos
        //{
        //    get { return this._lstInteresadosDatos.SelectedItem; }
        //    set
        //    {
        //        this._lstInteresadosDatos.SelectedItem = value;
        //        //this._lstInteresadosDatos.ScrollIntoView(value);
        //    }
        //}

        //public string IdCorresponsalSolicitudFiltrar
        //{
        //    get { return this._txtIdCorresponsalSolicitud.Text; }
        //}

        //public string IdCorresponsalDatosFiltrar
        //{
        //    get { return this._txtIdCorresponsalDatos.Text; }
        //}

        //public string DescripcionCorresponsalSolicitudFiltrar
        //{
        //    get { return this._txtDescripcionCorresponsalSolicitud.Text; }
        //}

        //public string DescripcionCorresponsalDatosFiltrar
        //{
        //    get { return this._txtDescripcionCorresponsalDatos.Text; }
        //}

        //public string DescripcionCorresponsalSolicitud
        //{
        //    get { return this._txtCorresponsalSolicitud.Text; }
        //    set { this._txtCorresponsalSolicitud.Text = value; }
        //}

        //public string DescripcionCorresponsalDatos
        //{
        //    get { return this._txtCorresponsalDatos.Text; }
        //    set { this._txtCorresponsalDatos.Text = value; }
        //}

        //public object CorresponsalesSolicitud
        //{
        //    get { return this._lstCorresponsalesSolicitud.DataContext; }
        //    set { this._lstCorresponsalesSolicitud.DataContext = value; }
        //}

        //public object CorresponsalSolicitud
        //{
        //    get { return this._lstCorresponsalesSolicitud.SelectedItem; }
        //    set { this._lstCorresponsalesSolicitud.SelectedItem = value; }
        //}

        //public object CorresponsalesDatos
        //{
        //    get { return this._lstCorresponsalesDatos.DataContext; }
        //    set { this._lstCorresponsalesDatos.DataContext = value; }
        //}

        //public object CorresponsalDatos
        //{
        //    get { return this._lstCorresponsalesDatos.SelectedItem; }
        //    set { this._lstCorresponsalesDatos.SelectedItem = value; }
        //}

        public bool AsociadosEstanCargados
        {
            get { return this._asociadosCargados; }
            set { this._asociadosCargados = value; }
        }


        public bool InteresadosEstanCargados
        {
            get { return this._interesadosCargados; }
            set { this._interesadosCargados = value; }
        }

        public bool CorresponsalesEstanCargados
        {
            get { return this._corresponsalesCargados; }
            set { this._corresponsalesCargados = value; }
        }

        public bool PoderesEstanCargados
        {
            get { return this._poderesCargados; }
            set { this._poderesCargados = value; }
        }

        //public object Sector
        //{
        //    get { return this._cbxSector.SelectedItem; }
        //    set { this._cbxSector.SelectedItem = value; }
        //}

        //public object Sectores
        //{
        //    get { return this._cbxSector.DataContext; }
        //    set { this._cbxSector.DataContext = value; }
        //}

        //public object StatusWeb
        //{
        //    get { return this._cbxEstadoDatos.SelectedItem; }
        //    set { this._cbxEstadoDatos.SelectedItem = value; }
        //}

        //public object StatusWebs
        //{
        //    get { return this._cbxEstadoDatos.DataContext; }
        //    set { this._cbxEstadoDatos.DataContext = value; }
        //}

        //public object TipoReproduccion
        //{
        //    get { return this._cbxTipoReproduccion.SelectedItem; }
        //    set { this._cbxTipoReproduccion.SelectedItem = value; }
        //}

        //public object TipoReproducciones
        //{
        //    get { return this._cbxTipoReproduccion.DataContext; }
        //    set { this._cbxTipoReproduccion.DataContext = value; }
        //}

        //public void PintarInfoAdicional()
        //{
        //    this._btnInfoAdicional.Background = Brushes.LightGreen;
        //    this._btnInfoAdicionalSolicitud.Background = Brushes.LightGreen;
        //}

        //public void PintarAnaqua()
        //{
        //    this._btnAnaqua.Background = Brushes.LightGreen;
        //}

        public void PintarInfoBoles()
        {
            this._btnInfobol.Background = Brushes.LightGreen;
        }

        //public void PintarOperaciones()
        //{
        //    this._btnOperacionesDatos.Background = Brushes.LightGreen;
        //}

        public void PintarBusquedas()
        {
            this._btnBusquedaDatos.Background = Brushes.LightGreen;
            //this._btnBusquedaSolicitud.Background = Brushes.LightGreen;
        }

        //public void PintarAuditoria()
        //{
        //    this._btnAuditoria.Background = Brushes.LightGreen;
        //}

        public void BorrarCeros()
        {
            this._txtClaseInternacional.Text = this._txtClaseInternacional.Text.Equals("0") ? "" : this._txtClaseInternacional.Text;
            this._txtClaseInternacional.Text = this._txtClaseInternacional.Text.Equals("0") ? "" : this._txtClaseInternacional.Text;
            this._txtClaseInternacionalByt.Text = this._txtClaseInternacionalByt.Text.Equals("0") ? "" : this._txtClaseInternacionalByt.Text;
            this._txtClaseNacionalByt.Text = this._txtClaseNacionalByt.Text.Equals("0") ? "" : this._txtClaseNacionalByt.Text;
            this._txtClaseNacional.Text = this._txtClaseNacional.Text.Equals("0") ? "" : this._txtClaseNacional.Text;
            //this._txtClaseNacionalDatos.Text = this._txtClaseNacionalDatos.Text.Equals("0") ? "" : this._txtClaseNacionalDatos.Text;
        }

        public string ClaseInternacional
        {
            get { return this._txtClaseInternacional.Text; }
        }

        public string ClaseNacional
        {
            get { return this._txtClaseNacional.Text; }
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

        #endregion


        public ConsultarMarcaTercero(object marcaTerceroSeleccionada)
        {
            InitializeComponent();

            this._cargada = false;
            this._asociadosCargados = false;
            this._interesadosCargados = false;
            this._corresponsalesCargados = false;
            this._poderesCargados = false;
            this._byt = false;
            this._presentador = new PresentadorConsultarMarcaTercero(this, marcaTerceroSeleccionada);
        }

        public ConsultarMarcaTercero(object marcaTerceroSeleccionada, string tab)
            : this(marcaTerceroSeleccionada)
        {
            this._presentador.CambiarAModificar();

            //foreach (TabItem item in this._tbcPestañas.Items)
            //{
            //    if (item.Header.Equals(tab))
            //        item.IsSelected = true;
            //}
        }

        #region Funciones

        private void _btnConsultar(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name.Equals("_btnConsultarMarca"))
                this._presentador.ConsultarMarcas();
        }      

        private void mostrarLstAsociadoSolicitud()
        {
            this._lstAsociadosSolicitud.ScrollIntoView(this.AsociadoSolicitud);
            this._txtAsociadoSolicitud.Visibility = System.Windows.Visibility.Collapsed;
            this._lstAsociadosSolicitud.Visibility = System.Windows.Visibility.Visible;
            this._lstAsociadosSolicitud.IsEnabled = true;
            this._btnConsultarAsociadoSolicitud.Visibility = System.Windows.Visibility.Visible;
            this._txtIdAsociadoSolicitud.Visibility = System.Windows.Visibility.Visible;
            this._txtNombreAsociadoSolicitud.Visibility = System.Windows.Visibility.Visible;
            this._lblIdAsociadoSolicitud.Visibility = System.Windows.Visibility.Visible;
            this._lblNombreAsociadoSolicitud.Visibility = System.Windows.Visibility.Visible;
        }

        private void ocultarLstAsociadoSolicitud()
        {
            this._lstAsociadosSolicitud.Visibility = System.Windows.Visibility.Collapsed;
            this._btnConsultarAsociadoSolicitud.Visibility = System.Windows.Visibility.Collapsed;
            this._txtIdAsociadoSolicitud.Visibility = System.Windows.Visibility.Collapsed;
            this._txtNombreAsociadoSolicitud.Visibility = System.Windows.Visibility.Collapsed;
            this._txtAsociadoSolicitud.Visibility = System.Windows.Visibility.Visible;
            this._lblIdAsociadoSolicitud.Visibility = System.Windows.Visibility.Collapsed;
            this._lblNombreAsociadoSolicitud.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void mostrarLstInteresadoSolicutud()
        {
            this._lstInteresadosSolicitud.ScrollIntoView(this.InteresadoSolicitud);
            this._txtInteresadoSolicitud.Visibility = System.Windows.Visibility.Collapsed;
            this._lstInteresadosSolicitud.Visibility = System.Windows.Visibility.Visible;
            this._lstInteresadosSolicitud.IsEnabled = true;
            this._btnConsultarInteresadoSolicitud.Visibility = System.Windows.Visibility.Visible;
            this._txtIdInteresadoSolicitud.Visibility = System.Windows.Visibility.Visible;
            this._txtNombreInteresadoSolicitud.Visibility = System.Windows.Visibility.Visible;
            this._lblIdInteresadoSolicitud.Visibility = System.Windows.Visibility.Visible;
            this._lblNombreInteresadoSolicitud.Visibility = System.Windows.Visibility.Visible;
        }

        private void ocultarLstInteresadoSolicutud()
        {
            this._presentador.CambiarInteresadoSolicitud();
            this._lstInteresadosSolicitud.Visibility = System.Windows.Visibility.Collapsed;
            this._btnConsultarInteresadoSolicitud.Visibility = System.Windows.Visibility.Collapsed;
            this._txtIdInteresadoSolicitud.Visibility = System.Windows.Visibility.Collapsed;
            this._txtNombreInteresadoSolicitud.Visibility = System.Windows.Visibility.Collapsed;
            this._txtInteresadoSolicitud.Visibility = System.Windows.Visibility.Visible;
            this._lblIdInteresadoSolicitud.Visibility = System.Windows.Visibility.Collapsed;
            this._lblNombreInteresadoSolicitud.Visibility = System.Windows.Visibility.Collapsed;
        }

        //private void mostrarLstCorresponsalSolicutud()
        //{
        //    this._lstCorresponsalesSolicitud.ScrollIntoView(this.CorresponsalSolicitud);
        //    this._txtCorresponsalSolicitud.Visibility = System.Windows.Visibility.Collapsed;
        //    this._lstCorresponsalesSolicitud.Visibility = System.Windows.Visibility.Visible;
        //    this._lstCorresponsalesSolicitud.IsEnabled = true;
        //    this._btnConsultarCorresponsalSolicitud.Visibility = System.Windows.Visibility.Visible;
        //    this._txtIdCorresponsalSolicitud.Visibility = System.Windows.Visibility.Visible;
        //    this._txtDescripcionCorresponsalSolicitud.Visibility = System.Windows.Visibility.Visible;
        //    this._lblIdCorresponsalSolicitud.Visibility = System.Windows.Visibility.Visible;
        //    this._lblDescripcionCorresponsalSolicitud.Visibility = System.Windows.Visibility.Visible;
        //}

        //private void ocultarLstCorresponsalSolicutud()
        //{
        //    this._presentador.CambiarCorresponsalSolicitud();
        //    this._lstCorresponsalesSolicitud.Visibility = System.Windows.Visibility.Collapsed;
        //    this._btnConsultarCorresponsalSolicitud.Visibility = System.Windows.Visibility.Collapsed;
        //    this._txtIdCorresponsalSolicitud.Visibility = System.Windows.Visibility.Collapsed;
        //    this._txtDescripcionCorresponsalSolicitud.Visibility = System.Windows.Visibility.Collapsed;
        //    this._txtCorresponsalSolicitud.Visibility = System.Windows.Visibility.Visible;
        //    this._lblIdCorresponsalSolicitud.Visibility = System.Windows.Visibility.Collapsed;
        //    this._lblDescripcionCorresponsalSolicitud.Visibility = System.Windows.Visibility.Collapsed;
        //}

        //private void mostrarLstAsocaidoDatos()
        //{
        //    this._lstAsociadosDatos.ScrollIntoView(this.AsociadoDatos);
        //    this._txtAsociadoDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._lstAsociadosDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._lstAsociadosDatos.IsEnabled = true;
        //    this._btnConsultarAsociadoDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._txtIdAsociadoDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._txtNombreAsociadoDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._lblIdAsociadoDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._lblNombreAsociadoDatos.Visibility = System.Windows.Visibility.Visible;
        //}

        //private void ocultarLstAsociadoDatos()
        //{
        //    this._lstAsociadosDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._btnConsultarAsociadoDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._txtIdAsociadoDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._txtNombreAsociadoDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._txtAsociadoDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._lblIdAsociadoDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._lblNombreAsociadoDatos.Visibility = System.Windows.Visibility.Collapsed;
        //}

        //private void mostrarLstInteresadoDatos()
        //{
        //    this._lstInteresadosDatos.ScrollIntoView(this.InteresadoDatos);
        //    this._txtInteresadoDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._lstInteresadosDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._lstInteresadosDatos.IsEnabled = true;
        //    this._btnConsultarInteresadoDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._txtIdInteresadoDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._txtNombreInteresadoDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._lblIdInteresadoDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._lblNombreInteresadoDatos.Visibility = System.Windows.Visibility.Visible;
        //}

        //private void ocultarLstInteresadoDatos()
        //{
        //    this._lstInteresadosDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._btnConsultarInteresadoDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._txtIdInteresadoDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._txtNombreInteresadoDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._txtInteresadoDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._lblIdInteresadoDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._lblNombreInteresadoDatos.Visibility = System.Windows.Visibility.Collapsed;
        //}

        //private void mostrarLstCorresponsalDatos()
        //{
        //    this._lstCorresponsalesDatos.ScrollIntoView(this.CorresponsalDatos);
        //    this._txtCorresponsalDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._lstCorresponsalesDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._lstCorresponsalesDatos.IsEnabled = true;
        //    this._btnConsultarCorresponsalDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._txtIdCorresponsalDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._txtDescripcionCorresponsalDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._lblIdCorresponsalDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._lblDescripcionCorresponsalDatos.Visibility = System.Windows.Visibility.Visible;
        //}

        //private void ocultarLstCorresponsalDatos()
        //{
        //    this._presentador.CambiarCorresponsalDatos();
        //    this._lstCorresponsalesDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._btnConsultarCorresponsalDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._txtIdCorresponsalDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._txtDescripcionCorresponsalDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._txtCorresponsalDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._lblIdCorresponsalDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._lblDescripcionCorresponsalDatos.Visibility = System.Windows.Visibility.Collapsed;
        //}

        #endregion

        #region Eventos generales

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!EstaCargada)
            {
                this._presentador.CargarPagina();
                if (this._presentador.CargarMarcasByt())
                    this._lstMarcasB.Visibility = System.Windows.Visibility.Visible;
                EstaCargada = true;
            }
        }

        private void _btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.Modificar();
        }

        private void _btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (_agregar)
                this._presentador.Cancelar();
            this._presentador.IrConsultarMarcasTercero();
        }

        private void _btnInfoAdicional_Click(object sender, RoutedEventArgs e)
        {
            string parametro = "";
            if (((Button)sender).Name.Equals("_btnInfoAdicionalSolicitud"))
                parametro = Recursos.Etiquetas.tabSolicitud;
            else if (((Button)sender).Name.Equals("_btnInfoAdicional"))
                parametro = Recursos.Etiquetas.tabDatos;

            this._presentador.IrInfoAdicional(parametro);
        }

        private void _btnAnaqua_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.IrAnaqua();
        }

        private void _btnBusqueda_Click(object sender, RoutedEventArgs e)
        {
            string parametro = "";
            if (((Button)sender).Name.Equals("_btnBusquedaSolicitud"))
                parametro = Recursos.Etiquetas.tabSolicitud;
            else if (((Button)sender).Name.Equals("_btnBusquedaDatos"))
                parametro = Recursos.Etiquetas.tabDatos;

            this._presentador.IrBusquedas(parametro);
        }

        private void _btnAuditoria_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.Auditoria();
        }
        #endregion

        #region Eventos Solicitudes

        private void _txtAsociadoSolicitud_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!this._asociadosCargados)
            {
                this._presentador.CargarAsociados();
            }

            //ocultarLstCorresponsalSolicutud();
            ocultarLstInteresadoSolicutud();
            ocultarLstPoderSolicutud();

            this._btnAceptar.IsDefault = false;
            this._btnConsultarAsociadoSolicitud.IsDefault = true;

            mostrarLstAsociadoSolicitud();

        }

        private void _lstAsociadosSolicitud_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this._presentador.CambiarAsociadoSolicitud();
            ocultarLstAsociadoSolicitud();
            //ocultarLstAsociadoDatos();
            this._btnConsultarAsociadoSolicitud.IsDefault = false;
            this._btnAceptar.IsDefault = true;
        }

        private void _OrdenarAsociadoSolicitud_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstAsociadosSolicitud);
        }

        private void _btnConsultarAsociadoSolicitud_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.BuscarAsociado(0);
        }

        private void _txtInteresadoSolicitud_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!this._interesadosCargados)
            {
                this._presentador.CargarInteresados();
            }

            //ocultarLstCorresponsalSolicutud();
            ocultarLstAsociadoSolicitud();
            ocultarLstPoderSolicutud();

            this._btnAceptar.IsDefault = false;
            this._btnConsultarInteresadoSolicitud.IsDefault = true;

            mostrarLstInteresadoSolicutud();
        }

        private void _btnConsultarInteresadoSolicitud_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.BuscarInteresado(0);
        }

        private void _lstInteresadosSolicitud_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this._presentador.CambiarInteresadoSolicitud();
            ocultarLstInteresadoSolicutud();
            //ocultarLstInteresadoDatos();

            this._btnConsultarInteresadoSolicitud.IsDefault = false;
            this._btnAceptar.IsDefault = true;
        }

        private void _OrdenarInteresadoSolicitud_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstInteresadosSolicitud);
        }

        //private void _btnConsultarCorresponsalSolicitud_Click(object sender, RoutedEventArgs e)
        //{
        //    this._presentador.BuscarCorresponsal(0);
        //}

        //private void _txtCorresponsalSolicitud_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    if (!this._corresponsalesCargados)
        //    {
        //        this._presentador.CargarCorresponsales();
        //    }

        //    ocultarLstInteresadoSolicutud();
        //    ocultarLstAsociadoSolicitud();
        //    ocultarLstPoderSolicutud();

        //    this._btnAceptar.IsDefault = false;
        //    this._btnConsultarCorresponsalSolicitud.IsDefault = true;

        //    mostrarLstCorresponsalSolicutud();
        //}

        //private void _lstCorresponsalesSolicitud_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    this._presentador.CambiarCorresponsalSolicitud();
        //    ocultarLstCorresponsalSolicutud();
        //    //ocultarLstCorresponsalDatos();

        //    this._btnConsultarCorresponsalSolicitud.IsDefault = false;
        //    this._btnAceptar.IsDefault = true;
        //}

        //private void _OrdenarCorresponsalSolicitud_Click(object sender, RoutedEventArgs e)
        //{
        //    this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstCorresponsalesSolicitud);
        //}

        private void _btnClaseCompletaSolicitud_Click(object sender, RoutedEventArgs e)
        {

        }

        private void _btnIrReclasificarSolicitud_Click(object sender, RoutedEventArgs e)
        {

        }

        private void _btnInglesSolicitud_Click(object sender, RoutedEventArgs e)
        {

        }

        private void _btnImprimirEdoCuentaSolicitud_Click(object sender, RoutedEventArgs e)
        {

        }

        private void _btnSaldoSolicitud_Click(object sender, RoutedEventArgs e)
        {

        }

        private void _btnPoderSolicitud_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mostrarLstPoderSolicitud()
        {
            this._txtPoderSolicitud.Visibility = System.Windows.Visibility.Collapsed;
            this._lstPoderesSolicitud.Visibility = System.Windows.Visibility.Visible;
            this._lstPoderesSolicitud.IsEnabled = true;
        }

        private void ocultarLstPoderSolicutud()
        {
            this._presentador.CambiarPoderSolicitud();
            this._lstPoderesSolicitud.Visibility = System.Windows.Visibility.Collapsed;
            this._txtPoderSolicitud.Visibility = System.Windows.Visibility.Visible;
        }

        private void _txtPoderSolicitud_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!this._poderesCargados)
                this._presentador.CargarPoderes();

            //ocultarLstCorresponsalSolicutud();
            ocultarLstAsociadoSolicitud();
            ocultarLstInteresadoSolicutud();

            mostrarLstPoderSolicitud();
        }

        private void _lstPoderesSolicitud_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //this._presentador.CambiarCorresponsalSolicitud();
            ocultarLstPoderSolicutud();
            //ocultarLstPoderDatos();
        }

        private void _OrdenarPoderSolicitud_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstPoderesSolicitud);
        }

        //private void _cbxTipoMarcaTerceroSolicitud_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    this._cbxTipoMarcaTerceroDatos.SelectedItem = ((ComboBox)sender).SelectedItem;
        //}

        public void Mensaje(string mensaje, int opcion)
        {
            if (opcion == 0)
                MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show(mensaje, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void _btnDuplicar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show(Recursos.MensajesConElUsuario.ConfirmacionDuplicarMarcaTercero,
                "Duplicar MarcaTercero", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                this._presentador.Duplicar();
            }
        }

        #endregion

        #region Eventos Datos

        //private void _txtAsociadoDatos_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    if (!this._asociadosCargados)
        //    {
        //        this._presentador.CargarAsociados();
        //    }

        //    ocultarLstPoderDatos();
        //    //ocultarLstCorresponsalDatos();
        //    //ocultarLstInteresadoDatos();

        //    this._btnAceptar.IsDefault = false;
        //    //this._btnConsultarAsociadoDatos.IsDefault = true;

        //    //mostrarLstAsocaidoDatos();
        //}

        //private void _lstAsociadosDatos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    this._presentador.CambiarAsociadoDatos();
        //    //ocultarLstAsociadoDatos();
        //    ocultarLstAsociadoSolicitud();

        //    //this._btnConsultarAsociadoDatos.IsDefault = false;
        //    this._btnAceptar.IsDefault = true;
        //}

        //private void _OrdenarAsociadoDatos_Click(object sender, RoutedEventArgs e)
        //{
        //    this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstAsociadosDatos);
        //}

        //private void _btnConsultarAsociadoDatos_Click(object sender, RoutedEventArgs e)
        //{
        //    this._presentador.BuscarAsociado(1);
        //}

        //private void _txtInteresadoDatos_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    if (!this._interesadosCargados)
        //    {
        //        this._presentador.CargarInteresados();
        //    }

        //    ocultarLstPoderDatos();
        //    //ocultarLstCorresponsalDatos();
        //    //ocultarLstAsociadoDatos();

        //    this._btnAceptar.IsDefault = false;
        //    //this._btnConsultarInteresadoDatos.IsDefault = true;

        //    //mostrarLstInteresadoDatos();
        //}

        //private void _btnConsultarInteresadoDatos_Click(object sender, RoutedEventArgs e)
        //{
        //    this._presentador.BuscarInteresado(1);
        //}

        //private void _lstInteresadosDatos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    this._presentador.CambiarInteresadoDatos();
        //    //ocultarLstInteresadoDatos();
        //    ocultarLstInteresadoSolicutud();

        //    //this._btnConsultarInteresadoDatos.IsDefault = false;
        //    this._btnAceptar.IsDefault = true;
        //}

        //private void _OrdenarInteresadoDatos_Click(object sender, RoutedEventArgs e)
        //{
        //    this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstInteresadosDatos);
        //}

        //private void _btnConsultarCorresponsalDatos_Click(object sender, RoutedEventArgs e)
        //{
        //    this._presentador.BuscarCorresponsal(1);
        //}

        //private void _txtCorresponsalDatos_GotFocus(object sender, RoutedEventArgs e)
        //{

        //    if (!this._corresponsalesCargados)
        //        this._presentador.CargarCorresponsales();

        //    ocultarLstPoderDatos();
        //    ocultarLstAsociadoDatos();
        //    ocultarLstInteresadoDatos();

        //    this._btnAceptar.IsDefault = false;
        //    this._btnConsultarCorresponsalDatos.IsDefault = true;

        //    mostrarLstCorresponsalDatos();
        //}

        //private void _lstCorresponsalesDatos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    this._presentador.CambiarCorresponsalDatos();
        //    ocultarLstCorresponsalSolicutud();
        //    ocultarLstCorresponsalDatos();

        //    this._btnConsultarCorresponsalDatos.IsDefault = false;
        //    this._btnAceptar.IsDefault = true;
        //}

        //private void _OrdenarCorresponsalDatos_Click(object sender, RoutedEventArgs e)
        //{
        //    this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstCorresponsalesDatos);
        //}

        //private void mostrarLstPoderDatos()
        //{
        //    this._txtPoderDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._lstPoderesDatos.Visibility = System.Windows.Visibility.Visible;
        //    this._lstPoderesDatos.IsEnabled = true;
        //}

        //private void ocultarLstPoderDatos()
        //{
        //    this._presentador.CambiarPoderDatos();
        //    this._lstPoderesDatos.Visibility = System.Windows.Visibility.Collapsed;
        //    this._txtPoderDatos.Visibility = System.Windows.Visibility.Visible;
        //}

        //private void _txtPoderDatos_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    if (!this._poderesCargados)
        //        this._presentador.CargarPoderes();

        //    ocultarLstAsociadoDatos();
        //    //ocultarLstCorresponsalDatos();
        //    ocultarLstInteresadoDatos();

        //    mostrarLstPoderDatos();
        //}

        //private void _lstPoderesDatos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    this._presentador.CambiarPoderDatos();
        //    ocultarLstPoderSolicutud();
        //    ocultarLstPoderDatos();
        //}

        //private void _OrdenarPoderDatos_Click(object sender, RoutedEventArgs e)
        //{
        //    this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstPoderesDatos);
        //}

        //private void _cbxTipoMarcaTerceroDatos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    this._cbxTipoMarcaTerceroSolicitud.SelectedItem = ((ComboBox)sender).SelectedItem;
        //}

        private void _btnInfobol_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.IrInfoBoles();
        }

        private void _btnOperacionesDatos_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.IrOperaciones();
        }

        private void _btnIrExplorador_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.IrSAPI();
        }

        #endregion

        #region BYT

        public void MostrarByt()
        {
            _agregar = true;
            this._gridByt.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ValidarByt()
        {
            this._chkByt.IsChecked = true;
            this._byt = true;
        }


        #endregion

        #region Marca

        private void _lstMarcas_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            if (this._presentador.CambiarMarca())
            {
                GestionarVisibilidadDatosDeMarca(Visibility.Visible);
                GestionarVisibilidadFiltroMarca(Visibility.Collapsed);
            }

        }

        private void _OrdenarMarcas_Click(object sender, RoutedEventArgs e)
        {
            this._presentador.OrdenarColumna(sender as GridViewColumnHeader, this._lstMarcas);
        }

        private void _txtNombreMarca_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((bool)this._chkByt.IsChecked)
            {
                GestionarVisibilidadDatosDeMarca(Visibility.Collapsed);

                GestionarVisibilidadFiltroMarca(Visibility.Visible);
                this._cbxPais.IsEnabled = false;
                this._txtClaseInternacionalByt.IsEnabled = false;
                this._txtClaseNacionalByt.IsEnabled = false;
            }
        }

        private void GestionarVisibilidadDatosDeMarca(object value)
        {
            this._txtNombreMarca.Visibility = (System.Windows.Visibility)value;
            //this._chkEtiqueta.Visibility = (System.Windows.Visibility)value;
         }

        private void GestionarVisibilidadFiltroMarca(object value)
        {
            this._lblNombreMarca.Visibility = (System.Windows.Visibility)value;
            this._txtNombreMarcaFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lblIdMarca.Visibility = (System.Windows.Visibility)value;
            this._txtIdMarcaFiltrar.Visibility = (System.Windows.Visibility)value;
            this._lstMarcas.Visibility = (System.Windows.Visibility)value;
            this._btnConsultarMarca.Visibility = (System.Windows.Visibility)value;
        }

        private void _txtMarcaFiltrar_GotFocus(object sender, RoutedEventArgs e)
        {
            this._btnConsultarMarca.IsDefault = true;
        }

        #endregion

        // Funcion Ir a Imprimir, DESCOMENTAR
        private void _impresion_Click(object sender, RoutedEventArgs e)
        {
        //    this._presentador.IrImprimir(((Button)sender).Name);
        }

        private void _chkByt_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)!this._chkByt.IsChecked)
            {
                GestionarVisibilidadDatosDeMarca(Visibility.Visible);

                GestionarVisibilidadFiltroMarca(Visibility.Collapsed);
                this._cbxPais.IsEnabled = true;
                this._txtClaseInternacionalByt.IsEnabled = true;
                this._txtClaseNacionalByt.IsEnabled = true;

            }
        }

        private void _btnMas_Click(object sender, RoutedEventArgs e)
        {

            if ((this._presentador.AgregarMarcaByt()) && (this._lstMarcasB.Visibility == System.Windows.Visibility.Collapsed))
                this._lstMarcasB.Visibility = System.Windows.Visibility.Visible;

        }

        private void _btnMenos_Click(object sender, RoutedEventArgs e)
        {
            if (this._presentador.DeshabilitarMarcasByt())
            {
                this._lstMarcasB.Visibility = System.Windows.Visibility.Collapsed;
            }

        }




        public void AgregarMarcaByt()
        {
            throw new NotImplementedException();
        }

        public void DeshabilitarMarcasByt()
        {
            throw new NotImplementedException();
        }


        public void CargarMarcasByt()
        {
            throw new NotImplementedException();
        }

        public void LimpiarMarcasByt()
        {
            throw new NotImplementedException();
        }


    }
}
