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
using Trascend.Bolet.Cliente.Contratos.Marcas;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.Cliente.Ventanas.Marcas;
using Trascend.Bolet.Cliente.Ventanas.Renovaciones;
using Trascend.Bolet.Cliente.Ventanas.Corresponsales;
using Trascend.Bolet.Cliente.Ventanas.Cartas;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.Cliente.Ventanas.Auditorias;
using Trascend.Bolet.ControlesByT.Ventanas;
using System.Text;
using System.IO;
using Trascend.Bolet.Cliente.Ventanas.Asociados;
using Trascend.Bolet.Cliente.Ventanas.Interesados;
using Trascend.Bolet.Cliente.Ventanas.Poderes;

using Diginsoft.Bolet.Cliente.Fac.Ventanas.FacReportes;
using Diginsoft.Bolet.Cliente.Fac.Ventanas.FacAsociadoMarcaPatentes;
using Diginsoft.Bolet.ObjetosComunes.ContratosServicios;
using Diginsoft.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Cliente.Presentadores.Marcas
{
    class PresentadorConsultarMarca : PresentadorBase
    {

        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        private IConsultarMarca _ventana, _ventanaPadreAux;


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
        private IPlanillaServicios _planillaServicios;
        private IInternacionalServicios _internacionalServicios;
        private IRenovacionServicios _renovacionServicios;
        private IInstruccionDeRenovacionServicios _instruccionDeRenovacionServicios;
        private IArchivoServicios _archivoServicios;
        private ICertificadoMarcaServicios _certificadoMarcaServicios;
        private ICartaServicios _cartaServicios;
        private IInstruccionCorrespondenciaServicios _instruccionCorrespondenciaServicios;
        private IInstruccionEnvioOriginalesServicios _instruccionEnvioOriginalesServicios;
        private IInstruccionOtrosServicios _instruccionOtrosServicios;
        private IInstruccionDescuentoServicios _instruccionDescuentoServicios;
        private IFacVistaFacturaServicioServicios _facVistaFacturaServicioServicios;
        private IInteresadoMultipleServicios _interesadoMultipleServicios;


        private IList<Asociado> _asociados;
        private IList<Poder> _poderesInterseccion;
        private IList<Interesado> _interesados;
        private IList<Corresponsal> _corresponsales;
        private IList<Auditoria> _auditorias;
        private IList<ListaDatosValores> _origenDeMarcas;

        private Interesado _interesadoAnterior;


        private Marca _marca;


        private object _ventanaPadre = null;

        private bool _cargaMarcaOrigen = false;


        /// <summary>
        /// Constructor Predeterminado
        /// </summary>
        /// <param name="ventana">página que satisface el contrato</param>
        public PresentadorConsultarMarca(IConsultarMarca ventana, object marca, object ventanaPadre)
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                this._ventana = ventana;
                this._ventanaPadre = ventanaPadre;
                                
                if (((Marca)marca).Internacional == null)
                    ((Marca)marca).Internacional = new Internacional();
                else
                    ((Marca)marca).Internacional = new Internacional(((Marca)marca).Internacional.Id);

                if (((Marca)marca).Nacional == null)
                    ((Marca)marca).Nacional = new Nacional();
                else
                    ((Marca)marca).Nacional = new Nacional(((Marca)marca).Nacional.Id);

                this._ventana.Marca = marca;

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
                this._interesadoMultipleServicios = (IInteresadoMultipleServicios)Activator.GetObject(typeof(IInteresadoMultipleServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InteresadoMultipleServicios"]);
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
                this._planillaServicios = (IPlanillaServicios)Activator.GetObject(typeof(IPlanillaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["PlanillaServicios"]);
                this._internacionalServicios = (IInternacionalServicios)Activator.GetObject(typeof(IInternacionalServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InternacionalServicios"]);
                this._renovacionServicios = (IRenovacionServicios)Activator.GetObject(typeof(IRenovacionServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["RenovacionServicios"]);
                this._instruccionDeRenovacionServicios = (IInstruccionDeRenovacionServicios)Activator.GetObject(typeof(IInstruccionDeRenovacionServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InstruccionDeRenovacionServicios"]);
                this._archivoServicios = (IArchivoServicios)Activator.GetObject(typeof(IArchivoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ArchivoServicios"]);
                this._certificadoMarcaServicios = (ICertificadoMarcaServicios)Activator.GetObject(typeof(ICertificadoMarcaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CertificadoMarcaServicios"]);
                this._cartaServicios = (ICartaServicios)Activator.GetObject(typeof(ICartaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CartaServicios"]);
                this._instruccionCorrespondenciaServicios = (IInstruccionCorrespondenciaServicios)Activator.GetObject(typeof(IInstruccionCorrespondenciaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InstruccionCorrespondenciaServicios"]);
                this._instruccionEnvioOriginalesServicios = (IInstruccionEnvioOriginalesServicios)Activator.GetObject(typeof(IInstruccionEnvioOriginalesServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InstruccionEnvioOriginalesServicios"]);
                this._instruccionOtrosServicios = (IInstruccionOtrosServicios)Activator.GetObject(typeof(IInstruccionOtrosServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InstruccionOtrosServicios"]);
                this._instruccionDescuentoServicios = (IInstruccionDescuentoServicios)Activator.GetObject(typeof(IInstruccionDescuentoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InstruccionDescuentoServicios"]);
                this._facVistaFacturaServicioServicios = 
                    (IFacVistaFacturaServicioServicios)Activator.GetObject(typeof(IFacVistaFacturaServicioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["FacVistaFacturaServicioServicios"]);


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
        }


        /// <summary>
        /// Método que actualiza el título de la ventana
        /// </summary>
        public void ActualizarTitulo()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleConsultarMarcas,
                Recursos.Ids.ConsultarMarcas);

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que carga los datos iniciales a mostrar en la página
        /// </summary>
        public void CargarPagina()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Archivo archivoMarca = null;
            String alertaInteresado = String.Empty;

            try
            {

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                bool marcaOrigenCargada = this._ventana.MarcaOrigenCargada;

                

                this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleConsultarMarca, "");
                


                if (!PoseePermisologia(UsuarioLogeado.Rol.Objetos, Recursos.Ids.SolicitudMarca))
                {
                    this._ventana.BloquearModificacion();
                }

                Marca marca = (Marca)this._ventana.Marca;
                _marca = marca;

                IList<Marca> listaDeMarcas;

                // En el caso de la Marca de Origen que solamente trae de la ventana el Id
                if (null == marca.Descripcion)
                {
                    listaDeMarcas = _marcaServicios.ObtenerMarcasFiltro(marca);
                    marca = listaDeMarcas[0];
                    this._ventana.Marca = marca;
                    _marca = marca;
                }
                 
               

                if ((!string.IsNullOrEmpty(marca.LocalidadMarca)) && (marca.LocalidadMarca.Equals("I")))
                    this._ventana.MarcarRadioMarcaNacional(false);
                else
                    this._ventana.MarcarRadioMarcaNacional(true);

                Anaqua anaqua = new Anaqua();
                anaqua.IdMarca = marca.Id;
                InfoAdicional infoAdicional = new InfoAdicional("M." + marca.Id);

                marca.InfoBoles = this._infoBolServicios.ConsultarInfoBolesPorMarca(marca);
                marca.Operaciones = this._operacionServicios.ConsultarOperacionesPorMarca(marca);
                marca.Busquedas = this._busquedaServicios.ConsultarBusquedasPorMarca(marca);
                marca.InstruccionesDeRenovacion = this._instruccionDeRenovacionServicios.ConsultarInstruccionesDeRenovacionPorMarca(marca);
                marca.InfoAdicional = this._infoAdicionalServicios.ConsultarPorId(infoAdicional);
                marca.Anaqua = this._anaquaServicios.ConsultarPorId(anaqua);

                //---
                string distingue = _marcaServicios.ObtenerDistingueDeMarca(marca);
                this._ventana.DistingueSolicitud = distingue;
                this._ventana.DistingueDatos = distingue;
                marca.Distingue = distingue;
                ((Marca)this._ventana.Marca).Distingue = distingue;
                //---

                IList<ListaDatosDominio> tiposMarcas = this._listaDatosDominioServicios.
                    ConsultarListaDatosDominioPorParametro(new ListaDatosDominio(Recursos.Etiquetas.cbiCategoriaMarca));
                ListaDatosDominio primerTipoMarca = new ListaDatosDominio();
                primerTipoMarca.Id = "NGN";
                tiposMarcas.Insert(0, primerTipoMarca);
                this._ventana.TipoMarcasDatos = tiposMarcas;
                this._ventana.TipoMarcasSolicitud = tiposMarcas;
                this._ventana.TipoMarcaDatos = this.BuscarTipoMarca(tiposMarcas, marca.Tipo);

                /*IList<Agente> agentes = this._agenteServicios.ConsultarTodos();
                Agente primerAgente = new Agente();
                primerAgente.Id = "NGN";
                agentes.Insert(0, primerAgente);
                this._ventana.Agentes = agentes;
                this._ventana.Agente = this.BuscarAgente(agentes, marca.Agente);*/

                IList<Pais> paises = this._paisServicios.ConsultarTodos();
                Pais primerPais = new Pais();
                primerPais.Id = int.MinValue;
                paises.Insert(0, primerPais);
                this._ventana.PaisesSolicitud = paises;
                this._ventana.PaisSolicitud = this.BuscarPais(paises, marca.Pais);

                IList<StatusWeb> statusWebs = this._statusWebServicios.ConsultarTodos();
                StatusWeb primerStatus = new StatusWeb();
                primerStatus.Id = "NGN";
                statusWebs.Insert(0, primerStatus);
                this._ventana.StatusWebs = statusWebs;
                this._ventana.StatusWeb = this.BuscarStatusWeb(statusWebs, marca.StatusWeb);

                IList<Condicion> condiciones = this._condicionServicios.ConsultarTodos();
                Condicion primeraCondicion = new Condicion();
                primeraCondicion.Id = int.MinValue;
                condiciones.Insert(0, primeraCondicion);
                this._ventana.Condiciones = condiciones;

                Condicion condicionAuxiliar = new Condicion(((Marca)this._ventana.Marca).NumeroCondiciones);
                this._ventana.Condicion = this.BuscarCondicion((IList<Condicion>)this._ventana.Condiciones, condicionAuxiliar);

                IList<TipoEstado> tipoEstados = this._tipoEstadoServicios.ConsultarTodos();
                TipoEstado primerDetalle = new TipoEstado();
                primerDetalle.Id = "";
                tipoEstados.Insert(0, primerDetalle);
                this._ventana.Detalles = tipoEstados;

                if (null != marca.TipoEstado)
                    this._ventana.Detalle = this.BuscarDetalle(tipoEstados, marca.TipoEstado);


                IList<Servicio> servicios = this._servicioServicios.ConsultarTodos(); 
                this._ventana.Servicios = servicios;
                this._ventana.Servicio = this.BuscarServicio(servicios, marca.Servicio);


                IList<Boletin> boletines = this._boletinServicios.ConsultarTodos();
                Boletin primerBoletin = new Boletin();
                primerBoletin.Id = int.MinValue;
                boletines.Insert(0, primerBoletin);
                this._ventana.BoletinesOrdenPublicacion = boletines;
                this._ventana.BoletinesPublicacion = boletines;
                this._ventana.BoletinesConcesion = boletines;
                this._ventana.BoletinOrdenPublicacion = this.BuscarBoletin(boletines, marca.BoletinOrdenPublicacion);
                this._ventana.BoletinConcesion = this.BuscarBoletin(boletines, marca.BoletinConcesion);
                this._ventana.BoletinPublicacion = this.BuscarBoletin(boletines, marca.BoletinPublicacion);


                Interesado interesado = (this._interesadoServicios.ConsultarInteresadoConTodo(marca.Interesado));


                this._ventana.NombreInteresadoDatos = interesado.Nombre;
                this._ventana.NombreInteresadoSolicitud = interesado.Nombre;
                this._ventana.InteresadoPaisSolicitud = interesado.Pais.NombreEspanol;
                this._ventana.InteresadoCiudadSolicitud = interesado.Ciudad;

                IList<Interesado> listaInteresado = new List<Interesado>();
                //Interesado primerInteresado = new Interesado(0);
                //listaInteresado.Add(primerInteresado);
                listaInteresado.Add(this._marca.Interesado);

                this._ventana.InteresadosSolicitud = listaInteresado;
                this._ventana.InteresadosDatos = listaInteresado;

                this._ventana.InteresadoSolicitud = this.BuscarInteresado((IList<Interesado>)this._ventana.InteresadosSolicitud, ((Marca)this._ventana.Marca).Interesado);
                this._ventana.InteresadoDatos = this.BuscarInteresado((IList<Interesado>)this._ventana.InteresadosDatos, ((Marca)this._ventana.Marca).Interesado);

                if (interesado.Alerta != null)
                {
                    if (!interesado.Alerta.Equals(""))
                    {
                        alertaInteresado += "Alerta de Interesado: " + interesado.Alerta;
                        this._ventana.Mensaje(alertaInteresado, 2);
                    }
                }


                IList<Asociado> listaAsociado = new List<Asociado>();
                Asociado primerAsociado = new Asociado(int.MinValue);
                listaAsociado.Add(primerAsociado);
                listaAsociado.Add(this._marca.Asociado);

                this._ventana.AsociadosSolicitud = listaAsociado;
                this._ventana.AsociadosDatos = listaAsociado;

                this._ventana.AsociadoSolicitud = this.BuscarAsociado((IList<Asociado>)this._ventana.AsociadosSolicitud, ((Marca)this._ventana.Marca).Asociado);
                this._ventana.AsociadoDatos = this.BuscarAsociado((IList<Asociado>)this._ventana.AsociadosDatos, ((Marca)this._ventana.Marca).Asociado);

                this._ventana.NombreAsociadoDatos = marca.Asociado != null ? marca.Asociado.Nombre : "";
                this._ventana.NombreAsociadoSolicitud = marca.Asociado != null ? marca.Asociado.Nombre : "";

                if ((null != marca.Asociado) && (null != marca.Asociado.TipoCliente))
                    this._ventana.PintarAsociado(marca.Asociado.TipoCliente.Id);
                else if (null != marca.Asociado)
                    this._ventana.PintarAsociado("1");

                if (marca.Corresponsal != null)
                {
                    //this._ventana.DescripcionCorresponsalSolicitud = marca.Corresponsal.GetType().Equals(typeof(Corresponsal)) ? marca.Corresponsal.Descripcion : "";
                    //this._ventana.DescripcionCorresponsalDatos = marca.Corresponsal.GetType().Equals(typeof(Corresponsal)) ? marca.Corresponsal.Descripcion : "";
                    CargarCorresponsales();
                }

                IList<Agente> agentes = this._agenteServicios.ConsultarTodos();
                Agente primerAgente = new Agente();
                primerAgente.Id = "NGN";
                agentes.Insert(0, primerAgente);
                this._ventana.Agentes = agentes;
                this._ventana.Agente = this.BuscarAgente(agentes, marca.Agente);


                this._ventana.NumPoderDatos = marca.Poder != null ? marca.Poder.NumPoder : "";

                if (marca.Poder != null)
                {
                    this._ventana.IdPoderDatos = marca.Poder.Id.ToString();
                    this._ventana.IdPoderSolicitud = marca.Poder.Id.ToString();


                    IList<Poder> poderes = new List<Poder>();
                    poderes.Add(marca.Poder);

                    this._ventana.PoderesDatos = poderes;
                    this._ventana.PoderesSolicitud = poderes;

                    this._ventana.PoderSolicitud = marca.Poder;
                    this._ventana.PoderDatos = marca.Poder;

                }
                else
                {
                    this._ventana.IdPoderDatos = "";
                    this._ventana.IdPoderSolicitud = "";
                }

                //this._ventana.NumPoderSolicitud = marca.Poder != null ? marca.Poder.NumPoder : "";

                IList<ListaDatosDominio> sectores = this._listaDatosDominioServicios.
                    ConsultarListaDatosDominioPorParametro(new ListaDatosDominio(Recursos.Etiquetas.cbiSector));
                ListaDatosDominio primerSector = new ListaDatosDominio();
                primerSector.Id = "NGN";
                sectores.Insert(0, primerSector);
                this._ventana.Sectores = sectores;
                this._ventana.Sector = this.BuscarSector(sectores, marca.Sector);

                IList<ListaDatosDominio> tipoReproducciones = this._listaDatosDominioServicios.
                    ConsultarListaDatosDominioPorParametro(new ListaDatosDominio(Recursos.Etiquetas.cbiTipoReproduccion));
                ListaDatosDominio primerTipoReproduccion = new ListaDatosDominio();
                primerTipoReproduccion.Id = "NGN";
                tipoReproducciones.Insert(0, primerTipoReproduccion);
                this._ventana.TipoReproducciones = tipoReproducciones;
                //this._ventana.TipoReproduccion = this.BuscarTipoReproduccion(tipoReproducciones, marca.Tipo);
                this._ventana.TipoReproduccion = this.BuscarTipoReproduccion(tipoReproducciones,marca.TipoRps.ToString());
                
                IList<ListaDatosDominio> tipoClasesNacional = this._listaDatosDominioServicios.
                    ConsultarListaDatosDominioPorParametro(new ListaDatosDominio(Recursos.Etiquetas.cbiCategoriaTipoClaseNacional));
                ListaDatosDominio primerTipoClase = new ListaDatosDominio();
                primerTipoClase.Id = "NGN";
                tipoClasesNacional.Insert(0, primerTipoClase);
                this._ventana.TiposClaseNacional = tipoClasesNacional;
                this._ventana.TipoClaseNacional = this.BuscarClaseNacional(tipoClasesNacional, marca.TipoCnac);

                //string prueba = ConfigurationManager.AppSettings["RutaImagenesDeMarcas"] + marca.Id + ".BMP";

                if (File.Exists(ConfigurationManager.AppSettings["RutaImagenesDeMarcas"] + marca.Id + ".BMP"))
                {
                    //marca.BEtiqueta = true;
                    this._ventana.PintarEtiqueta();
                }
                //else
                //    marca.BEtiqueta = false;



                Auditoria auditoria = new Auditoria();
                auditoria.Fk = ((Marca)this._ventana.Marca).Id;
                auditoria.Tabla = "MYP_MARCAS";
                this._auditorias = this._marcaServicios.AuditoriaPorFkyTabla(auditoria);

                Renovacion renovacion = new Renovacion();
                renovacion.Marca = marca;
                IList<Renovacion> renovaciones = this._renovacionServicios.ObtenerRenovacionFiltro(renovacion);

                if (marca.LocalidadMarca.Equals("N"))
                {
                    archivoMarca = new Archivo(marca.Id.ToString());
                    archivoMarca.TipoDeDocumento = "M";
                    archivoMarca = this._archivoServicios.ConsultarPorId(archivoMarca);
                }
                else if (marca.LocalidadMarca.Equals("I"))
                {
                    if ((marca.CodigoMarcaInternacional != 0) && (marca.CorrelativoExpediente != 0))
                    {
                        archivoMarca = new Archivo(marca.CodigoMarcaInternacional.ToString(), marca.CorrelativoExpediente.ToString());
                        archivoMarca.TipoDeDocumento = "I";
                        archivoMarca = this._archivoServicios.ObtenerArchivoDeMarcaOPatenteInternacional(archivoMarca);
                    }
                    else
                    {
                        archivoMarca = new Archivo(marca.Id.ToString());
                        archivoMarca.TipoDeDocumento = "M";
                        archivoMarca = this._archivoServicios.ConsultarPorId(archivoMarca);
                    }
                }

                if (renovaciones.Count > 0)
                    this._ventana.PintarRenovacion();

                if (null != marca.InfoAdicional && !string.IsNullOrEmpty(marca.InfoAdicional.Id))
                    this._ventana.PintarInfoAdicional();

                if (null != marca.Anaqua)
                    this._ventana.PintarAnaqua();

                if (null != marca.InfoBoles && marca.InfoBoles.Count > 0)
                    this._ventana.PintarInfoBoles();

                if (null != marca.Operaciones && marca.Operaciones.Count > 0)
                    this._ventana.PintarOperaciones();

                if (null != marca.Busquedas && marca.Busquedas.Count > 0)
                    this._ventana.PintarBusquedas();

                if (null != this._auditorias && this._auditorias.Count > 0)
                    this._ventana.PintarAuditoria();

                if ((null != marca.InstruccionesDeRenovacion) 
                    && (marca.InstruccionesDeRenovacion.Count > 0))
                    this._ventana.PintarInstRenovacion();

                if (null != archivoMarca)
                {
                    this._ventana.PintarArchivo();
                }

                CertificadoMarca certificadoConsultar = new CertificadoMarca();
                CertificadoMarca certificado = null;
                certificadoConsultar.IdMarca = this._marca.Id;

                certificado = this._certificadoMarcaServicios.ConsultarPorId(certificadoConsultar);

                if (certificado != null)
                    this._ventana.PintarCertificado();

                FacVistaFacturaServicio facVistaFacServicio = new FacVistaFacturaServicio();
                facVistaFacServicio.Id = this._marca.Id;
                facVistaFacServicio.Tipo = "M";

                IList<FacVistaFacturaServicio> listaFacturas = 
                    this._facVistaFacturaServicioServicios.ObtenerFacVistaFacturaServiciosFiltro(facVistaFacServicio);

                if (listaFacturas.Count > 0)
                    this._ventana.PintarFacturacion();


                String mensaje = String.Format("La marca {0} ", this._marca.Id.ToString());
                String cadenaMensaje = String.Empty;
                bool flag = false;

                #region Instrucciones de Correspondencia 

                InstruccionCorrespondencia instruccionEnvioEmails = new InstruccionCorrespondencia();
                InstruccionCorrespondencia instruccion = null;
                instruccionEnvioEmails.Id = this._marca.Id;
                instruccionEnvioEmails.AplicadaA = "M";
                instruccionEnvioEmails.Concepto = "C";

                InstruccionEnvioOriginales instruccionEnvioOriginales = new InstruccionEnvioOriginales();
                InstruccionEnvioOriginales instruccionEO = null;
                instruccionEnvioOriginales.Id = this._marca.Id;
                instruccionEnvioOriginales.AplicadaA = "M";
                instruccionEnvioOriginales.Concepto = "C";

                instruccion = this._instruccionCorrespondenciaServicios.ObtenerInstruccionCorrespondencia(instruccionEnvioEmails);
                instruccionEO = this._instruccionEnvioOriginalesServicios.ObtenerInstruccionEnvioOriginales(instruccionEnvioOriginales);
                if ((instruccion != null) || (instruccionEO != null))
                {
                    this._ventana.PintarIconoBotonCorrespondencia();
                }

                if(instruccion != null) 
                {
                    flag = true;
                    cadenaMensaje += " tiene Instrucción de Correspondencia por Envío de Email";
                }

                if (instruccionEO != null)
                {
                    if (flag)
                    {
                        cadenaMensaje += ", " + " tiene Instrucción de Correspondencia por Envío de Originales ";
                        flag = false;
                    }
                    else
                        cadenaMensaje += " tiene Instrucción de Correspondencia por Envío de Originales ";
                }

                 
                #endregion


                #region Instrucciones de Facturacion 

                InstruccionCorrespondencia instruccionFacEnvioEmails = new InstruccionCorrespondencia();
                InstruccionCorrespondencia instruccionFac = null;
                instruccionEnvioEmails.Id = this._marca.Id;
                instruccionEnvioEmails.AplicadaA = "M";
                instruccionEnvioEmails.Concepto = "F";

                InstruccionEnvioOriginales instruccionFacEnvioOriginales = new InstruccionEnvioOriginales();
                InstruccionEnvioOriginales instruccionFac_EO = null;
                instruccionFacEnvioOriginales.Id = this._marca.Id;
                instruccionFacEnvioOriginales.AplicadaA = "M";
                instruccionFacEnvioOriginales.Concepto = "F";

                instruccionFac = this._instruccionCorrespondenciaServicios.ObtenerInstruccionCorrespondencia(instruccionFacEnvioEmails);
                instruccionFac_EO = this._instruccionEnvioOriginalesServicios.ObtenerInstruccionEnvioOriginales(instruccionFacEnvioOriginales);
                if ((instruccionFac != null) || (instruccionFac_EO != null))
                {
                    this._ventana.PintarIconoBotonFacturacion();
                }

                if (instruccionFac != null)
                {
                    cadenaMensaje += " tiene Instrucción de Facturación por Envío de Email ";
                    flag = true;
                }

                if (instruccionFac_EO != null)
                {
                    if (flag)
                    {
                        cadenaMensaje += "," + " tiene Instrucción de Facturación por Envío de Originales ";
                    }
                    else
                    {
                        cadenaMensaje += " tiene Instrucción de Facturación por Envío de Originales ";
                        flag = true;
                    }
                }

                #endregion

                #region Instrucciones de Descuento de Marca

                InstruccionDescuento instruccionDescuentoFiltro = new InstruccionDescuento();
                instruccionDescuentoFiltro.CodigoOperacion = marca.Id;
                instruccionDescuentoFiltro.AplicaA = "M";

                IList<InstruccionDescuento> instruccionesD = 
                    this._instruccionDescuentoServicios.ObtenerInstruccionesDeDescuentoMarcaOPatente(instruccionDescuentoFiltro);

                if (instruccionesD.Count > 0)
                {
                    this._ventana.PintarIconoBotonDescuento();
                    if (flag)
                    {
                        cadenaMensaje += "," + String.Format(" tiene {0} Instruccion(es) de Descuento ", instruccionesD.Count.ToString());
                    }
                    else
                    {
                        cadenaMensaje += String.Format(" tiene {0} Instruccion(es) de Descuento ", instruccionesD.Count.ToString());
                        flag = true;
                    }
                }

                #endregion

                #region Instrucciones No Tipificadas  De Marca

                InstruccionOtros instruccionNoTipificadaFiltro = new InstruccionOtros();
                instruccionNoTipificadaFiltro.Cod_MarcaOPatente = marca.Id;
                instruccionNoTipificadaFiltro.AplicaA = "M";

                IList<InstruccionOtros> instrucciones = 
                    this._instruccionOtrosServicios.ObtenerInstruccionesNoTipificadasPorFiltro(instruccionNoTipificadaFiltro);

                if (instrucciones.Count > 0)
                {
                    this._ventana.PintarIconoBotonOtros();
                    if(flag)
                        cadenaMensaje += "," + String.Format(" tiene {0} Instruccion(es) de Otro tipo ", instrucciones.Count.ToString());
                    else
                        cadenaMensaje += String.Format(" tiene {0} Instruccion(es) de Otro tipo ", instrucciones.Count.ToString());
                }

                #endregion

                #region Origen de Marca

                ListaDatosValores primerOrigenMarca = new ListaDatosValores();
                primerOrigenMarca.Id = "NGN";
                this._origenDeMarcas =
                            this._listaDatosValoresServicios.ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiOrigenClienteAsociado));
                this._origenDeMarcas.Insert(0, primerOrigenMarca);
                this._ventana.OrigenMarcasSolicitud = this._origenDeMarcas;
                this._ventana.OrigenMarcasDatos = this._origenDeMarcas;
                ListaDatosValores origenMarca = new ListaDatosValores();

                if (marca.OrigenMarca != null)
                {
                    origenMarca.Valor = marca.OrigenMarca;                    
                }
                else
                {
                    origenMarca.Valor = "";
                }

                this._ventana.OrigenMarcaSolicitud = this.BuscarListaDeDatosValores(this._origenDeMarcas, origenMarca);
                this._ventana.OrigenMarcaDatos = this.BuscarListaDeDatosValores(this._origenDeMarcas, origenMarca);

                #endregion
                


                #region Marca de Origen

                //Pintando el label para indicar que la marca cargada en pantalla es la Marca Origen
                if (marcaOrigenCargada)
                    this._ventana.PintarLblMarcaOrigen(marcaOrigenCargada);

                //Pintando el codigo de la marca origen en el textbox de la pestana Solicitud
                this._ventana.IdMarcaOrigenSolicitud = this._marca.MarcaOrigen.ToString();
                this._ventana.IdMarcaOrigenDatos = this._marca.MarcaOrigen.ToString();
                //Generando una lista de Marcas Origen para la lista de marca origen en pestana Solicitud
                int? idMarcaOrigen = marca.MarcaOrigen;
                
                //if (idMarcaOrigen != 0)
                if(null != idMarcaOrigen)
                {
                    Marca marcaOrigen = new Marca(idMarcaOrigen.Value);

                    marcaOrigen = this._marcaServicios.ConsultarMarcaConTodo(marcaOrigen);
                    IList<Marca> listaMarcas = new List<Marca>();
                    Marca primeraMarca = new Marca(int.MinValue);
                    //listaMarcas.Insert(0, primeraMarca);
                    listaMarcas.Add(primeraMarca);
                    listaMarcas.Add(marcaOrigen);
                    //Asociamos la lista de Marcas de Origen con la lista en la ventana
                    this._ventana.MarcaOrigenSolicitud = listaMarcas;
                    this._ventana.MarcaOrigenDatos = listaMarcas;
                    this._ventana.MarcasOrigenSolicitud = this.BuscarMarca((IList<Marca>)this._ventana.MarcaOrigenSolicitud, listaMarcas[1]);
                    this._ventana.MarcasOrigenDatos = this.BuscarMarca((IList<Marca>)this._ventana.MarcaOrigenDatos, listaMarcas[1]);
                }
                else
                {
                    this._ventana.IdMarcaOrigenSolicitud = null;
                    this._ventana.IdMarcaOrigenDatos = null;
                }
                
                #endregion


                #region Expediente Traspaso Renovacion

                //Pintando el Id del Expediente de Traspaso de Renovacion
                this._ventana.IdExpTraspasoRenovacionDatos = marca.ExpTraspasoRenovacion;
                this._ventana.IdExpTraspasoRenovacionSolicitud = marca.ExpTraspasoRenovacion;

                #endregion

                #region Internacional

                IList<Pais> paisesInternacionales = this._paisServicios.ConsultarTodos();
                Pais primerPaisInt = new Pais();
                primerPais.Id = int.MinValue;
                paisesInternacionales.Insert(0, primerPais);
                this._ventana.PaisesInternacionales = paisesInternacionales;
                this._ventana.PaisesInternacionalesDatos = paisesInternacionales;

                if (null != marca.PaisInternacional)
                {
                    Pais paisABuscar = new Pais(marca.PaisInternacional.Id);
                    this._ventana.PaisInternacional = this.BuscarPais(paisesInternacionales, paisABuscar);
                    this._ventana.PaisInternacionalDatos = this.BuscarPais(paisesInternacionales, paisABuscar);
                }

                IList<ListaDatosValores> localidades = this._listaDatosValoresServicios.
                    ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiLocalidadMarca));

                ListaDatosValores primerLocalidad = new ListaDatosValores();
                primerLocalidad.Descripcion = string.Empty;
                primerLocalidad.Valor = "NGN";

                localidades.Insert(0, primerLocalidad);
                this._ventana.TipoClaseInternacionales = localidades;
                this._ventana.TipoClaseInternacionalesDatos = localidades;

                if ((null != marca.LocalidadMarca) && (!marca.LocalidadMarca.Equals("")))
                {
                    ListaDatosValores localidadABuscar = new ListaDatosValores(marca.LocalidadMarca);
                    this._ventana.TipoClaseInternacional = this.BuscarLocalidad(localidades, localidadABuscar);
                    this._ventana.TipoClaseInternacionalDatos = this.BuscarLocalidad(localidades, localidadABuscar);
                }

                if (null != marca.AsociadoInternacional)
                {
                    IList<Asociado> asociados = new List<Asociado>();
                    asociados.Add(marca.AsociadoInternacional);

                    this._ventana.AsociadosInternacionales = asociados;
                    this._ventana.AsociadoInternacional = marca.AsociadoInternacional;

                    this._ventana.AsociadosInternacionalesDatos = asociados;
                    this._ventana.AsociadoInternacionalDatos = marca.AsociadoInternacional;

                    this._ventana.TextoAsociadoInternacional = marca.AsociadoInternacional.Nombre;
                }
                else
                    CargarAsociadoInternacionalVacio();
                #endregion

                this._ventana.BorrarCeros();

                this._ventana.FocoPredeterminado();

                CalcularSaldos();

                if ((instruccion != null || instruccionEO != null) || (instruccionFac != null || instruccionFac_EO != null) || 
                    (instruccionesD.Count > 0) || (instrucciones.Count > 0))
                {
                    this._ventana.Mensaje(mensaje + cadenaMensaje, 2); 
                }

                if (this._marca.ExpTraspasoRenovacion != null)
                {
                    String ruta = ConfigurationManager.AppSettings["RutaExpedienteTyRMarca"].ToString() + this._marca.ExpTraspasoRenovacion + ".pdf";
                    if (File.Exists(ruta))
                        this._ventana.PintarBotonExpTyR();
                }

                IList<InteresadoMultiple> interesadosAdicionales = this._interesadoMultipleServicios.ConsultarInteresadosDeMarca(this._marca);
                if (interesadosAdicionales.Count > 0)
                    this._ventana.PintarOtrosInteresados();

                

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


        /// <summary>
        /// Método que carga la ventana de consulta marcas
        /// </summary>
        public void IrConsultarMarcas()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            if (this._ventanaPadre != null)
            {
                this.Navegar((Page)_ventanaPadre);
            }
            else
                this.Navegar(new ConsultarMarcas());

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que guardar los datos de la ventana y los almacena en las variables
        /// </summary>
        /// <returns></returns>
        public Marca CargarMarcaDeLaPantalla()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            Marca marca = (Marca)this._ventana.Marca;

            marca.Operacion = "MODIFY";

            if (null != this._ventana.Agente)
                marca.Agente = !((Agente)this._ventana.Agente).Id.Equals("NGN") ? (Agente)this._ventana.Agente : null;

            if (null != this._ventana.AsociadoSolicitud)
                marca.Asociado = ((Asociado)this._ventana.AsociadoSolicitud).Id != int.MinValue ? (Asociado)this._ventana.AsociadoSolicitud : null;
            else if (!this._ventana.IdAsociadoSolicitud.Equals(""))
                marca.Asociado = new Asociado(int.Parse(this._ventana.IdAsociadoSolicitud));


            if (null != this._ventana.BoletinOrdenPublicacion)
                marca.BoletinOrdenPublicacion = ((Boletin)this._ventana.BoletinOrdenPublicacion).Id != int.MinValue ? (Boletin)this._ventana.BoletinOrdenPublicacion : null;

            if (null != this._ventana.BoletinConcesion)
                marca.BoletinConcesion = ((Boletin)this._ventana.BoletinConcesion).Id != int.MinValue ? (Boletin)this._ventana.BoletinConcesion : null;

            if (null != this._ventana.BoletinPublicacion)
                marca.BoletinPublicacion = ((Boletin)this._ventana.BoletinPublicacion).Id != int.MinValue ? (Boletin)this._ventana.BoletinPublicacion : null;

            if (null != this._ventana.InteresadoSolicitud)
                marca.Interesado = !((Interesado)this._ventana.InteresadoSolicitud).Id.Equals("NGN") ? ((Interesado)this._ventana.InteresadoSolicitud) : null;
            else if (!this._ventana.IdInteresadoSolicitud.Equals(""))
                marca.Interesado = new Interesado(int.Parse(this._ventana.IdInteresadoSolicitud));

            if (null != this._ventana.Servicio)
                marca.Servicio = !((Servicio)this._ventana.Servicio).Id.Equals("") ? ((Servicio)this._ventana.Servicio) : null;

            if (null != this._ventana.Detalle)
                marca.TipoEstado = !((TipoEstado)this._ventana.Detalle).Id.Equals("") ? ((TipoEstado)this._ventana.Detalle) : null;

            if (null != this._ventana.PoderSolicitud)
                marca.Poder = ((Poder)this._ventana.PoderSolicitud).Id != int.MinValue ? ((Poder)this._ventana.PoderSolicitud) : null;

            if (null != this._ventana.PaisSolicitud)
                marca.Pais = ((Pais)this._ventana.PaisSolicitud).Id != int.MinValue ? ((Pais)this._ventana.PaisSolicitud) : null;

            if (null != this._ventana.StatusWeb)
                marca.StatusWeb = ((StatusWeb)this._ventana.StatusWeb).Id.Equals("NGN") ? ((StatusWeb)this._ventana.StatusWeb) : null;

            if (null != this._ventana.CorresponsalSolicitud)
                marca.Corresponsal = ((Corresponsal)this._ventana.CorresponsalSolicitud).Id != int.MinValue ? ((Corresponsal)this._ventana.CorresponsalSolicitud) : null;

            if (null != this._ventana.Sector)
                marca.Sector = !((ListaDatosDominio)this._ventana.Sector).Id.Equals("NGN") ? ((ListaDatosDominio)this._ventana.Sector).Id : null;

            if (null != this._ventana.TipoClaseNacional)
                marca.TipoCnac = !((ListaDatosDominio)this._ventana.TipoClaseNacional).Id.Equals("NGN") ? ((ListaDatosDominio)this._ventana.TipoClaseNacional).Id : null;

            if (null != this._ventana.TipoReproduccion)
                marca.TipoRps = ((ListaDatosDominio)this._ventana.TipoReproduccion).Id[0];

            if (null != this._ventana.TipoMarcaDatos)
                marca.Tipo = !((ListaDatosDominio)this._ventana.TipoMarcaDatos).Id.Equals("NGN") ? ((ListaDatosDominio)this._ventana.TipoMarcaDatos).Id : null;

            if (string.IsNullOrEmpty(this._ventana.IdInternacional))
                marca.Internacional = null;

            if (string.IsNullOrEmpty(this._ventana.IdNacional))
                marca.Nacional = null;

            if (null != this._ventana.Condicion)
            {
                if (((Condicion)this._ventana.Condicion).Id == int.MinValue)
                    marca.NumeroCondiciones = null;
                else
                    marca.NumeroCondiciones = ((Condicion)this._ventana.Condicion).Id;
            }

            if (this._ventana.DistingueSolicitud != null)
            {
                marca.Distingue = this._ventana.DistingueSolicitud;

                int len = this._ventana.DistingueSolicitud.Length;
                if (len <= 2000)
                    marca.Fichas = this._ventana.DistingueSolicitud.ToUpper();
                else
                {
                    String fichasAux = this._ventana.DistingueSolicitud.Substring(0, 1999);
                    marca.Fichas = fichasAux.ToUpper();
                    int longitud = marca.Fichas.Length;
                }

            }

            if ((this._ventana.OrigenMarcaSolicitud != null) && (!((ListaDatosValores)this._ventana.OrigenMarcaSolicitud).Id.Equals("NGN")))
            {
                marca.OrigenMarca = ((ListaDatosValores)this._ventana.OrigenMarcaSolicitud).Descripcion;
            }
            else
                marca.OrigenMarca = null;
          
            


               
            //if (null != ((Marca)this._ventana.Marca).MarcaOrigen)
            //    marca.MarcaOrigen = ((Marca)this._ventana.Marca).MarcaOrigen;

            return marca;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que se encarga de cambiar el boton y habilitar los campos de la ventana para
        /// su modificación
        /// </summary>
        public void CambiarAModificar()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this._ventana.HabilitarCampos = true;
            this._ventana.TextoBotonModificar = Recursos.Etiquetas.btnAceptar;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que dependiendo del estado de la página, habilita los campos o 
        /// modifica los datos de la Marca
        /// </summary>
        public void Modificar()
        {
            String distingueDeMarca = String.Empty;
            bool realizado = false;
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

                //Modifica los datos del Pais
                else
                {
                    if (ValidarPoder())
                    {
                        Marca marca = CargarMarcaDeLaPantalla();

                        //---
                        if ((this._ventana.DistingueSolicitud != null) && (!this._ventana.DistingueSolicitud.Equals("")))
                        {
                            distingueDeMarca = this._ventana.DistingueSolicitud;                            
                        }

                        //---

                        if (ValidarMarcaInternacional())
                        {

                            if (!this._ventana.EsMarcaNacional)
                                marca = AgregarDatosInternacionales(marca);
                            else
                                marca.LocalidadMarca = "N";

                            if (marca.Interesado != null)
                            {
                                //if (marca.Poder != null)
                                //{
                                marca.XDistingue = "";
                                bool exitoso = this._marcaServicios.InsertarOModificar(marca, UsuarioLogeado.Hash);

                                if (exitoso)
                                {
                                    if(distingueDeMarca != null)
                                        realizado = this._marcaServicios.ActualizarDistingueDeMarca(marca,distingueDeMarca.ToUpper());
                                    this._ventana.HabilitarCampos = false;
                                    this._ventana.TextoBotonModificar = Recursos.Etiquetas.btnModificar;

                                    //if (marca.Servicio.Id.Equals("AB"))
                                    //{
                                    //    this._ventana.DeshabilitarBotonModificar();
                                    //}
                                }
                                //}
                                //else
                                //    this._ventana.Mensaje(Recursos.MensajesConElUsuario.ErrorSinPoder, 0);
                            }
                            else
                                this._ventana.Mensaje(Recursos.MensajesConElUsuario.ErrorSinInteresado, 0);
                        }
                        else
                            this._ventana.Mensaje(Recursos.MensajesConElUsuario.ErrorMarcaInternacional, 0);
                    }
                    else
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.ErrorInteresadoNoPoseePoderesConAgente, 0);

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


        /// <summary>
        /// Método que se encarga de validar la marca internacional antes de modificarla
        /// </summary>
        /// <returns></returns>
        private bool ValidarMarcaInternacional()
        {
            bool retorno = true;

            if (!this._ventana.EsMarcaNacional)
                if (((Pais)this._ventana.PaisInternacional).Id == int.MinValue)
                    retorno = false;
                else
                    if (((ListaDatosValores)this._ventana.TipoClaseInternacional).Valor.Equals("NGN"))
                        retorno = false;
                    else
                        if (this._ventana.ClaseInternacionalMarca.Equals(string.Empty))
                            retorno = false;

            return retorno;
        }


        /// <summary>
        /// Método que se encarga de agregarle los datos pertenecientes a las marcas internacionales
        /// </summary>
        /// <param name="marca"></param>
        /// <returns></returns>
        private Marca AgregarDatosInternacionales(Marca marca)
        {
            try
            {
                marca.LocalidadMarca = "I";
                marca.ClasificacionInternacional = ((ListaDatosValores)this._ventana.TipoClaseInternacionalDatos).Valor;
                marca.PaisInternacional = (Pais)this._ventana.PaisInternacional;
                marca.AsociadoInternacional = (Asociado)this._ventana.AsociadoInternacional;
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

            return marca;
        }


        /// <summary>
        /// Metodo que se encarga de eliminar una Marca
        /// </summary>
        public void Eliminar()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                //if (this._anexoServicios.Eliminar((Anexo)this._ventana.Anexo, UsuarioLogeado.Hash))
                //{
                //    _paginaPrincipal.MensajeUsuario = Recursos.MensajesConElUsuario.PaisEliminado;
                //    this.Navegar(_paginaPrincipal);
                //}

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
        /// Método que se ecarga la descripcion de la situacion
        /// </summary>
        /// <param name="tab"></param>
        public void DescripcionSituacion()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this._ventana.SituacionDescripcion = ((Servicio)this._ventana.Servicio).Descripcion;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que se ecarga la descripcion de la situacion
        /// </summary>
        /// <param name="tab"></param>
        public void DescripcionDetalle()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this._ventana.DetalleDescripcion = ((TipoEstado)this._ventana.Detalle).Descripcion;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que se encarga de mostrar la ventana de información adicional
        /// </summary>
        /// <param name="tab"></param>
        public void IrInfoAdicional(string tab)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this.Navegar(new GestionarInfoAdicional(CargarMarcaDeLaPantalla(), tab, this._ventanaPadre));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que se encarga de mostrar la ventana de InfoBoles
        /// </summary>
        public void IrInfoBoles()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            //this.Navegar(new ListaInfoBoles(CargarMarcaDeLaPantalla()));

            this.Navegar(new ListaInfoBoles(CargarMarcaDeLaPantalla(), this._ventana));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }



        /// <summary>
        /// Método que se encarga de mostrar la ventana de Archivo
        /// </summary>
        public void IrArchivo()
        {
            

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Archivo archivoConsultar = null;
                Archivo archivo = null;

                Marca marca = CargarMarcaDeLaPantalla();


                if (marca.LocalidadMarca.Equals("N"))
                {
                    archivoConsultar = new Archivo(marca.Id.ToString());
                    archivoConsultar.TipoDeDocumento = "M";
                    archivo = this._archivoServicios.ConsultarPorId(archivoConsultar);
                }
                else if (marca.LocalidadMarca.Equals("I"))
                {
                    if ((marca.CodigoMarcaInternacional != 0) && (marca.CorrelativoExpediente != 0))
                    {
                        archivoConsultar = new Archivo(marca.CodigoMarcaInternacional.ToString(), marca.CorrelativoExpediente.ToString());
                        archivoConsultar.TipoDeDocumento = "I";
                        archivo = this._archivoServicios.ObtenerArchivoDeMarcaOPatenteInternacional(archivoConsultar);
                    }
                    else
                    {
                        archivoConsultar = new Archivo(marca.Id.ToString());
                        archivoConsultar.TipoDeDocumento = "M";
                        archivo = this._archivoServicios.ConsultarPorId(archivoConsultar);
                    }
                    
                }

                
                if (archivo != null)
                    this.Navegar(new GestionarArchivoDeMarca(archivo, marca, this._ventana));
                else
                {
                    archivoConsultar.Fecha = DateTime.Today;

                    if (marca.LocalidadMarca.Equals("N"))
                        archivoConsultar.TipoDeDocumento = "M";
                    else if (marca.LocalidadMarca.Equals("I"))
                        archivoConsultar.TipoDeDocumento = "I";

                    archivoConsultar.Aux = "1";
                    this.Navegar(new GestionarArchivoDeMarca(archivoConsultar, marca, this._ventana));
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





        /// <summary>
        /// Método que se encarga de mostrar la ventana de las operaciones de la Marca
        /// </summary>
        public void IrOperaciones()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this.Navegar(new ListaOperaciones(CargarMarcaDeLaPantalla(),this._ventana));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que se encarga de mostrar la ventana de Anaqua de la Marca
        /// </summary>
        public void IrAnaqua()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this.Navegar(new GestionarAnaqua(CargarMarcaDeLaPantalla()));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que se encarga de mostrar la ventana de la lista de búsquedas de la marca
        /// </summary>
        /// <param name="tab"></param>
        public void IrBusquedas(string tab)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this.Navegar(new ListaBusquedas(CargarMarcaDeLaPantalla(), tab));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que se encarga de mostrar la ventana con la lista de Auditorías
        /// </summary>
        public void Auditoria()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion


                Auditoria auditoria = new Auditoria();
                auditoria.Fk = ((Marca)this._ventana.Marca).Id;
                auditoria.Tabla = "MYP_MARCAS";
                this._auditorias = this._marcaServicios.AuditoriaPorFkyTabla(auditoria);
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


        /// <summary>
        /// Método que se encarga de duplicar la marca
        /// </summary>
        public void Duplicar()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this.Navegar(new AgregarMarca(CargarMarcaDeLaPantalla()));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

        }


        /// <summary>
        /// Metodo que abre el explorador de internet predeterminado del sistema a una página determinada
        /// </summary>
        public void IrSAPI()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                this.IrURL(ConfigurationManager.AppSettings["UrlSAPI"].ToString());

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
        }


        #region Metodos de los filtros de asociados

        /// <summary>
        /// Método que cambia asociado solicitud
        /// </summary>
        public void CambiarAsociadoSolicitud()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if ((Asociado)this._ventana.AsociadoSolicitud != null)
                {
                    Asociado asociado = this._asociadoServicios.ConsultarAsociadoConTodo((Asociado)this._ventana.AsociadoSolicitud);
                    this._ventana.NombreAsociadoSolicitud = ((Asociado)this._ventana.AsociadoSolicitud).Nombre;
                    this._ventana.IdAsociadoSolicitud = ((Asociado)this._ventana.AsociadoSolicitud).Id.ToString();
                    this._ventana.AsociadoDatos = (Asociado)this._ventana.AsociadoSolicitud;
                    this._ventana.NombreAsociadoDatos = ((Asociado)this._ventana.AsociadoSolicitud).Nombre;
                    this._ventana.IdAsociadoDatos = ((Asociado)this._ventana.AsociadoSolicitud).Id.ToString();

                    if (asociado != null)
                        if (asociado.TipoCliente != null)
                            this._ventana.PintarAsociado(asociado.TipoCliente.Id);
                        else
                            this._ventana.PintarAsociado("1");
                    else
                        this._ventana.PintarAsociado("5");

                    this._ventana.ConvertirEnteroMinimoABlanco();

                    CalcularSaldos();
                }

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException e)
            {
                this._ventana.NombreAsociadoSolicitud = "";
                this._ventana.NombreAsociadoDatos = "";
            }
        }


        /// <summary>
        /// Metodo para cambiar la marca de origen cuando se selecciona la lista de marcas de origen en 
        /// la pestana Solicitud de la ventana ConsultarMarca
        /// </summary>
        public void CambiarMarcaOrigenSolicitud()
        {

           Marca marcaOrigen;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (((Marca)this._ventana.MarcasOrigenSolicitud) != null &&
                        ((Marca)this._ventana.MarcasOrigenSolicitud).Id != int.MinValue)
                {
                    marcaOrigen = this._marcaServicios.ConsultarMarcaConTodo(((Marca)this._ventana.MarcasOrigenSolicitud));
                    this._ventana.IdMarcaOrigenSolicitud = marcaOrigen.Id.ToString();
                    ((Marca)this._ventana.Marca).MarcaOrigen = marcaOrigen.Id;
                    this._ventana.IdMarcaOrigenDatos = marcaOrigen.Id.ToString();
                    
                }
                else
                {
                    marcaOrigen = ((Marca)this._ventana.MarcasOrigenSolicitud);
                    this._ventana.IdMarcaOrigenSolicitud = null;
                    this._ventana.IdMarcaOrigenDatos = null;
                    //this._ventana.IdMarcaOrigenSolicitud = marcaOrigen.Id.ToString();
                    //this._ventana.IdMarcaOrigenDatos = marcaOrigen.Id.ToString();
                    ((Marca)this._ventana.Marca).MarcaOrigen = null;
                    //this._ventana.ConvertirEnteroMinimoABlanco();
                }


                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

            }
            catch (ApplicationException e)
            {
                this._ventana.IdMarcaOrigenSolicitud = String.Empty;
                this._ventana.IdMarcaOrigenDatos = String.Empty;
            }

        }


        /// <summary>
        /// Metodo para cambiar la marca de origen cuando se selecciona de la lista de marcas de origen
        /// en la pestaña Datos de la ventana ConsultarMarca
        /// </summary>
        public void CambiarMarcaOrigenDatos()
        {

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Marca marcaOrigen;

                if (((Marca)this._ventana.MarcasOrigenDatos) != null &&
                   ((Marca)this._ventana.MarcasOrigenDatos).Id != int.MinValue)
                {
                    marcaOrigen = this._marcaServicios.ConsultarMarcaConTodo(((Marca)this._ventana.MarcasOrigenDatos));
                    this._ventana.IdMarcaOrigenDatos = marcaOrigen.Id.ToString();
                    ((Marca)this._ventana.Marca).MarcaOrigen = marcaOrigen.Id;
                    this._ventana.IdMarcaOrigenSolicitud = marcaOrigen.Id.ToString();
                }
                else
                {
                    marcaOrigen = ((Marca)this._ventana.MarcasOrigenDatos);
                    this._ventana.IdMarcaOrigenDatos = null;
                    this._ventana.IdMarcaOrigenSolicitud = null;
                    //this._ventana.IdMarcaOrigenDatos = marcaOrigen.Id.ToString();
                    //this._ventana.IdMarcaOrigenSolicitud = marcaOrigen.Id.ToString();
                    ((Marca)this._ventana.Marca).MarcaOrigen = null;
                    //this._ventana.ConvertirEnteroMinimoABlanco();
                }


                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException e)
            {
                this._ventana.IdMarcaOrigenSolicitud = String.Empty;
                this._ventana.IdMarcaOrigenDatos = String.Empty;
            }

        }


        public void BuscarMarcaOrigen(String filtrarEnTab)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            Marca marcaABuscar = new Marca();

            switch (filtrarEnTab)
            {
                case "_btnConsultarMarcaOrigenSolicitud":
                    marcaABuscar.Id = this._ventana.IdMarcaOrigenSolicitudFiltrar.Equals("") ? 0 : int.Parse(this._ventana.IdMarcaOrigenSolicitudFiltrar);
                    break;

                case "_btnConsultarMarcaOrigenDatos":
                    //patenteABuscar.Id = this._ventana.IdPatenteMadreDatosFiltrar.Equals("") ? 0 : int.Parse(this._ventana.IdPatenteMadreDatosFiltrar);
                    marcaABuscar.Id = this._ventana.IdMarcaOrigenDatosFiltrar.Equals("") ? 0 : int.Parse(this._ventana.IdMarcaOrigenDatosFiltrar);
                    break;
            }

            IList<Marca> marcasFiltradas = this._marcaServicios.ObtenerMarcasFiltro(marcaABuscar);

            if (marcasFiltradas.Count != 0)
            {
                Marca primeraMarca = new Marca();
                primeraMarca.Id = int.MinValue;
                marcasFiltradas.Insert(0, primeraMarca);
                this._ventana.MarcaOrigenSolicitud = marcasFiltradas;
                this._ventana.MarcaOrigenDatos = marcasFiltradas;
                //this._ventana.PatenteMadreDatos = patentesFiltradas;
            }
            else
            {
                switch (filtrarEnTab)
                {
                    case "_btnConsultarMarcaOrigenSolicitud":
                        this._ventana.IdMarcaOrigenSolicitudFiltrar = null;
                        break;

                    case "_btnConsultarMarcaOrigenDatos":
                        this._ventana.IdMarcaOrigenDatosFiltrar = null;
                        break;
                }

            }

        }


        /// <summary>
        /// Método que cambia asociado datos
        /// </summary>
        public void CambiarAsociadoDatos()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if ((Asociado)this._ventana.AsociadoDatos != null)
                {
                    Asociado asociado = this._asociadoServicios.ConsultarAsociadoConTodo((Asociado)this._ventana.AsociadoDatos);
                    this._ventana.NombreAsociadoDatos = ((Asociado)this._ventana.AsociadoDatos).Nombre;
                    this._ventana.IdAsociadoDatos = ((Asociado)this._ventana.AsociadoDatos).Id.ToString();
                    this._ventana.AsociadoSolicitud = (Asociado)this._ventana.AsociadoDatos;
                    this._ventana.NombreAsociadoSolicitud = ((Asociado)this._ventana.AsociadoDatos).Nombre;
                    this._ventana.IdAsociadoSolicitud = ((Asociado)this._ventana.AsociadoDatos).Id.ToString();

                    if (asociado != null)
                        if (asociado.TipoCliente != null)
                            this._ventana.PintarAsociado(asociado.TipoCliente.Id);
                        else
                            this._ventana.PintarAsociado("1");
                    else
                        this._ventana.PintarAsociado("5");

                    this._ventana.ConvertirEnteroMinimoABlanco();
                }

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException e)
            {
                this._ventana.NombreAsociadoSolicitud = "";
                this._ventana.NombreAsociadoDatos = "";
            }
        }


        /// <summary>
        /// Método que filtra un asociado
        /// </summary>
        /// <param name="filtrarEn"></param>
        public void BuscarAsociado(int filtrarEn)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            Asociado primerAsociado = new Asociado(int.MinValue);


            Asociado asociado = new Asociado();
            IList<Asociado> asociadosFiltrados;
            if (filtrarEn == 0)
            {
                asociado.Nombre = this._ventana.NombreAsociadoSolicitudFiltrar.ToUpper();
                asociado.Id = this._ventana.IdAsociadoSolicitudFiltrar.Equals("") ? 0
                                  : int.Parse(this._ventana.IdAsociadoSolicitudFiltrar);
            }
            else
            {
                asociado.Nombre = this._ventana.NombreAsociadoDatosFiltrar.ToUpper();
                asociado.Id = this._ventana.IdAsociadoDatosFiltrar.Equals("") ? 0
                                  : int.Parse(this._ventana.IdAsociadoDatosFiltrar);
            }
            if ((!asociado.Nombre.Equals("")) || (asociado.Id != 0))
                asociadosFiltrados = this._asociadoServicios.ObtenerAsociadosFiltro(asociado);
            else
                asociadosFiltrados = new List<Asociado>();

            if (asociadosFiltrados.Count != 0)
            {
                asociadosFiltrados.Insert(0, primerAsociado);
                this._ventana.AsociadosSolicitud = asociadosFiltrados;
                //this._ventana.AsociadoSolicitud = primerAsociado;
                this._ventana.AsociadosDatos = asociadosFiltrados;
                //this._ventana.AsociadoDatos = primerAsociado;
            }
            else
            {
                asociadosFiltrados.Insert(0, primerAsociado);
                this._ventana.AsociadosSolicitud = this._asociados;
                this._ventana.AsociadoSolicitud = primerAsociado;
                this._ventana.AsociadosDatos = this._asociados;
                this._ventana.AsociadoDatos = primerAsociado;
                this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
            }


            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que carga los asociados
        /// </summary>
        public void CargarAsociados()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            //Mouse.OverrideCursor = Cursors.Wait;

            //Marca marca = (Marca)this._ventana.Marca;
            //IList<Asociado> asociados = this._asociadoServicios.ConsultarTodos();
            //Asociado primerAsociado = new Asociado();
            //primerAsociado.Id = int.MinValue;
            //asociados.Insert(0, primerAsociado);
            //this._ventana.AsociadosSolicitud = asociados;
            //this._ventana.AsociadosDatos = asociados;
            //this._ventana.AsociadoSolicitud = this.BuscarAsociado(asociados, marca.Asociado);
            //this._ventana.AsociadoDatos = this.BuscarAsociado(asociados, marca.Asociado);
            //this._ventana.NombreAsociadoDatos = ((Marca)this._ventana.Marca).Asociado.Nombre;
            //this._ventana.NombreAsociadoSolicitud = ((Marca)this._ventana.Marca).Asociado.Nombre;
            //this._asociados = asociados;
            //this._ventana.AsociadosEstanCargados = true;

            //Mouse.OverrideCursor = null;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        #endregion


        #region Metodos de los filtros de interesados

        /// <summary>
        /// Método que cambia interesado solicitud
        /// </summary>
        public void CambiarInteresadoSolicitud()
        {

            String alertaInteresado = String.Empty;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if ((Interesado)this._ventana.InteresadoSolicitud != null)
                {
                    _interesadoAnterior = (Interesado)this._ventana.InteresadoSolicitud;
                    Interesado interesadoAux = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.InteresadoSolicitud);

                    if (null != interesadoAux)
                    {
                        if (interesadoAux.Alerta != null)
                        {
                            if (!interesadoAux.Alerta.Equals(""))
                            {
                                alertaInteresado += "Alerta de Interesado: " + interesadoAux.Alerta;
                                this._ventana.Mensaje(alertaInteresado, 2);
                            }
                        }
                        this._ventana.NombreInteresadoSolicitud = ((Interesado)this._ventana.InteresadoSolicitud).Nombre;
                        this._ventana.IdInteresadoSolicitud = ((Interesado)this._ventana.InteresadoSolicitud).Id.ToString();
                        this._ventana.InteresadoDatos = (Interesado)this._ventana.InteresadoSolicitud;
                        this._ventana.NombreInteresadoDatos = ((Interesado)this._ventana.InteresadoSolicitud).Nombre;
                        this._ventana.IdInteresadoDatos = ((Interesado)this._ventana.InteresadoSolicitud).Id.ToString();
                        this._ventana.InteresadoPaisSolicitud = interesadoAux.Pais != null ? interesadoAux.Pais.NombreEspanol : "";
                        this._ventana.InteresadoCiudadSolicitud = interesadoAux.Ciudad != null ? interesadoAux.Ciudad : "";

                        //Poder poderAux = new Poder(int.MinValue);

                        //this._ventana.PoderDatos = null;
                        //this._ventana.PoderSolicitud = null;
                        //this._ventana.IdPoderDatos = "";
                        //this._ventana.IdPoderSolicitud = "";
                        //this._ventana.NumPoderDatos = "";
                    }
                    else
                    {
                        this._ventana.NombreInteresadoSolicitud = "";
                        this._ventana.IdInteresadoSolicitud = "";
                        this._ventana.InteresadoDatos = "";
                        this._ventana.NombreInteresadoDatos = "";
                        this._ventana.IdInteresadoDatos = "";
                        this._ventana.InteresadoPaisSolicitud = "";
                        this._ventana.InteresadoCiudadSolicitud = "";
                    }


                }
                else
                {
                    Interesado interesadoAux = new Interesado(int.Parse(this._ventana.IdInteresadoSolicitud));
                    _interesadoAnterior = this._interesadoServicios.ConsultarInteresadoConTodo(interesadoAux);
                }



                this._ventana.PoderesDatos = null;
                this._ventana.PoderesSolicitud = null;
                this._ventana.PoderDatos = null;
                this._ventana.PoderSolicitud = null;

                this._ventana.IdPoderDatos = string.Empty;
                this._ventana.IdPoderSolicitud = string.Empty;
                this._ventana.NumPoderDatos = string.Empty;

                this._ventana.ConvertirEnteroMinimoABlanco();

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException e)
            {
                this._ventana.NombreInteresadoSolicitud = "";
                this._ventana.NombreInteresadoDatos = "";
                this._ventana.InteresadoPaisSolicitud = "";
                this._ventana.InteresadoCiudadSolicitud = "";
            }
        }


        /// <summary>
        /// Método que cambia interesado datos
        /// </summary>
        public void CambiarInteresadoDatos()
        {

            String alertaInteresado = String.Empty;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if ((Interesado)this._ventana.InteresadoDatos != null)
                {
                    _interesadoAnterior = (Interesado)this._ventana.InteresadoSolicitud;
                    Interesado interesadoAux = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.InteresadoDatos);

                    if (null != interesadoAux)
                    {
                        if (interesadoAux.Alerta != null)
                        {
                            if (!interesadoAux.Alerta.Equals(""))
                            {
                                alertaInteresado += "Alerta de Interesado: " + interesadoAux.Alerta;
                                this._ventana.Mensaje(alertaInteresado, 2);
                            }
                        }
                        this._ventana.InteresadoDatos = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.InteresadoDatos);
                        this._ventana.NombreInteresadoDatos = ((Interesado)this._ventana.InteresadoDatos).Nombre;
                        this._ventana.IdInteresadoDatos = ((Interesado)this._ventana.InteresadoDatos).Id.ToString();
                        this._ventana.InteresadoSolicitud = (Interesado)this._ventana.InteresadoDatos;
                        this._ventana.NombreInteresadoSolicitud = ((Interesado)this._ventana.InteresadoDatos).Nombre;
                        this._ventana.IdInteresadoSolicitud = ((Interesado)this._ventana.InteresadoDatos).Id.ToString();
                        this._ventana.InteresadoPaisSolicitud = interesadoAux.Pais != null ? interesadoAux.Pais.NombreEspanol : "";
                        this._ventana.InteresadoCiudadSolicitud = interesadoAux.Ciudad != null ? interesadoAux.Ciudad : "";

                    }
                    else
                    {
                        this._ventana.NombreInteresadoSolicitud = "";
                        this._ventana.IdInteresadoSolicitud = "";
                        this._ventana.InteresadoDatos = "";
                        this._ventana.NombreInteresadoDatos = "";
                        this._ventana.IdInteresadoDatos = "";
                        this._ventana.InteresadoPaisSolicitud = "";
                        this._ventana.InteresadoCiudadSolicitud = "";
                    }


                }
                else
                {
                    Interesado interesadoAux = new Interesado(int.Parse(this._ventana.IdInteresadoSolicitud));
                    _interesadoAnterior = this._interesadoServicios.ConsultarInteresadoConTodo(interesadoAux);
                }



                this._ventana.PoderesDatos = null;
                this._ventana.PoderesSolicitud = null;
                this._ventana.PoderDatos = null;
                this._ventana.PoderSolicitud = null;

                this._ventana.IdPoderDatos = string.Empty;
                this._ventana.IdPoderSolicitud = string.Empty;
                this._ventana.NumPoderDatos = string.Empty;

                this._ventana.ConvertirEnteroMinimoABlanco();

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException e)
            {
                this._ventana.NombreInteresadoSolicitud = "";
                this._ventana.NombreInteresadoDatos = "";
                this._ventana.InteresadoPaisSolicitud = "";
                this._ventana.InteresadoCiudadSolicitud = "";
            }
        }


        /// <summary>
        /// Método que filtra un interesado
        /// </summary>
        /// <param name="filtrarEn">0 filtro desde Solicitud, 1 filtro desde datos</param>
        public void BuscarInteresado(int filtrarEn)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            Interesado interesadoAux = new Interesado(int.Parse(this._ventana.IdInteresadoSolicitud));
            _interesadoAnterior = this._interesadoServicios.ConsultarInteresadoConTodo(interesadoAux);
            Interesado primerInteresado = new Interesado(int.MinValue);


            Interesado interesado = new Interesado();
            IList<Interesado> interesadosFiltrados;

            if (filtrarEn == 1)
            {
                interesado.Nombre = this._ventana.NombreInteresadoDatosFiltrar.ToUpper();
                interesado.Id = this._ventana.IdInteresadoDatosFiltrar.Equals("") ? int.MinValue
                                        : int.Parse(this._ventana.IdInteresadoDatosFiltrar);
            }
            else
            {
                interesado.Nombre = this._ventana.NombreInteresadoSolicitudFiltrar.ToUpper();
                interesado.Id = this._ventana.IdInteresadoSolicitudFiltrar.Equals("") ? 0
                                        : int.Parse(this._ventana.IdInteresadoSolicitudFiltrar);
            }

            if ((!interesado.Nombre.Equals("")) || (interesado.Id != int.MinValue))
                interesadosFiltrados = this._interesadoServicios.ObtenerInteresadosFiltro(interesado);
            else
                interesadosFiltrados = new List<Interesado>();

            if (interesadosFiltrados.Count != 0)
            {
                //interesadosFiltrados.Insert(0, new Interesado(int.MinValue));
                this._ventana.InteresadosSolicitud = interesadosFiltrados;
                //this._ventana.InteresadoSolicitud = interesado;
                this._ventana.InteresadosDatos = interesadosFiltrados;
                //this._ventana.InteresadoDatos = interesado;
            }
            else
            {
                interesadosFiltrados.Add(_interesadoAnterior);
                //interesadosFiltrados.Insert(0, primerInteresado);
                this._ventana.InteresadosSolicitud = interesadosFiltrados;
                this._ventana.InteresadoSolicitud = _interesadoAnterior;
                this._ventana.InteresadosDatos = interesadosFiltrados;
                this._ventana.InteresadoDatos = _interesadoAnterior;
                this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
            }

            //if (filtrarEn == 0)
            //{
            //    interesado.Nombre = this._ventana.NombreInteresadoSolicitudFiltrar.ToUpper();
            //    interesado.Id = this._ventana.IdInteresadoSolicitudFiltrar.Equals("") ? 0
            //                        : int.Parse(this._ventana.IdInteresadoSolicitudFiltrar);


            //}
            //else
            //{
            //    //interesado.Nombre = this._ventana.NombreInteresadoDatosFiltrar.ToUpper();
            //    interesado.Nombre = "";
            //    interesado.Id = this._ventana.IdInteresadoDatosFiltrar.Equals("") ? 0
            //                        : int.Parse(this._ventana.IdInteresadoDatosFiltrar);

            //}
            //if ((!interesado.Nombre.Equals("")) || (interesado.Id != 0))
            //    interesadosFiltrados = this._interesadoServicios.ObtenerInteresadosFiltro(interesado);
            //else
            //    interesadosFiltrados = new List<Interesado>();

            //if (interesadosFiltrados.Count != 0)
            //{
            //    interesadosFiltrados.Insert(0, new Interesado(0));
            //    this._ventana.InteresadosSolicitud = interesadosFiltrados;
            //    this._ventana.InteresadoSolicitud = interesado;
            //    this._ventana.InteresadosDatos = interesadosFiltrados;
            //    this._ventana.InteresadoDatos = interesado;
            //}
            //else
            //{
            //    interesadosFiltrados.Insert(0, primerInteresado);
            //    this._ventana.InteresadosSolicitud = interesadosFiltrados;
            //    this._ventana.InteresadoSolicitud = primerInteresado;
            //    this._ventana.InteresadosDatos = this._interesados;
            //    this._ventana.InteresadoDatos = primerInteresado;
            //    this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
            //}

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que carga los interesados
        /// </summary>
        public void CargarInteresados()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            Mouse.OverrideCursor = Cursors.Wait;
            Marca marca = (Marca)this._ventana.Marca;

            //IList<Interesado> interesados = this._interesadoServicios.ConsultarTodos();
            Interesado primerInteresado = new Interesado();
            primerInteresado.Id = int.MinValue;
            //interesados.Insert(0, primerInteresado);
            //this._ventana.InteresadosDatos = interesados;
            //this._ventana.InteresadosSolicitud = interesados;
            //((Marca)this._ventana.Marca).Interesado = this.BuscarInteresado(interesados, marca.Interesado);
            //Interesado interesado = this.BuscarInteresado(interesados, marca.Interesado);
            //this._ventana.InteresadoSolicitud = interesado;
            //this._ventana.InteresadoDatos = interesado;
            //interesado = this._interesadoServicios.ConsultarInteresadoConTodo(interesado);
            //this._ventana.InteresadoPaisSolicitud = interesado.Pais.NombreEspanol;
            //this._ventana.InteresadoCiudadSolicitud = interesado.Ciudad;
            this._ventana.NombreInteresadoDatos = ((Marca)this._ventana.Marca).Interesado.Nombre;
            this._ventana.NombreInteresadoSolicitud = ((Marca)this._ventana.Marca).Interesado.Nombre;
            //this._interesados = interesados;

            this._ventana.InteresadosEstanCargados = true;

            Mouse.OverrideCursor = null;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        #endregion


        #region Metodos de los filtros de corresponsales

        /// <summary>
        /// Método que cambia corresponsal solicitud
        /// </summary>
        public void CambiarCorresponsalSolicitud()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if ((Corresponsal)this._ventana.CorresponsalSolicitud != null)
                {
                    //Corresponsal corresponsal = this._corresponsalServicios.ConsultarCorresponsalConTodo((Corresponsal)this._ventana.CorresponsalSolicitud);
                    this._ventana.DescripcionCorresponsalSolicitud = ((Corresponsal)this._ventana.CorresponsalSolicitud).Descripcion;
                    this._ventana.IdCorresponsalSolicitud = ((Corresponsal)this._ventana.CorresponsalSolicitud).Id.ToString();

                    this._ventana.CorresponsalDatos = (Corresponsal)this._ventana.CorresponsalSolicitud;
                    this._ventana.DescripcionCorresponsalDatos = ((Corresponsal)this._ventana.CorresponsalSolicitud).Descripcion;
                    this._ventana.IdCorresponsalDatos = ((Corresponsal)this._ventana.CorresponsalSolicitud).Id.ToString();

                    this._ventana.ConvertirEnteroMinimoABlanco();
                }

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException e)
            {
                this._ventana.DescripcionCorresponsalDatos = "";
                this._ventana.DescripcionCorresponsalSolicitud = "";
            }
        }


        /// <summary>
        /// Método que cambia corresponsal datos
        /// </summary>
        public void CambiarCorresponsalDatos()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if ((Corresponsal)this._ventana.CorresponsalDatos != null)
                {
                    //Corresponsal corresponsal = this._corresponsalServicios.ConsultarCorresponsalConTodo((Corresponsal)this._ventana.CorresponsalDatos);
                    this._ventana.DescripcionCorresponsalDatos = ((Corresponsal)this._ventana.CorresponsalDatos).Descripcion;
                    this._ventana.IdCorresponsalDatos = ((Corresponsal)this._ventana.CorresponsalDatos).Id.ToString();
                    this._ventana.CorresponsalSolicitud = (Corresponsal)this._ventana.CorresponsalDatos;

                    this._ventana.DescripcionCorresponsalSolicitud = ((Corresponsal)this._ventana.CorresponsalDatos).Descripcion;
                    this._ventana.IdCorresponsalSolicitud = ((Corresponsal)this._ventana.CorresponsalDatos).Id.ToString();

                    this._ventana.ConvertirEnteroMinimoABlanco();
                }

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException e)
            {
                this._ventana.DescripcionCorresponsalDatos = "";
                this._ventana.DescripcionCorresponsalSolicitud = "";
            }
        }


        /// <summary>
        /// Metodo que cambia la Carta Orden en el tab de Solicitud
        /// </summary>
        public void CambiarCartaOrden(int tabActual)
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (tabActual == 0)
                {
                    if ((Carta)this._ventana.CartaOrdenSolicitud != null)
                    {
                        this._ventana.IdCartaOrdenSolicitud = ((Carta)this._ventana.CartaOrdenSolicitud).Id != 0 
                            ? ((Carta)this._ventana.CartaOrdenSolicitud).Id.ToString() : "";
                        this._ventana.IdCartaOrdenDatos = ((Carta)this._ventana.CartaOrdenSolicitud).Id != 0
                            ? ((Carta)this._ventana.CartaOrdenSolicitud).Id.ToString() : "";
                        this._ventana.CartaOrdenDatos = (Carta)this._ventana.CartaOrdenSolicitud;

                        ((Marca)this._ventana.Marca).Carta = ((Carta)this._ventana.CartaOrdenSolicitud).Id != 0 
                            ? (Carta)this._ventana.CartaOrdenSolicitud : null;

                        

                    }
                }
                else if (tabActual == 1)
                {
                    if ((Carta)this._ventana.CartaOrdenDatos != null)
                    {
                        this._ventana.IdCartaOrdenDatos = ((Carta)this._ventana.CartaOrdenDatos).Id != 0
                            ? ((Carta)this._ventana.CartaOrdenDatos).Id.ToString() : "";
                        this._ventana.IdCartaOrdenSolicitud = ((Carta)this._ventana.CartaOrdenDatos).Id != 0
                            ? ((Carta)this._ventana.CartaOrdenDatos).Id.ToString() : "";
                        this._ventana.CartaOrdenSolicitud = (Carta)this._ventana.CartaOrdenDatos;

                        ((Marca)this._ventana.Marca).Carta = ((Carta)this._ventana.CartaOrdenDatos).Id != 0 
                            ? (Carta)this._ventana.CartaOrdenDatos : null;

                    }
                }


                

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception e)
            {
                    
                throw;
            }

        }


        /// <summary>
        /// Método que filtra un corresponsal
        /// </summary>
        /// <param name="filtrarEn"></param>
        public void BuscarCorresponsal(int filtrarEn)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            IEnumerable<Corresponsal> corresponsalesFiltrados = this._corresponsales;

            if (filtrarEn == 0)
            {
                if (!string.IsNullOrEmpty(this._ventana.IdCorresponsalSolicitudFiltrar))
                {
                    corresponsalesFiltrados = from p in corresponsalesFiltrados
                                              where p.Id == int.Parse(this._ventana.IdCorresponsalSolicitudFiltrar)
                                              select p;
                }

                if (!string.IsNullOrEmpty(this._ventana.DescripcionCorresponsalSolicitudFiltrar))
                {
                    corresponsalesFiltrados = from p in corresponsalesFiltrados
                                              where p.Descripcion != null &&
                                              p.Descripcion.ToLower().Contains(this._ventana.DescripcionCorresponsalSolicitudFiltrar.ToLower())
                                              select p;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this._ventana.IdCorresponsalDatosFiltrar))
                {
                    corresponsalesFiltrados = from p in corresponsalesFiltrados
                                              where p.Id == int.Parse(this._ventana.IdCorresponsalDatosFiltrar)
                                              select p;
                }

                if (!string.IsNullOrEmpty(this._ventana.DescripcionCorresponsalDatosFiltrar))
                {
                    corresponsalesFiltrados = from p in corresponsalesFiltrados
                                              where p.Descripcion != null &&
                                              p.Descripcion.ToLower().Contains(this._ventana.DescripcionCorresponsalDatosFiltrar.ToLower())
                                              select p;
                }
            }

            // filtrarEn = 0 significa en el listview de la pestaña solicitud
            // filtrarEn = 1 significa en el listview de la pestaña Datos 
            if (filtrarEn == 0)
            {
                if (corresponsalesFiltrados.ToList<Corresponsal>().Count != 0)
                    this._ventana.CorresponsalesSolicitud = corresponsalesFiltrados.ToList<Corresponsal>();
                else
                {
                    this._ventana.CorresponsalesSolicitud = this._corresponsales;
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                }
            }
            else
            {
                if (corresponsalesFiltrados.ToList<Corresponsal>().Count != 0)
                    this._ventana.CorresponsalesDatos = corresponsalesFiltrados.ToList<Corresponsal>();
                else
                {
                    this._ventana.CorresponsalesDatos = this._corresponsales;
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                }
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que carga los corresponsales
        /// </summary>
        public void CargarCorresponsales()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            Mouse.OverrideCursor = Cursors.Wait;

            IList<Corresponsal> corresponsales = this._corresponsalServicios.ConsultarTodos();
            Corresponsal primerCorresponsal = new Corresponsal();
            primerCorresponsal.Id = int.MinValue;
            corresponsales.Insert(0, primerCorresponsal);
            this._ventana.CorresponsalesSolicitud = corresponsales;
            this._ventana.CorresponsalesDatos = corresponsales;
            this._ventana.CorresponsalDatos = this.BuscarCorresponsal(corresponsales, ((Marca)this._ventana.Marca).Corresponsal);
            this._ventana.CorresponsalSolicitud = this.BuscarCorresponsal(corresponsales, ((Marca)this._ventana.Marca).Corresponsal);

            this._ventana.DescripcionCorresponsalDatos = null == ((Marca)this._ventana.Marca).Corresponsal ?
                                                         null : ((Corresponsal)this._ventana.CorresponsalSolicitud).Descripcion;
            this._ventana.DescripcionCorresponsalSolicitud = null == ((Marca)this._ventana.Marca).Corresponsal ?
                                                             null : ((Corresponsal)this._ventana.CorresponsalSolicitud).Descripcion;
            this._corresponsales = corresponsales;

            this._ventana.CorresponsalesEstanCargados = true;


            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }

        #endregion

        /// <summary>
        /// Metodo que sirve para cargar la carta orden
        /// </summary>
        /// <param name="nombreCampoTexto">Nombre del campo de texto</param>
        public void CargarCartaOrden(String nombreCampoTexto)
        {
            Carta cartaMarca = null;
            Carta primeraCarta = new Carta();
            IList<Carta> cartas = new List<Carta>();

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            cartaMarca = (((Marca)this._ventana.Marca).Carta != null) ? ((Marca)this._ventana.Marca).Carta : null;
            cartas.Add(primeraCarta);
            if (cartaMarca != null)
                cartas.Add(cartaMarca);

            this._ventana.CartasOrdenSolicitud = cartas;
            this._ventana.CartasOrdenDatos = cartas;
            
            if (cartaMarca != null)
            {
                this._ventana.CartaOrdenSolicitud = this.BuscarCarta(cartas, cartaMarca);
                this._ventana.CartaOrdenDatos = this.BuscarCarta(cartas, cartaMarca);
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        #region Metodos de la lista de poderes


        /// <summary>
        /// Método que cambia poder solicitud
        /// </summary>
        public void CambiarPoderSolicitud()
        {
            IList<Poder> _poderesFiltrados = new List<Poder>();
            IList<Interesado> _listaInteresados = new List<Interesado>();
            Poder poderABuscar = null;
            Interesado interesadoPoder = null;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (((Poder)this._ventana.PoderSolicitud != null) && (((Poder)this._ventana.PoderSolicitud).Id != int.MinValue))
                {
                    if ((this._ventana.Agente == null) || (((Agente)this._ventana.Agente).Id.Equals("NGN")))
                    {

                        if (((Interesado)this._ventana.InteresadoSolicitud).Id != int.MinValue)
                        {
                            this._ventana.Agentes = null;
                            IList<Agente> agentes = this._agenteServicios.ObtenerAgentesDeUnPoder((Poder)this._ventana.PoderSolicitud);
                            this._ventana.Agentes = agentes;
                            this._ventana.IdPoderSolicitud = ((Poder)this._ventana.PoderSolicitud).Id.ToString();
                            this._ventana.IdPoderDatos = ((Poder)this._ventana.PoderSolicitud).Id.ToString();
                            ((Marca)this._ventana.Marca).Poder = (Poder)this._ventana.PoderSolicitud;
                        }
                        else
                        {
                            this._ventana.InteresadosSolicitud = null;
                            interesadoPoder = this._interesadoServicios.ObtenerInteresadosDeUnPoder((Poder)this._ventana.PoderSolicitud);
                            _listaInteresados.Add(interesadoPoder);
                            this._ventana.InteresadosSolicitud = _listaInteresados;
                            this._ventana.InteresadosDatos = _listaInteresados;
                            this._ventana.Agentes = null;
                            IList<Agente> agentes = this._agenteServicios.ObtenerAgentesDeUnPoder((Poder)this._ventana.PoderSolicitud);
                            this._ventana.Agentes = agentes;
                            this._ventana.IdPoderSolicitud = ((Poder)this._ventana.PoderSolicitud).Id.ToString();
                            this._ventana.IdPoderDatos = ((Poder)this._ventana.PoderSolicitud).Id.ToString();
                            ((Marca)this._ventana.Marca).Poder = (Poder)this._ventana.PoderSolicitud;


                        }
                    }
                    else
                    {
                        Poder primerPoder = new Poder();
                        primerPoder.Id = int.MinValue;
                        _poderesFiltrados = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado((Agente)this._ventana.Agente, (Interesado)this._ventana.InteresadoSolicitud);
                        if (_poderesFiltrados.Count != 0)
                        {
                            poderABuscar = this.BuscarPoder(_poderesFiltrados, (Poder)this._ventana.PoderSolicitud);

                            if (poderABuscar != null)
                            {
                                _poderesFiltrados.Insert(0, primerPoder);
                                this._ventana.PoderesSolicitud = _poderesFiltrados;
                                this._ventana.PoderesDatos = _poderesFiltrados;
                                this._ventana.PoderSolicitud = poderABuscar;
                                this._ventana.PoderDatos = poderABuscar;
                                //muestro y cargo en la marca de la ventana
                                this._ventana.IdPoderSolicitud = ((Poder)this._ventana.PoderSolicitud).Id.ToString();
                                this._ventana.IdPoderDatos = ((Poder)this._ventana.PoderSolicitud).Id.ToString();
                                ((Marca)this._ventana.Marca).Poder = (Poder)this._ventana.PoderSolicitud;


                            }
                            else
                            {
                                this._ventana.Mensaje("El Poder no pertenece al Interesado", 0);
                                _poderesFiltrados.Insert(0, primerPoder);
                                this._ventana.PoderesSolicitud = _poderesFiltrados;
                                this._ventana.PoderesDatos = _poderesFiltrados;
                                this._ventana.PoderSolicitud = poderABuscar;
                                this._ventana.PoderDatos = poderABuscar;
                                //muestro y cargo en la marca de la ventana
                                this._ventana.IdPoderSolicitud = ((Poder)this._ventana.PoderSolicitud).Id.ToString();
                                this._ventana.IdPoderDatos = ((Poder)this._ventana.PoderSolicitud).Id.ToString();
                                ((Marca)this._ventana.Marca).Poder = (Poder)this._ventana.PoderSolicitud;

                            }
                        }
                        else
                        {
                            this._ventana.Mensaje("El poder seleccionado no relaciona al Agente con el Interesado", 0);
                            //muestro y cargo en la patente de la ventana
                            this._ventana.IdPoderSolicitud = ((Poder)this._ventana.PoderSolicitud).Id.ToString();
                            this._ventana.IdPoderDatos = ((Poder)this._ventana.PoderSolicitud).Id.ToString();
                            ((Marca)this._ventana.Marca).Poder = (Poder)this._ventana.PoderSolicitud;


                        }
                    }
                }
                else
                {
                    this._ventana.IdPoderSolicitud = ((Poder)this._ventana.PoderSolicitud).Id.ToString();
                    this._ventana.IdPoderDatos = ((Poder)this._ventana.PoderSolicitud).Id.ToString();
                    ((Marca)this._ventana.Marca).Poder = (Poder)this._ventana.PoderSolicitud;
                    this._ventana.PoderDatos = this._ventana.PoderSolicitud;
                }

                this._ventana.ConvertirEnteroMinimoABlanco();

                #region Codigo original comentado NO  BORRAR
                //if ((Poder)this._ventana.PoderSolicitud != null)
                //{
                //    this._ventana.IdPoderSolicitud = ((Poder)this._ventana.PoderSolicitud).Id.ToString();
                //    this._ventana.IdPoderDatos = ((Poder)this._ventana.PoderSolicitud).Id.ToString();
                //    this._ventana.PoderDatos = (Poder)this._ventana.PoderSolicitud;
                //    this._ventana.NumPoderDatos = ((Poder)this._ventana.PoderSolicitud).NumPoder;
                //    //---
                //    ((Marca)this._ventana.Marca).Poder = (Poder)this._ventana.PoderSolicitud;
                //    //---
                //    this._ventana.ConvertirEnteroMinimoABlanco();
                //} 
                #endregion

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException e)
            {
                this._ventana.IdPoderSolicitud = "";
                this._ventana.IdPoderDatos = "";
            }
        }


        /// <summary>
        /// Método que cambia poder datos
        /// </summary>
        public void CambiarPoderDatos()
        {
            IList<Poder> _poderesFiltrados = new List<Poder>();
            IList<Interesado> _listaInteresados = new List<Interesado>();
            Poder poderABuscar = null;
            Interesado interesadoPoder = null;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                #region Codigo original comentado NO BORRAR
                //if ((Poder)this._ventana.PoderDatos != null)
                //{
                //    this._ventana.NumPoderDatos = ((Poder)this._ventana.PoderDatos).NumPoder;
                //    this._ventana.PoderSolicitud = (Poder)this._ventana.PoderDatos;
                //    this._ventana.IdPoderSolicitud = ((Poder)this._ventana.PoderDatos).Id.ToString();
                //    this._ventana.IdPoderDatos = ((Poder)this._ventana.PoderDatos).Id.ToString();
                //    //---
                //    ((Marca)this._ventana.Marca).Poder = (Poder)this._ventana.PoderDatos;
                //    //---
                //    this._ventana.ConvertirEnteroMinimoABlanco();
                //} 
                #endregion

                if (((Poder)this._ventana.PoderDatos != null) && (((Poder)this._ventana.PoderDatos).Id != int.MinValue))
                {
                    if ((this._ventana.Agente == null) || (((Agente)this._ventana.Agente).Id.Equals("NGN")))
                    {

                        if (((Interesado)this._ventana.InteresadoDatos).Id != int.MinValue)
                        {
                            this._ventana.Agentes = null;
                            IList<Agente> agentes = this._agenteServicios.ObtenerAgentesDeUnPoder((Poder)this._ventana.PoderDatos);
                            this._ventana.Agentes = agentes;
                            this._ventana.IdPoderSolicitud = ((Poder)this._ventana.PoderDatos).Id.ToString();
                            this._ventana.IdPoderDatos = ((Poder)this._ventana.PoderDatos).Id.ToString();
                            ((Marca)this._ventana.Marca).Poder = (Poder)this._ventana.PoderDatos;
                        }
                        else
                        {
                            this._ventana.InteresadosSolicitud = null;
                            interesadoPoder = this._interesadoServicios.ObtenerInteresadosDeUnPoder((Poder)this._ventana.PoderDatos);
                            _listaInteresados.Add(interesadoPoder);
                            this._ventana.InteresadosSolicitud = _listaInteresados;
                            this._ventana.InteresadosDatos = _listaInteresados;
                            this._ventana.Agentes = null;
                            IList<Agente> agentes = this._agenteServicios.ObtenerAgentesDeUnPoder((Poder)this._ventana.PoderDatos);
                            this._ventana.Agentes = agentes;
                            this._ventana.IdPoderSolicitud = ((Poder)this._ventana.PoderDatos).Id.ToString();
                            this._ventana.IdPoderDatos = ((Poder)this._ventana.PoderDatos).Id.ToString();
                            ((Marca)this._ventana.Marca).Poder = (Poder)this._ventana.PoderDatos;
                            
                        }
                    }
                    else
                    {
                        Poder primerPoder = new Poder();
                        primerPoder.Id = int.MinValue;
                        _poderesFiltrados = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado((Agente)this._ventana.Agente, (Interesado)this._ventana.InteresadoDatos);
                        if (_poderesFiltrados.Count != 0)
                        {
                            poderABuscar = this.BuscarPoder(_poderesFiltrados, (Poder)this._ventana.PoderDatos);

                            if (poderABuscar != null)
                            {
                                _poderesFiltrados.Insert(0, primerPoder);
                                this._ventana.PoderesDatos = _poderesFiltrados;
                                this._ventana.PoderDatos = poderABuscar;
                                this._ventana.PoderesSolicitud = _poderesFiltrados;
                                this._ventana.PoderSolicitud = poderABuscar;
                                //muestro y cargo en la marca de la ventana
                                this._ventana.IdPoderSolicitud = ((Poder)this._ventana.PoderDatos).Id.ToString();
                                this._ventana.IdPoderDatos = ((Poder)this._ventana.PoderDatos).Id.ToString();
                                ((Marca)this._ventana.Marca).Poder = (Poder)this._ventana.PoderDatos;


                            }
                            else
                            {
                                this._ventana.Mensaje("El Poder no pertenece al Interesado", 0);
                                _poderesFiltrados.Insert(0, primerPoder);
                                this._ventana.PoderesDatos = _poderesFiltrados;
                                this._ventana.PoderesSolicitud = _poderesFiltrados;
                                this._ventana.PoderDatos = poderABuscar;
                                this._ventana.PoderSolicitud = poderABuscar;
                                //muestro y cargo en la marca de la ventana
                                this._ventana.IdPoderSolicitud = ((Poder)this._ventana.PoderDatos).Id.ToString();
                                this._ventana.IdPoderDatos = ((Poder)this._ventana.PoderDatos).Id.ToString();
                                ((Marca)this._ventana.Marca).Poder = (Poder)this._ventana.PoderDatos;

                            }
                        }
                        else
                        {
                            this._ventana.Mensaje("El poder seleccionado no relaciona al Agente con el Interesado", 0);
                            //muestro y cargo en la patente de la ventana
                            this._ventana.IdPoderSolicitud = ((Poder)this._ventana.PoderDatos).Id.ToString();
                            this._ventana.IdPoderDatos = ((Poder)this._ventana.PoderDatos).Id.ToString();
                            ((Marca)this._ventana.Marca).Poder = (Poder)this._ventana.PoderDatos;


                        }
                    }
                }
                else
                {
                    this._ventana.IdPoderSolicitud = ((Poder)this._ventana.PoderDatos).Id.ToString();
                    this._ventana.IdPoderDatos = ((Poder)this._ventana.PoderDatos).Id.ToString();
                    ((Marca)this._ventana.Marca).Poder = (Poder)this._ventana.PoderDatos;
                    this._ventana.PoderSolicitud = this._ventana.PoderDatos;
                }

                this._ventana.ConvertirEnteroMinimoABlanco();


                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException e)
            {
                this._ventana.IdPoderSolicitud = "";
                this._ventana.IdPoderDatos = "";
            }
        }


        /// <summary>
        /// Método que carga los poderes
        /// </summary>
        public void CargarPoderes()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            #region codigo original comentado
            //Mouse.OverrideCursor = Cursors.Wait;

            //Marca marca = (Marca)this._ventana.Marca;
            //IList<Poder> poderes = this._poderServicios.ConsultarTodos();
            //Poder poder = new Poder();
            //poder.Id = int.MinValue;
            //poderes.Insert(0, poder);
            //this._ventana.PoderesDatos = poderes;
            //this._ventana.PoderesSolicitud = poderes;
            //this._ventana.PoderDatos = this.BuscarPoder(poderes, marca.Poder);
            //this._ventana.PoderSolicitud = this.BuscarPoder(poderes, marca.Poder);

            //this._ventana.PoderesEstanCargados = true;

            //Mouse.OverrideCursor = null;
            /////nuevooo 
            #endregion


            Mouse.OverrideCursor = Cursors.Wait;

            CargarPoderesEntreInteresadoAgente();

            Marca marca = null != this._ventana.Marca ? (Marca)this._ventana.Marca : new Marca();

            #region Codigo original comentado
            //Poder poder = null;
            ////Poder poder = _marca.Poder;
            //if (!this._ventana.IdPoderDatos.Equals(""))
            //    poder = new Poder(int.Parse(this._ventana.IdPoderDatos));

            //this._ventana.PoderSolicitud = poder != null ? poder.Id.ToString() : "";
            ////this._ventana.PoderSolicitud = poder.Id.ToString();
            //this._ventana.IdPoderSolicitud = poder != null ? poder.Id.ToString() : "";

            //this._ventana.PoderDatos = poder != null ? poder.Id.ToString() : "";

            //this._ventana.PoderesSolicitud = this._poderesInterseccion;
            //this._ventana.PoderesDatos = this._poderesInterseccion;

            //this._ventana.PoderSolicitud = poder;
            //this._ventana.PoderDatos = poder; 
            #endregion

            
            if (_marca.Poder != null)
            {
                this._ventana.PoderesSolicitud = this._poderesInterseccion;
                this._ventana.PoderesDatos = this._poderesInterseccion;

                this._ventana.PoderSolicitud = this.BuscarPoder(_poderesInterseccion, this._marca.Poder);
                this._ventana.PoderDatos = this.BuscarPoder(_poderesInterseccion, this._marca.Poder);
            }
            Mouse.OverrideCursor = null;




            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }

        private void CargarPoderesEntreInteresadoAgente()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            Mouse.OverrideCursor = Cursors.Wait;

            Interesado interesadoAux = new Interesado(int.Parse(this._ventana.IdInteresadoSolicitud));


            if (!(interesadoAux.Equals("")) && (this._ventana.Agente != null))
            {
                _poderesInterseccion = this._poderServicios
                    .ObtenerPoderesEntreAgenteEInteresado((Agente)this._ventana.Agente, interesadoAux);

                if (_poderesInterseccion.Count() == 0)
                {
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.ErrorInteresadoNoPoseePoderesConAgente, 0);
                    //---
                    Poder primerPoder = new Poder();
                    primerPoder.Id = int.MinValue;
                    _poderesInterseccion.Add(primerPoder);

                    //--
                }
                else
                {
                    _poderesInterseccion.Insert(0, new Poder(int.MinValue));
                    this._ventana.PoderesSolicitud = _poderesInterseccion;
                    this._ventana.PoderesDatos = _poderesInterseccion;
                    this._ventana.mostrarLstPoderSolicitud();
                }

                //_poderesInterseccion.Insert(0, new Poder(int.MinValue));
            }
            else
            {
                this._ventana.Mensaje("La Marca no posee Apoderado", 0);

                if (((Marca)this._ventana.Marca).Poder != null)
                {
                    Poder primerPoder = new Poder();
                    primerPoder.Id = int.MinValue;
                    Poder poderActualDeMarca = ((Marca)this._ventana.Marca).Poder;
                    _poderesInterseccion = new List<Poder>();
                    _poderesInterseccion.Add(poderActualDeMarca);
                    _poderesInterseccion.Insert(0, primerPoder);
                }
                
                //MENSAJE DE ERROR
            }

            Mouse.OverrideCursor = null;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que se usa para validar que cuando se modifique el poder sea actualizado
        /// </summary>
        /// <returns>true si el poder es válido, false en caso contrario</returns>
        private bool ValidarPoder()
        {
            bool retorno = false;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
                if (!this._ventana.IdPoderSolicitud.Equals(""))
                {
                    IList<Poder> poderesAux = new List<Poder>();

                    Interesado interesadoAux = new Interesado(int.Parse(this._ventana.IdInteresadoSolicitud));

                    if(this._ventana.Agente != null)
                        poderesAux = this._poderServicios
                            .ObtenerPoderesEntreAgenteEInteresado((Agente)this._ventana.Agente, interesadoAux);



                    if (poderesAux.Count != 0)
                    {
                        foreach (Poder poder in poderesAux)
                        {
                            if (poder.Id == int.Parse(this._ventana.IdPoderSolicitud))
                            {
                                retorno = true;
                                break;
                            }
                        }
                    }
                    else
                        retorno = true;


                }
                else
                    retorno = true;

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

            return retorno;
        }

        #endregion


        #region Impresiones

        /// <summary>
        /// Método que se encarga de cargar la ventana de impresión de la Marca
        /// </summary>
        /// <param name="nombreBoton"></param>
        public void IrImprimir(string nombreBoton)
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                switch (nombreBoton)
                {
                    case "_btnFM02":
                        ImprimirFM02("Normal");
                        break;
                    case "_btnFM02Venen":
                        ImprimirFM02Venen("Normal");
                        break;
                    case "_btnAnexoFM02":
                        ImprimirAnexoFM02("Normal");
                        break;
                    case "_btnLFM02":
                        ImprimirFM02("Laser");
                        break;
                    case "_btnLFM02Venen":
                        ImprimirFM02Venen("Laser");
                        break;
                    case "_btnLAnexoFM02":
                        ImprimirAnexoFM02("Laser");
                        break;
                    case "_btnCarpeta":
                        ImprimirCarpeta("Normal");
                        break;
                    default:
                        break;
                }

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(string.Format(Recursos.MensajesConElUsuario.ExcepcionRutaNoAutorizada, ConfigurationManager.AppSettings["txtPrint"]), true);
            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ExcepcionPaquetes, true);
            }
        }

        /// <summary>
        /// Método que se encarga de llamar al formato de impresión FM02
        /// </summary>
        /// <param name="modo">Láser en caso de ser Láser, o normal en caso de ser normal</param>
        private void ImprimirFM02(string modo)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            if (ValidarMarcaAntesDeImprimirFM02())
            {
                string paqueteProcedimiento = "PCK_MYP_MARCAS";
                string procedimiento = modo.Equals("Laser") ? "P31" : "P1";
                ParametroProcedimiento parametro =
                    new ParametroProcedimiento(((Marca)this._ventana.Marca).Id, UsuarioLogeado, 1, paqueteProcedimiento, procedimiento);

                Planilla planilla = this._planillaServicios.ImprimirProcedimiento(parametro);
                if (planilla != null)
                {
                    //Impresion _ventana =
                    //    new Impresion(Recursos.Etiquetas.btnFM02, planilla.Folio.Replace("\n", Environment.NewLine));
                    //_ventana.ShowDialog();

                    ////Llamado al archivo .bat 
                    ////if (_ventana.ClickImprimir)
                    ////    this.EjecutarArchivoBAT(ConfigurationManager.AppSettings["batPrint"], 
                    ////ConfigurationManager.AppSettings["txtPrint"]);

                    //parametro.Via = 0;
                    //planilla = this._planillaServicios.ImprimirProcedimiento(parametro);

                    this.LlamarProcedimientoDeBaseDeDatos(parametro, Recursos.Etiquetas.btnFM02);
                }
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }

        /// <summary>
        /// Método que se encarga de llamar al formato de impresión FM02Venen
        /// </summary>
        /// <param name="modo">Láser en caso de ser Láser, o normal en caso de ser normal</param>
        private void ImprimirFM02Venen(string modo)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            if (ValidarMarcaAntesDeImprimirFM02Venen())
            {
                string paqueteProcedimiento = "PCK_MYP_MARCAS";
                string procedimiento = modo.Equals("Laser") ? "P32" : "P2";
                ParametroProcedimiento parametro =
                    new ParametroProcedimiento(((Marca)this._ventana.Marca).Id, UsuarioLogeado, 1, paqueteProcedimiento, procedimiento);

                Planilla planilla = this._planillaServicios.ImprimirProcedimiento(parametro);
                if (planilla != null)
                {
                    //Impresion _ventana =
                    //    new Impresion(Recursos.Etiquetas.btnFM02Venen, planilla.Folio.Replace("\n", Environment.NewLine));

                    //_ventana.ShowDialog();

                    ////Llamado al archivo .bat 
                    ////if (_ventana.ClickImprimir)
                    ////    this.EjecutarArchivoBAT(ConfigurationManager.AppSettings["batPrint"], 
                    ////ConfigurationManager.AppSettings["txtPrint"]);

                    //parametro.Via = 0;
                    //planilla = this._planillaServicios.ImprimirProcedimiento(parametro);

                    this.LlamarProcedimientoDeBaseDeDatos(parametro, Recursos.Etiquetas.btnFM02Venen);
                }
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }

        /// <summary>
        /// Método que se encarga de llamar al formato de impresión AnexoFM02
        /// </summary>
        /// <param name="modo">Láser en caso de ser Láser, o normal en caso de ser normal</param>
        private void ImprimirAnexoFM02(string modo)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            if (ValidarMarcaAntesDeImprimirAnexoFM02())
            {
                string paqueteProcedimiento = "PCK_MYP_MARCAS";
                string procedimiento = modo.Equals("Laser") ? "P33" : "P3";
                ParametroProcedimiento parametro =
                    new ParametroProcedimiento(((Marca)this._ventana.Marca).Id, UsuarioLogeado, 1, paqueteProcedimiento, procedimiento);

                Planilla planilla = this._planillaServicios.ImprimirProcedimiento(parametro);
                if (planilla != null)
                {
                    //Impresion _ventana =
                    //    new Impresion(Recursos.Etiquetas.btnAnexoFM02, planilla.Folio.Replace("\n", Environment.NewLine));

                    //_ventana.ShowDialog();

                    ////Llamado al archivo .bat 
                    ////if (_ventana.ClickImprimir)
                    ////    this.EjecutarArchivoBAT(ConfigurationManager.AppSettings["batPrint"], 
                    ////ConfigurationManager.AppSettings["txtPrint"]);

                    //parametro.Via = 0;
                    //planilla = this._planillaServicios.ImprimirProcedimiento(parametro);

                    this.LlamarProcedimientoDeBaseDeDatos(parametro, Recursos.Etiquetas.btnAnexoFM02);
                }
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }

        /// <summary>
        /// Método que se encarga de llamar al formato de impresión Carpeta
        /// </summary>
        /// <param name="modo">Láser en caso de ser Láser, o normal en caso de ser normal</param>
        private void ImprimirCarpeta(string modo)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            if (ValidarMarcaAntesDeImprimirCarpeta())
            {
                string paqueteProcedimiento = "PCK_MYP_MARCAS";
                string procedimiento = modo.Equals("Laser") ? "" : "P12";
                ParametroProcedimiento parametro =
                    new ParametroProcedimiento(((Marca)this._ventana.Marca).Id, UsuarioLogeado, 1, paqueteProcedimiento, procedimiento);

                Planilla planilla = this._planillaServicios.ImprimirProcedimiento(parametro);
                if (planilla != null)
                {
                    //Impresion _ventana =
                    //    new Impresion(Recursos.Etiquetas.btnAnexoFM02, planilla.Folio.Replace("\n", Environment.NewLine));

                    //_ventana.ShowDialog();

                    ////Llamado al archivo .bat 
                    ////if (_ventana.ClickImprimir)
                    ////    this.EjecutarArchivoBAT(ConfigurationManager.AppSettings["batPrint"], 
                    ////ConfigurationManager.AppSettings["txtPrint"]);

                    //parametro.Via = 0;
                    //planilla = this._planillaServicios.ImprimirProcedimiento(parametro);

                    this.LlamarProcedimientoDeBaseDeDatos(parametro, Recursos.Etiquetas.btnCarpeta);
                }
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }

        /// <summary>
        /// Método que realiza todas las validaciones de la Marca antes de imprimir
        /// </summary>
        /// <returns>true en caso de que todo este correcto, false en caso contrario</returns>
        private bool ValidarMarcaAntesDeImprimirFM02()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            bool retorno = true;

            Marca marca = CargarMarcaDeLaPantalla();

            if ((null == marca.Poder) || (string.IsNullOrEmpty(marca.Poder.NumPoder)))
                retorno = retorno ?
                    this._ventana.MensajeAlerta(Recursos.MensajesConElUsuario.MarcaSinNumeroDePoder) == retorno : retorno;

            if (retorno)    
            {
                //if (((this._ventana.ClaseInternacional.Equals("")) && (this._ventana.ClaseNacional.Equals(""))) && (retorno))
                //    retorno = retorno ?
                //        this._ventana.MensajeAlerta(Recursos.MensajesConElUsuario.MarcaSinClase) == retorno : retorno;

                if ((this._ventana.ClaseInternacional.Equals("")) && (this._ventana.ClaseNacional.Equals("")))
                    retorno = retorno ?
                        this._ventana.MensajeAlerta(Recursos.MensajesConElUsuario.MarcaSinClase) == retorno : retorno;


                if (retorno)
                {
                    //if ((null == marca.EtiquetaDescripcion) && (retorno))
                    //    retorno = retorno ?
                    //        this._ventana.MensajeAlerta(Recursos.MensajesConElUsuario.MarcaSinDescripcionDelSigno) == retorno : retorno;
                    //else if ((marca.EtiquetaDescripcion.Equals("")) && (retorno))
                    //    retorno = retorno ?
                    //        this._ventana.MensajeAlerta(Recursos.MensajesConElUsuario.MarcaSinDescripcionDelSigno) == retorno : retorno;
                    if (null == marca.EtiquetaDescripcion)
                        retorno = retorno ?
                            this._ventana.MensajeAlerta(Recursos.MensajesConElUsuario.MarcaSinDescripcionDelSigno) == retorno : retorno;
                    else if (marca.EtiquetaDescripcion.Equals(""))
                        retorno = retorno ?
                            this._ventana.MensajeAlerta(Recursos.MensajesConElUsuario.MarcaSinDescripcionDelSigno) == retorno : retorno;

                    if (retorno)    
                    {
                        //if ((null == marca.Distingue) || (marca.Distingue.Equals("")) && (retorno))
                        //    retorno = retorno ?
                        //        this._ventana.MensajeAlerta(Recursos.MensajesConElUsuario.MarcaSinDistingue) == retorno : retorno; 
                        if (null == marca.Distingue)
                            retorno = retorno ?
                                this._ventana.MensajeAlerta(Recursos.MensajesConElUsuario.MarcaSinDistingue) == retorno : retorno; 
                        else if(marca.Distingue.Equals(""))
                            retorno = retorno ?
                                this._ventana.MensajeAlerta(Recursos.MensajesConElUsuario.MarcaSinDistingue) == retorno : retorno;

                    }
  
                }

            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }

        /// <summary>
        /// Método que realiza todas las validaciones de la Marca antes de imprimir
        /// </summary>
        /// <returns>true en caso de que todo este correcto, false en caso contrario</returns>
        private bool ValidarMarcaAntesDeImprimirFM02Venen()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            bool retorno = true;

            Marca marca = CargarMarcaDeLaPantalla();
           
            if ((null == marca.Distingue) || (marca.Distingue.Equals("")))
                retorno = retorno ?
                    this._ventana.MensajeAlerta(Recursos.MensajesConElUsuario.MarcaSinDistingue) == retorno : retorno;

            else if (marca.Distingue.Length > 1800)
                retorno = retorno ?
                    this._ventana.MensajeAlerta(Recursos.MensajesConElUsuario.MarcaSinDistingue) == retorno : retorno;
            
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }

        /// <summary>
        /// Método que realiza todas las validaciones de la Marca antes de imprimir
        /// </summary>
        /// <returns>true en caso de que todo este correcto, false en caso contrario</returns>
        private bool ValidarMarcaAntesDeImprimirAnexoFM02()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            bool retorno = true;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }

        /// <summary>
        /// Método que realiza todas las validaciones de la Marca antes de imprimir
        /// </summary>
        /// <returns>true en caso de que todo este correcto, false en caso contrario</returns>
        private bool ValidarMarcaAntesDeImprimirCarpeta()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return true;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }

        #endregion


        /// <summary>
        ///Método que realiza el llamado al explorador para abrir el cartel de la marca
        /// </summary>
        public void GenerarCartel()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                this.IrURL(ConfigurationManager.AppSettings["UrlGenerarCartel"] + ((Marca)this._ventana.Marca).Id);

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
        }


        /// <summary>
        /// Método que se encarga de abrir el certificado de la marca en formato .pdf
        /// </summary>
        public void VerCertificado()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                //System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["rutaCertificados"].ToString() + ((Marca)this._ventana.Marca).CodigoRegistro + ".pdf");

                CertificadoMarca certificadoConsultar = new CertificadoMarca();
                CertificadoMarca certificado = null;
                certificadoConsultar.IdMarca = this._marca.Id;

                certificado = this._certificadoMarcaServicios.ConsultarPorId(certificadoConsultar);

                if (certificado != null)
                    this.Navegar(new GestionarCertificadoDeMarca(certificado, this._marca, this._ventana));
                else
                {
                    certificado = new CertificadoMarca(this._marca.Id);
                    certificado.FechaRecibo = DateTime.Today;
                    certificado.Operacion = "CREATE";
                    this.Navegar(new GestionarCertificadoDeMarca(certificado, this._marca, this._ventana));
                }


                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Win32Exception ex)
            {
                logger.Error(ex.Message);
                this._ventana.ArchivoNoEncontrado(Recursos.MensajesConElUsuario.ErrorCertificadoNoEncontrado);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }


        /// <summary>
        /// Método que se encarga de abrir el Solicitud de la marca en formato .pdf
        /// </summary>
        public void VerSolicitud()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["rutaSolicitudes"].ToString() + ((Marca)this._ventana.Marca).Id + ".pdf");

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Win32Exception ex)
            {
                logger.Error(ex.Message);
                this._ventana.ArchivoNoEncontrado(string.Format(Recursos.MensajesConElUsuario.ErrorSolicitudNoEncontrada, ((Marca)this._ventana.Marca).Id.ToString()));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }


        /// <summary>
        /// Método que se encarga de abrir el expediente de la marca en formato .pdf
        /// </summary>
        public void VerExpediente()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

               

                if (((Marca)this._ventana.Marca).LocalidadMarca != null)
                {
                    if (((Marca)this._ventana.Marca).LocalidadMarca.Equals("N"))
                    {
                        System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["rutaExpedientes"].ToString() + ((Marca)this._ventana.Marca).Id + ".pdf");
                    }
                    else if (((Marca)this._ventana.Marca).LocalidadMarca.Equals("I"))
                    {
                        if (((Marca)this._ventana.Marca).CodigoMarcaInternacional != 0 && ((Marca)this._ventana.Marca).CorrelativoExpediente != 0)

                            //prueba = ConfigurationManager.AppSettings["rutaExpedientesMarcasInternacionales"].ToString() + ((Marca)this._ventana.Marca).CodigoMarcaInternacional.ToString() + "-" + ((Marca)this._ventana.Marca).CorrelativoExpediente.ToString() + ".pdf";
                            System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["rutaExpedientesMarcasInternacionales"].ToString() + ((Marca)this._ventana.Marca).CodigoMarcaInternacional.ToString() + "-" + ((Marca)this._ventana.Marca).CorrelativoExpediente.ToString() + ".pdf");
                        else
                            System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["rutaExpedientesMarcasInternacionales"].ToString() + ((Marca)this._ventana.Marca).Id + ".pdf");

                    } 
                 }

                else
                {
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.MarcaLocalidadMarcaNulo, 1);
                }

  
                //System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["rutaExpedientes"].ToString() + ((Marca)this._ventana.Marca).Id + ".pdf");

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Win32Exception ex)
            {
                logger.Error(ex.Message);
                this._ventana.ArchivoNoEncontrado(string.Format(Recursos.MensajesConElUsuario.ErrorExpedienteNoEncontrado, ((Marca)this._ventana.Marca).Id.ToString()));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }


        /// <summary>
        /// Método que consulta la clase internacional y lo pega en distingue
        /// </summary>
        public void TomarClaseInternacional()
        {
            Internacional internacionalAux = new Internacional();

            internacionalAux = _internacionalServicios.ConsultarPorId(new Internacional(int.Parse(this._ventana.IdInternacional)));

            this._ventana.DistingueSolicitud = internacionalAux.Descripcion;
            this._ventana.DistingueDatos = internacionalAux.Descripcion;
        }


        public void MostrarEtiqueta()
        {
            Marca marcaAux = ((Marca)this._ventana.Marca);
            if (((Marca)this._ventana.Marca).BEtiqueta)
            {
                EtiquetaMarca detalleEtiqueta = new EtiquetaMarca(ConfigurationManager.AppSettings["RutaImagenesDeMarcas"] + marcaAux.Id + ".BMP", marcaAux.Descripcion);
                detalleEtiqueta.ShowDialog();

            }
        }


        public void MostrarDistingueIngles()
        {
            Marca marcaAux = ((Marca)this._ventana.Marca);
            if ((null != marcaAux.InfoAdicional) && !((Marca)this._ventana.Marca).Distingue.Equals(""))
            {
                ChildWindow detalleEtiqueta = new ChildWindow(marcaAux.InfoAdicional.Info);
                detalleEtiqueta.ShowDialog();

            }
        }


        public void BuscarCarta(int filtrarEn)
        {
            Carta primercaCarta = new Carta();
            Carta carta = new Carta();
            IList<Carta> listaCartas = null;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                
                if (filtrarEn == 0)
                {
                    carta.DescripcionDepartamento = this._ventana.DescripcionCartaOrdenSolicitudFiltrar.ToUpper();
                    carta.Id = this._ventana.IdCartaOrdenSolicitudFiltrar.Equals("") ? 0
                        : int.Parse(this._ventana.IdCartaOrdenSolicitudFiltrar);

                }
                else if (filtrarEn == 1)
                {
                    carta.DescripcionDepartamento = this._ventana.DescripcionCartaOrdenDatosFiltrar.ToUpper();
                    carta.Id = this._ventana.IdCartaOrdenDatosFiltrar.Equals("") ? 0
                        : int.Parse(this._ventana.IdCartaOrdenDatosFiltrar);
                }

                if ((!carta.DescripcionDepartamento.Equals("")) || (carta.Id != 0))
                    listaCartas = this._cartaServicios.ObtenerCartasFiltro(carta);
                else
                    listaCartas = new List<Carta>();

                if (listaCartas.Count != 0)
                {
                    listaCartas.Insert(0, primercaCarta);
                    this._ventana.CartasOrdenSolicitud = listaCartas;
                    this._ventana.CartasOrdenDatos = listaCartas;
                }
                else
                {

                    listaCartas.Insert(0, primercaCarta);
                    this._ventana.CartasOrdenSolicitud = listaCartas;
                    this._ventana.CartasOrdenDatos = listaCartas;
                    this._ventana.CartaOrdenSolicitud = primercaCarta;
                    this._ventana.CartaOrdenDatos = primercaCarta;
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

        }


        public void IrRenovacionDeMarca()
        {
            this.Navegar(new ConsultarRenovaciones(this._ventana.Marca, this._ventana));
        }


        public string ObtenerIdMarca()
        {
            return ((Marca)this._ventana.Marca).Id.ToString();
        }


        public void VerInstruccionesDeRenovacion()
        {
            Navegar(new ListaInstruccionesRenovacion(this._ventana.Marca));
        }


        public void IrVentanaAsociado()
        {
            if ((Asociado)this._ventana.AsociadoSolicitud != null)
            {
                Asociado asociado = ((Asociado)this._ventana.AsociadoSolicitud).Id != int.MinValue ? (Asociado)this._ventana.AsociadoSolicitud : null;
                Navegar(new ConsultarAsociado(asociado, this._ventana, false));
            }
        }


        public void IrVentanaInteresado()
        {
            if ((Interesado)this._ventana.InteresadoSolicitud != null)
            {
                Interesado interesado = ((Interesado)this._ventana.InteresadoSolicitud).Id != int.MinValue ? (Interesado)this._ventana.InteresadoSolicitud : null;
                Navegar(new ConsultarInteresado(interesado, this._ventana));
            }
        }


        public void IrVentanaMarcaOrigen()
        {
            int? _idMarcaOrigen = ((Marca)this._ventana.Marca).MarcaOrigen;
            
            if (_idMarcaOrigen != null)
            {
                int idMarcaOrigen = _idMarcaOrigen.Value;
                Marca marca = new Marca(idMarcaOrigen);
                this._ventana.MarcaOrigenCargada = true;
                //Navegar(new ConsultarMarca(marca, this._ventana));
                //this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleConsultarMarcaOrigen, "");
                Navegar(new ConsultarMarca(marca, this._ventana, this._ventana.MarcaOrigenCargada));
            }
        }


        public void IrVentanaPoder()
        {
            try
            {

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if ((Poder)this._ventana.PoderSolicitud != null)
                {

                    Poder poder = ((Marca)this._ventana.Marca).Poder.Id != int.MinValue ? ((Marca)this._ventana.Marca).Poder : null;
                    //--
                    object obj = this._ventana.PoderSolicitud;
                    //--
                    Interesado interesadoPoder = ((Marca)this._ventana.Marca).Interesado;
                    IList<Poder> listaPoderes = new List<Poder>();
                    listaPoderes = this._poderServicios.ConsultarPoderesPorInteresado(interesadoPoder);

                    if ((listaPoderes.Count != 0) && (poder != null))
                    {
                        if (EncontrarPoder(listaPoderes, poder))
                            Navegar(new ConsultarPoder(poder, this._ventana));

                        else
                            this._ventana.Mensaje("El Poder no corresponde con el Interesado. Cambielo", 0);
                    }
                    else
                        this._ventana.Mensaje("La Marca no tiene un poder válido. Asignele uno", 0);

                    //Navegar(new ConsultarPoder(poder, this._ventana));
                }


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


        }



        /// <summary>
        /// Metodo para encontrar un poder en una lista de poderes de un interesado especifico
        /// </summary>
        /// <param name="listaPoderes">Lista de poderes del Interesado</param>
        /// <param name="poder">Poder a buscar en la lista de poderes</param>
        /// <returns>Retorna true si encuentra el poder en la lista</returns>
        public bool EncontrarPoder(IList<Poder> listaPoderes, Poder poder)
        {
            bool poderEncontrado = false;

            foreach (Poder item in listaPoderes)
            {
                if (item.Id == poder.Id)
                {
                    poderEncontrado = true;
                    break;
                }
            }

            return poderEncontrado;
        }


        public bool ConsultarAsociado()
        {
            bool retorno = false;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion


                if ((!this._ventana.IdAsociadoInternacionalFiltrar.Equals(string.Empty)) || (!this._ventana.NombreAsociadoInternacionalFiltrar.Equals(string.Empty)))
                {
                    Asociado asociadoAux = new Asociado();
                    asociadoAux.Id = !this._ventana.IdAsociadoInternacionalFiltrar.Equals(string.Empty) ? int.Parse(this._ventana.IdAsociadoInternacionalFiltrar) : 0;
                    asociadoAux.Nombre = !this._ventana.NombreAsociadoInternacionalFiltrar.Equals(string.Empty) ? this._ventana.NombreAsociadoInternacionalFiltrar : string.Empty;

                    IList<Asociado> resultados = this._asociadoServicios.ObtenerAsociadosFiltro(asociadoAux);

                    resultados.Insert(0, new Asociado());

                    this._ventana.AsociadosInternacionalesDatos = resultados;
                    this._ventana.AsociadosInternacionales = resultados;

                    retorno = true;
                }

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

            return retorno;
        }


        public bool ConsultarAsociadoDatos()
        {
            bool retorno = false;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion


                if ((!this._ventana.IdAsociadoInternacionalFiltrarDatos.Equals(string.Empty)) || (!this._ventana.NombreAsociadoInternacionalFiltrarDatos.Equals(string.Empty)))
                {
                    Asociado asociadoAux = new Asociado();
                    asociadoAux.Id = !this._ventana.IdAsociadoInternacionalFiltrarDatos.Equals(string.Empty) ? int.Parse(this._ventana.IdAsociadoInternacionalFiltrarDatos) : 0;
                    asociadoAux.Nombre = !this._ventana.NombreAsociadoInternacionalFiltrarDatos.Equals(string.Empty) ? this._ventana.NombreAsociadoInternacionalFiltrarDatos : string.Empty;


                    IList<Asociado> resultados = this._asociadoServicios.ObtenerAsociadosFiltro(asociadoAux);


                    resultados.Insert(0, new Asociado());


                    this._ventana.AsociadosInternacionalesDatos = resultados;
                    this._ventana.AsociadosInternacionales = resultados;

                    retorno = true;
                }

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

            return retorno;
        }


        public bool CambiarAsociadoInternacionalSolicitud()
        {
            bool retorno = false;

            try
            {
                if (this._ventana.AsociadoInternacional != null)
                {
                    if (((Asociado)this._ventana.AsociadoInternacional).Id != 0)
                    {
                        this._ventana.TextoAsociadoInternacional = ((Asociado)this._ventana.AsociadoInternacional).Nombre;
                        this._ventana.AsociadoInternacionalDatos = this._ventana.AsociadoInternacional;
                        this._ventana.AsociadoInternacional = this._ventana.AsociadoInternacional;
                    }
                    else
                    {
                        this._ventana.AsociadoInternacional = null;
                        this._ventana.AsociadoInternacionalDatos = null;
                    }
                    retorno = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            return retorno;
        }


        public bool CambiarAsociadoInternacionalDatos()
        {
            bool retorno = false;

            try
            {
                if (this._ventana.AsociadoInternacionalDatos != null)
                {
                    if (((Asociado)this._ventana.AsociadoInternacionalDatos).Id != 0)
                    {
                        this._ventana.TextoAsociadoInternacional = ((Asociado)this._ventana.AsociadoInternacionalDatos).Nombre;
                        this._ventana.AsociadoInternacional = this._ventana.AsociadoInternacionalDatos;
                        this._ventana.AsociadoInternacionalDatos = this._ventana.AsociadoInternacionalDatos;
                    }
                    else
                    {
                        this._ventana.AsociadoInternacional = null;
                        this._ventana.AsociadoInternacionalDatos = null;
                    }

                    retorno = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            return retorno;
        }


        public void IrVentanaCorresponsal()
        {
            if ((Corresponsal)this._ventana.CorresponsalSolicitud != null)
            {
                Corresponsal corresponsal = ((Corresponsal)this._ventana.CorresponsalSolicitud).Id != int.MinValue ? (Corresponsal)this._ventana.CorresponsalSolicitud : null;
                Navegar(new AgregarCorresponsal(this._ventana, corresponsal));
            }
        }



        public void CargarAsociadoInternacionalVacio()
        {
            IList<Asociado> asociados = new List<Asociado>();
            asociados.Add(new Asociado());
            this._ventana.AsociadosInternacionalesDatos = asociados;
            this._ventana.AsociadosInternacionales = asociados;
        }

        public void IrVentanaImprimirEdoCuenta()
        {
            if ((Asociado)this._ventana.AsociadoSolicitud != null)
            {
                Asociado Asociado = ((Asociado)this._ventana.AsociadoSolicitud).Id != int.MinValue ? (Asociado)this._ventana.AsociadoSolicitud : null;
                Navegar(new PendientesRpt("2", Asociado));

            }
        }

        public void IrVentanaFacturacionDatos()
        {
            if ((Marca)this._ventana.Marca != null)
            {
                Marca Marca = ((Marca)this._ventana.Marca).Id != int.MinValue ? (Marca)this._ventana.Marca : null;
                string Id = System.Convert.ToString(Marca.Id);
                Navegar(new ConsultarFacVistaFacturaServicios(Id, "M"));

            }
        }

        public void CalcularSaldos()
        {
            if ((Asociado)this._ventana.AsociadoSolicitud != null)
            {
                Asociado Asociado = ((Asociado)this._ventana.AsociadoSolicitud).Id != int.MinValue ? (Asociado)this._ventana.AsociadoSolicitud : null;

                double? w_1, w_2, w_3, w_4, w_5, w_6, msaldope;
                w_1 = 0;
                w_2 = 0;
                w_3 = 0;
                w_4 = 0;
                w_5 = 0;
                w_6 = 0;
                msaldope = 0;
                string moneda = "";
                int casociado = Asociado.Id;
                int? dias = 30;
                CalcularSaldosAsociado(casociado, dias, ref w_1, ref w_2, ref w_3, ref w_4, ref w_5, ref w_6, ref msaldope, ref  moneda);

                if (moneda == "US")
                {

                    this._ventana.SaldoVencidoSolicitud = System.Convert.ToString(w_2);
                    this._ventana.SaldoVencidoDatos = System.Convert.ToString(w_2);
                    this._ventana.SaldoPorVencerSolicitud = System.Convert.ToString(w_4);
                    this._ventana.SaldoPorVencerDatos = System.Convert.ToString(w_4);
                    this._ventana.TotalSolicitud = System.Convert.ToString(w_2 + w_4);
                    this._ventana.TotalDatos = System.Convert.ToString(w_2 + w_4);

                }
                else
                {
                    this._ventana.SaldoVencidoSolicitud = System.Convert.ToString(w_1);
                    this._ventana.SaldoVencidoDatos = System.Convert.ToString(w_1);
                    this._ventana.SaldoPorVencerSolicitud = System.Convert.ToString(w_3);
                    this._ventana.SaldoPorVencerDatos = System.Convert.ToString(w_3);
                    this._ventana.TotalSolicitud = System.Convert.ToString(w_1 + w_3);
                    this._ventana.TotalDatos = System.Convert.ToString(w_1 + w_3);
                }
            }

        }


        /// <summary>
        /// Metodo que sirve para cambiar el Agente que se selecciona el combo de la ventana de consulta de la marca
        /// </summary>
        public void CambiarAgente()
        {
            IList<Poder> _poderesFiltrados = null; 
            Poder poderABuscar = null;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion


                if ((this._ventana.Agente != null) && (!((Agente)this._ventana.Agente).Id.Equals("NGN")))
                {
                    if ((this._ventana.InteresadoSolicitud != null) && (((Interesado)this._ventana.InteresadoSolicitud).Id != int.MinValue))
                    {
                        if ((null != this._ventana.PoderSolicitud) && 
                            (((Poder)this._ventana.PoderSolicitud).Id != int.MinValue))
                        {

                            ((Marca)this._ventana.Marca).Agente = (Agente)this._ventana.Agente;
                                                                             
                             
                            //Para validar que el Agente tiene poderes con el Interesado
                            Poder primerPoder = new Poder();
                            primerPoder.Id = int.MinValue;

                            _poderesFiltrados = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado((Agente)this._ventana.Agente, (Interesado)this._ventana.InteresadoSolicitud);

                            if (_poderesFiltrados.Count != 0)
                            {
                                poderABuscar = this.BuscarPoder(_poderesFiltrados, (Poder)this._ventana.PoderSolicitud);

                                if (poderABuscar != null)
                                {
                                    _poderesFiltrados.Insert(0, primerPoder);
                                    this._ventana.PoderesSolicitud = _poderesFiltrados;
                                    this._ventana.PoderesDatos = _poderesFiltrados;
                                    this._ventana.PoderSolicitud = poderABuscar;
                                }
                                else
                                {
                                    this._ventana.Mensaje("Seleccione un poder que relacione al Agente con el Interesado", 0);
                                    _poderesFiltrados.Insert(0, primerPoder);
                                    this._ventana.PoderesSolicitud = _poderesFiltrados;
                                    this._ventana.PoderesDatos = _poderesFiltrados;

                                }
                            }

                            else
                            {
                                this._ventana.Mensaje("El Agente no tiene poderes con el Interesado", 0);
                                _poderesFiltrados.Insert(0, primerPoder);
                                this._ventana.PoderesSolicitud = _poderesFiltrados;
                                this._ventana.PoderesDatos = _poderesFiltrados;
                            }

                            

                            
                        }
                        else
                        {
                            this._ventana.PoderesSolicitud = null;

                            CargarPoderesEntreInteresadoAgente();

                            ((Marca)this._ventana.Marca).Agente = (Agente)this._ventana.Agente;
                        }
                    }
                    else
                    {
                        this._ventana.Mensaje("Debe seleccionar un Interesado para la Marca", 0);
                        ((Marca)this._ventana.Marca).Agente = (Agente)this._ventana.Agente;
                    }
                }

                //else
                //{
                //    ((Marca)this._ventana.Marca).Agente = (Agente)this._ventana.Agente;
                //}

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException e)
            {
                throw;
            }
        }


        /// <summary>
        /// Metodo que sirve para consultar una carta orden de una marca
        /// </summary>
        public void ConsultarCartaOrden()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (this._ventana.IdCartaOrdenSolicitud != null && !this._ventana.IdCartaOrdenSolicitud.Equals(""))
                {
                    Carta carta = new Carta();
                    List<Carta> listaCartas = null;
                    carta.Id = int.Parse(this._ventana.IdCartaOrdenSolicitud);

                    listaCartas = this._cartaServicios.ObtenerCartasFiltro(carta).ToList<Carta>();

                    if (listaCartas.Count > 0)
                        carta = listaCartas[0];

                    this.Navegar(new ConsultarCarta(carta, this._ventana));

                }


                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception)
            {
                
                throw;
            }
        }



        /// <summary>
        /// Metodo que abre la ventana para ver la Instruccion de Correspondencia de la marca consultada
        /// </summary>
        public void GestionarInstruccionDeCorrespondencia()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                InstruccionCorrespondencia instruccionAConsultarEmails = null;
                InstruccionCorrespondencia instruccionEmails = null;

                InstruccionEnvioOriginales instruccionAConsultarEOriginales = null;
                InstruccionEnvioOriginales instruccionEOriginales = null;

                Marca marca = CargarMarcaDeLaPantalla();


                //Se obtiene la instruccion de Correspondencia de Envio de Emails de la Marca seleccionada
                instruccionAConsultarEmails = new InstruccionCorrespondencia(marca.Id);
                instruccionAConsultarEmails.Marca = marca;
                instruccionAConsultarEmails.AplicadaA = "M";
                instruccionAConsultarEmails.Concepto = "C"; //Concepto = Correspondencia "C"

                instruccionEmails = this._instruccionCorrespondenciaServicios.ObtenerInstruccionCorrespondencia(instruccionAConsultarEmails);


                //Se obtiene la instruccion de Correspondencia de Envio de Originales de la Marca seleccionada
                instruccionAConsultarEOriginales = new InstruccionEnvioOriginales(marca.Id);
                instruccionAConsultarEOriginales.Marca = marca;
                instruccionAConsultarEOriginales.AplicadaA = "M";
                instruccionAConsultarEOriginales.Concepto = "C";  //Concepto = Correspondencia "C"

                instruccionEOriginales = this._instruccionEnvioOriginalesServicios.ObtenerInstruccionEnvioOriginales(instruccionAConsultarEOriginales);

                //Logica para presentar los resultados de la consulta
                if ((instruccionEmails != null) || (instruccionEOriginales != null))
                {
                    if (instruccionEmails == null)
                    {
                        this.Navegar(new GestionarInstruccionCorrespondenciaMarca(instruccionAConsultarEmails, instruccionEOriginales, marca, this._ventana, this._ventanaPadre));
                    }
                    else if (instruccionEOriginales == null)
                    {
                        instruccionEmails.Marca = marca;
                        this.Navegar(new GestionarInstruccionCorrespondenciaMarca(instruccionEmails, instruccionAConsultarEOriginales, marca, this._ventana, this._ventanaPadre));
                    }
                    else
                    {
                        this.Navegar(new GestionarInstruccionCorrespondenciaMarca(instruccionEmails, instruccionEOriginales, marca, this._ventana, this._ventanaPadre));
                    }
                    
                }
                else
                    this.Navegar(new GestionarInstruccionCorrespondenciaMarca(instruccionAConsultarEmails, instruccionAConsultarEOriginales, marca, this._ventana, this._ventanaPadre));
                
               


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
        /// Metodo que llama a la ventana de Instrucciones de Facturacion
        /// </summary>
        public void GestionarInstruccionDeFacturacion()
        {
            try
            {

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                InstruccionCorrespondencia instruccionFac_Emails = null;
                InstruccionCorrespondencia instruccionEmails = null;

                InstruccionEnvioOriginales instruccionFac_EOriginales = null;
                InstruccionEnvioOriginales instruccionEOriginales = null;

                Marca marca = CargarMarcaDeLaPantalla();

                //Se obtiene la instruccion de Facturacion de Envio de Emails de la Marca seleccionada
                instruccionFac_Emails = new InstruccionCorrespondencia(marca.Id);
                instruccionFac_Emails.Marca = marca;
                instruccionFac_Emails.AplicadaA = "M";
                instruccionFac_Emails.Concepto = "F"; //Concepto = Facturacion "F"

                instruccionEmails = this._instruccionCorrespondenciaServicios.ObtenerInstruccionCorrespondencia(instruccionFac_Emails);


                //Se obtiene la instruccion de Facturacion de Envio de Originales de la Marca seleccionada
                instruccionFac_EOriginales = new InstruccionEnvioOriginales(marca.Id);
                instruccionFac_EOriginales.Marca = marca;
                instruccionFac_EOriginales.AplicadaA = "M";
                instruccionFac_EOriginales.Concepto = "F";  //Concepto = Facturacion "F"

                instruccionEOriginales = this._instruccionEnvioOriginalesServicios.ObtenerInstruccionEnvioOriginales(instruccionFac_EOriginales);

                //Logica para presentar los resultados de la consulta
                if ((instruccionEmails != null) || (instruccionEOriginales != null))
                {
                    if (instruccionEmails == null)
                    {
                        this.Navegar(new GestionarInstruccionFacturacionMarca(instruccionFac_Emails, instruccionEOriginales, marca, this._ventana, this._ventanaPadre));
                    }
                    else if (instruccionEOriginales == null)
                    {
                        instruccionEmails.Marca = marca;
                        this.Navegar(new GestionarInstruccionFacturacionMarca(instruccionEmails, instruccionFac_EOriginales, marca, this._ventana, this._ventanaPadre));
                    }
                    else
                    {
                        this.Navegar(new GestionarInstruccionFacturacionMarca(instruccionEmails, instruccionEOriginales, marca, this._ventana, this._ventanaPadre));
                    }

                }
                else
                    this.Navegar(new GestionarInstruccionFacturacionMarca(instruccionFac_Emails, instruccionFac_EOriginales, marca, this._ventana, this._ventanaPadre));
                


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
        /// Metodo que muestra la lista de las instrucciones no tipificadas de una marca
        /// </summary>
        public void IrListaInstruccionesNoTipificadas()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this.Navegar(new ListaInstruccionesNoTipificadasDeMarca(CargarMarcaDeLaPantalla(), this._ventana));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Metodo que muestra la lista de todas las instrucciones de descuento de una marca
        /// </summary>
        public void IrListaInstruccionesDeDescuento()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this.Navegar(new ListaInstruccionesDescuentoMarca(CargarMarcaDeLaPantalla(), this._ventana));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }

        /// <summary>
        /// Metodo que presenta el pdf del Expediente TyR de una Marca especifica consultada
        /// </summary>
        public void VerExpedienteTyR(String botonPresionado)
        {

            String ruta = String.Empty;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                switch (botonPresionado)
                {
                    case "_btnExptyrSolicitud" :
                        if ((this._ventana.IdExpTraspasoRenovacionSolicitud != null) && (!this._ventana.IdExpTraspasoRenovacionSolicitud.Equals(String.Empty)))
                        {
                            ruta = ConfigurationManager.AppSettings["RutaExpedienteTyRMarca"].ToString() + this._ventana.IdExpTraspasoRenovacionSolicitud + ".pdf";
                            System.Diagnostics.Process.Start(ruta);
                        }
                        break;
                    case "_btnExptyrDatos" :
                        if ((this._ventana.IdExpTraspasoRenovacionDatos != null) && (!this._ventana.IdExpTraspasoRenovacionDatos.Equals(String.Empty)))
                        {
                            ruta = ConfigurationManager.AppSettings["RutaExpedienteTyRMarca"].ToString() + this._ventana.IdExpTraspasoRenovacionDatos + ".pdf";
                            System.Diagnostics.Process.Start(ruta);
                        }
                        break;
                }

                
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Win32Exception ex)
            {
                logger.Error(ex.Message);
                this._ventana.ArchivoNoEncontrado(string.Format(Recursos.MensajesConElUsuario.ErrorExpedienteTyRMarcaNoExiste, this._ventana.IdExpTraspasoRenovacionDatos));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }

        /// <summary>
        /// Metodo que muestra la lista de los Interesados asociados a la Marca consultada
        /// </summary>
        public void VerAsociadosMultiplesMarca()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                this.Navegar(new ListaInteresadosMarca(this._ventana.Marca, this._ventana, this._ventanaPadre));

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Win32Exception ex)
            {
                logger.Error(ex.Message);
                this._ventana.ArchivoNoEncontrado(string.Format(Recursos.MensajesConElUsuario.ErrorExpedienteTyRMarcaNoExiste, this._ventana.IdExpTraspasoRenovacionDatos));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }
    }
}
