﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using NLog;
using Trascend.Bolet.Cliente.Ayuda;
using Trascend.Bolet.Cliente.Contratos.Traspasos.CambiosDeDomicilio;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.Cliente.Ventanas.Traspasos.CambiosDeDomicilio;
using Trascend.Bolet.Cliente.Ventanas.Marcas;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.Cliente.Ventanas.Auditorias;
using Trascend.Bolet.Cliente.Ventanas.Asociados;
using Trascend.Bolet.Cliente.Ventanas.CadenaDeCambio;

namespace Trascend.Bolet.Cliente.Presentadores.Traspasos.CambiosDeDomicilio
{
    class PresentadorGestionarCambioDeDomicilio : PresentadorBase
    {
        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private bool _agregar = true;
        private IGestionarCambioDeDomicilio _ventana;

        private IMarcaServicios _marcaServicios;
        private IAnaquaServicios _anaquaServicios;
        private IAsociadoServicios _asociadoServicios;
        private IAgenteServicios _agenteServicios;
        private IPoderServicios _poderServicios;
        private IBoletinServicios _boletinServicios;
        private IPaisServicios _paisServicios;
        private IListaDatosDominioServicios _listaDatosDominioServicios;
        private IListaDatosValoresServicios _listaDatosValoresServicios;
        private IInteresadoServicios _interesadoServicios;
        private IServicioServicios _servicioServicios;
        private ITipoEstadoServicios _tipoEstadoServicios;
        private ICorresponsalServicios _corresponsalServicios;
        private ICondicionServicios _condicionServicios;
        private IInfoAdicionalServicios _infoAdicionalServicios;
        private IInfoBolServicios _infoBolServicios;
        private IOperacionServicios _operacionServicios;
        private IBusquedaServicios _busquedaServicios;
        private IStatusWebServicios _statusWebServicios;
        private ICambioDeDomicilioServicios _cambioDeDomicilioServicios;
        private ICadenaDeCambiosServicios _cadenaDeCambiosServicios;
        private IInstruccionCorrespondenciaServicios _instruccionCorrespondenciaServicios;
        private IInstruccionEnvioOriginalesServicios _instruccionEnvioOriginalesServicios;
        private IInstruccionDescuentoServicios _instruccionDescuentoServicios;
        private IInstruccionOtrosServicios _instruccionOtrosServicios;


        private IList<Asociado> _asociados;
        private IList<Interesado> _interesados;
        private IList<Corresponsal> _corresponsales;
        private IList<Auditoria> _auditorias;
        private IList<Marca> _marcas;
        private IList<Interesado> _interesadosAnterior;
        private IList<Interesado> _interesadosActual;
        private IList<Agente> _agentesApoderados;
        private IList<Poder> _poderes;

        private IList<Poder> _poderesInterseccion;
        private IList<Poder> _poderesInteresadoActual;
        private IList<Poder> _poderesApoderado;

        /// <summary>
        /// Constructor Predeterminado
        /// </summary>
        /// <param name="ventana">página que satisface el contrato</param>
        public PresentadorGestionarCambioDeDomicilio(IGestionarCambioDeDomicilio ventana, object cambioDeDomicilio)
        {
            try
            {

                this._ventana = ventana;

                if (cambioDeDomicilio != null)
                {
                    this._ventana.CambioDeDomicilio = cambioDeDomicilio;
                    _agregar = false;
                    CambiarAModificar();
                }
                else
                {
                    CambioDeDomicilio cambioDeDomicilioAgregar = new CambioDeDomicilio();
                    this._ventana.CambioDeDomicilio = cambioDeDomicilioAgregar;

                    ((CambioDeDomicilio)this._ventana.CambioDeDomicilio).FechaDomicilio = DateTime.Now;
                    this._ventana.Marca = null;
                    this._ventana.Poder = null;
                    this._ventana.InteresadoAnterior = null;
                    this._ventana.InteresadoActual = null;
                    this._ventana.AgenteApoderado = null;

                    CambiarAModificar();

                    this._ventana.TextoBotonRegresar = Recursos.Etiquetas.btnCancelar;

                    this._ventana.ActivarControlesAlAgregar();

                }

                #region Servicios

                this._marcaServicios = (IMarcaServicios)Activator.GetObject(typeof(IMarcaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["MarcaServicios"]);
                this._asociadoServicios = (IAsociadoServicios)Activator.GetObject(typeof(IAsociadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsociadoServicios"]);
                this._agenteServicios = (IAgenteServicios)Activator.GetObject(typeof(IAgenteServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AgenteServicios"]);
                this._poderServicios = (IPoderServicios)Activator.GetObject(typeof(IPoderServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["PoderServicios"]);
                this._boletinServicios = (IBoletinServicios)Activator.GetObject(typeof(IBoletinServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["BoletinServicios"]);
                this._paisServicios = (IPaisServicios)Activator.GetObject(typeof(IPaisServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["PaisServicios"]);
                this._listaDatosDominioServicios = (IListaDatosDominioServicios)Activator.GetObject(typeof(IListaDatosDominioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosDominioServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);
                this._interesadoServicios = (IInteresadoServicios)Activator.GetObject(typeof(IInteresadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InteresadoServicios"]);
                this._servicioServicios = (IServicioServicios)Activator.GetObject(typeof(IServicioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ServicioServicios"]);
                this._tipoEstadoServicios = (ITipoEstadoServicios)Activator.GetObject(typeof(ITipoEstadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["TipoEstadoServicios"]);
                this._corresponsalServicios = (ICorresponsalServicios)Activator.GetObject(typeof(ICorresponsalServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CorresponsalServicios"]);
                this._condicionServicios = (ICondicionServicios)Activator.GetObject(typeof(ICondicionServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CondicionServicios"]);
                this._anaquaServicios = (IAnaquaServicios)Activator.GetObject(typeof(IAnaquaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AnaquaServicios"]);
                this._infoAdicionalServicios = (IInfoAdicionalServicios)Activator.GetObject(typeof(IInfoAdicionalServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InfoAdicionalServicios"]);
                this._infoBolServicios = (IInfoBolServicios)Activator.GetObject(typeof(IInfoBolServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InfoBolServicios"]);
                this._operacionServicios = (IOperacionServicios)Activator.GetObject(typeof(IOperacionServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["OperacionServicios"]);
                this._busquedaServicios = (IBusquedaServicios)Activator.GetObject(typeof(IBusquedaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["BusquedaServicios"]);
                this._statusWebServicios = (IStatusWebServicios)Activator.GetObject(typeof(IStatusWebServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["StatusWebServicios"]);
                this._cambioDeDomicilioServicios = (ICambioDeDomicilioServicios)Activator.GetObject(typeof(ICambioDeDomicilioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CambioDeDomicilioServicios"]);
                this._cadenaDeCambiosServicios = (ICadenaDeCambiosServicios)Activator.GetObject(typeof(ICadenaDeCambiosServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CadenaDeCambiosServicios"]);
                this._instruccionCorrespondenciaServicios = (IInstruccionCorrespondenciaServicios)Activator.GetObject(typeof(IInstruccionCorrespondenciaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InstruccionCorrespondenciaServicios"]);
                this._instruccionEnvioOriginalesServicios = (IInstruccionEnvioOriginalesServicios)Activator.GetObject(typeof(IInstruccionEnvioOriginalesServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InstruccionEnvioOriginalesServicios"]);
                this._instruccionDescuentoServicios = (IInstruccionDescuentoServicios)Activator.GetObject(typeof(IInstruccionDescuentoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InstruccionDescuentoServicios"]);
                this._instruccionOtrosServicios = (IInstruccionOtrosServicios)Activator.GetObject(typeof(IInstruccionOtrosServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InstruccionOtrosServicios"]);

                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }



        /// <summary>
        /// Constructor Predeterminado sobrecargado que contiene una ventana Padre
        /// </summary>
        /// <param name="ventana">página que satisface el contrato</param>
        public PresentadorGestionarCambioDeDomicilio(IGestionarCambioDeDomicilio ventana, object cambioDeDomicilio, object ventanaPadre)
        {
            try
            {

                this._ventana = ventana;

                this._ventanaPadre = ventanaPadre;

                if (cambioDeDomicilio != null)
                {
                    this._ventana.CambioDeDomicilio = cambioDeDomicilio;
                    _agregar = false;
                    CambiarAModificar();
                }
                else
                {
                    CambioDeDomicilio cambioDeDomicilioAgregar = new CambioDeDomicilio();
                    this._ventana.CambioDeDomicilio = cambioDeDomicilioAgregar;

                    ((CambioDeDomicilio)this._ventana.CambioDeDomicilio).FechaDomicilio = DateTime.Now;
                    this._ventana.Marca = null;
                    this._ventana.Poder = null;
                    this._ventana.InteresadoAnterior = null;
                    this._ventana.InteresadoActual = null;
                    this._ventana.AgenteApoderado = null;

                    CambiarAModificar();

                    this._ventana.TextoBotonRegresar = Recursos.Etiquetas.btnCancelar;

                    this._ventana.ActivarControlesAlAgregar();

                }

                #region Servicios

                this._marcaServicios = (IMarcaServicios)Activator.GetObject(typeof(IMarcaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["MarcaServicios"]);
                this._asociadoServicios = (IAsociadoServicios)Activator.GetObject(typeof(IAsociadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsociadoServicios"]);
                this._agenteServicios = (IAgenteServicios)Activator.GetObject(typeof(IAgenteServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AgenteServicios"]);
                this._poderServicios = (IPoderServicios)Activator.GetObject(typeof(IPoderServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["PoderServicios"]);
                this._boletinServicios = (IBoletinServicios)Activator.GetObject(typeof(IBoletinServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["BoletinServicios"]);
                this._paisServicios = (IPaisServicios)Activator.GetObject(typeof(IPaisServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["PaisServicios"]);
                this._listaDatosDominioServicios = (IListaDatosDominioServicios)Activator.GetObject(typeof(IListaDatosDominioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosDominioServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);
                this._interesadoServicios = (IInteresadoServicios)Activator.GetObject(typeof(IInteresadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InteresadoServicios"]);
                this._servicioServicios = (IServicioServicios)Activator.GetObject(typeof(IServicioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ServicioServicios"]);
                this._tipoEstadoServicios = (ITipoEstadoServicios)Activator.GetObject(typeof(ITipoEstadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["TipoEstadoServicios"]);
                this._corresponsalServicios = (ICorresponsalServicios)Activator.GetObject(typeof(ICorresponsalServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CorresponsalServicios"]);
                this._condicionServicios = (ICondicionServicios)Activator.GetObject(typeof(ICondicionServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CondicionServicios"]);
                this._anaquaServicios = (IAnaquaServicios)Activator.GetObject(typeof(IAnaquaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AnaquaServicios"]);
                this._infoAdicionalServicios = (IInfoAdicionalServicios)Activator.GetObject(typeof(IInfoAdicionalServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InfoAdicionalServicios"]);
                this._infoBolServicios = (IInfoBolServicios)Activator.GetObject(typeof(IInfoBolServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InfoBolServicios"]);
                this._operacionServicios = (IOperacionServicios)Activator.GetObject(typeof(IOperacionServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["OperacionServicios"]);
                this._busquedaServicios = (IBusquedaServicios)Activator.GetObject(typeof(IBusquedaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["BusquedaServicios"]);
                this._statusWebServicios = (IStatusWebServicios)Activator.GetObject(typeof(IStatusWebServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["StatusWebServicios"]);
                this._cambioDeDomicilioServicios = (ICambioDeDomicilioServicios)Activator.GetObject(typeof(ICambioDeDomicilioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CambioDeDomicilioServicios"]);
                this._cadenaDeCambiosServicios = (ICadenaDeCambiosServicios)Activator.GetObject(typeof(ICadenaDeCambiosServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CadenaDeCambiosServicios"]);
                this._instruccionCorrespondenciaServicios = (IInstruccionCorrespondenciaServicios)Activator.GetObject(typeof(IInstruccionCorrespondenciaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InstruccionCorrespondenciaServicios"]);
                this._instruccionEnvioOriginalesServicios = (IInstruccionEnvioOriginalesServicios)Activator.GetObject(typeof(IInstruccionEnvioOriginalesServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InstruccionEnvioOriginalesServicios"]);
                this._instruccionDescuentoServicios = (IInstruccionDescuentoServicios)Activator.GetObject(typeof(IInstruccionDescuentoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InstruccionDescuentoServicios"]);
                this._instruccionOtrosServicios = (IInstruccionOtrosServicios)Activator.GetObject(typeof(IInstruccionOtrosServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InstruccionOtrosServicios"]);

                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }




        public void ActualizarTitulo()
        {
            if (_agregar == true)
                this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleAgregarCambioDeDomicilio,
                Recursos.Ids.GestionarCambioDeDomicilio);
            else
                this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleGestionarCambioDeDomicilio,
                Recursos.Ids.GestionarCambioDeDomicilio);
        }


        /// <summary>
        /// Método que carga los datos iniciales a mostrar en la página
        /// </summary>
        public void CargarPagina()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                ActualizarTitulo();

                if (_agregar == false)
                {
                    CambioDeDomicilio cambioDeDomicilio = (CambioDeDomicilio)this._ventana.CambioDeDomicilio;

                    if (((CambioDeDomicilio)cambioDeDomicilio).Marca != null)
                        this._ventana.Marca = this._marcaServicios.ConsultarMarcaConTodo(((CambioDeDomicilio)cambioDeDomicilio).Marca);



                    this._ventana.NombreMarca = ((Marca)this._ventana.Marca).Descripcion;
                    this._ventana.AgenteApoderado = ((CambioDeDomicilio)cambioDeDomicilio).Agente;
                    this._ventana.Poder = cambioDeDomicilio.Poder;
                    if (cambioDeDomicilio.Acta != null)
                        this._ventana.CambiarCheckAsientoLibro(true);



                    if (((Marca)this._ventana.Marca).LocalidadMarca != null)
                    {
                        this._ventana.EsMarcaNacional(!((Marca)this._ventana.Marca).LocalidadMarca.Equals("I"));

                        if (((Marca)this._ventana.Marca).LocalidadMarca.Equals("I"))
                        {
                            ListaDatosValores itemBuscado = new ListaDatosValores();
                            itemBuscado.Valor = ((Marca)this._ventana.Marca).ClasificacionInternacional;
                            IList<ListaDatosValores> items = this._listaDatosValoresServicios.
                                ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiLocalidadMarca));
                            this._ventana.TipoClase = this.BuscarListaDeDatosValores(items, itemBuscado).Descripcion;
                        }
                        else
                        {
                            this._ventana.EsMarcaNacional(true);
                            this._ventana.BorrarCerosInternacional();
                        }
                    }
                    else
                    {
                        this._ventana.EsMarcaNacional(true);
                        this._ventana.BorrarCerosInternacional();
                    }

                    this._ventana.IdCadenaDeCambios = cambioDeDomicilio.CadenaDeCambios.ToString();

                    CargarMarca();

                    CargarInteresado("Anterior");

                    CargarInteresado("Actual");

                    CargarApoderado();

                    CargarPoder();

                    LlenarListasPoderes((CambioDeDomicilio)this._ventana.CambioDeDomicilio);

                    ValidarInteresado();

                    CargaBoletines();

                    if (System.IO.File.Exists(ConfigurationManager.AppSettings["RutaPlanillaPDFCambioDomicilioMarca"].ToString() + ((CambioDeDomicilio)this._ventana.CambioDeDomicilio).Id + ".pdf"))
                    {
                        this._ventana.PintarVerPlanilla();
                    }

                }
                else
                {
                    CargarMarca();

                    CargarInteresado("Anterior");

                    CargarInteresado("Actual");

                    CargarApoderado();

                    CargarPoder();

                    CargaBoletines();
                }

                this._ventana.ConvertirEnteroMinimoABlanco();
                this._ventana.FocoPredeterminado();

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        private void CargarInteresado(string tipo)
        {
            Interesado primerInteresado = new Interesado(int.MinValue);

            if (tipo.Equals("Anterior"))
            {
                this._interesadosAnterior = new List<Interesado>();

                this._interesadosAnterior.Add(primerInteresado);

                if (((CambioDeDomicilio)this._ventana.CambioDeDomicilio).InteresadoAnterior != null)
                {
                    this._ventana.InteresadoAnterior = this._interesadoServicios.ConsultarInteresadoConTodo(((CambioDeDomicilio)this._ventana.CambioDeDomicilio).InteresadoAnterior);
                    this._ventana.NombreInteresadoAnterior = ((Interesado)this._ventana.InteresadoAnterior).Nombre;
                    this._ventana.IdInteresadoAnterior = ((Interesado)this._ventana.InteresadoAnterior).Id.ToString();

                    if ((Interesado)this._ventana.InteresadoAnterior != null)
                    {
                        this._interesadosAnterior.Add((Interesado)this._ventana.InteresadoAnterior);
                        this._ventana.InteresadosAnteriorFiltrados = this._interesadosAnterior;
                        this._ventana.InteresadoAnteriorFiltrado = this.BuscarInteresado((IList<Interesado>)this._ventana.InteresadosAnteriorFiltrados, (Interesado)this._ventana.InteresadoAnterior);
                        this._ventana.IdInteresadoAnterior = ((Interesado)this._ventana.InteresadoAnterior).Id.ToString();
                    }
                }
                else
                {
                    this._ventana.InteresadoAnterior = primerInteresado;
                    this._ventana.InteresadosAnteriorFiltrados = this._interesadosAnterior;
                    this._ventana.InteresadoAnteriorFiltrado = primerInteresado;
                    this._ventana.IdInteresadoAnterior = primerInteresado.Id.ToString();

                }
            }
            else if (tipo.Equals("Actual"))
            {
                this._interesadosActual = new List<Interesado>();

                this._interesadosActual.Add(primerInteresado);

                if (((CambioDeDomicilio)this._ventana.CambioDeDomicilio).InteresadoActual != null)
                {
                    this._ventana.InteresadoActual = this._interesadoServicios.ConsultarInteresadoConTodo(((CambioDeDomicilio)this._ventana.CambioDeDomicilio).InteresadoActual);
                    this._ventana.NombreInteresadoActual = ((Interesado)this._ventana.InteresadoActual).Nombre;
                    this._ventana.IdInteresadoActual = ((Interesado)this._ventana.InteresadoActual).Id.ToString();

                    if ((Interesado)this._ventana.InteresadoActual != null)
                    {
                        this._interesadosActual.Add((Interesado)this._ventana.InteresadoActual);
                        this._ventana.InteresadosActualFiltrados = this._interesadosActual;
                        this._ventana.InteresadoActualFiltrado = this.BuscarInteresado((IList<Interesado>)this._ventana.InteresadosActualFiltrados, (Interesado)this._ventana.InteresadoActual);
                        this._ventana.IdInteresadoActual = ((Interesado)this._ventana.InteresadoActual).Id.ToString();
                    }
                }
                else
                {
                    this._ventana.InteresadoActual = primerInteresado;
                    this._ventana.InteresadosActualFiltrados = this._interesadosActual;
                    this._ventana.InteresadoActualFiltrado = primerInteresado;
                    this._ventana.IdInteresadoActual = primerInteresado.Id.ToString();

                }
            }
        }


        public void IrConsultarMarcas()
        {
            this.Navegar(new ConsultarMarcas());
        }


        public CambioDeDomicilio CargarCambioDeDomicilioDeLaPantalla()
        {

            CambioDeDomicilio cambioDeDomicilio = (CambioDeDomicilio)this._ventana.CambioDeDomicilio;

            if (!this._ventana.Fecha.Equals(""))
                cambioDeDomicilio.FechaDomicilio = DateTime.Parse(this._ventana.Fecha);
            else
                cambioDeDomicilio.FechaDomicilio = null;

            if ((null != this._ventana.MarcaFiltrada) && (((Marca)this._ventana.MarcaFiltrada).Id != int.MinValue))
            {
                cambioDeDomicilio.Marca = (Marca)this._ventana.MarcaFiltrada;
                cambioDeDomicilio.InteresadoAnterior = ((Marca)this._ventana.MarcaFiltrada).Interesado;
                cambioDeDomicilio.Agente = ((Marca)this._ventana.MarcaFiltrada).Agente;
                cambioDeDomicilio.Poder = ((Marca)this._ventana.MarcaFiltrada).Poder;
            }

            if (null != this._ventana.InteresadoAnterior)
                cambioDeDomicilio.InteresadoAnterior = ((Interesado)this._ventana.InteresadoAnterior).Id != int.MinValue ? (Interesado)this._ventana.InteresadoAnterior : null;

            if (null != this._ventana.InteresadoActual)
                cambioDeDomicilio.InteresadoActual = ((Interesado)this._ventana.InteresadoActual).Id != int.MinValue ? (Interesado)this._ventana.InteresadoActual : null;

            if (null != this._ventana.AgenteApoderado)
                cambioDeDomicilio.Agente = !((Agente)this._ventana.AgenteApoderado).Id.Equals("") ? (Agente)this._ventana.AgenteApoderado : null;

            if (null != this._ventana.Poder)
                cambioDeDomicilio.Poder = ((Poder)this._ventana.Poder).Id != int.MinValue ? (Poder)this._ventana.Poder : null;

            if (null != this._ventana.Boletin)
                cambioDeDomicilio.BoletinPublicacion = ((Boletin)this._ventana.Boletin).Id != int.MinValue ? (Boletin)this._ventana.Boletin : null;

            if ((null != this._ventana.IdCadenaDeCambios) && (!this._ventana.IdCadenaDeCambios.Equals(String.Empty)) && (!this._ventana.IdCadenaDeCambios.Equals("0")))
                cambioDeDomicilio.CadenaDeCambios = int.Parse(this._ventana.IdCadenaDeCambios);
            else
                cambioDeDomicilio.CadenaDeCambios = null;

            return cambioDeDomicilio;
        }


        public void CambiarAModificar()
        {
            this._ventana.HabilitarCampos = true;
            this._ventana.TextoBotonModificar = Recursos.Etiquetas.btnAceptar;
        }


        /// <summary>
        /// Método que dependiendo del estado de la página, habilita los campos o 
        /// modifica los datos del usuario
        /// </summary>
        public void Modificar()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            String distingueMarca = String.Empty;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                //Habilitar campos
                if (this._ventana.TextoBotonModificar == Recursos.Etiquetas.btnModificar)
                {
                    this._ventana.HabilitarCampos = true;
                    this._ventana.TextoBotonModificar = Recursos.Etiquetas.btnAceptar;
                }

                //Modifica o inserta los datos del Cambio de Domicilio
                else if (this._ventana.TextoBotonModificar == Recursos.Etiquetas.btnAceptar)
                {
                    CambioDeDomicilio cambioDeDomicilio = CargarCambioDeDomicilioDeLaPantalla();

                    cambioDeDomicilio.Marca = (Marca)this._ventana.Marca;
                    if (null != cambioDeDomicilio.Marca)
                    {
                        //Comentar el proceso de llenado de la marca
                        //Solamente se debe verificar si existe la marca

                        #region Preparando la Marca del Cambio de Domicilio para el Hibrido

                        distingueMarca = cambioDeDomicilio.Marca.XDistingue;
                        cambioDeDomicilio.Marca.Distingue = distingueMarca;
                        cambioDeDomicilio.Marca.XDistingue = String.Empty; 

                        #endregion  

                        cambioDeDomicilio.Marca.InfoBoles =
                            this._infoBolServicios.ConsultarInfoBolesPorMarca(cambioDeDomicilio.Marca);
                        cambioDeDomicilio.Marca.Operaciones =
                            this._operacionServicios.ConsultarOperacionesPorMarca(cambioDeDomicilio.Marca);
                        cambioDeDomicilio.Marca.Busquedas =
                            this._busquedaServicios.ConsultarBusquedasPorMarca(cambioDeDomicilio.Marca);
                        if (cambioDeDomicilio.Marca.InfoAdicional != null)
                            cambioDeDomicilio.Marca.InfoAdicional =
                                this._infoAdicionalServicios.ConsultarPorId(cambioDeDomicilio.Marca.InfoAdicional);
                        if (cambioDeDomicilio.Marca.Anaqua != null)
                            cambioDeDomicilio.Marca.Anaqua =
                                this._anaquaServicios.ConsultarPorId(cambioDeDomicilio.Marca.Anaqua);
                        

                        if (null != cambioDeDomicilio.InteresadoActual)
                        {
                            if (null != cambioDeDomicilio.FechaDomicilio)
                            {
                                int? exitoso =
                                    this._cambioDeDomicilioServicios.InsertarOModificarCambioDeDomicilio(cambioDeDomicilio,
                                                                                                         UsuarioLogeado.Hash);
                                if ((!exitoso.Equals(null)) && (this._agregar == false))
                                {
                                    this._marcaServicios.ActualizarDistingueDeMarca(cambioDeDomicilio.Marca, distingueMarca);
                                    this._ventana.HabilitarCampos = false;
                                    this._ventana.TextoBotonModificar = Recursos.Etiquetas.btnModificar;
                                }
                                else if ((!exitoso.Equals(null)) && (this._agregar == true))
                                {
                                    cambioDeDomicilio.Id = exitoso.Value;
                                    this._marcaServicios.ActualizarDistingueDeMarca(cambioDeDomicilio.Marca, distingueMarca);
                                    this.Navegar(new GestionarCambioDeDomicilio(cambioDeDomicilio));
                                }
                                else
                                    this.Navegar(Recursos.MensajesConElUsuario.ErrorAlGenerarTraspaso, true);
                            }
                            else
                                this._ventana.Mensaje(Recursos.MensajesConElUsuario.ErrorSinFecha, 1);
                        }
                        else
                            this._ventana.Mensaje(Recursos.MensajesConElUsuario.ErrorSinInteresadoActual, 1);

                    }
                    else
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.ErrorTraspasoSinMarca, 1);



                }

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(ex.Message, true);
            }
            catch (RemotingException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
            }
            catch (SocketException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        /// <summary>
        /// Metodo que se encarga de eliminar un Cambio de Domicilio
        /// </summary>
        public void Eliminar()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (this._cambioDeDomicilioServicios.Eliminar((CambioDeDomicilio)this._ventana.CambioDeDomicilio, UsuarioLogeado.Hash))
                {
                    _paginaPrincipal.MensajeUsuario = Recursos.MensajesConElUsuario.CambioDeDomicilioEliminado;
                    this.Navegar(_paginaPrincipal);
                }

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(ex.Message, true);
            }
            catch (RemotingException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
            }
            catch (SocketException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        /// <summary>
        /// Método que carga los boletines registrados
        /// </summary>
        private void CargaBoletines()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
            CambioDeDomicilio cambioDeDomicilio = (CambioDeDomicilio)this._ventana.CambioDeDomicilio;
            Boletin primerBoletin = new Boletin(int.MinValue);
            IList<Boletin> boletines = this._boletinServicios.ConsultarTodos();
            boletines.Insert(0, primerBoletin);
            this._ventana.Boletines = boletines;
            if (_agregar == false)
                this._ventana.Boletin = BuscarBoletin(boletines, cambioDeDomicilio.BoletinPublicacion);

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

        }


        public void Auditoria()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion


                this.Navegar(new ListaAuditorias(_auditorias));


                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(ex.Message, true);
            }
            catch (RemotingException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
            }
            catch (SocketException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }


        /// <summary>
        /// Método que ordena una columna
        /// </summary>
        public void OrdenarColumna(GridViewColumnHeader column, ListView ListaResultados)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            String field = column.Tag as String;

            if (this._ventana.CurSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(this._ventana.CurSortCol).Remove(this._ventana.CurAdorner);
                ListaResultados.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (this._ventana.CurSortCol == column && this._ventana.CurAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            this._ventana.CurSortCol = column;
            this._ventana.CurAdorner = new SortAdorner(this._ventana.CurSortCol, newDir);
            AdornerLayer.GetAdornerLayer(this._ventana.CurSortCol).Add(this._ventana.CurAdorner);
            ListaResultados.Items.SortDescriptions.Add(
                new SortDescription(field, newDir));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        public void LlenarListaAgenteEInteresado(Poder poder, bool cargaInicial)
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Mouse.OverrideCursor = Cursors.Wait;

                Interesado interesado = new Interesado();
                IList<Agente> agentesInteresadoFiltrados;
                IList<Interesado> interesadosFiltrados = new List<Interesado>();
                Poder poderFiltrar = new Poder();

                Interesado primerInteresado = new Interesado(int.MinValue);
                Agente primerAgente = new Agente("");

                Agente agenteActual = new Agente();

                agentesInteresadoFiltrados = new List<Agente>();

                if (poder.Id == null)
                    poderFiltrar.Id = this._ventana.IdPoderFiltrar.Equals("") ? 0 : int.Parse(this._ventana.IdPoderFiltrar);
                else
                    poderFiltrar.Id = poder.Id;

                if (poderFiltrar.Id != 0)
                {
                    interesado = this._interesadoServicios.ObtenerInteresadosDeUnPoder((Poder)this._ventana.PoderFiltrado);
                    agentesInteresadoFiltrados = this._agenteServicios.ObtenerAgentesDeUnPoder((Poder)this._ventana.PoderFiltrado);
                }

                if (interesado != null)
                {
                    interesadosFiltrados.Insert(0, primerInteresado);
                    interesadosFiltrados.Add(interesado);
                    this._ventana.InteresadosActualFiltrados = interesadosFiltrados;

                    if (cargaInicial)
                        this._ventana.InteresadoActualFiltrado = this.BuscarInteresado(interesadosFiltrados, interesado);
                    else
                        this._ventana.InteresadoActualFiltrado = primerInteresado;
                }
                else
                {
                    interesadosFiltrados.Insert(0, primerInteresado);
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                    this._ventana.InteresadoActualFiltrado = primerInteresado;
                }

                if (agentesInteresadoFiltrados.Count != 0)
                {
                    agenteActual = (Agente)this._ventana.AgenteApoderadoFiltrado;
                    agentesInteresadoFiltrados.Insert(0, primerAgente);
                    this._ventana.AgenteApoderadoFiltrados = agentesInteresadoFiltrados;
                    this._ventana.AgenteApoderadoFiltrado = BuscarAgente(agentesInteresadoFiltrados, agenteActual);
                }
                else
                {
                    agentesInteresadoFiltrados.Insert(0, primerAgente);
                    this._ventana.AgenteApoderadoFiltrados = this._agentesApoderados;
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                    this._ventana.AgenteApoderadoFiltrado = primerAgente;
                }

                Mouse.OverrideCursor = null;

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(ex.Message, true);
            }
            catch (RemotingException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
            }
            catch (SocketException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }

        }


        private void LlenarListaAgente(Poder poder)
        {
            Agente primerAgente = new Agente("");

            this._agentesApoderados = this._agenteServicios.ObtenerAgentesDeUnPoder(poder);
            this._agentesApoderados.Insert(0, primerAgente);
            this._ventana.AgenteApoderadoFiltrados = this._agentesApoderados;
            this._ventana.AgenteApoderadoFiltrado = primerAgente;
        }


        #region Marcas


        private void CargarMarca()
        {
            this._marcas = new List<Marca>();
            Marca primeraMarca = new Marca(int.MinValue);
            this._marcas.Add(primeraMarca);

            if ((Marca)this._ventana.Marca != null)
            {
                IList<ListaDatosDominio> tipoClasesNacional = this._listaDatosDominioServicios.
                    ConsultarListaDatosDominioPorParametro(new ListaDatosDominio(Recursos.Etiquetas.cbiCategoriaTipoClaseNacional));


                //this._ventana.TiposClaseNacional = tipoClasesNacional;
                ListaDatosDominio listaDatoAux = this.BuscarClaseNacional(tipoClasesNacional, ((Marca)this._ventana.Marca).Tipo);
                if(listaDatoAux != null)
                    this._ventana.TipoClaseNacional = listaDatoAux.Descripcion;

                this._marcas.Add((Marca)this._ventana.Marca);
                this._ventana.MarcasFiltradas = this._marcas;
                this._ventana.MarcaFiltrada = (Marca)this._ventana.Marca;
                this._ventana.IdMarca = ((Marca)this._ventana.Marca).Id.ToString();

                if (null != ((Marca)this._ventana.Marca).Asociado)
                    this._ventana.PintarAsociado(((Marca)this._ventana.Marca).Asociado.TipoCliente.Id);
                else
                    this._ventana.PintarAsociado("5");
            }
            else
            {
                this._ventana.MarcasFiltradas = this._marcas;
                this._ventana.MarcaFiltrada = primeraMarca;
            }

        }


        public void ConsultarMarcas()
        {

            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Marca primeraMarca = new Marca(int.MinValue);


                Marca marca = new Marca();
                IList<Marca> marcasFiltradas;
                marca.Descripcion = this._ventana.NombreMarcaFiltrar.ToUpper();
                marca.Id = this._ventana.IdMarcaFiltrar.Equals("") ? 0 : int.Parse(this._ventana.IdMarcaFiltrar);

                if ((!marca.Descripcion.Equals("")) || (marca.Id != 0))
                    marcasFiltradas = this._marcaServicios.ObtenerMarcasFiltro(marca);
                else
                    marcasFiltradas = new List<Marca>();

                if (marcasFiltradas.ToList<Marca>().Count != 0)
                {
                    marcasFiltradas.Insert(0, primeraMarca);
                    this._ventana.MarcasFiltradas = marcasFiltradas.ToList<Marca>();
                    this._ventana.MarcaFiltrada = primeraMarca;
                }
                else
                {
                    marcasFiltradas.Insert(0, primeraMarca);
                    this._ventana.MarcasFiltradas = this._marcas;
                    this._ventana.MarcaFiltrada = primeraMarca;
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                }

                Mouse.OverrideCursor = null;

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(ex.Message, true);
            }
            catch (RemotingException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
            }
            catch (SocketException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        public bool CambiarMarca()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            bool retorno = false, flag = false;
            String cadenaMensaje = String.Empty;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion


                if (this._ventana.MarcaFiltrada != null)
                {
                    this._ventana.Marca = this._ventana.MarcaFiltrada;
                    this._ventana.NombreMarca = ((Marca)this._ventana.MarcaFiltrada).Descripcion;
                    this._ventana.IdMarca = ((Marca)this._ventana.MarcaFiltrada).Id.ToString();

                    if (((Marca)this._ventana.Marca).LocalidadMarca != null)
                    {
                        this._ventana.EsMarcaNacional(!((Marca)this._ventana.Marca).LocalidadMarca.Equals("I"));

                        if (((Marca)this._ventana.Marca).LocalidadMarca.Equals("I"))
                        {
                            ListaDatosValores itemBuscado = new ListaDatosValores();
                            itemBuscado.Valor = ((Marca)this._ventana.Marca).ClasificacionInternacional;
                            IList<ListaDatosValores> items = this._listaDatosValoresServicios.
                                ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiLocalidadMarca));
                            this._ventana.TipoClase = this.BuscarListaDeDatosValores(items, itemBuscado).Descripcion;
                        }
                    }
                    else
                        this._ventana.EsMarcaNacional(true);

                    this._marcas.RemoveAt(0);
                    this._marcas.Add((Marca)this._ventana.MarcaFiltrada);
                    this._ventana.InteresadoAnterior = ((Marca)this._ventana.Marca).Interesado;

                    if (((Marca)this._ventana.Marca).Interesado != null)
                        this._ventana.IdInteresadoAnterior = (((Marca)this._ventana.Marca).Interesado).Id.ToString();

                    IList<Interesado> listaAux = new List<Interesado>();
                    listaAux.Add(new Interesado(int.MinValue));

                    if (((Marca)this._ventana.MarcaFiltrada).Id != int.MinValue)
                    {
                        listaAux.Add((Interesado)this._ventana.InteresadoAnterior);

                        this._ventana.InteresadosAnteriorFiltrados = listaAux;
                        this._ventana.InteresadoAnteriorFiltrado = this.BuscarInteresado((IList<Interesado>)this._ventana.InteresadosAnteriorFiltrados,
                            (Interesado)this._ventana.InteresadoAnterior);
                    }
                    else
                    {
                        this._ventana.InteresadosAnteriorFiltrados = listaAux;
                        this._ventana.IdInteresadoAnterior = int.MinValue.ToString();
                        this._ventana.InteresadoAnteriorFiltrado = this.BuscarInteresado((IList<Interesado>)this._ventana.InteresadosAnteriorFiltrados,
                            new Interesado(int.MinValue));
                    }

                    this._ventana.AgenteApoderado = ((Marca)this._ventana.Marca).Agente;
                    this._ventana.Poder = ((Marca)this._ventana.Marca).Poder;


                    if (null != ((Marca)this._ventana.Marca).Asociado)
                        this._ventana.PintarAsociado(((Marca)this._ventana.Marca).Asociado.TipoCliente.Id);
                    else
                        this._ventana.PintarAsociado("5");

                    retorno = true;

                    #region Instrucciones de Correspondencia

                    InstruccionCorrespondencia instruccionEnvioEmails = new InstruccionCorrespondencia();
                    InstruccionCorrespondencia instruccion = null;
                    instruccionEnvioEmails.Id = ((Marca)this._ventana.Marca).Id;
                    instruccionEnvioEmails.AplicadaA = "M";
                    instruccionEnvioEmails.Concepto = "C";

                    InstruccionEnvioOriginales instruccionEnvioOriginales = new InstruccionEnvioOriginales();
                    InstruccionEnvioOriginales instruccionEO = null;
                    instruccionEnvioOriginales.Id = ((Marca)this._ventana.Marca).Id;
                    instruccionEnvioOriginales.AplicadaA = "M";
                    instruccionEnvioOriginales.Concepto = "C";

                    instruccion = this._instruccionCorrespondenciaServicios.ObtenerInstruccionCorrespondencia(instruccionEnvioEmails);
                    instruccionEO = this._instruccionEnvioOriginalesServicios.ObtenerInstruccionEnvioOriginales(instruccionEnvioOriginales);
                    
                    if (instruccion != null)
                    {
                        flag = true;
                        cadenaMensaje += " tiene Instrucción de Correspondencia por Envío de Email";

                    }

                    if (instruccionEO != null)
                    {
                        if (flag)
                            cadenaMensaje += ", ";
                        cadenaMensaje += "tiene Instrucción de Correspondencia por Envío de Originales";
                    }
                    
                    #endregion

                    #region Instrucciones de Facturacion

                    InstruccionCorrespondencia instruccionFacEnvioEmails = new InstruccionCorrespondencia();
                    InstruccionCorrespondencia instruccionFac = null;
                    instruccionEnvioEmails.Id = ((Marca)this._ventana.Marca).Id;
                    instruccionEnvioEmails.AplicadaA = "M";
                    instruccionEnvioEmails.Concepto = "F";

                    InstruccionEnvioOriginales instruccionFacEnvioOriginales = new InstruccionEnvioOriginales();
                    InstruccionEnvioOriginales instruccionFac_EO = null;
                    instruccionFacEnvioOriginales.Id = ((Marca)this._ventana.Marca).Id;
                    instruccionFacEnvioOriginales.AplicadaA = "M";
                    instruccionFacEnvioOriginales.Concepto = "F";

                    instruccionFac = this._instruccionCorrespondenciaServicios.ObtenerInstruccionCorrespondencia(instruccionFacEnvioEmails);
                    instruccionFac_EO = this._instruccionEnvioOriginalesServicios.ObtenerInstruccionEnvioOriginales(instruccionFacEnvioOriginales);
                    
                    if (instruccionFac != null)
                    {
                        if (flag)
                            cadenaMensaje += ", ";
                        cadenaMensaje += "tiene Instrucción de Facturación por Envío de Email";
                    }

                    if (instruccionFac_EO != null)
                    {
                        if (flag)
                            cadenaMensaje += ", ";
                        cadenaMensaje += "tiene Instrucción de Facturación por Envío de Originales";
                    }

                    #endregion

                    #region Instrucciones de Descuento de Marca

                    InstruccionDescuento instruccionDescuentoFiltro = new InstruccionDescuento();
                    instruccionDescuentoFiltro.CodigoOperacion = ((Marca)this._ventana.Marca).Id;
                    instruccionDescuentoFiltro.AplicaA = "M";

                    IList<InstruccionDescuento> instruccionesD =
                        this._instruccionDescuentoServicios.ObtenerInstruccionesDeDescuentoMarcaOPatente(instruccionDescuentoFiltro);

                    if (instruccionesD.Count > 0)
                    {
                        if (flag)
                            cadenaMensaje += ", ";
                        cadenaMensaje += String.Format(" tiene {0} Instruccion(es) de Descuento", instruccionesD.Count.ToString());
                    }

                    #endregion

                    #region Instrucciones No Tipificadas  De Marca

                    InstruccionOtros instruccionNoTipificadaFiltro = new InstruccionOtros();
                    instruccionNoTipificadaFiltro.Cod_MarcaOPatente = ((Marca)this._ventana.Marca).Id;
                    instruccionNoTipificadaFiltro.AplicaA = "M";

                    IList<InstruccionOtros> instrucciones =
                        this._instruccionOtrosServicios.ObtenerInstruccionesNoTipificadasPorFiltro(instruccionNoTipificadaFiltro);

                    if (instrucciones.Count > 0)
                    {
                        if (flag)
                            cadenaMensaje += ", ";
                        cadenaMensaje += String.Format(" tiene {0} Instruccion(es) de Otro tipo", instrucciones.Count.ToString());
                    }

                    #endregion

                    if (cadenaMensaje.Length > 0)
                        this._ventana.Mensaje(string.Format("La Marca {0}", ((Marca)this._ventana.Marca).Id.ToString()) + cadenaMensaje, 2);
                }

                this._ventana.BorrarCerosInternacional();
                this._ventana.ConvertirEnteroMinimoABlanco();

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(ex.Message, true);
            }
            catch (RemotingException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
            }
            catch (SocketException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }

            return retorno;
        }


        #endregion


        #region InteresadoAnterior


        public void ConsultarInteresadosAnterior()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Interesado primerInteresado = new Interesado(int.MinValue);

                Interesado interesado = new Interesado();
                IList<Interesado> interesadosFiltrados;
                interesado.Nombre = this._ventana.NombreInteresadoAnteriorFiltrar.ToUpper();
                interesado.Id = this._ventana.IdInteresadoAnteriorFiltrar.Equals("") ? 0 : int.Parse(this._ventana.IdInteresadoAnteriorFiltrar);

                if ((!interesado.Nombre.Equals("")) || (interesado.Id != 0))
                    interesadosFiltrados = this._interesadoServicios.ObtenerInteresadosFiltro(interesado);
                else
                    interesadosFiltrados = new List<Interesado>();

                if (interesadosFiltrados.Count != 0)
                {
                    interesadosFiltrados.Insert(0, primerInteresado);
                    this._ventana.InteresadosAnteriorFiltrados = interesadosFiltrados;
                    this._ventana.InteresadoAnteriorFiltrado = primerInteresado;
                }
                else
                {
                    interesadosFiltrados.Insert(0, primerInteresado);
                    this._ventana.InteresadosAnteriorFiltrados = this._interesadosAnterior;
                    this._ventana.InteresadoAnteriorFiltrado = primerInteresado;
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                }

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(ex.Message, true);
            }
            catch (RemotingException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
            }
            catch (SocketException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        public bool CambiarInteresadoAnterior()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            bool retorno = false;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (this._ventana.InteresadoAnteriorFiltrado != null)
                {
                    this._ventana.InteresadoAnterior =
                        this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.InteresadoAnteriorFiltrado);
                    this._ventana.NombreInteresadoAnterior = ((Interesado)this._ventana.InteresadoAnteriorFiltrado).Nombre;
                    this._ventana.IdInteresadoAnterior = ((Interesado)this._ventana.InteresadoAnteriorFiltrado).Id.ToString();
                    this._interesadosAnterior.RemoveAt(0);
                    this._interesadosAnterior.Add((Interesado)this._ventana.InteresadoAnteriorFiltrado);
                    retorno = true;

                    this._ventana.ConvertirEnteroMinimoABlanco();
                }

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(ex.Message, true);
            }
            catch (RemotingException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
            }
            catch (SocketException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }

            return retorno;
        }


        #endregion


        #region InteresadoActual


        private void ValidarInteresado()
        {
            if (((Interesado)this._ventana.InteresadoActualFiltrado).Id == int.MinValue)     // No tengo interesadoActual    1
            {
                if (((Agente)this._ventana.AgenteApoderadoFiltrado).Id.Equals(""))           //No tengo agente           2
                {
                    if (((Poder)this._ventana.PoderFiltrado).Id != int.MinValue)             //tengo poder            3
                    {
                        LlenarListaAgenteEInteresado((Poder)this._ventana.Poder, true);

                        //this._ventana.GestionarBotonConsultarInteresado(false);
                        //this._ventana.GestionarBotonConsultarApoderado(false);
                        this._ventana.GestionarBotonConsultarInteresado(true);
                        this._ventana.GestionarBotonConsultarApoderado(true);
                    }
                }
                else                                                                       //Si tengo agente                                                                      
                {
                    if (((CambioDeDomicilio)this._ventana.PoderFiltrado).Id == int.MinValue)
                        //this._ventana.GestionarBotonConsultarInteresado(false);
                        this._ventana.GestionarBotonConsultarInteresado(true);

                    else
                    {
                        LlenarListaAgenteEInteresado((Poder)this._ventana.Poder, true);

                        //this._ventana.GestionarBotonConsultarInteresado(false);
                        //this._ventana.GestionarBotonConsultarApoderado(false);
                        //this._ventana.GestionarBotonConsultarPoder(false);
                        this._ventana.GestionarBotonConsultarInteresado(true);
                        this._ventana.GestionarBotonConsultarApoderado(true);
                        this._ventana.GestionarBotonConsultarPoder(true);
                    }

                }
            }
            else                                                                           //Si tengo interesadoActual
            {
                if (((Agente)this._ventana.AgenteApoderadoFiltrado).Id.Equals(""))         //No tengo Agente
                {
                    if (((Poder)this._ventana.PoderFiltrado).Id == int.MinValue)           //No tengo poder
                        //this._ventana.GestionarBotonConsultarPoder(false);
                        this._ventana.GestionarBotonConsultarPoder(true);

                    else
                    {
                        LlenarListaAgenteEInteresado((Poder)this._ventana.Poder, true);

                        //this._ventana.GestionarBotonConsultarInteresado(false);
                        //this._ventana.GestionarBotonConsultarApoderado(false);
                        //this._ventana.GestionarBotonConsultarPoder(false);

                        this._ventana.GestionarBotonConsultarInteresado(true);
                        this._ventana.GestionarBotonConsultarApoderado(true);
                        this._ventana.GestionarBotonConsultarPoder(true);

                    }
                }
                else                                                                        //tengo Agente
                {
                    if (((Poder)this._ventana.PoderFiltrado).Id == int.MinValue)            //No tengo poder
                    {
                        ValidarListaDePoderes(this._poderesInteresadoActual, this._poderesApoderado);

                        //this._ventana.GestionarBotonConsultarPoder(false);
                        this._ventana.GestionarBotonConsultarPoder(true);
                    }
                    else                                                                  //Si tengo poder
                    {
                        LlenarListaAgenteEInteresado((Poder)this._ventana.Poder, true);
                        ValidarListaDePoderes(this._poderesInteresadoActual, this._poderesApoderado);

                        /*this._ventana.GestionarBotonConsultarInteresado(false);
                        this._ventana.GestionarBotonConsultarApoderado(false);
                        this._ventana.GestionarBotonConsultarPoder(false);
                        */
                        this._ventana.GestionarBotonConsultarInteresado(true);
                        this._ventana.GestionarBotonConsultarApoderado(true);
                        this._ventana.GestionarBotonConsultarPoder(true);
                    }
                }
            }
        }


        public void ConsultarInteresadosActual()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Interesado primerInteresado = new Interesado(int.MinValue);

                Interesado interesado = new Interesado();
                IList<Interesado> interesadosFiltrados;
                interesado.Nombre = this._ventana.NombreInteresadoActualFiltrar.ToUpper();
                interesado.Id = this._ventana.IdInteresadoActualFiltrar.Equals("") ? 0 : int.Parse(this._ventana.IdInteresadoActualFiltrar);

                if ((!interesado.Nombre.Equals("")) || (interesado.Id != 0))
                    interesadosFiltrados = this._interesadoServicios.ObtenerInteresadosFiltro(interesado);
                else
                    interesadosFiltrados = new List<Interesado>();

                if (interesadosFiltrados.Count != 0)
                {
                    interesadosFiltrados.Insert(0, primerInteresado);
                    this._ventana.InteresadosActualFiltrados = interesadosFiltrados;
                    this._ventana.InteresadoActualFiltrado = primerInteresado;
                }
                else
                {
                    interesadosFiltrados.Insert(0, primerInteresado);
                    this._ventana.InteresadosActualFiltrados = this._interesadosActual;
                    this._ventana.InteresadoActualFiltrado = primerInteresado;
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                }

                Mouse.OverrideCursor = null;

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(ex.Message, true);
            }
            catch (RemotingException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
            }
            catch (SocketException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        public bool CambiarInteresadoActual()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            bool retorno = false;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (((Interesado)this._ventana.InteresadoActualFiltrado).Id != int.MinValue)
                {
                    if (!((Agente)this._ventana.AgenteApoderadoFiltrado).Id.Equals(""))
                    {
                        if (((Poder)this._ventana.PoderFiltrado).Id != int.MinValue)
                        {
                            this._ventana.InteresadoActual = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.InteresadoActualFiltrado);
                            this._ventana.NombreInteresadoActual = ((Interesado)this._ventana.InteresadoActual).Nombre;
                            this._ventana.IdInteresadoActual = ((Interesado)this._ventana.InteresadoActual).Id.ToString();
                            retorno = true;
                        }
                        else
                        {
                            this._poderesInteresadoActual = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.InteresadoActualFiltrado));

                            LimpiarListaPoder();

                            if ((this.ValidarListaDePoderes(this._poderesInteresadoActual, this._poderesApoderado)))
                            {
                                this._ventana.InteresadoActual = this._ventana.InteresadoActualFiltrado;
                                this._ventana.NombreInteresadoActual = ((Interesado)this._ventana.InteresadoActualFiltrado).Nombre;
                                this._ventana.IdInteresadoActual = ((Interesado)this._ventana.InteresadoActual).Id.ToString();
                                retorno = true;
                            }
                            else if (!this.ValidarListaDePoderes(this._poderesInteresadoActual, this._poderesApoderado))
                            {
                                //this._ventana.ConvertirEnteroMinimoABlanco();
                                this._ventana.Mensaje(string.Format(Recursos.MensajesConElUsuario.ErrorInteresadoNoPoseePoderConAgente, "Actual"), 0);
                            }
                        }
                    }
                    else
                    {
                        if (((Poder)this._ventana.PoderFiltrado).Id == int.MinValue)
                        {
                            Poder primerPoder = new Poder(int.MinValue);

                            this._poderesInteresadoActual = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.InteresadoActualFiltrado));
                            this._poderesInteresadoActual.Insert(0, primerPoder);
                            this._ventana.PoderesFiltrados = this._poderesInteresadoActual;
                            this._ventana.PoderFiltrado = primerPoder;

                            this._poderesInteresadoActual = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.InteresadoActualFiltrado));
                            this._ventana.InteresadoActual = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.InteresadoActualFiltrado);
                            this._ventana.NombreInteresadoActual = ((Interesado)this._ventana.InteresadoActual).Nombre;
                            this._ventana.IdInteresadoActual = ((Interesado)this._ventana.InteresadoActual).Id.ToString();
                            retorno = true;
                        }
                        else
                        {

                            this._poderesInteresadoActual = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.InteresadoActualFiltrado));
                            this._ventana.InteresadoActual = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.InteresadoActualFiltrado);
                            this._ventana.NombreInteresadoActual = ((Interesado)this._ventana.InteresadoActual).Nombre;
                            this._ventana.IdInteresadoActual = ((Interesado)this._ventana.InteresadoActual).Id.ToString();
                            retorno = true;
                        }
                    }

                }
                else
                {
                    this._ventana.InteresadoActual = this._ventana.InteresadoActualFiltrado;
                    this._ventana.NombreInteresadoActual = ((Interesado)this._ventana.InteresadoActual).Nombre;
                    this._ventana.IdInteresadoActual = ((Interesado)this._ventana.InteresadoActual).Id.ToString();
                    retorno = true;
                }

                this._ventana.ConvertirEnteroMinimoABlanco();

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(ex.Message, true);
            }
            catch (RemotingException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
            }
            catch (SocketException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }

            return retorno;
        }


        public bool VerificarCambioInteresado()
        {
            bool retorno = false;

            if ((((Interesado)this._ventana.InteresadoActualFiltrado).Id != int.MinValue) || !(((Agente)this._ventana.AgenteApoderadoFiltrado).Id.Equals("")))
                retorno = true;

            return retorno;
        }


        public void LimpiarListaInteresado()
        {
            Interesado primerInteresado = new Interesado(int.MinValue);
            IList<Interesado> listaInteresados = new List<Interesado>();
            listaInteresados.Add(primerInteresado);

            this._ventana.InteresadosActualFiltrados = listaInteresados;
            this._ventana.InteresadoActualFiltrado = BuscarInteresado(listaInteresados, primerInteresado);
            this._ventana.InteresadoActual = this._ventana.InteresadoActualFiltrado;
        }


        #endregion


        #region Agente Apoderado


        private void CargarApoderado()
        {
            Agente primerAgente = new Agente("");

            this._agentesApoderados = new List<Agente>();
            this._agentesApoderados.Add(primerAgente);

            if ((Agente)this._ventana.AgenteApoderado != null)
            {
                this._agentesApoderados.Add((Agente)this._ventana.AgenteApoderado);
                this._ventana.AgenteApoderadoFiltrados = this._agentesApoderados;
                this._ventana.AgenteApoderadoFiltrado = this.BuscarAgente((IList<Agente>)this._ventana.AgenteApoderadoFiltrados, (Agente)this._ventana.AgenteApoderado);
                this._ventana.IdAgenteApoderado = ((Agente)this._ventana.AgenteApoderado).Id;
            }
            else
            {
                this._ventana.AgenteApoderado = primerAgente;
                this._ventana.AgenteApoderadoFiltrados = this._agentesApoderados;
                this._ventana.AgenteApoderadoFiltrado = primerAgente;
                this._ventana.IdAgenteApoderado = primerAgente.Id;
            }

        }


        public void ConsultarApoderados()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Agente primerAgente = new Agente("");


                Agente apoderadoInteresado = new Agente();
                IList<Agente> agentesInteresadoFiltrados;
                apoderadoInteresado.Nombre = this._ventana.NombreAgenteApoderadoFiltrar.ToUpper();
                apoderadoInteresado.Id = this._ventana.IdAgenteFiltrar.ToUpper();

                if ((!apoderadoInteresado.Nombre.Equals("")) || (!apoderadoInteresado.Id.Equals("")))
                    agentesInteresadoFiltrados = this._agenteServicios.ObtenerAgentesSinPoderesFiltro(apoderadoInteresado);
                else
                    agentesInteresadoFiltrados = new List<Agente>();

                if (agentesInteresadoFiltrados.ToList<Agente>().Count != 0)
                {
                    agentesInteresadoFiltrados.Insert(0, primerAgente);
                    this._ventana.AgenteApoderadoFiltrados = agentesInteresadoFiltrados;
                    this._ventana.AgenteApoderadoFiltrado = primerAgente;
                }
                else
                {
                    agentesInteresadoFiltrados.Insert(0, primerAgente);
                    this._ventana.AgenteApoderadoFiltrados = this._agentesApoderados;
                    this._ventana.AgenteApoderadoFiltrado = primerAgente;
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                }

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(ex.Message, true);
            }
            catch (RemotingException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
            }
            catch (SocketException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        public bool CambiarApoderado()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            bool retorno = false;
            IList<Poder> _poderesFiltrados = new List<Poder>();
            Poder poderABuscar = null;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (!((Agente)this._ventana.AgenteApoderadoFiltrado).Id.Equals(""))
                {
                    if (((Interesado)this._ventana.InteresadoActual).Id != int.MinValue)
                    {
                        if ((null != this._ventana.PoderFiltrado) && ((Poder)this._ventana.PoderFiltrado).Id != int.MinValue)
                        {
                            this._ventana.AgenteApoderado = this._ventana.AgenteApoderadoFiltrado;
                            this._ventana.NombreAgenteApoderado = ((Agente)this._ventana.AgenteApoderadoFiltrado).Nombre;
                            this._ventana.IdAgenteApoderado = ((Agente)this._ventana.AgenteApoderadoFiltrado).Id;
                            //--
                            Poder primerPoder = new Poder();
                            primerPoder.Id = int.MinValue;

                            _poderesFiltrados = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado((Agente)this._ventana.AgenteApoderadoFiltrado, (Interesado)this._ventana.InteresadoActual);

                            if (_poderesFiltrados.Count != 0)
                            {
                                poderABuscar = this.BuscarPoder(_poderesFiltrados, (Poder)this._ventana.PoderFiltrado);

                                if (poderABuscar != null)
                                {
                                    _poderesFiltrados.Insert(0, primerPoder);
                                    this._ventana.PoderesFiltrados = _poderesFiltrados;
                                    this._ventana.PoderFiltrado = poderABuscar;
                                }
                            }

                            else
                            {
                                this._ventana.Mensaje("Apoderado no posee poderes con el interesado", 1);
                                _poderesFiltrados.Insert(0, primerPoder);
                                this._ventana.PoderesFiltrados = _poderesFiltrados;
                            }


                            //--

                            retorno = true;
                        }
                        else
                        {
                            //this._poderesApoderado = this._poderServicios.ConsultarPoderesPorAgente(((Agente)_ventana.AgenteApoderadoFiltrado));
                            this._poderesApoderado = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado((Agente)_ventana.AgenteApoderadoFiltrado, (Interesado)this._ventana.InteresadoActualFiltrado);

                            LimpiarListaPoder();

                            if ((this.ValidarListaDePoderes(this._poderesInteresadoActual, this._poderesApoderado)))
                            {
                                this._ventana.AgenteApoderado = this._ventana.AgenteApoderadoFiltrado;
                                this._ventana.NombreAgenteApoderado = ((Agente)this._ventana.AgenteApoderadoFiltrado).Nombre;
                                this._ventana.IdAgenteApoderado = ((Agente)this._ventana.AgenteApoderadoFiltrado).Id;
                                retorno = true;
                            }
                            else if (!this.ValidarListaDePoderes(this._poderesInteresadoActual, this._poderesApoderado))
                            {
                                //this._ventana.ConvertirEnteroMinimoABlanco();
                                //this._ventana.Mensaje(string.Format(Recursos.MensajesConElUsuario.ErrorAgenteNoPoseePoderConInteresado, "Cedente"), 0);
                                this._ventana.Mensaje(string.Format(Recursos.MensajesConElUsuario.ErrorAgenteNoPoseePoderConInteresado, "interesado"), 0);
                                this._ventana.AgenteApoderado = this._ventana.AgenteApoderadoFiltrado;
                                this._ventana.NombreAgenteApoderado = ((Agente)this._ventana.AgenteApoderadoFiltrado).Nombre;
                                this._ventana.IdAgenteApoderado = ((Agente)this._ventana.AgenteApoderadoFiltrado).Id;
                                retorno = true;
                            }
                        }
                    }
                    else
                    {
                        if (((Poder)this._ventana.PoderFiltrado).Id != int.MinValue)
                        {
                            this._ventana.AgenteApoderado = this._ventana.AgenteApoderadoFiltrado;
                            this._ventana.NombreAgenteApoderado = ((Agente)this._ventana.AgenteApoderadoFiltrado).Nombre;
                            this._ventana.IdAgenteApoderado = ((Agente)this._ventana.AgenteApoderadoFiltrado).Id;
                            retorno = true;
                        }
                        else
                        {
                            //this._poderesApoderado = this._poderServicios.ConsultarPoderesPorAgente(((Agente)_ventana.AgenteApoderadoFiltrado));
                            this._ventana.AgenteApoderado = this._ventana.AgenteApoderadoFiltrado;
                            this._ventana.NombreAgenteApoderado = ((Agente)this._ventana.AgenteApoderadoFiltrado).Nombre;
                            this._ventana.IdAgenteApoderado = ((Agente)this._ventana.AgenteApoderadoFiltrado).Id;
                            retorno = true;
                        }
                    }
                }
                else
                {
                    this._ventana.AgenteApoderado = this._ventana.AgenteApoderadoFiltrado;
                    this._ventana.NombreAgenteApoderado = ((Agente)this._ventana.AgenteApoderadoFiltrado).Nombre;
                    this._ventana.IdAgenteApoderado = ((Agente)this._ventana.AgenteApoderadoFiltrado).Id;
                    retorno = true;
                }

                this._ventana.ConvertirEnteroMinimoABlanco();

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(ex.Message, true);
            }
            catch (RemotingException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
            }
            catch (SocketException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }

            return retorno;
        }


        public bool VerificarCambioAgente()
        {
            bool retorno = false;

            if (!(((Agente)this._ventana.AgenteApoderadoFiltrado).Id.Equals("")) || (((Interesado)this._ventana.InteresadoActualFiltrado).Id != int.MinValue))
                retorno = true;

            return retorno;
        }


        public void LimpiarListaAgente()
        {
            Agente primerAgente = new Agente("");
            IList<Agente> listaAgentes = new List<Agente>();
            listaAgentes.Add(primerAgente);

            this._ventana.AgenteApoderadoFiltrados = listaAgentes;
            this._ventana.AgenteApoderadoFiltrado = BuscarAgente(listaAgentes, primerAgente);
            this._ventana.AgenteApoderado = this._ventana.AgenteApoderadoFiltrado;

        }


        #endregion


        #region Poder


        private void CargarPoder()
        {
            Poder primerPoder = new Poder(int.MinValue);

            this._poderes = new List<Poder>();
            this._poderes.Add(primerPoder);

            if (((CambioDeDomicilio)this._ventana.CambioDeDomicilio).Poder != null)
            {
                this._poderes.Add((Poder)this._ventana.Poder);
                this._ventana.PoderesFiltrados = this._poderes;
                this._ventana.PoderFiltrado = this.BuscarPoder((IList<Poder>)this._ventana.PoderesFiltrados, (Poder)this._ventana.Poder);
            }
            else
            {
                this._ventana.PoderesFiltrados = this._poderes;
                this._ventana.PoderFiltrado = this.BuscarPoder(this._poderes, this._poderes[0]);
            }


        }


        public void ConsultarPoderes()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Poder pimerPoder = new Poder(int.MinValue);


                Poder poder = new Poder();
                IList<Poder> poderesFiltrados;

                if (!this._ventana.IdPoderFiltrar.Equals(""))
                    poder.Id = int.Parse(this._ventana.IdPoderFiltrar);
                else
                    poder.Id = 0;

                if (!this._ventana.FechaPoderFiltrar.Equals(""))
                    poder.Fecha = DateTime.Parse(this._ventana.FechaPoderFiltrar);

                if ((!poder.Fecha.Equals("")) || (poder.Id != 0))
                    poderesFiltrados = this._poderServicios.ObtenerPoderesFiltro(poder);
                else
                    poderesFiltrados = new List<Poder>();

                if (poderesFiltrados.ToList<Poder>().Count != 0)
                {
                    poderesFiltrados.Insert(0, pimerPoder);
                    this._ventana.PoderesFiltrados = poderesFiltrados;
                    this._ventana.PoderFiltrado = pimerPoder;
                }
                else
                {
                    poderesFiltrados.Insert(0, pimerPoder);
                    this._ventana.PoderesFiltrados = this._poderes;
                    this._ventana.PoderFiltrado = pimerPoder;
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                }

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(ex.Message, true);
            }
            catch (RemotingException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
            }
            catch (SocketException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        public bool CambiarPoder()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            bool retorno = false;
            IList<Poder> _poderesFiltrados = new List<Poder>();
            Poder poderABuscar = null;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (((Poder)this._ventana.PoderFiltrado).Id != int.MinValue)
                {
                    if (((Agente)this._ventana.AgenteApoderadoFiltrado).Id.Equals(""))
                    {
                        if (((Interesado)this._ventana.InteresadoActualFiltrado).Id != int.MinValue)
                        {
                            LimpiarListaAgente();

                            LlenarListaAgente((Poder)this._ventana.PoderFiltrado);

                            this._ventana.Poder = this._ventana.PoderFiltrado;
                            this._ventana.IdPoder = ((Poder)this._ventana.PoderFiltrado).Id.ToString();
                            retorno = true;

                        }
                        else
                        {
                            LimpiarListaInteresado();

                            LimpiarListaAgente();

                            LlenarListaAgenteEInteresado((Poder)this._ventana.PoderFiltrado, false);

                            this._ventana.Poder = this._ventana.PoderFiltrado;
                            this._ventana.IdPoder = ((Poder)this._ventana.PoderFiltrado).Id.ToString();
                            retorno = true;
                        }
                    }
                    else
                    {
                        //El Agente es diferente a Vacio
                        //--

                        Poder primerPoder = new Poder();
                        primerPoder.Id = int.MinValue;
                        _poderesFiltrados = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado((Agente)this._ventana.AgenteApoderadoFiltrado, (Interesado)this._ventana.InteresadoActualFiltrado);
                        if (_poderesFiltrados.Count != 0)
                        {
                            poderABuscar = this.BuscarPoder(_poderesFiltrados, (Poder)this._ventana.PoderFiltrado);

                            if (poderABuscar != null)
                            {
                                _poderesFiltrados.Insert(0, primerPoder);
                                this._ventana.PoderesFiltrados = _poderesFiltrados;
                                this._ventana.PoderFiltrado = poderABuscar;
                                this._ventana.Poder = this._ventana.PoderFiltrado;
                                this._ventana.IdPoder = ((Poder)this._ventana.PoderFiltrado).Id.ToString();
                                retorno = true;
                            }
                            else
                            {
                                //this._ventana.Mensaje(string.Format(Recursos.MensajesConElUsuario.ErrorAgenteNoPoseePoderConInteresado, "interesado"), 0);
                                this._ventana.Mensaje("El Poder no pertenece al Interesado", 1);
                                this._ventana.Poder = this._ventana.PoderFiltrado;
                                this._ventana.IdPoder = ((Poder)this._ventana.PoderFiltrado).Id.ToString();
                                retorno = true;
                            }
                        }

                        //--
                        else
                        {
                            this._ventana.Mensaje(string.Format(Recursos.MensajesConElUsuario.ErrorPoderFiltradoNoAsociadoInteresado), 0);
                            this._ventana.Poder = this._ventana.PoderFiltrado;
                            this._ventana.IdPoder = ((Poder)this._ventana.PoderFiltrado).Id.ToString();
                            retorno = true;
                        }
                    }
                }
                else
                {
                    this._ventana.Poder = this._ventana.PoderFiltrado;
                    this._ventana.IdPoder = ((Poder)this._ventana.PoderFiltrado).Id.ToString();
                    retorno = true;
                }

                this._ventana.ConvertirEnteroMinimoABlanco();

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(ex.Message, true);
            }
            catch (RemotingException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
            }
            catch (SocketException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }

            return retorno;
        }


        public void LlenarListasPoderes(CambioDeDomicilio cambioDeDomicilio)
        {
            if (cambioDeDomicilio.InteresadoActual != null)
                this._poderesInteresadoActual = this._poderServicios.ConsultarPoderesPorInteresado(cambioDeDomicilio.InteresadoActual);

            if (cambioDeDomicilio.Agente != null)
                //this._poderesApoderado = this._poderServicios.ConsultarPoderesPorAgente(cambioDeDomicilio.Agente);
                this._poderesApoderado = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado(cambioDeDomicilio.Agente, cambioDeDomicilio.InteresadoActual);
        }


        public bool ValidarListaDePoderes(IList<Poder> listaPoderesA, IList<Poder> listaPoderesB)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            bool retorno = false;
            IList<Poder> listaIntereseccionInteresado = new List<Poder>();
            Poder primerPoder = new Poder(int.MinValue);

            Poder poderActual = new Poder();

            listaIntereseccionInteresado.Add(primerPoder);


            if ((listaPoderesA.Count != 0) && (listaPoderesB.Count != 0))
            {
                foreach (Poder poderA in listaPoderesA)
                {
                    foreach (Poder poderB in listaPoderesB)
                    {
                        if (poderA.Id == poderB.Id)
                        {
                            listaIntereseccionInteresado.Add(poderA);
                            retorno = true;
                        }

                    }

                }

                if (listaIntereseccionInteresado.Count != 0)
                {
                    poderActual = (Poder)this._ventana.PoderFiltrado;
                    this._poderesInterseccion = listaIntereseccionInteresado;
                    this._ventana.PoderesFiltrados = listaIntereseccionInteresado;
                    this._ventana.PoderFiltrado = BuscarPoder((IList<Poder>)this._ventana.PoderesFiltrados, poderActual);
                    if (this._ventana.PoderFiltrado == null)
                        this._ventana.Mensaje("El Apoderado no se encuentra registrado en el poder", 1);
                }
                else
                    retorno = false;
            }

            Mouse.OverrideCursor = null;

            return retorno;
        }


        public bool VerificarCambioPoder()
        {
            bool retorno = false;

            if (((Poder)this._ventana.PoderFiltrado).Id != int.MinValue)
                retorno = true;

            return retorno;
        }


        public void LimpiarListaPoder()
        {
            Poder primerPoder = new Poder(int.MinValue);
            IList<Poder> listaPoderes = new List<Poder>();
            listaPoderes.Add(primerPoder);

            this._ventana.PoderesFiltrados = listaPoderes;
            this._ventana.PoderFiltrado = BuscarPoder(listaPoderes, primerPoder);
            this._ventana.Poder = this._ventana.PoderFiltrado;

        }


        public void MensajeListaPoderesVacia()
        {
            Poder primerPoder = new Poder();
            IList<Poder> _poderesCambioDomicilio = new List<Poder>();
            _poderesCambioDomicilio = (IList<Poder>)this._ventana.PoderesFiltrados;
            if (_poderesCambioDomicilio.Count == 1)
            {
                primerPoder = _poderesCambioDomicilio[0];
                if (primerPoder.Id == int.MinValue)
                {
                    this._ventana.Mensaje("El Apoderado no tiene Poderes con el Interesado", 1);
                }
            }
                
        }


        #endregion


        public void IrImprimir(string nombreBoton)
        {
            try
            {
                switch (nombreBoton)
                {
                    case "_btnPlanilla":
                        ImprimirPlanilla();
                        break;
                    case "_btnAnexo":
                        ImprimirAnexo();
                        break;
                    case "_btnCarpeta":
                        ImprimirCarpeta();
                        break;
                    case "_btnPlanillaVan":
                        ImprimirPlanillaVan();
                        break;
                    case "_btnPlanillaVienen":
                        ImprimirPlanillaVienen();
                        break;
                    case "_btnVanDir":
                        ImprimirVanDir();
                        break;


                    default:
                        break;
                }
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
        }


        private void ImprimirPlanillaVienen()
        {
            if (ValidarMarcaAntesDeImprimirCarpeta())
            {
                string paqueteProcedimiento = "PCK_MYP_MDOMICILIOS";
                string procedimiento = "P5";
                ParametroProcedimiento parametro =
                    new ParametroProcedimiento(((CambioDeDomicilio)this._ventana.CambioDeDomicilio).Id, UsuarioLogeado, 1, paqueteProcedimiento, procedimiento);

                this.LlamarProcedimientoDeBaseDeDatos(parametro, Recursos.Etiquetas.btnPlanillaVienen);
            }
        }


        private void ImprimirPlanillaVan()
        {
            if (ValidarMarcaAntesDeImprimirCarpeta())
            {
                string paqueteProcedimiento = "PCK_MYP_MDOMICILIOS";
                string procedimiento = "P4";
                ParametroProcedimiento parametro =
                    new ParametroProcedimiento(((CambioDeDomicilio)this._ventana.CambioDeDomicilio).Id, UsuarioLogeado, 1, paqueteProcedimiento, procedimiento);

                this.LlamarProcedimientoDeBaseDeDatos(parametro, Recursos.Etiquetas.btnPlanillaVan);
            }
        }


        private void ImprimirCarpeta()
        {
            if (ValidarMarcaAntesDeImprimirCarpeta())
            {
                string paqueteProcedimiento = "PCK_MYP_MDOMICILIOS";
                string procedimiento = "P3";
                ParametroProcedimiento parametro =
                    new ParametroProcedimiento(((CambioDeDomicilio)this._ventana.CambioDeDomicilio).Id, UsuarioLogeado, 1, paqueteProcedimiento, procedimiento);

                this.LlamarProcedimientoDeBaseDeDatos(parametro, Recursos.Etiquetas.btnCarpeta);
            }
        }


        private bool ValidarMarcaAntesDeImprimirCarpeta()
        {
            return true;
        }


        private void ImprimirAnexo()
        {
            if (ValidarMarcaAntesDeImprimirCarpeta())
            {
                string paqueteProcedimiento = "PCK_MYP_MDOMICILIOS";
                string procedimiento = "P2";
                ParametroProcedimiento parametro =
                    new ParametroProcedimiento(((CambioDeDomicilio)this._ventana.CambioDeDomicilio).Id, UsuarioLogeado, 1, paqueteProcedimiento, procedimiento);

                this.LlamarProcedimientoDeBaseDeDatos(parametro, Recursos.Etiquetas.btnAnexo);
            }
        }


        private bool ValidarMarcaAntesDeImprimirAnexo()
        {
            return true;
        }


        private void ImprimirPlanilla()
        {
            if (ValidarMarcaAntesDeImprimirCarpeta())
            {
                string paqueteProcedimiento = "PCK_MYP_MDOMICILIOS";
                string procedimiento = "P1";
                ParametroProcedimiento parametro =
                    new ParametroProcedimiento(((CambioDeDomicilio)this._ventana.CambioDeDomicilio).Id, UsuarioLogeado, 1, paqueteProcedimiento, procedimiento);

                this.LlamarProcedimientoDeBaseDeDatos(parametro, Recursos.Etiquetas.btnPlanilla);
            }
        }


        private bool ValidarMarcaAntesDeImprimirPlanilla()
        {
            return true;
        }


        private void ImprimirVanDir()
        {
            if (ValidarMarcaAntesDeImprimirCarpeta())
            {
                string paqueteProcedimiento = "PCK_MYP_MDOMICILIOS";
                string procedimiento = "P6";
                ParametroProcedimiento parametro =
                    new ParametroProcedimiento(((CambioDeDomicilio)this._ventana.CambioDeDomicilio).Id, UsuarioLogeado, 1, paqueteProcedimiento, procedimiento);

                this.LlamarProcedimientoDeBaseDeDatos(parametro, Recursos.Etiquetas.btnPlanilla);
            }
        }


        private bool ValidarMarcaAntesDeImprimirVanDir()
        {
            return true;
        }


        public void IrVentanaAsociado()
        {
            if ((null != (Marca)this._ventana.Marca) && (((Marca)this._ventana.Marca).Asociado != null))
            {
                Asociado asociado = ((Marca)this._ventana.Marca).Asociado.Id != int.MinValue ? ((Marca)this._ventana.Marca).Asociado : null;
                Navegar(new ConsultarAsociado(asociado, this._ventana, false));
            }
        }



        /// <summary>
        /// Metodo que abre el pdf de la planilla 
        /// </summary>
        public void IrVerPlanilla()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                String ruta = ConfigurationManager.AppSettings["RutaPlanillaPDFCambioDomicilioMarca"].ToString() + ((CambioDeDomicilio)this._ventana.CambioDeDomicilio).Id + ".pdf";
                System.Diagnostics.Process.Start(ruta);
                
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Win32Exception ex)
            {
                logger.Error(ex.Message);
                this._ventana.ArchivoNoEncontrado(string.Format(Recursos.MensajesConElUsuario.ErrorPlanillaPdfCambioDomicilioNoEncontrada, ((CambioDeDomicilio)this._ventana.CambioDeDomicilio).Id.ToString()));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado + ": " + ex.Message, true);
            }
        }

        /// <summary>
        /// Metodo que consulta una Cadena de Cambios en un Cambio de Domicilio
        /// </summary>
        public void IrConsultarCadenaDeCambios()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (!this._ventana.IdMarca.Equals(String.Empty))
                {
                    if (!this._ventana.IdCadenaDeCambios.Equals(String.Empty))
                    {
                        CadenaDeCambios cadenaCambiosAux = new CadenaDeCambios();
                        cadenaCambiosAux.TipoCambio = "M";
                        cadenaCambiosAux.CodigoOperacion = ((Marca)this._ventana.Marca).Id;
                        cadenaCambiosAux.Id = int.Parse(this._ventana.IdCadenaDeCambios);
                        IList<CadenaDeCambios> cadenasDeCambios = this._cadenaDeCambiosServicios.ObtenerCadenasCambioFiltro(cadenaCambiosAux);
                        if (cadenasDeCambios.Count > 0)
                        {
                            this.Navegar(new GestionarCadenaDeCambios(cadenasDeCambios[0], this._ventana));
                        }
                        else
                        {
                            this._ventana.Mensaje("La Cadena de Cambios especificada no existe", 0);
                            this._ventana.IdCadenaDeCambios = String.Empty;
                        }
                    }
                    else
                    {
                        Marca marcaCambioDomicilio = (Marca)this._ventana.Marca;
                        this.Navegar(new ListadoCadenasDeCambios(marcaCambioDomicilio, "M", this._ventana));
                    }
                    
                }
                else
                    this._ventana.Mensaje("Debe agregar una Marca para ver sus Cadenas de Cambios asociadas", 0);


                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado + ": " + ex.Message, true);
            }
        }

        /// <summary>
        /// Metodo que consulta la marca seleccionada en el Cambio de Domicilio
        /// </summary>
        public void IrConsultarMarca()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (!this._ventana.IdMarca.Equals(String.Empty))
                {
                    Marca marcaAux = new Marca();
                    marcaAux.Id = int.Parse(this._ventana.IdMarca);
                    IList<Marca> marcas = this._marcaServicios.ObtenerMarcasFiltro(marcaAux);
                    if (marcas.Count > 0)
                    {
                        this.Navegar(new ConsultarMarca(marcas[0], this._ventana));
                    }
                    else
                        this._ventana.Mensaje("La Marca no existe", 0);
                }
                else
                    this._ventana.Mensaje("Debe seleccionar una Marca para poder consultarla", 0);

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado + ": " + ex.Message, true);
            }
        }
    }
}
