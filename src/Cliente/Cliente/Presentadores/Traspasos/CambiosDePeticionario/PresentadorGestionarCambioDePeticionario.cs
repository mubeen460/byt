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
using Trascend.Bolet.Cliente.Contratos.Traspasos.CambiosDePeticionario;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.Cliente.Ventanas.Traspasos.CambiosPeticionario;
using Trascend.Bolet.Cliente.Ventanas.Marcas;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.Cliente.Ventanas.Auditorias;
using Trascend.Bolet.Cliente.Ventanas.Asociados;
using Trascend.Bolet.Cliente.Ventanas.Interesados;
using Trascend.Bolet.Cliente.Ventanas.CadenaDeCambio;

namespace Trascend.Bolet.Cliente.Presentadores.Traspasos.CambiosDePeticionario
{
    class PresentadorGestionarCambioDePeticionario : PresentadorBase
    {
        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private bool _agregar = true;
        private IGestionarCambioDePeticionario _ventana;

        private IMarcaServicios _marcaServicios;
        private IAnaquaServicios _anaquaServicios;
        private IAsociadoServicios _asociadoServicios;
        private IAgenteServicios _agenteServicios;
        private IPoderServicios _poderServicios;
        private IBoletinServicios _boletinServicios;
        private IPaisServicios _paisServicios;
        private IListaDatosDominioServicios _listaDatosDominioServicios;
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
        private ICambioPeticionarioServicios _cambioPeticionarioServicios;
        private IListaDatosValoresServicios _listaDatosValoresServicios;
        private ICadenaDeCambiosServicios _cadenaDeCambiosServicios;
        private IInstruccionCorrespondenciaServicios _instruccionCorrespondenciaServicios;
        private IInstruccionEnvioOriginalesServicios _instruccionEnvioOriginalesServicios;
        private IInstruccionDescuentoServicios _instruccionDescuentoServicios;
        private IInstruccionOtrosServicios _instruccionOtrosServicios;

        private IList<Interesado> _interesadosAnterior;
        private IList<Interesado> _interesadosActual;
        private IList<Agente> _agentesActual;
        private IList<Agente> _agentesAnterior;
        private IList<Marca> _marcas;

        private IList<Poder> _poderesAnterior;
        private IList<Poder> _poderesActual;

        private IList<Poder> _poderesApoderadosAnterior;
        private IList<Poder> _poderesApoderadosActual;

        private IList<Poder> _poderesInterseccionAnterior;
        private IList<Poder> _poderesInterseccionActual;

        /// <summary>
        /// Constructor Predeterminado
        /// </summary>
        /// <param name="ventana">página que satisface el contrato</param>
        public PresentadorGestionarCambioDePeticionario(IGestionarCambioDePeticionario ventana, object cambioPeticionario)
        {
            try
            {

                this._ventana = ventana;

                if (cambioPeticionario != null)
                {
                    this._ventana.CambioPeticionario = cambioPeticionario;
                    _agregar = false;
                    CambiarAModificar();
                }
                else
                {
                    CambioPeticionario cambioPeticionarioAgregar = new CambioPeticionario();
                    this._ventana.CambioPeticionario = cambioPeticionarioAgregar;

                    ((CambioPeticionario)this._ventana.CambioPeticionario).FechaPeticionario = DateTime.Now;
                    this._ventana.Marca = null;
                    this._ventana.PoderAnterior = null;
                    this._ventana.PoderActual = null;
                    this._ventana.InteresadoAnterior = null;
                    this._ventana.InteresadoActual = null;
                    this._ventana.ApoderadoAnterior = null;
                    this._ventana.ApoderadoActual = null;
                    this._ventana.Boletin = null;

                    CambiarAModificar();

                    this._ventana.TextoBotonRegresar = Recursos.Etiquetas.btnCancelar;

                    this._ventana.ActivarControlesAlAgregar();

                }

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
                this._cambioPeticionarioServicios = (ICambioPeticionarioServicios)Activator.GetObject(typeof(ICambioPeticionarioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CambioPeticionarioServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);
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

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }



        /// <summary>
        /// Constructor Predeterminado sobrecargado donde se pasa la ventana Padre desde donde se esta llamando el proceso
        /// </summary>
        /// <param name="ventana">página que satisface el contrato</param>
        public PresentadorGestionarCambioDePeticionario(IGestionarCambioDePeticionario ventana, object cambioPeticionario, object ventanaPadre)
        {
            try
            {

                this._ventana = ventana;

                this._ventanaPadre = ventanaPadre;

                if (cambioPeticionario != null)
                {
                    this._ventana.CambioPeticionario = cambioPeticionario;
                    _agregar = false;
                    CambiarAModificar();
                }
                else
                {
                    CambioPeticionario cambioPeticionarioAgregar = new CambioPeticionario();
                    this._ventana.CambioPeticionario = cambioPeticionarioAgregar;

                    ((CambioPeticionario)this._ventana.CambioPeticionario).FechaPeticionario = DateTime.Now;
                    this._ventana.Marca = null;
                    this._ventana.PoderAnterior = null;
                    this._ventana.PoderActual = null;
                    this._ventana.InteresadoAnterior = null;
                    this._ventana.InteresadoActual = null;
                    this._ventana.ApoderadoAnterior = null;
                    this._ventana.ApoderadoActual = null;
                    this._ventana.Boletin = null;

                    CambiarAModificar();

                    this._ventana.TextoBotonRegresar = Recursos.Etiquetas.btnCancelar;

                    this._ventana.ActivarControlesAlAgregar();

                }

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
                this._cambioPeticionarioServicios = (ICambioPeticionarioServicios)Activator.GetObject(typeof(ICambioPeticionarioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CambioPeticionarioServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);
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
                this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleAgregarCambioPeticionario,
                Recursos.Ids.GestionarCambioPeticionario);
            else
                this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleGestionarCambioPeticionario,
                Recursos.Ids.GestionarCambioPeticionario);
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

                    CambioPeticionario cesion = (CambioPeticionario)this._ventana.CambioPeticionario;

                    this._ventana.Marca = this._marcaServicios.ConsultarMarcaConTodo(cesion.Marca);

                    this._ventana.NombreMarca = ((Marca)this._ventana.Marca).Descripcion;

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

                    this._ventana.ApoderadoAnterior = cesion.AgenteAnterior;
                    this._ventana.ApoderadoActual = cesion.AgenteActual;
                    this._ventana.PoderAnterior = cesion.PoderAnterior;
                    this._ventana.PoderActual = cesion.PoderActual;

                    CargaBoletines();

                    this._ventana.Boletin = this.BuscarBoletin((IList<Boletin>)this._ventana.Boletines, cesion.BoletinPublicacion);

                    CargarMarca();

                    CargarInteresado("Anterior");

                    CargarApoderado("Anterior");

                    CargarPoder("Anterior");

                    CargarInteresado("Actual");

                    CargarApoderado("Actual");

                    CargarPoder("Actual");

                    LlenarListasPoderes((CambioPeticionario)this._ventana.CambioPeticionario);

                    ValidarAnterior();

                    ValidarActual();

                    this._ventana.IdCadenaDeCambios = cesion.CadenaDeCambios.ToString();

                }
                else
                {
                    CargarMarca();

                    CargarInteresado("Anterior");

                    CargarApoderado("Anterior");

                    CargarPoder("Anterior");

                    CargarInteresado("Actual");

                    CargarApoderado("Actual");

                    CargarPoder("Actual");

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

        private void CargaBoletines()
        {
            Boletin primerBoletin = new Boletin(int.MinValue);
            IList<Boletin> boletines = this._boletinServicios.ConsultarTodos();
            boletines.Insert(0, primerBoletin);
            this._ventana.Boletines = boletines;

        }

        private void CargarInteresado(string tipo)
        {
            Interesado primerInteresado = new Interesado(int.MinValue);

            if (tipo.Equals("Anterior"))
            {
                this._interesadosAnterior = new List<Interesado>();

                this._interesadosAnterior.Add(primerInteresado);

                if (((CambioPeticionario)this._ventana.CambioPeticionario).InteresadoAnterior != null)
                {
                    this._ventana.InteresadoAnterior = this._interesadoServicios.ConsultarInteresadoConTodo(((CambioPeticionario)this._ventana.CambioPeticionario).InteresadoAnterior);
                    this._ventana.NombreAnterior = ((Interesado)this._ventana.InteresadoAnterior).Nombre;
                    this._ventana.IdAnterior = ((Interesado)this._ventana.InteresadoAnterior).Id.ToString();

                    if ((Interesado)this._ventana.InteresadoAnterior != null)
                    {
                        this._interesadosAnterior.Add((Interesado)this._ventana.InteresadoAnterior);
                        this._ventana.AnteriorsFiltrados = this._interesadosAnterior;
                        this._ventana.AnteriorFiltrado = this.BuscarInteresado((IList<Interesado>)this._ventana.AnteriorsFiltrados, (Interesado)this._ventana.InteresadoAnterior);
                        this._ventana.IdAnterior = ((Interesado)this._ventana.InteresadoAnterior).Id.ToString();
                    }
                }
                else
                {
                    this._ventana.InteresadoAnterior = primerInteresado;
                    this._ventana.AnteriorsFiltrados = this._interesadosAnterior;
                    this._ventana.AnteriorFiltrado = primerInteresado;
                    this._ventana.IdAnterior = primerInteresado.Id.ToString();

                }
            }
            else if (tipo.Equals("Actual"))
            {
                this._interesadosActual = new List<Interesado>();
                this._interesadosActual.Add(primerInteresado);

                if (((CambioPeticionario)this._ventana.CambioPeticionario).InteresadoActual != null)
                {
                    this._ventana.InteresadoActual = this._interesadoServicios.ConsultarInteresadoConTodo(((CambioPeticionario)this._ventana.CambioPeticionario).InteresadoActual);
                    this._ventana.NombreActual = ((Interesado)this._ventana.InteresadoActual).Nombre;
                    this._ventana.IdActual = ((Interesado)this._ventana.InteresadoActual).Id.ToString();

                    if ((Interesado)this._ventana.InteresadoActual != null)
                    {
                        this._interesadosActual.Add((Interesado)this._ventana.InteresadoActual);
                        this._ventana.ActualsFiltrados = this._interesadosActual;
                        this._ventana.ActualFiltrado = this.BuscarInteresado((IList<Interesado>)this._ventana.ActualsFiltrados, (Interesado)this._ventana.InteresadoActual);
                        this._ventana.IdActual = ((Interesado)this._ventana.InteresadoActual).Id.ToString();
                    }
                }
                else
                {
                    this._ventana.InteresadoActual = primerInteresado;
                    this._ventana.ActualsFiltrados = this._interesadosActual;
                    this._ventana.ActualFiltrado = primerInteresado;
                    this._ventana.IdActual = primerInteresado.Id.ToString();
                }
            }
        }

        private void CargarApoderado(string tipo)
        {
            Agente primerAgente = new Agente("");

            if (tipo.Equals("Anterior"))
            {
                this._agentesAnterior = new List<Agente>();
                this._agentesAnterior.Add(primerAgente);

                if (((CambioPeticionario)this._ventana.CambioPeticionario).AgenteAnterior != null)
                {
                    this._agentesAnterior.Add((Agente)this._ventana.ApoderadoAnterior);
                    this._ventana.ApoderadosAnteriorFiltrados = this._agentesAnterior;
                    this._ventana.ApoderadoAnteriorFiltrado = this.BuscarAgente((IList<Agente>)this._ventana.ApoderadosAnteriorFiltrados, (Agente)this._ventana.ApoderadoAnterior);
                    this._ventana.IdApoderadoAnterior = ((Agente)this._ventana.ApoderadoAnterior).Id;
                }
                else
                {
                    this._ventana.ApoderadoAnterior = primerAgente;
                    this._ventana.ApoderadosAnteriorFiltrados = this._agentesAnterior;
                    this._ventana.ApoderadoAnteriorFiltrado = primerAgente;
                    this._ventana.IdApoderadoAnterior = primerAgente.Id;
                }
            }
            else if (tipo.Equals("Actual"))
            {
                this._agentesActual = new List<Agente>();
                this._agentesActual.Add(primerAgente);

                if (((CambioPeticionario)this._ventana.CambioPeticionario).AgenteActual != null)
                {
                    this._agentesActual.Add((Agente)this._ventana.ApoderadoActual);
                    this._ventana.ApoderadosActualFiltrados = this._agentesActual;
                    this._ventana.ApoderadoActualFiltrado = this.BuscarAgente((IList<Agente>)this._ventana.ApoderadosActualFiltrados, (Agente)this._ventana.ApoderadoActual);
                    this._ventana.IdApoderadoActual = ((Agente)this._ventana.ApoderadoActual).Id;
                }
                else
                {
                    this._ventana.ApoderadoActual = primerAgente;
                    this._ventana.ApoderadosActualFiltrados = this._agentesActual;
                    this._ventana.ApoderadoActualFiltrado = primerAgente;
                    this._ventana.IdApoderadoActual = primerAgente.Id;
                }
            }
        }

        private void CargarPoder(string tipo)
        {
            Poder primerPoder = new Poder(int.MinValue);

            if (tipo.Equals("Anterior"))
            {
                this._poderesAnterior = new List<Poder>();
                this._poderesAnterior.Add(primerPoder);

                if (((CambioPeticionario)this._ventana.CambioPeticionario).PoderAnterior != null)
                {
                    this._poderesAnterior.Add((Poder)this._ventana.PoderAnterior);
                    this._ventana.PoderesAnteriorFiltrados = this._poderesAnterior;
                    this._ventana.PoderAnteriorFiltrado = this.BuscarPoder((IList<Poder>)this._ventana.PoderesAnteriorFiltrados, (Poder)this._ventana.PoderAnterior);
                }
                else
                {
                    this._ventana.PoderesAnteriorFiltrados = this._poderesAnterior;
                    this._ventana.PoderAnteriorFiltrado = this.BuscarPoder(this._poderesAnterior, this._poderesAnterior[0]);
                }
            }
            else if (tipo.Equals("Actual"))
            {
                this._poderesActual = new List<Poder>();
                this._poderesActual.Add(primerPoder);

                if (((CambioPeticionario)this._ventana.CambioPeticionario).PoderActual != null)
                {
                    this._poderesActual.Add((Poder)this._ventana.PoderActual);
                    this._ventana.PoderesActualFiltrados = this._poderesActual;
                    this._ventana.PoderActualFiltrado = this.BuscarPoder((IList<Poder>)this._ventana.PoderesActualFiltrados, (Poder)this._ventana.PoderActual);
                }
                else
                {
                    this._ventana.PoderesActualFiltrados = this._poderesActual;
                    this._ventana.PoderActualFiltrado = this.BuscarPoder(this._poderesActual, this._poderesActual[0]);
                }
            }
        }

        public CambioPeticionario CargarCambioPeticionarioDeLaPantalla()
        {

            CambioPeticionario cambioPeticionario = (CambioPeticionario)this._ventana.CambioPeticionario;

            if ((null != this._ventana.MarcaFiltrada) && (((Marca)this._ventana.MarcaFiltrada).Id != int.MinValue))
            {
                cambioPeticionario.Marca = (Marca)this._ventana.MarcaFiltrada;
                cambioPeticionario.InteresadoAnterior = ((Marca)this._ventana.MarcaFiltrada).Interesado;
                cambioPeticionario.AgenteAnterior = ((Marca)this._ventana.MarcaFiltrada).Agente;
                cambioPeticionario.PoderAnterior = ((Marca)this._ventana.MarcaFiltrada).Poder;
            }

            if (null != this._ventana.InteresadoAnterior)
                cambioPeticionario.InteresadoAnterior = ((Interesado)this._ventana.InteresadoAnterior).Id != int.MinValue ?
                                                                        (Interesado)this._ventana.InteresadoAnterior : null;

            if (null != this._ventana.InteresadoActual)
                cambioPeticionario.InteresadoActual = ((Interesado)this._ventana.InteresadoActual).Id != int.MinValue ?
                                                                        (Interesado)this._ventana.InteresadoActual : null;

            if (null != this._ventana.ApoderadoActual)
                cambioPeticionario.AgenteActual = !((Agente)this._ventana.ApoderadoActual).Id.Equals("") ?
                                                                (Agente)this._ventana.ApoderadoActual : null;

            if (null != this._ventana.ApoderadoAnterior)
                cambioPeticionario.AgenteAnterior = !((Agente)this._ventana.ApoderadoAnterior).Id.Equals("") ?
                                                                (Agente)this._ventana.ApoderadoAnterior : null;

            if (null != this._ventana.PoderActual)
                cambioPeticionario.PoderActual = ((Poder)this._ventana.PoderActual).Id != int.MinValue ?
                                                                    (Poder)this._ventana.PoderActual : null;

            if (null != this._ventana.PoderAnterior)
                cambioPeticionario.PoderAnterior = ((Poder)this._ventana.PoderAnterior).Id != int.MinValue ?
                                                                    (Poder)this._ventana.PoderAnterior : null;

            if (null != this._ventana.Boletin)
                cambioPeticionario.BoletinPublicacion = ((Boletin)this._ventana.Boletin).Id != int.MinValue ?
                                                                            (Boletin)this._ventana.Boletin : null;

            if ((null != this._ventana.IdCadenaDeCambios) && (!this._ventana.IdCadenaDeCambios.Equals(String.Empty)))
            {
                cambioPeticionario.CadenaDeCambios = int.Parse(this._ventana.IdCadenaDeCambios);
            }
            else
                cambioPeticionario.CadenaDeCambios = null;


            return cambioPeticionario;
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

                //Modifica o inserta los datos del Cambio Peticionario
                else if (this._ventana.TextoBotonModificar == Recursos.Etiquetas.btnAceptar)
                {
                    CambioPeticionario cambioPeticionario = CargarCambioPeticionarioDeLaPantalla();

                    cambioPeticionario.Marca = (Marca)this._ventana.Marca;

                    if (null != cambioPeticionario.Marca)
                    {

                        #region Preparando la Marca del Cambio de Domicilio para el Hibrido

                        distingueMarca = cambioPeticionario.Marca.XDistingue;
                        cambioPeticionario.Marca.Distingue = distingueMarca;
                        cambioPeticionario.Marca.XDistingue = String.Empty;

                        #endregion 

                        cambioPeticionario.Marca.InfoBoles =
                            this._infoBolServicios.ConsultarInfoBolesPorMarca(cambioPeticionario.Marca);
                        cambioPeticionario.Marca.Operaciones =
                            this._operacionServicios.ConsultarOperacionesPorMarca(cambioPeticionario.Marca);
                        cambioPeticionario.Marca.Busquedas =
                            this._busquedaServicios.ConsultarBusquedasPorMarca(cambioPeticionario.Marca);

                        if (null != cambioPeticionario.Marca.InfoAdicional)
                            cambioPeticionario.Marca.InfoAdicional =
                                this._infoAdicionalServicios.ConsultarPorId(cambioPeticionario.Marca.InfoAdicional);
                        if (null != cambioPeticionario.Marca.Anaqua)
                            cambioPeticionario.Marca.Anaqua =
                                this._anaquaServicios.ConsultarPorId(cambioPeticionario.Marca.Anaqua);

                        if (null != cambioPeticionario.InteresadoActual)
                        {
                            int? exitoso =
                                this._cambioPeticionarioServicios.InsertarOModificarCambioPeticionario(
                                    cambioPeticionario, UsuarioLogeado.Hash);
                            if ((!exitoso.Equals(null)) && (this._agregar == false))
                            {
                                this._marcaServicios.ActualizarDistingueDeMarca(cambioPeticionario.Marca, distingueMarca);
                                this._ventana.HabilitarCampos = false;
                                this._ventana.TextoBotonModificar = Recursos.Etiquetas.btnModificar;
                            }
                            else if ((!exitoso.Equals(null)) && (this._agregar == true))
                            {
                                cambioPeticionario.Id = exitoso.Value;
                                this._marcaServicios.ActualizarDistingueDeMarca(cambioPeticionario.Marca, distingueMarca);
                                this.Navegar(new GestionarCambioPeticionario(cambioPeticionario));
                            }
                            else
                                this.Navegar(Recursos.MensajesConElUsuario.ErrorAlGenerarTraspaso, true);
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
        /// Metodo que se encarga de eliminar una Marca
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

                if (this._cambioPeticionarioServicios.Eliminar((CambioPeticionario)this._ventana.CambioPeticionario, UsuarioLogeado.Hash))
                {
                    _paginaPrincipal.MensajeUsuario = Recursos.MensajesConElUsuario.CambioPeticionarioEliminado;
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

        public void LlenarListasPoderes(CambioPeticionario cesion)
        {
            //NOTA: Por cuestiones de no perder el hilo de la programacion, el parametro que se le esta pasando al metodo se llama cesion
            //pero, el tipo del parametro es CambioPeticionario

            if (cesion.InteresadoAnterior != null)
                this._poderesAnterior = this._poderServicios.ConsultarPoderesPorInteresado(cesion.InteresadoAnterior);

            if (cesion.InteresadoActual != null)
                this._poderesActual = this._poderServicios.ConsultarPoderesPorInteresado(cesion.InteresadoActual);

            if (cesion.AgenteAnterior != null)
                //this._poderesApoderadosAnterior = this._poderServicios.ConsultarPoderesPorAgente(cesion.AgenteAnterior);
                this._poderesApoderadosAnterior = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado(cesion.AgenteAnterior, cesion.InteresadoAnterior);

            if (cesion.AgenteActual != null)
                //this._poderesApoderadosActual = this._poderServicios.ConsultarPoderesPorAgente(cesion.AgenteActual);
                this._poderesApoderadosActual = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado(cesion.AgenteActual, cesion.InteresadoActual);
        }

        public bool ValidarListaDePoderes(IList<Poder> listaPoderesA, IList<Poder> listaPoderesB, string tipo)
        {

            bool retorno = false;
            IList<Poder> listaIntereseccionAnterior = new List<Poder>();
            IList<Poder> listaIntereseccionActual = new List<Poder>();
            Poder primerPoder = new Poder(int.MinValue);

            Poder poderActual = new Poder();

            listaIntereseccionAnterior.Add(primerPoder);
            listaIntereseccionActual.Add(primerPoder);

            if ((listaPoderesA.Count != 0) && (listaPoderesB.Count != 0))
            {
                foreach (Poder poderA in listaPoderesA)
                {
                    foreach (Poder poderB in listaPoderesB)
                    {
                        if ((poderA.Id == poderB.Id) && (tipo.Equals("Anterior")))
                        {
                            listaIntereseccionAnterior.Add(poderA);
                            retorno = true;
                        }

                        else if ((poderA.Id == poderB.Id) && (tipo.Equals("Actual")))
                        {
                            listaIntereseccionActual.Add(poderA);
                            retorno = true;
                        }
                    }

                }

                if ((listaIntereseccionAnterior.Count != 0) && (tipo.Equals("Anterior")))
                {
                    poderActual = (Poder)this._ventana.PoderAnteriorFiltrado;
                    this._poderesInterseccionAnterior = listaIntereseccionAnterior;
                    this._ventana.PoderesAnteriorFiltrados = listaIntereseccionAnterior;
                    this._ventana.PoderAnteriorFiltrado = BuscarPoder((IList<Poder>)this._ventana.PoderesAnteriorFiltrados, poderActual);
                }


                else if ((listaIntereseccionActual.Count != 0) && (tipo.Equals("Actual")))
                {
                    poderActual = (Poder)this._ventana.PoderActualFiltrado;
                    this._poderesInterseccionActual = listaIntereseccionActual;
                    this._ventana.PoderesActualFiltrados = listaIntereseccionActual;
                    this._ventana.PoderActualFiltrado = BuscarPoder((IList<Poder>)this._ventana.PoderesActualFiltrados, poderActual);
                }

                else
                    retorno = false;
            }

            return retorno;
        }

        public void LlenarListaAgenteEInteresado(Poder poder, string tipo, bool cargaInicial)
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Interesado interesado = new Interesado();
                IList<Agente> agentesInteresadoFiltrados;
                IList<Interesado> interesadosFiltrados = new List<Interesado>();
                Poder poderFiltrar = new Poder();

                Interesado primerInteresado = new Interesado(int.MinValue);
                Agente primerAgente = new Agente("");

                Agente agenteActual = new Agente();

                agentesInteresadoFiltrados = new List<Agente>();

                if (tipo.Equals("Anterior"))
                {
                    if (poder.Id == null)
                        poderFiltrar.Id = this._ventana.IdPoderAnteriorFiltrar.Equals("") ? 0 : int.Parse(this._ventana.IdPoderAnteriorFiltrar);
                    else
                        poderFiltrar.Id = poder.Id;

                    if (poderFiltrar.Id != 0)
                    {
                        interesado = this._interesadoServicios.ObtenerInteresadosDeUnPoder((Poder)this._ventana.PoderAnteriorFiltrado);
                        agentesInteresadoFiltrados = this._agenteServicios.ObtenerAgentesDeUnPoder((Poder)this._ventana.PoderAnteriorFiltrado);
                    }

                    if (interesado != null)
                    {
                        interesadosFiltrados.Insert(0, primerInteresado);
                        interesadosFiltrados.Add(interesado);
                        this._ventana.AnteriorsFiltrados = interesadosFiltrados;

                        if (cargaInicial)
                            this._ventana.AnteriorFiltrado = this.BuscarInteresado(interesadosFiltrados, interesado);
                        else
                            this._ventana.AnteriorFiltrado = primerInteresado;
                    }
                    else
                    {
                        interesadosFiltrados.Insert(0, primerInteresado);
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                        this._ventana.AnteriorFiltrado = primerInteresado;
                    }

                    if (agentesInteresadoFiltrados.Count != 0)
                    {
                        agenteActual = (Agente)this._ventana.ApoderadoAnteriorFiltrado;
                        agentesInteresadoFiltrados.Insert(0, primerAgente);
                        this._ventana.ApoderadosAnteriorFiltrados = agentesInteresadoFiltrados;
                        this._ventana.ApoderadoAnteriorFiltrado = BuscarAgente(agentesInteresadoFiltrados, agenteActual);
                    }
                    else
                    {
                        agentesInteresadoFiltrados.Insert(0, primerAgente);
                        this._ventana.ApoderadosAnteriorFiltrados = this._agentesAnterior;
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                        this._ventana.ApoderadoAnteriorFiltrado = primerAgente;
                    }
                }
                else if (tipo.Equals("Actual"))
                {
                    if (poder.Id == null)
                        poderFiltrar.Id = this._ventana.IdPoderActualFiltrar.Equals("") ? 0 : int.Parse(this._ventana.IdPoderActualFiltrar);
                    else
                        poderFiltrar.Id = poder.Id;

                    if (poderFiltrar.Id != 0)
                    {
                        interesado = this._interesadoServicios.ObtenerInteresadosDeUnPoder((Poder)this._ventana.PoderActualFiltrado);
                        agentesInteresadoFiltrados = this._agenteServicios.ObtenerAgentesDeUnPoder((Poder)this._ventana.PoderActualFiltrado);
                    }

                    if (interesado != null)
                    {
                        interesadosFiltrados.Insert(0, primerInteresado);
                        interesadosFiltrados.Add(interesado);
                        this._ventana.ActualsFiltrados = interesadosFiltrados;

                        if (cargaInicial)
                            this._ventana.ActualFiltrado = this.BuscarInteresado(interesadosFiltrados, interesado);
                        else
                            this._ventana.ActualFiltrado = primerInteresado;
                    }
                    else
                    {
                        interesadosFiltrados.Insert(0, primerInteresado);
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                        this._ventana.ActualFiltrado = primerInteresado;
                    }

                    if (agentesInteresadoFiltrados.Count != 0)
                    {
                        agenteActual = (Agente)this._ventana.ApoderadoActualFiltrado;
                        agentesInteresadoFiltrados.Insert(0, primerAgente);
                        this._ventana.ApoderadosActualFiltrados = agentesInteresadoFiltrados;
                        this._ventana.ApoderadoActualFiltrado = BuscarAgente(agentesInteresadoFiltrados, agenteActual);
                    }
                    else
                    {
                        agentesInteresadoFiltrados.Insert(0, primerAgente);
                        this._ventana.ApoderadosActualFiltrados = this._agentesActual;
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                        this._ventana.ApoderadoActualFiltrado = primerAgente;
                    }
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

        }

        private void LlenarListaAgente(Poder poder, string tipo)
        {
            Agente primerAgente = new Agente("");

            if (tipo.Equals("Anterior"))
            {
                this._agentesAnterior = this._agenteServicios.ObtenerAgentesDeUnPoder(poder);
                this._agentesAnterior.Insert(0, primerAgente);
                this._ventana.ApoderadosAnteriorFiltrados = this._agentesAnterior;
                this._ventana.ApoderadoAnteriorFiltrado = primerAgente;
            }
            else if (tipo.Equals("Actual"))
            {
                this._agentesActual = this._agenteServicios.ObtenerAgentesDeUnPoder(poder);
                this._agentesActual.Insert(0, primerAgente);
                this._ventana.ApoderadosActualFiltrados = this._agentesActual;
                this._ventana.ApoderadoActualFiltrado = primerAgente;
            }
        }

        public bool VerificarCambioInteresado(string tipo)
        {
            bool retorno = false;

            if (tipo.Equals("Anterior"))
            {
                if ((((Interesado)this._ventana.AnteriorFiltrado).Id != int.MinValue) || !(((Agente)this._ventana.ApoderadoAnteriorFiltrado).Id.Equals("")))
                    retorno = true;
            }
            if (tipo.Equals("Actual"))
            {
                if ((((Interesado)this._ventana.ActualFiltrado).Id != int.MinValue) || !(((Agente)this._ventana.ApoderadoActualFiltrado).Id.Equals("")))
                    retorno = true;
            }

            return retorno;
        }

        public bool VerificarCambioAgente(string tipo)
        {
            bool retorno = false;

            if (tipo.Equals("Anterior"))
            {
                if (!(((Agente)this._ventana.ApoderadoAnteriorFiltrado).Id.Equals("")) || (((Interesado)this._ventana.AnteriorFiltrado).Id != int.MinValue))
                    retorno = true;
            }
            if (tipo.Equals("Actual"))
            {
                if (!(((Agente)this._ventana.ApoderadoAnteriorFiltrado).Id.Equals("")) || (((Interesado)this._ventana.ActualFiltrado).Id != int.MinValue))
                    retorno = true;
            }

            return retorno;
        }

        public bool VerificarCambioPoder(string tipo)
        {
            if (tipo.Equals("Anterior"))
            {
                if (((Poder)this._ventana.PoderAnteriorFiltrado).Id != int.MinValue)
                    return true;
            }
            if (tipo.Equals("Actual"))
            {
                if (((Poder)this._ventana.PoderActualFiltrado).Id != int.MinValue)
                    return true;
            }

            return false;
        }

        public void LimpiarListaInteresado(string tipo)
        {
            Interesado primerInteresado = new Interesado(int.MinValue);
            IList<Interesado> listaInteresados = new List<Interesado>();
            listaInteresados.Add(primerInteresado);

            if (tipo.Equals("Anterior"))
            {
                this._ventana.AnteriorsFiltrados = listaInteresados;
                this._ventana.AnteriorFiltrado = BuscarInteresado(listaInteresados, primerInteresado);
                this._ventana.InteresadoAnterior = this._ventana.AnteriorFiltrado;
            }
            else if (tipo.Equals("Actual"))
            {
                this._ventana.ActualsFiltrados = listaInteresados;
                this._ventana.ActualFiltrado = BuscarInteresado(listaInteresados, primerInteresado);
                this._ventana.InteresadoActual = this._ventana.ActualFiltrado;
            }
        }

        public void LimpiarListaAgente(string tipo)
        {
            Agente primerAgente = new Agente("");
            IList<Agente> listaAgentes = new List<Agente>();
            listaAgentes.Add(primerAgente);

            if (tipo.Equals("Anterior"))
            {
                this._ventana.ApoderadosAnteriorFiltrados = listaAgentes;
                this._ventana.ApoderadoAnteriorFiltrado = BuscarAgente(listaAgentes, primerAgente);
                this._ventana.ApoderadoAnterior = this._ventana.ApoderadoAnteriorFiltrado;
            }
            else if (tipo.Equals("Actual"))
            {
                this._ventana.ApoderadosActualFiltrados = listaAgentes;
                this._ventana.ApoderadoActualFiltrado = BuscarAgente(listaAgentes, primerAgente);
                this._ventana.ApoderadoActual = this._ventana.ApoderadoActualFiltrado;
            }
        }

        public void LimpiarListaPoder(string tipo)
        {
            Poder primerPoder = new Poder(int.MinValue);
            IList<Poder> listaPoderes = new List<Poder>();
            listaPoderes.Add(primerPoder);

            if (tipo.Equals("Anterior"))
            {
                this._ventana.PoderesAnteriorFiltrados = listaPoderes;
                this._ventana.PoderAnteriorFiltrado = BuscarPoder(listaPoderes, primerPoder);
                this._ventana.PoderAnterior = this._ventana.PoderAnteriorFiltrado;
            }
            else if (tipo.Equals("Actual"))
            {
                this._ventana.PoderesActualFiltrados = listaPoderes;
                this._ventana.PoderActualFiltrado = BuscarPoder(listaPoderes, primerPoder);
                this._ventana.PoderActual = this._ventana.PoderActualFiltrado;
            }
        }


        public void IrVentanaAsociado()
        {
            if ((null != (Marca)this._ventana.Marca) && (((Marca)this._ventana.Marca).Asociado != null))
            {
                Asociado asociado = ((Marca)this._ventana.Marca).Asociado.Id != int.MinValue ? ((Marca)this._ventana.Marca).Asociado : null;
                Navegar(new ConsultarAsociado(asociado, this._ventana, false));
            }
        }

        #region Marca

        public void IrConsultarMarcas()
        {
            this.Navegar(new ConsultarMarcas());
        }

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
                    this._ventana.InteresadoAnterior = ((Marca)this._ventana.Marca).Interesado;

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

                    if (((Marca)this._ventana.Marca).Interesado != null)
                        this._ventana.IdAnterior = (((Marca)this._ventana.Marca).Interesado).Id.ToString();

                    IList<Interesado> listaAux = new List<Interesado>();
                    listaAux.Add(new Interesado(int.MinValue));

                    if (((Marca)this._ventana.MarcaFiltrada).Id != int.MinValue)
                    {
                        listaAux.Add((Interesado)this._ventana.InteresadoAnterior);

                        this._ventana.AnteriorsFiltrados = listaAux;
                        this._ventana.AnteriorFiltrado = this.BuscarInteresado((IList<Interesado>)this._ventana.AnteriorsFiltrados,
                            (Interesado)this._ventana.InteresadoAnterior);
                    }
                    else
                    {
                        this._ventana.AnteriorsFiltrados = listaAux;
                        this._ventana.IdAnterior = int.MinValue.ToString();
                        this._ventana.AnteriorFiltrado = this.BuscarInteresado((IList<Interesado>)this._ventana.AnteriorsFiltrados,
                            new Interesado(int.MinValue));
                    }

                    this._ventana.ApoderadoAnterior = ((Marca)this._ventana.Marca).Agente;

                    if (((Marca)this._ventana.Marca).Agente == null)
                        this._ventana.IdApoderadoAnterior = "";
                    else
                        this._ventana.IdApoderadoAnterior = (((Marca)this._ventana.Marca).Agente).Id;

                    this._ventana.PoderAnterior = ((Marca)this._ventana.Marca).Poder;


                    if (null != ((Marca)this._ventana.Marca).Asociado)
                        this._ventana.PintarAsociado(((Marca)this._ventana.Marca).Asociado.TipoCliente.Id);
                    else
                        this._ventana.PintarAsociado("5");

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


                    retorno = true;
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

        #region Anterior

        private void ValidarAnterior()
        {
            if (((Interesado)this._ventana.AnteriorFiltrado).Id == int.MinValue)
            {
                if (((Agente)this._ventana.ApoderadoAnteriorFiltrado).Id.Equals(""))
                {
                    if (((Poder)this._ventana.PoderAnteriorFiltrado).Id != int.MinValue)
                    {
                        LlenarListaAgenteEInteresado((Poder)this._ventana.PoderAnterior, "Anterior", true);

                        //this._ventana.GestionarBotonConsultarInteresados("Anterior", false);
                        //this._ventana.GestionarBotonConsultarApoderados("Anterior", false);
                        this._ventana.GestionarBotonConsultarInteresados("Anterior", true);
                        this._ventana.GestionarBotonConsultarApoderados("Anterior", true);
                    }
                }
                else
                {
                    if (((Poder)this._ventana.PoderAnteriorFiltrado).Id == int.MinValue)
                        //this._ventana.GestionarBotonConsultarInteresados("Anterior", false);
                        this._ventana.GestionarBotonConsultarInteresados("Anterior", true);

                    else
                    {
                        LlenarListaAgenteEInteresado((Poder)this._ventana.PoderAnterior, "Anterior", true);

                        //this._ventana.GestionarBotonConsultarInteresados("Anterior", false);
                        //this._ventana.GestionarBotonConsultarApoderados("Anterior", false);
                        //this._ventana.GestionarBotonConsultarPoderes("Anterior", false);
                        this._ventana.GestionarBotonConsultarInteresados("Anterior", true);
                        this._ventana.GestionarBotonConsultarApoderados("Anterior", true);
                        this._ventana.GestionarBotonConsultarPoderes("Anterior", true);
                    }

                }
            }
            else
            {
                if (((Agente)this._ventana.ApoderadoAnteriorFiltrado).Id.Equals(""))
                {
                    if (((Poder)this._ventana.PoderAnteriorFiltrado).Id == int.MinValue)
                        //this._ventana.GestionarBotonConsultarPoderes("Anterior", false);
                        this._ventana.GestionarBotonConsultarPoderes("Anterior", true);

                    else
                    {
                        LlenarListaAgenteEInteresado((Poder)this._ventana.PoderAnterior, "Anterior", true);

                        //this._ventana.GestionarBotonConsultarInteresados("Anterior", false);
                        //this._ventana.GestionarBotonConsultarApoderados("Anterior", false);
                        //this._ventana.GestionarBotonConsultarPoderes("Anterior", false);
                        this._ventana.GestionarBotonConsultarInteresados("Anterior", true);
                        this._ventana.GestionarBotonConsultarApoderados("Anterior", true);
                        this._ventana.GestionarBotonConsultarPoderes("Anterior", true);

                    }
                }
                else
                {
                    if (((Poder)this._ventana.PoderAnteriorFiltrado).Id == int.MinValue)
                    {
                        ValidarListaDePoderes(this._poderesAnterior, this._poderesApoderadosAnterior, "Anterior");

                        //this._ventana.GestionarBotonConsultarPoderes("Anterior", false);
                        this._ventana.GestionarBotonConsultarPoderes("Anterior", true);
                    }
                    else
                    {
                        LlenarListaAgenteEInteresado((Poder)this._ventana.PoderAnterior, "Anterior", true);
                        ValidarListaDePoderes(this._poderesAnterior, this._poderesApoderadosAnterior, "Anterior");

                        //this._ventana.GestionarBotonConsultarInteresados("Anterior", false);
                        //this._ventana.GestionarBotonConsultarApoderados("Anterior", false);
                        //this._ventana.GestionarBotonConsultarPoderes("Anterior", false);
                        this._ventana.GestionarBotonConsultarInteresados("Anterior", true);
                        this._ventana.GestionarBotonConsultarApoderados("Anterior", true);
                        this._ventana.GestionarBotonConsultarPoderes("Anterior", true);
                    }
                }
            }
        }

        public void ConsultarAnteriors()
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
                interesado.Nombre = this._ventana.NombreAnteriorFiltrar.ToUpper();
                interesado.Id = this._ventana.IdAnteriorFiltrar.Equals("") ? 0 : int.Parse(this._ventana.IdAnteriorFiltrar);

                if ((!interesado.Nombre.Equals("")) || (interesado.Id != 0))
                    interesadosFiltrados = this._interesadoServicios.ObtenerInteresadosFiltro(interesado);
                else
                    interesadosFiltrados = new List<Interesado>();

                if (interesadosFiltrados.Count != 0)
                {
                    interesadosFiltrados.Insert(0, primerInteresado);
                    this._ventana.AnteriorsFiltrados = interesadosFiltrados;
                    this._ventana.AnteriorFiltrado = primerInteresado;
                }
                else
                {
                    interesadosFiltrados.Insert(0, primerInteresado);
                    this._ventana.AnteriorsFiltrados = this._interesadosAnterior;
                    this._ventana.AnteriorFiltrado = primerInteresado;
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

        public void ConsultarApoderadosAnterior()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Agente primerAgente = new Agente("");


                Agente apoderadoAnterior = new Agente();
                IList<Agente> agentesAnteriorFiltrados;
                apoderadoAnterior.Nombre = this._ventana.NombreApoderadoAnteriorFiltrar.ToUpper();
                apoderadoAnterior.Id = this._ventana.IdApoderadoAnteriorFiltrar.ToUpper();

                if ((!apoderadoAnterior.Nombre.Equals("")) || (!apoderadoAnterior.Id.Equals("")))
                    agentesAnteriorFiltrados = this._agenteServicios.ObtenerAgentesSinPoderesFiltro(apoderadoAnterior);
                else
                    agentesAnteriorFiltrados = new List<Agente>();

                if (agentesAnteriorFiltrados.Count != 0)
                {
                    agentesAnteriorFiltrados.Insert(0, primerAgente);
                    this._ventana.ApoderadoAnteriorFiltrado = primerAgente;
                    this._ventana.ApoderadosAnteriorFiltrados = agentesAnteriorFiltrados.ToList<Agente>();
                }
                else
                {
                    agentesAnteriorFiltrados.Insert(0, primerAgente);
                    this._ventana.ApoderadosAnteriorFiltrados = this._agentesAnterior;
                    this._ventana.ApoderadoAnteriorFiltrado = primerAgente;
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

        public void ConsultarPoderesAnterior()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Poder primerPoder = new Poder(int.MinValue);


                Poder poderAnterior = new Poder();
                IList<Poder> poderesAnteriorFiltrados;

                if (!this._ventana.IdPoderAnteriorFiltrar.Equals(""))
                    poderAnterior.Id = int.Parse(this._ventana.IdPoderAnteriorFiltrar);
                else
                    poderAnterior.Id = 0;

                if (!this._ventana.FechaPoderAnteriorFiltrar.Equals(""))
                    poderAnterior.Fecha = DateTime.Parse(this._ventana.FechaPoderAnteriorFiltrar);

                if ((!poderAnterior.Fecha.Equals("")) || (poderAnterior.Id != 0))
                    poderesAnteriorFiltrados = this._poderServicios.ObtenerPoderesFiltro(poderAnterior);
                else
                    poderesAnteriorFiltrados = new List<Poder>();

                if (poderesAnteriorFiltrados.ToList<Poder>().Count != 0)
                {
                    poderesAnteriorFiltrados.Insert(0, primerPoder);
                    this._ventana.PoderesAnteriorFiltrados = this._poderesAnterior;
                    this._ventana.PoderAnteriorFiltrado = primerPoder;
                    this._ventana.PoderesAnteriorFiltrados = poderesAnteriorFiltrados;
                }
                else
                {
                    poderesAnteriorFiltrados.Insert(0, primerPoder);
                    this._ventana.PoderesAnteriorFiltrados = this._poderesAnterior;
                    this._ventana.PoderAnteriorFiltrado = primerPoder;
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

        public bool CambiarAnterior()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            bool retorno = false;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (((Interesado)this._ventana.AnteriorFiltrado).Id != int.MinValue)
                {
                    if (!((Agente)this._ventana.ApoderadoAnteriorFiltrado).Id.Equals(""))
                    {
                        if (((Poder)this._ventana.PoderAnteriorFiltrado).Id != int.MinValue)
                        {
                            this._ventana.InteresadoAnterior = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.AnteriorFiltrado);
                            this._ventana.NombreAnterior = ((Interesado)this._ventana.InteresadoAnterior).Nombre;
                            this._ventana.IdAnterior = ((Interesado)this._ventana.InteresadoAnterior).Id.ToString();
                            retorno = true;
                        }
                        else
                        {
                            this._poderesAnterior = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.AnteriorFiltrado));

                            LimpiarListaPoder("Anterior");

                            if ((this.ValidarListaDePoderes(this._poderesAnterior, this._poderesApoderadosAnterior, "Anterior")))
                            {
                                this._ventana.InteresadoAnterior = this._ventana.AnteriorFiltrado;
                                this._ventana.NombreAnterior = ((Interesado)this._ventana.AnteriorFiltrado).Nombre;
                                this._ventana.IdAnterior = ((Interesado)this._ventana.AnteriorFiltrado).Id.ToString();
                                retorno = true;
                            }
                            else if (!this.ValidarListaDePoderes(this._poderesAnterior, _poderesApoderadosAnterior, "Anterior"))
                            {
                                this._ventana.Mensaje(string.Format(Recursos.MensajesConElUsuario.ErrorInteresadoNoPoseePoderConAgente, "Anterior"), 0);
                            }
                        }
                    }
                    else
                    {
                        if (((Poder)this._ventana.PoderAnteriorFiltrado).Id == int.MinValue)
                        {
                            Poder primerPoder = new Poder(int.MinValue);

                            this._poderesAnterior = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.AnteriorFiltrado));
                            this._poderesAnterior.Insert(0, primerPoder);
                            this._ventana.PoderesAnteriorFiltrados = this._poderesAnterior;
                            this._ventana.PoderAnteriorFiltrado = primerPoder;

                            this._poderesAnterior = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.AnteriorFiltrado));
                            this._ventana.InteresadoAnterior = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.AnteriorFiltrado);
                            this._ventana.NombreAnterior = ((Interesado)this._ventana.InteresadoAnterior).Nombre;
                            this._ventana.IdAnterior = ((Interesado)this._ventana.InteresadoAnterior).Id.ToString();
                            retorno = true;
                        }
                        else
                        {

                            this._poderesAnterior = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.AnteriorFiltrado));
                            this._ventana.InteresadoAnterior = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.AnteriorFiltrado);
                            this._ventana.NombreAnterior = ((Interesado)this._ventana.InteresadoAnterior).Nombre;
                            this._ventana.IdAnterior = ((Interesado)this._ventana.InteresadoAnterior).Id.ToString();
                            retorno = true;
                        }
                    }

                }
                else
                {
                    this._ventana.InteresadoAnterior = this._ventana.AnteriorFiltrado;
                    this._ventana.NombreAnterior = ((Interesado)this._ventana.InteresadoAnterior).Nombre;
                    this._ventana.IdAnterior = ((Interesado)this._ventana.InteresadoAnterior).Id.ToString();
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

        public bool CambiarApoderadoAnterior()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            bool retorno = false;
            IList<Poder> _poderesFiltrados = new List<Poder>();
            Poder poderABuscar = null;
            bool listaDePoderesValidada = false;
            Poder primerPoder = new Poder();
            primerPoder.Id = int.MinValue;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (!((Agente)this._ventana.ApoderadoAnteriorFiltrado).Id.Equals(""))
                {
                    if (((Interesado)this._ventana.AnteriorFiltrado).Id != int.MinValue)
                    {
                        //if (((Poder)this._ventana.PoderAnteriorFiltrado).Id != int.MinValue)
                        if ((null != this._ventana.PoderAnteriorFiltrado) && (((Poder)this._ventana.PoderAnteriorFiltrado).Id != int.MinValue))
                        {
                            this._ventana.ApoderadoAnterior = this._ventana.ApoderadoAnteriorFiltrado;
                            this._ventana.NombreApoderadoAnterior = ((Agente)this._ventana.ApoderadoAnteriorFiltrado).Nombre;
                            this._ventana.IdApoderadoAnterior = ((Agente)this._ventana.ApoderadoAnteriorFiltrado).Id;
                            //-- 
                            //Para validar que el Agente que estoy seleccionando tenga Poderes con el Sobreviviente
                            //Poder primerPoder = new Poder();
                            //primerPoder.Id = int.MinValue;

                            _poderesFiltrados = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado((Agente)this._ventana.ApoderadoAnteriorFiltrado, (Interesado)this._ventana.AnteriorFiltrado);

                            if (_poderesFiltrados.Count != 0)
                            {
                                poderABuscar = this.BuscarPoder(_poderesFiltrados, (Poder)this._ventana.PoderAnteriorFiltrado);

                                if (poderABuscar != null)
                                {
                                    _poderesFiltrados.Insert(0, primerPoder);
                                    this._ventana.PoderesAnteriorFiltrados = _poderesFiltrados;
                                    this._ventana.PoderAnteriorFiltrado = poderABuscar;
                                }
                                else
                                {
                                    this._ventana.Mensaje("Seleccione un poder que relacione al Apoderado Anterior con el Interesado Anterior", 0);
                                    _poderesFiltrados.Insert(0, primerPoder);
                                    this._ventana.PoderesAnteriorFiltrados = _poderesFiltrados;

                                }
                            }

                            else
                            {
                                this._ventana.Mensaje("Apoderado Anterior no posee poderes con el Interesado Anterior", 0);
                                _poderesFiltrados.Insert(0, primerPoder);
                                this._ventana.PoderesAnteriorFiltrados = _poderesFiltrados;
                            }

                            //--
                            retorno = true;
                        }
                        else
                        {

                            

                            this._poderesApoderadosAnterior = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado((Agente)_ventana.ApoderadoAnteriorFiltrado, (Interesado)this._ventana.AnteriorFiltrado);

                            #region CODIGO ORIGINAL COMENTADO - NO BORRAR
                            //LimpiarListaPoder("Anterior");
                            //listaDePoderesValidada = this.ValidarListaDePoderes(this._poderesAnterior, this._poderesApoderadosAnterior, "Interesado Anterior");
                            #endregion

                            if (this._poderesApoderadosAnterior.Count > 0)
                            {
                                this._poderesApoderadosAnterior.Insert(0, primerPoder);
                                this._ventana.PoderesAnteriorFiltrados = this._poderesApoderadosAnterior;
                                listaDePoderesValidada = true;
                            }

                            if(listaDePoderesValidada)
                            {
                                this._ventana.ApoderadoAnterior = this._ventana.ApoderadoAnteriorFiltrado;
                                this._ventana.NombreApoderadoAnterior = ((Agente)this._ventana.ApoderadoAnteriorFiltrado).Nombre;
                                this._ventana.IdApoderadoAnterior = ((Agente)this._ventana.ApoderadoAnteriorFiltrado).Id;
                                retorno = true;
                            }
                            else
                            {
                                this._ventana.Mensaje(string.Format(Recursos.MensajesConElUsuario.ErrorAgenteNoPoseePoderConInteresado, "Interesado Anterior"), 0);
                                this._ventana.ApoderadoAnterior = this._ventana.ApoderadoAnteriorFiltrado;
                                this._ventana.NombreApoderadoAnterior = ((Agente)this._ventana.ApoderadoAnteriorFiltrado).Nombre;
                                this._ventana.IdApoderadoAnterior = ((Agente)this._ventana.ApoderadoAnteriorFiltrado).Id;
                                retorno = true;
                            }
                        }
                    }
                    else
                    {
                        //NOTA: Se hace de esta forma porque si no hay un Interesado Actual se debe mostrar el mensaje pidiendo un Interesado
                        //if (((Poder)this._ventana.PoderAnteriorFiltrado).Id != int.MinValue)
                        if ((null != this._ventana.PoderAnteriorFiltrado) && (((Poder)this._ventana.PoderAnteriorFiltrado).Id != int.MinValue))
                        {
                            //--
                            this._ventana.Mensaje("Debe seleccionar un Interesado Anterior", 0);
                            LimpiarListaPoder("Anterior");
                            //--
                            this._ventana.ApoderadoAnterior = this._ventana.ApoderadoAnteriorFiltrado;
                            this._ventana.NombreApoderadoAnterior = ((Agente)this._ventana.ApoderadoAnteriorFiltrado).Nombre;
                            this._ventana.IdApoderadoAnterior = ((Agente)this._ventana.ApoderadoAnteriorFiltrado).Id;
                            
                            retorno = true;
                        }
                        else
                        {
                            //this._poderesApoderadosAnterior = this._poderServicios.ConsultarPoderesPorAgente(((Agente)_ventana.ApoderadoAnteriorFiltrado));
                            //--
                            this._ventana.Mensaje("Debe seleccionar un Interesado Anterior", 0);
                            //--
                            this._ventana.ApoderadoAnterior = this._ventana.ApoderadoAnteriorFiltrado;
                            this._ventana.NombreApoderadoAnterior = ((Agente)this._ventana.ApoderadoAnteriorFiltrado).Nombre;
                            this._ventana.IdApoderadoAnterior = ((Agente)this._ventana.ApoderadoAnteriorFiltrado).Id;
                            retorno = true;
                        }
                    }
                }
                else
                {
                    this._ventana.ApoderadoAnterior = this._ventana.ApoderadoAnteriorFiltrado;
                    this._ventana.NombreApoderadoAnterior = ((Agente)this._ventana.ApoderadoAnteriorFiltrado).Nombre;
                    this._ventana.IdApoderadoAnterior = ((Agente)this._ventana.ApoderadoAnteriorFiltrado).Id;
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

        public bool CambiarPoderAnterior()
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

                if (((Poder)this._ventana.PoderAnteriorFiltrado).Id != int.MinValue)
                {
                    if (((Agente)this._ventana.ApoderadoAnteriorFiltrado).Id.Equals(""))
                    {
                        if (((Interesado)this._ventana.AnteriorFiltrado).Id != int.MinValue)
                        {
                            LimpiarListaAgente("Anterior");

                            LlenarListaAgente((Poder)this._ventana.PoderAnteriorFiltrado, "Anterior");

                            this._ventana.PoderAnterior = this._ventana.PoderAnteriorFiltrado;
                            this._ventana.IdPoderAnterior = ((Poder)this._ventana.PoderAnteriorFiltrado).Id.ToString();
                            retorno = true;

                        }
                        else
                        {
                            LimpiarListaInteresado("Anterior");

                            LimpiarListaAgente("Anterior");

                            LlenarListaAgenteEInteresado((Poder)this._ventana.PoderAnteriorFiltrado, "Anterior", false);

                            this._ventana.PoderAnterior = this._ventana.PoderAnteriorFiltrado;
                            this._ventana.IdPoderAnterior = ((Poder)this._ventana.PoderAnteriorFiltrado).Id.ToString();
                            retorno = true;
                        }
                    }
                    else
                    {
                        //El Agente es diferente a Vacio
                        //--

                        Poder primerPoder = new Poder();
                        primerPoder.Id = int.MinValue;
                        _poderesFiltrados = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado((Agente)this._ventana.ApoderadoAnteriorFiltrado, (Interesado)this._ventana.AnteriorFiltrado);
                        if (_poderesFiltrados.Count != 0)
                        {
                            poderABuscar = this.BuscarPoder(_poderesFiltrados, (Poder)this._ventana.PoderAnteriorFiltrado);

                            if (poderABuscar != null)
                            {
                                _poderesFiltrados.Insert(0, primerPoder);
                                this._ventana.PoderesAnteriorFiltrados = _poderesFiltrados;
                                this._ventana.PoderAnteriorFiltrado = poderABuscar;
                                this._ventana.PoderAnterior = this._ventana.PoderAnteriorFiltrado;
                                this._ventana.IdPoderAnterior = ((Poder)this._ventana.PoderAnteriorFiltrado).Id.ToString();
                                retorno = true;
                            }
                            else
                            {
                                //this._ventana.Mensaje(string.Format(Recursos.MensajesConElUsuario.ErrorAgenteNoPoseePoderConInteresado, "interesado"), 0);
                                this._ventana.Mensaje("El Poder no pertenece al Interesado Anterior", 0);
                                this._ventana.PoderAnterior = this._ventana.PoderAnteriorFiltrado;
                                this._ventana.IdPoderAnterior = ((Poder)this._ventana.PoderAnteriorFiltrado).Id.ToString();
                                retorno = true;
                            }
                        }
                        else
                        {
                            this._ventana.Mensaje("El poder seleccionado no relaciona al Apoderado Anterior con el Interesado Anterior", 0);
                            this._ventana.PoderAnterior = this._ventana.PoderAnteriorFiltrado;
                            this._ventana.IdPoderAnterior = ((Poder)this._ventana.PoderAnteriorFiltrado).Id.ToString();
                            retorno = true;
                        }
                    }
                }
                else
                {
                    this._ventana.PoderAnterior = this._ventana.PoderAnteriorFiltrado;
                    this._ventana.IdPoderAnterior = ((Poder)this._ventana.PoderAnteriorFiltrado).Id.ToString();
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

        #endregion

        #region Actual

        private void ValidarActual()
        {
            if (((Interesado)this._ventana.ActualFiltrado).Id == int.MinValue)
            {
                if (((Agente)this._ventana.ApoderadoActualFiltrado).Id.Equals(""))
                {
                    if (((Poder)this._ventana.PoderActualFiltrado).Id != int.MinValue)
                    {
                        LlenarListaAgenteEInteresado((Poder)this._ventana.PoderActual, "Actual", true);
                        //this._ventana.GestionarBotonConsultarInteresados("Actual", false);
                        //this._ventana.GestionarBotonConsultarApoderados("Actual", false);
                        this._ventana.GestionarBotonConsultarInteresados("Actual", true);
                        this._ventana.GestionarBotonConsultarApoderados("Actual", true);
                    }
                }
                else
                {
                    if (((Poder)this._ventana.PoderActualFiltrado).Id == int.MinValue)
                        //this._ventana.GestionarBotonConsultarInteresados("Actual", false);
                        this._ventana.GestionarBotonConsultarInteresados("Actual", true);

                    else
                    {

                        LlenarListaAgenteEInteresado((Poder)this._ventana.PoderActual, "Actual", true);

                        //this._ventana.GestionarBotonConsultarInteresados("Actual", false);
                        //this._ventana.GestionarBotonConsultarApoderados("Actual", false);
                        //this._ventana.GestionarBotonConsultarPoderes("Actual", false);
                        this._ventana.GestionarBotonConsultarInteresados("Actual", true);
                        this._ventana.GestionarBotonConsultarApoderados("Actual", true);
                        this._ventana.GestionarBotonConsultarPoderes("Actual", true);
                    }

                }
            }
            else
            {
                if (((Agente)this._ventana.ApoderadoActualFiltrado).Id.Equals(""))
                {
                    if (((Poder)this._ventana.PoderActualFiltrado).Id == int.MinValue)
                        //this._ventana.GestionarBotonConsultarPoderes("Actual", false);
                        this._ventana.GestionarBotonConsultarPoderes("Actual", true);

                    else
                    {
                        LlenarListaAgenteEInteresado((Poder)this._ventana.PoderActual, "Actual", true);

                        //this._ventana.GestionarBotonConsultarInteresados("Actual", false);
                        //this._ventana.GestionarBotonConsultarApoderados("Actual", false);
                        //this._ventana.GestionarBotonConsultarPoderes("Actual", false);
                        this._ventana.GestionarBotonConsultarInteresados("Actual", true);
                        this._ventana.GestionarBotonConsultarApoderados("Actual", true);
                        this._ventana.GestionarBotonConsultarPoderes("Actual", true);

                    }
                }
                else
                {
                    if (((Poder)this._ventana.PoderActualFiltrado).Id == int.MinValue)
                    {

                        ValidarListaDePoderes(this._poderesActual, this._poderesApoderadosActual, "Actual");

                        //this._ventana.GestionarBotonConsultarPoderes("Actual", false);
                        this._ventana.GestionarBotonConsultarPoderes("Actual", true);
                    }
                    else
                    {
                        LlenarListaAgenteEInteresado((Poder)this._ventana.PoderActual, "Actual", true);
                        ValidarListaDePoderes(this._poderesActual, this._poderesApoderadosActual, "Actual");

                        //this._ventana.GestionarBotonConsultarInteresados("Actual", false);
                        //this._ventana.GestionarBotonConsultarApoderados("Actual", false);
                        //this._ventana.GestionarBotonConsultarPoderes("Actual", false);
                        this._ventana.GestionarBotonConsultarInteresados("Actual", true);
                        this._ventana.GestionarBotonConsultarApoderados("Actual", true);
                        this._ventana.GestionarBotonConsultarPoderes("Actual", true);
                    }
                }
            }
        }

        public void ConsultarActuals()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Interesado primerInteresado = new Interesado(int.MinValue);


                Interesado interesadoActual = new Interesado();
                IList<Interesado> interesadoActualsFiltrados;
                interesadoActual.Nombre = this._ventana.NombreActualFiltrar.ToUpper();
                interesadoActual.Id = this._ventana.IdActualFiltrar.Equals("") ? 0 : int.Parse(this._ventana.IdActualFiltrar);

                if ((!interesadoActual.Nombre.Equals("")) || (interesadoActual.Id != 0))
                    interesadoActualsFiltrados = this._interesadoServicios.ObtenerInteresadosFiltro(interesadoActual);
                else
                    interesadoActualsFiltrados = new List<Interesado>();

                if (interesadoActualsFiltrados.ToList<Interesado>().Count != 0)
                {
                    interesadoActualsFiltrados.Insert(0, primerInteresado);
                    this._ventana.ActualsFiltrados = interesadoActualsFiltrados.ToList<Interesado>();
                    this._ventana.ActualFiltrado = primerInteresado;
                }
                else
                {
                    interesadoActualsFiltrados.Insert(0, primerInteresado);
                    this._ventana.ActualsFiltrados = this._interesadosActual;
                    this._ventana.ActualFiltrado = primerInteresado;
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

        public void ConsultarApoderadosActual()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Agente primerAgente = new Agente("");


                Agente apoderadoActual = new Agente();
                IList<Agente> agentesActualFiltrados;
                apoderadoActual.Nombre = this._ventana.NombreApoderadoActualFiltrar.ToUpper();
                apoderadoActual.Id = this._ventana.IdApoderadoActualFiltrar.ToUpper();

                if ((!apoderadoActual.Nombre.Equals("")) || (!apoderadoActual.Id.Equals("")))
                    agentesActualFiltrados = this._agenteServicios.ObtenerAgentesSinPoderesFiltro(apoderadoActual);
                else
                    agentesActualFiltrados = new List<Agente>();

                if (agentesActualFiltrados.Count != 0)
                {
                    agentesActualFiltrados.Insert(0, primerAgente);
                    this._ventana.ApoderadoActualFiltrado = primerAgente;
                    this._ventana.ApoderadosActualFiltrados = agentesActualFiltrados.ToList<Agente>();
                }
                else
                {
                    agentesActualFiltrados.Insert(0, primerAgente);
                    this._ventana.ApoderadosActualFiltrados = this._agentesActual;
                    this._ventana.ApoderadoActualFiltrado = primerAgente;
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

        public void ConsultarPoderesActual()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Poder primerPoder = new Poder(int.MinValue);


                Poder poderActual = new Poder();
                IList<Poder> poderesActualFiltrados;

                if (!this._ventana.IdPoderActualFiltrar.Equals(""))
                    poderActual.Id = int.Parse(this._ventana.IdPoderActualFiltrar);
                else
                    poderActual.Id = 0;

                if (!this._ventana.FechaPoderActualFiltrar.Equals(""))
                    poderActual.Fecha = DateTime.Parse(this._ventana.FechaPoderActualFiltrar);

                if ((!poderActual.Fecha.Equals("")) || (poderActual.Id != 0))
                    poderesActualFiltrados = this._poderServicios.ObtenerPoderesFiltro(poderActual);
                else
                    poderesActualFiltrados = new List<Poder>();

                if (poderesActualFiltrados.ToList<Poder>().Count != 0)
                {
                    poderesActualFiltrados.Insert(0, primerPoder);
                    this._ventana.PoderesActualFiltrados = this._poderesActual;
                    this._ventana.PoderActualFiltrado = primerPoder;
                    this._ventana.PoderesActualFiltrados = poderesActualFiltrados;
                }
                else
                {
                    poderesActualFiltrados.Insert(0, primerPoder);
                    this._ventana.PoderesActualFiltrados = this._poderesActual;
                    this._ventana.PoderActualFiltrado = primerPoder;
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

        public bool CambiarActual()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            bool retorno = false;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (((Interesado)this._ventana.ActualFiltrado).Id != int.MinValue)
                {
                    if (!((Agente)this._ventana.ApoderadoActualFiltrado).Id.Equals(""))
                    {
                        if (((Poder)this._ventana.PoderActualFiltrado).Id != int.MinValue)
                        {
                            this._ventana.InteresadoActual = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.ActualFiltrado);
                            this._ventana.NombreActual = ((Interesado)this._ventana.InteresadoActual).Nombre;
                            this._ventana.IdActual = ((Interesado)this._ventana.InteresadoActual).Id.ToString();
                            retorno = true;
                        }
                        else
                        {
                            this._poderesActual = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.ActualFiltrado));

                            LimpiarListaPoder("Actual");

                            if ((this.ValidarListaDePoderes(this._poderesActual, this._poderesApoderadosActual, "Actual")))
                            {
                                this._ventana.InteresadoActual = this._ventana.ActualFiltrado;
                                this._ventana.NombreActual = ((Interesado)this._ventana.ActualFiltrado).Nombre;
                                this._ventana.IdActual = ((Interesado)this._ventana.ActualFiltrado).Id.ToString();
                                retorno = true;
                            }
                            else if (!this.ValidarListaDePoderes(this._poderesActual, _poderesApoderadosActual, "Actual"))
                            {
                                this._ventana.Mensaje(string.Format(Recursos.MensajesConElUsuario.ErrorInteresadoNoPoseePoderConAgente, "Actual"), 0);
                            }
                        }
                    }
                    else
                    {
                        if (((Poder)this._ventana.PoderActualFiltrado).Id == int.MinValue)
                        {
                            Poder primerPoder = new Poder(int.MinValue);

                            this._poderesActual = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.ActualFiltrado));
                            this._poderesActual.Insert(0, primerPoder);
                            this._ventana.PoderesActualFiltrados = this._poderesActual;
                            this._ventana.PoderActualFiltrado = primerPoder;

                            this._poderesActual = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.ActualFiltrado));
                            this._ventana.InteresadoActual = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.ActualFiltrado);
                            this._ventana.NombreActual = ((Interesado)this._ventana.InteresadoActual).Nombre;
                            this._ventana.IdActual = ((Interesado)this._ventana.InteresadoActual).Id.ToString();
                            retorno = true;
                        }
                        else
                        {

                            this._poderesActual = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.ActualFiltrado));
                            this._ventana.InteresadoActual = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.ActualFiltrado);
                            this._ventana.NombreActual = ((Interesado)this._ventana.InteresadoActual).Nombre;
                            this._ventana.IdActual = ((Interesado)this._ventana.InteresadoActual).Id.ToString();
                            retorno = true;
                        }
                    }

                }
                else
                {
                    this._ventana.InteresadoActual = this._ventana.ActualFiltrado;
                    this._ventana.NombreActual = ((Interesado)this._ventana.InteresadoActual).Nombre;
                    this._ventana.IdActual = ((Interesado)this._ventana.InteresadoActual).Id.ToString();
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

        public bool CambiarApoderadoActual()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            bool retorno = false;
            IList<Poder> _poderesFiltrados = new List<Poder>();
            Poder poderABuscar = null;
            bool listaDePoderesValidada = false;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (!((Agente)this._ventana.ApoderadoActualFiltrado).Id.Equals(""))
                {
                    if (((Interesado)this._ventana.ActualFiltrado).Id != int.MinValue)
                    {
                        //if (((Poder)this._ventana.PoderActualFiltrado).Id != int.MinValue)
                        if ((null != this._ventana.PoderActualFiltrado) && (((Poder)this._ventana.PoderActualFiltrado).Id != int.MinValue))
                        {
                            this._ventana.ApoderadoActual = this._ventana.ApoderadoActualFiltrado;
                            this._ventana.NombreApoderadoActual = ((Agente)this._ventana.ApoderadoActualFiltrado).Nombre;
                            this._ventana.IdApoderadoActual = ((Agente)this._ventana.ApoderadoActualFiltrado).Id;
                            //-- 
                            //Para validar que el Agente que estoy seleccionando tenga Poderes con el Interesado Actual
                            Poder primerPoder = new Poder();
                            primerPoder.Id = int.MinValue;

                            _poderesFiltrados = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado((Agente)this._ventana.ApoderadoActualFiltrado, (Interesado)this._ventana.ActualFiltrado);

                            if (_poderesFiltrados.Count != 0)
                            {
                                poderABuscar = this.BuscarPoder(_poderesFiltrados, (Poder)this._ventana.PoderActualFiltrado);

                                if (poderABuscar != null)
                                {
                                    _poderesFiltrados.Insert(0, primerPoder);
                                    this._ventana.PoderesActualFiltrados = _poderesFiltrados;
                                    this._ventana.PoderActualFiltrado = poderABuscar;
                                }
                                else
                                {
                                    this._ventana.Mensaje("Seleccione un poder que relacione al Apoderado Actual con el Interesado Actual", 0);
                                    _poderesFiltrados.Insert(0, primerPoder);
                                    this._ventana.PoderesActualFiltrados = _poderesFiltrados;

                                }
                            }

                            else
                            {
                                this._ventana.Mensaje("Apoderado Actual no posee poderes con el Interesado Actual", 0);
                                _poderesFiltrados.Insert(0, primerPoder);
                                this._ventana.PoderesActualFiltrados = _poderesFiltrados;
                            }

                            //--
                            retorno = true;
                        }
                        else
                        {
                            //this._poderesApoderadosActual = this._poderServicios.ConsultarPoderesPorAgente(((Agente)_ventana.ApoderadoActualFiltrado));
                            this._poderesApoderadosActual = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado((Agente)this._ventana.ApoderadoActualFiltrado, (Interesado)this._ventana.ActualFiltrado);
                            
                            LimpiarListaPoder("Actual");

                            listaDePoderesValidada = this.ValidarListaDePoderes(this._poderesActual, this._poderesApoderadosActual, "Actual");

                            //if ((this.ValidarListaDePoderes(this._poderesActual, this._poderesApoderadosActual, "Actual")))
                            if (listaDePoderesValidada) 
                            {
                                this._ventana.ApoderadoActual = this._ventana.ApoderadoActualFiltrado;
                                this._ventana.NombreApoderadoActual = ((Agente)this._ventana.ApoderadoActualFiltrado).Nombre;
                                this._ventana.IdApoderadoActual = ((Agente)this._ventana.ApoderadoActualFiltrado).Id;

                                retorno = true;
                            }
                            //else if (!this.ValidarListaDePoderes(this._poderesActual, this._poderesApoderadosActual, "Actual"))
                            else
                            {
                                this._ventana.Mensaje(string.Format(Recursos.MensajesConElUsuario.ErrorAgenteNoPoseePoderConInteresado, "Actual"), 0);
                                this._ventana.ApoderadoActual = this._ventana.ApoderadoActualFiltrado;
                                this._ventana.NombreApoderadoActual = ((Agente)this._ventana.ApoderadoActualFiltrado).Nombre;
                                this._ventana.IdApoderadoActual = ((Agente)this._ventana.ApoderadoActualFiltrado).Id;
                                retorno = true;
                            }
                        }
                    }
                    else
                    {
                        //NOTA: Se hace de esta forma porque si no hay un Interesado Actual se debe mostrar el mensaje pidiendo un Interesado
                        //if (((Poder)this._ventana.PoderActualFiltrado).Id != int.MinValue)
                        if ((null != this._ventana.PoderActualFiltrado) && (((Poder)this._ventana.PoderActualFiltrado).Id != int.MinValue))
                        {
                            //--
                            this._ventana.Mensaje("Debe seleccionar un Interesado Actual", 0);
                            LimpiarListaPoder("Actual");
                            //--
                            this._ventana.ApoderadoActual = this._ventana.ApoderadoActualFiltrado;
                            this._ventana.NombreApoderadoActual = ((Agente)this._ventana.ApoderadoActualFiltrado).Nombre;
                            this._ventana.IdApoderadoActual = ((Agente)this._ventana.ApoderadoActualFiltrado).Id;
                            retorno = true;
                        }
                        else
                        {
                            //this._poderesApoderadosActual = this._poderServicios.ConsultarPoderesPorAgente(((Agente)_ventana.ApoderadoActualFiltrado));
                            //--
                            this._ventana.Mensaje("Debe seleccionar un Interesado Actual", 0);
                            LimpiarListaPoder("Actual");
                            //--
                            this._ventana.ApoderadoActual = this._ventana.ApoderadoActualFiltrado;
                            this._ventana.NombreApoderadoActual = ((Agente)this._ventana.ApoderadoActualFiltrado).Nombre;
                            this._ventana.IdApoderadoActual = ((Agente)this._ventana.ApoderadoActualFiltrado).Id;
                            retorno = true;
                        }
                    }
                }
                else
                {
                    this._ventana.ApoderadoActual = this._ventana.ApoderadoActualFiltrado;
                    this._ventana.NombreApoderadoActual = ((Agente)this._ventana.ApoderadoActualFiltrado).Nombre;
                    this._ventana.IdApoderadoActual = ((Agente)this._ventana.ApoderadoActualFiltrado).Id;
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

        public bool CambiarPoderActual()
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

                if (((Poder)this._ventana.PoderActualFiltrado).Id != int.MinValue)
                {
                    if (((Agente)this._ventana.ApoderadoActualFiltrado).Id.Equals(""))
                    {
                        if (((Interesado)this._ventana.ActualFiltrado).Id != int.MinValue)
                        {
                            LimpiarListaAgente("Actual");

                            LlenarListaAgente((Poder)this._ventana.PoderActualFiltrado, "Actual");

                            this._ventana.PoderActual = this._ventana.PoderActualFiltrado;
                            this._ventana.IdPoderActual = ((Poder)this._ventana.PoderActualFiltrado).Id.ToString();
                            retorno = true;

                        }
                        else
                        {
                            LimpiarListaInteresado("Actual");

                            LimpiarListaAgente("Actual");

                            LlenarListaAgenteEInteresado((Poder)this._ventana.PoderActualFiltrado, "Actual", false);

                            this._ventana.PoderActual = this._ventana.PoderActualFiltrado;
                            this._ventana.IdPoderActual = ((Poder)this._ventana.PoderActualFiltrado).Id.ToString();
                            retorno = true;
                        }
                    }
                    else
                    {
                        //El Agente es diferente a Vacio
                        //--

                        Poder primerPoder = new Poder();
                        primerPoder.Id = int.MinValue;
                        _poderesFiltrados = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado((Agente)this._ventana.ApoderadoActualFiltrado, (Interesado)this._ventana.ActualFiltrado);
                        
                        if (_poderesFiltrados.Count != 0)
                        {
                            poderABuscar = this.BuscarPoder(_poderesFiltrados, (Poder)this._ventana.PoderActualFiltrado);

                            if (poderABuscar != null)
                            {
                                _poderesFiltrados.Insert(0, primerPoder);
                                this._ventana.PoderesActualFiltrados = _poderesFiltrados;
                                this._ventana.PoderActualFiltrado = poderABuscar;
                                this._ventana.PoderActual = this._ventana.PoderActualFiltrado;
                                this._ventana.IdPoderActual = ((Poder)this._ventana.PoderActualFiltrado).Id.ToString();
                                retorno = true;
                            }
                            else
                            {
                                //this._ventana.Mensaje(string.Format(Recursos.MensajesConElUsuario.ErrorAgenteNoPoseePoderConInteresado, "interesado"), 0);
                                this._ventana.Mensaje("El Poder no pertenece al Interesado Actual", 0);
                                this._ventana.PoderActual = this._ventana.PoderActualFiltrado;
                                this._ventana.IdPoderActual = ((Poder)this._ventana.PoderActualFiltrado).Id.ToString();
                                retorno = true;
                            }
                        }
                        else
                        {
                            this._ventana.Mensaje("El poder seleccionado no relaciona al Apoderado Actual con el Interesado Actual", 0);
                            this._ventana.PoderActual = this._ventana.PoderActualFiltrado;
                            this._ventana.IdPoderActual = ((Poder)this._ventana.PoderActualFiltrado).Id.ToString();
                            retorno = true;
                        }
                    }
                }
                else
                {
                    this._ventana.PoderActual = this._ventana.PoderActualFiltrado;
                    this._ventana.IdPoderActual = ((Poder)this._ventana.PoderActualFiltrado).Id.ToString();
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

        #endregion

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
                        Marca marcaCambioNombre = (Marca)this._ventana.Marca;
                        this.Navegar(new ListadoCadenasDeCambios(marcaCambioNombre, "M", this._ventana));
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

        /// <summary>
        /// Metodo que consulta los interesados de un Cambio de Peticionario (Anterior y Actual)
        /// </summary>
        /// <param name="nombreBoton"></param>
        public void ConsultarInteresadosCambioPeticionario(string nombreBoton)
        {
            Interesado interesadoAux = new Interesado();

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (nombreBoton.Equals("_btnVerInteresadoAnterior"))
                {
                    if (!this._ventana.IdAnterior.Equals(String.Empty))
                        interesadoAux.Id = int.Parse(this._ventana.IdAnterior);
                    else
                        interesadoAux.Id = int.MinValue;
                }
                else if (nombreBoton.Equals("_btnVerInteresadoActual"))
                {
                    if (!this._ventana.IdActual.Equals(String.Empty))
                        interesadoAux.Id = int.Parse(this._ventana.IdActual);
                    else
                        interesadoAux.Id = int.MinValue;
                }

                if (interesadoAux.Id != int.MinValue)
                {
                    IList<Interesado> interesados = this._interesadoServicios.ObtenerInteresadosFiltro(interesadoAux);
                    if (interesados.Count > 0)
                        this.Navegar(new ConsultarInteresado(interesados[0], this._ventana)); 
                }
                
                

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