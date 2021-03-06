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
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Cliente.Presentadores.Marcas
{
    class PresentadorConsultarMarcas : PresentadorBase
    {
        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private IConsultarMarcas _ventana;

        private IMarcaServicios _marcaServicios;
        private IAsociadoServicios _asociadoServicios;
        private IInteresadoServicios _interesadoServicios;
        private ICorresponsalServicios _corresponsalServicios;
        private IServicioServicios _servicioServicios;
        private ITipoEstadoServicios _tipoEstadoServicios;
        private IPaisServicios _paisServicios;
        private ICondicionServicios _condicionServicios;
        private IBoletinServicios _boletinServicios;
        private IListaDatosValoresServicios _listaDatosValoresServicios;
        private IInfoAdicionalServicios _infoAdicionalServicios;

        private IList<Marca> _marcas;
        private IList<Asociado> _asociados;
        private IList<Asociado> _asociadosint;
        private IList<Interesado> _interesados;
        private IList<Corresponsal> _corresponsales;
        private IList<ListaDatosValores> _origenesMarca;

        private int _filtroValido;//Variable utilizada para limitar a que el filtro se ejecute solo cuando 
        //dos filtros sean utilizados

        private bool _soloLocalidad = false; //Variable utilizada para indicar que solamente se hara la consulta de las marcas tomando en cuenta la localidad de la misma
        private String _localidadAConsultar; //Variable que indica que localidad se va a consultar, si marcas Nacionales o marcas Internacionales
        private bool _consultaInfoAdicional = false; //Variable que indica que se consultara por InfoAdicional (Distingue en Ingles)

        /// <summary>
        /// Constructor Predeterminado
        /// </summary>
        /// <param name="ventana">página que satisface el contrato</param>
        public PresentadorConsultarMarcas(IConsultarMarcas ventana)
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                this._ventana = ventana;
                Marca marca = new Marca();
                marca.Internacional = new Internacional();
                marca.Nacional = new Nacional();

                this._ventana.MarcaParaFiltrar = marca;

                this._marcaServicios = (IMarcaServicios)Activator.GetObject(typeof(IMarcaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["MarcaServicios"]);
                this._asociadoServicios = (IAsociadoServicios)Activator.GetObject(typeof(IAsociadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsociadoServicios"]);
                this._interesadoServicios = (IInteresadoServicios)Activator.GetObject(typeof(IInteresadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InteresadoServicios"]);
                this._corresponsalServicios = (ICorresponsalServicios)Activator.GetObject(typeof(ICorresponsalServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CorresponsalServicios"]);
                this._servicioServicios = (IServicioServicios)Activator.GetObject(typeof(IServicioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ServicioServicios"]);
                this._tipoEstadoServicios = (ITipoEstadoServicios)Activator.GetObject(typeof(ITipoEstadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["TipoEstadoServicios"]);
                this._paisServicios = (IPaisServicios)Activator.GetObject(typeof(IPaisServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["PaisServicios"]);
                this._condicionServicios = (ICondicionServicios)Activator.GetObject(typeof(ICondicionServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CondicionServicios"]);
                this._boletinServicios = (IBoletinServicios)Activator.GetObject(typeof(IBoletinServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["BoletinServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IBoletinServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);
                this._infoAdicionalServicios = (IInfoAdicionalServicios)Activator.GetObject(typeof(IInfoAdicionalServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InfoAdicionalServicios"]);

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
        /// Método que se encarga de actualizar el títutlo de la ventana
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
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                ActualizarTitulo();

                this._ventana.LimpiarCampos();

                IList<Asociado> asociados = new List<Asociado>();
                Asociado primerAsociado = new Asociado();
                primerAsociado.Id = int.MinValue;
                asociados.Insert(0, primerAsociado);
                this._ventana.Asociados = asociados;
                this._asociados = asociados;

                IList<Asociado> asociadosInt = new List<Asociado>();
                Asociado primerAsociadoInt = new Asociado();
                primerAsociadoInt.Id = int.MinValue;
                asociadosInt.Insert(0, primerAsociadoInt);
                this._ventana.AsociadosInt = asociadosInt;
                this._asociadosint = asociadosInt;

                IList<Interesado> interesados = new List<Interesado>();
                Interesado primerInteresado = new Interesado();
                primerInteresado.Id = int.MinValue;
                interesados.Insert(0, primerInteresado);
                this._ventana.Interesados = interesados;
                this._interesados = interesados;

                IList<Corresponsal> corresponsales = this._corresponsalServicios.ConsultarTodos();
                Corresponsal primerCorresponsal = new Corresponsal();
                primerCorresponsal.Id = int.MinValue;
                corresponsales.Insert(0, primerCorresponsal);
                this._ventana.Corresponsales = corresponsales;
                this._corresponsales = corresponsales;

                IList<Pais> paises = this._paisServicios.ConsultarTodos();
                Pais primerPais = new Pais();
                primerPais.Id = int.MinValue;
                paises.Insert(0, primerPais);
                this._ventana.Paises = paises;
                this._ventana.PaisesPrioridad = paises;

                IList<ListaDatosValores> tiposBusqueda = this._listaDatosValoresServicios.
                                ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiLocalidadMarca));
                this._ventana.TiposBusqueda = tiposBusqueda;
                this._ventana.TipoBusqueda = this.BuscarTipoDeBusqueda(tiposBusqueda);

                IList<TipoEstado> tipoEstados = this._tipoEstadoServicios.ConsultarTodos();
                TipoEstado primerDetalle = new TipoEstado();
                primerDetalle.Id = "NGN";
                tipoEstados.Insert(0, primerDetalle);
                this._ventana.Detalles = tipoEstados;

                IList<Servicio> servicios = this._servicioServicios.ConsultarTodos();
                Servicio primerServicio = new Servicio();
                primerServicio.Id = "NGN";
                servicios.Insert(0, primerServicio);
                this._ventana.Servicios = servicios;
                //this._ventana.Servicio = this.BuscarServicio(servicios, marca.Servicio);

                IList<Condicion> condiciones = this._condicionServicios.ConsultarTodos();
                Condicion primeraCondicion = new Condicion();
                primeraCondicion.Id = int.MinValue;
                condiciones.Insert(0, primeraCondicion);
                this._ventana.Condiciones = condiciones;

                IList<Boletin> boletines = this._boletinServicios.ConsultarTodos();
                Boletin primerBoletin = new Boletin();
                primerBoletin.Id = int.MinValue;
                boletines.Insert(0, primerBoletin);
                this._ventana.BoletinesOrdenPublicacion = boletines;
                this._ventana.BoletinesPublicacion = boletines;
                this._ventana.BoletinesConcesion = boletines;

                IList<ListaDatosValores> origenAsociados =
                    this._listaDatosValoresServicios.ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiOrigenClienteAsociado));
                ListaDatosValores primerOrigenAsociado = new ListaDatosValores();
                primerOrigenAsociado.Id = "NGN";
                origenAsociados.Insert(0, primerOrigenAsociado);
                this._ventana.OrigenesAsociados = origenAsociados;

                IList<ListaDatosValores> origenInteresados =
                    this._listaDatosValoresServicios.ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiOrigenClienteAsociado));
                ListaDatosValores primerOrigenInteresado = new ListaDatosValores();
                primerOrigenInteresado.Id = "NGN";
                origenInteresados.Insert(0, primerOrigenInteresado);
                this._ventana.OrigenesInteresados = origenInteresados;

                this._origenesMarca =
                            this._listaDatosValoresServicios.ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiOrigenClienteAsociado));
                ListaDatosValores primerOrigenMarca = new ListaDatosValores();
                primerOrigenMarca.Id = "NGN";
                this._origenesMarca.Insert(0, primerOrigenMarca);
                this._ventana.OrigenesMarca = this._origenesMarca;

                this._ventana.TotalHits = "0";
                this._ventana.FocoPredeterminado();

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
        /// Método que realiza una consulta al servicio, con el fin de filtrar los datos que se muestran 
        /// por pantalla
        /// </summary>
        public void Consultar()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Mouse.OverrideCursor = Cursors.Wait;
                bool consultaResumen = false;
                bool confirmacion = false;

                _filtroValido = 0;

                Marca MarcaAuxiliar = new Marca();
                MarcaAuxiliar.Id = int.MinValue;
                IList<Marca> marcasDesinfladas = new List<Marca>();

                MarcaAuxiliar = ObtenerMarcaFiltro();

                _soloLocalidad = ConsultaSoloLocalidad(MarcaAuxiliar);

                if (_soloLocalidad)
                {
                    confirmacion = this._ventana.ConfirmarConsultaPorLocalidad(_localidadAConsultar);
                }



                if ((confirmacion && _soloLocalidad) || (confirmacion && !_soloLocalidad) || (!confirmacion && !_soloLocalidad))
                {
                    //if (_filtroValido >= 2)
                    if ((_filtroValido >= 2) && (!_consultaInfoAdicional))
                    {

                        this._marcas = this._marcaServicios.ObtenerMarcasFiltro(MarcaAuxiliar);

                        //IList<Marca> marcasDesinfladas = new List<Marca>();

                        foreach (Marca marca in this._marcas)
                        {
                            MarcaAuxiliar = new Marca(marca.Id);
                            MarcaAuxiliar.PrimeraReferencia = marca.PrimeraReferencia;
                            Asociado asociadoAuxiliar = new Asociado();
                            Asociado asociadoIntAuxiliar = new Asociado();
                            Interesado interesadoAuxiliar = new Interesado();
                            Servicio servicioAuxiliar = new Servicio();

                            MarcaAuxiliar.Descripcion = marca.Descripcion != null ? marca.Descripcion : "";

                            if ((marca.Asociado != null) && (!string.IsNullOrEmpty(marca.Asociado.Nombre)))
                            {
                                asociadoAuxiliar.Nombre = marca.Asociado.Nombre;
                                asociadoAuxiliar.Id = marca.Asociado.Id;
                                MarcaAuxiliar.Asociado = asociadoAuxiliar;
                            }

                            if ((marca.AsociadoInternacional != null) && (!string.IsNullOrEmpty(marca.AsociadoInternacional.Nombre)))
                            {
                                asociadoIntAuxiliar.Nombre = marca.AsociadoInternacional.Nombre;
                                asociadoIntAuxiliar.Id = marca.AsociadoInternacional.Id;
                                MarcaAuxiliar.Asociado = asociadoIntAuxiliar;
                            }

                            if ((marca.Interesado != null) && (!string.IsNullOrEmpty(marca.Interesado.Nombre)))
                            {
                                interesadoAuxiliar.Nombre = marca.Interesado.Nombre;
                                interesadoAuxiliar.Id = marca.Interesado.Id;
                                MarcaAuxiliar.Interesado = interesadoAuxiliar;
                            }

                            if ((marca.Servicio != null) && (!string.IsNullOrEmpty(marca.Servicio.Descripcion)))
                            {
                                servicioAuxiliar = marca.Servicio;
                                MarcaAuxiliar.Servicio = servicioAuxiliar;
                            }

                            MarcaAuxiliar.Nacional = marca.Nacional;
                            MarcaAuxiliar.Internacional = marca.Internacional;
                            MarcaAuxiliar.CodigoInscripcion = marca.CodigoInscripcion;
                            //---
                            MarcaAuxiliar.FechaInscripcion = marca.FechaInscripcion;
                            MarcaAuxiliar.CodigoMarcaInternacional = marca.CodigoMarcaInternacional;
                            MarcaAuxiliar.CorrelativoExpediente = marca.CorrelativoExpediente;
                            MarcaAuxiliar.FechaRenovacion = marca.FechaRenovacion;
                            MarcaAuxiliar.CodigoRegistro = marca.CodigoRegistro;
                            MarcaAuxiliar.FechaRegistro = marca.FechaRegistro;
                            if (marca.Etiqueta.Equals("SI"))
                                MarcaAuxiliar.Etiqueta = "SI";
                            else
                                MarcaAuxiliar.Etiqueta = "NO";
                            //--

                            MarcaAuxiliar.FechaPublicacion = marca.FechaPublicacion != null ? marca.FechaPublicacion : null;

                            marcasDesinfladas.Add(MarcaAuxiliar);

                        }

                        this._ventana.Resultados = marcasDesinfladas;
                        this._ventana.TotalHits = marcasDesinfladas.Count.ToString();
                        if (marcasDesinfladas.Count == 0)
                            this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                    }
                    else if ((_filtroValido >= 2) && (_consultaInfoAdicional))
                    {
                        IList<InfoAdicional> infoAdicionalLista = this._infoAdicionalServicios.ObtenerDistingueInglesPorFiltro(MarcaAuxiliar.InfoAdicional);
                        Marca marcaAux = new Marca();
                        marcaAux = MarcaAuxiliar;
                        IList<Marca> marcaLista = new List<Marca>();
                        String codigoMarca = String.Empty;
                        String[] codigos = null;
                        

                        foreach (InfoAdicional item in infoAdicionalLista)
                        {
                            codigos = item.Id.Split('.');
                            marcaAux.Id = int.Parse(codigos[1]);
                            marcaLista = this._marcaServicios.ObtenerMarcasFiltro(marcaAux);
                            if (marcaLista.Count != 0)
                            {
                                marcasDesinfladas.Add(marcaLista[0]);
                            }

                            codigos = null;
                            //marcaLista.RemoveAt(0); 
                        }

                        this._marcas = marcasDesinfladas;
                        this._ventana.Resultados = marcasDesinfladas;
                        this._ventana.TotalHits = marcasDesinfladas.Count.ToString();
                        if (marcasDesinfladas.Count == 0)
                            this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                    }
                    else
                    {

                        this._ventana.Resultados = null;
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.ErrorFiltroIncompleto, 0);


                    }
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
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        /// <summary>
        /// Método que devuelve la marca que se utilizara para realizar el filtrado
        /// </summary>
        /// <returns>Marca cargada con el filtro</returns>
        private Marca ObtenerMarcaFiltro()
        {
            Marca marcaAuxiliar = ((Marca)this._ventana.MarcaParaFiltrar);
            try
            {

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (ValidarBusquedaDeMarcas())
                {
                    //if (this._ventana.InternacionalEstaSeleccionado) 
                    //{
                    //    marcaAuxiliar = TomarDatosMarcaFiltroInternacional(marcaAuxiliar);
                    //}

                    if (this._ventana.NacionalEstaSeleccionado)
                    {
                        marcaAuxiliar = TomarDatosMarcaFiltroNacional(marcaAuxiliar);
                    }

                    if (this._ventana.BoletinesEstaSeleccionado)
                    {
                        marcaAuxiliar = TomarDatosMarcaFiltroBoletines(marcaAuxiliar);
                    }

                    if (this._ventana.IndicadoresEstaSeleccionado)
                    {
                        marcaAuxiliar = TomarDatosMarcaFiltroIndicadores(marcaAuxiliar);
                    }

                    if (this._ventana.PrioridadesEstaSeleccionado)
                    {
                        marcaAuxiliar = TomarDatosMarcaFiltroPrioridades(marcaAuxiliar);
                    }

                    if (this._ventana.TYREstaSeleccionado)
                    {
                        marcaAuxiliar = TomarDatosMarcaFiltroTYR(marcaAuxiliar);
                    }

                    //---
                   

                    //---
                }
                else
                {
                    //Error ya que debe seleccionar al menos un check
                    //this._ventana.Mensaje()
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

            return marcaAuxiliar;
        }


        /// <summary>
        /// Método que devuelve la marca con los datos del check TYR
        /// </summary>
        /// <param name="marcaAuxiliar">Marca a cargar</param>
        /// <returns>marca cargada con los datos de TYR</returns>
        private Marca TomarDatosMarcaFiltroTYR(Marca marcaAuxiliar)
        {
            try
            {

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (!this._ventana.CodigoRegistro.Equals(""))
                {
                    marcaAuxiliar.CodigoRegistro = this._ventana.CodigoRegistro.ToUpper();
                    _filtroValido = 2;
                }
                else
                    this._ventana.CodigoRegistro = string.Empty;


                if (!this._ventana.FechaRegistro.Equals(""))
                {
                    DateTime fechaRegistro = DateTime.Parse(this._ventana.FechaRegistro);
                    _filtroValido = 2;
                    marcaAuxiliar.FechaRegistro = fechaRegistro;
                }

                if (this._ventana.RenovadoPorOtroTramitante)
                {
                    marcaAuxiliar.BRenovacionOtroTramitante = this._ventana.RenovadoPorOtroTramitante;
                    _filtroValido = 2;
                }

                if ((null != this._ventana.Condicion) && (((Condicion)this._ventana.Condicion).Id != int.MinValue))
                {
                    Condicion condicionAux = (Condicion)this._ventana.Condicion;
                    marcaAuxiliar.NumeroCondiciones = condicionAux.Id;
                    _filtroValido = 2;
                }
                else
                    marcaAuxiliar.NumeroCondiciones = null;

                if (this._ventana.InstruccionesDeRenovacion)
                {
                    marcaAuxiliar.BInstruccionesRenovacion = this._ventana.InstruccionesDeRenovacion;
                    _filtroValido = 2;
                }

                if (!this._ventana.ExpCambioPendiente.Equals("") || !this._ventana.ExpCambioPendiente.Equals(String.Empty))
                {
                    _filtroValido = 2;
                    //---
                    marcaAuxiliar.ExpTraspasoRenovacion = this._ventana.ExpCambioPendiente;
                }

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                logger.Debug(ex.Message);
            }

            return marcaAuxiliar;
        }


        /// <summary>
        /// Método que devuelve la marca con los datos del check Prioridades
        /// </summary>
        /// <param name="marcaAuxiliar">Marca a cargar</param>
        /// <returns>marca cargada con los datos de Prioridades</returns>
        private Marca TomarDatosMarcaFiltroPrioridades(Marca marcaAuxiliar)
        {

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            if ((null != this._ventana.PaisPrioridad) && (((Pais)this._ventana.PaisPrioridad).Id != int.MinValue))
            {
                marcaAuxiliar.Pais = new Pais();
                marcaAuxiliar.Pais = ((Pais)this._ventana.PaisPrioridad);
                _filtroValido = 2;
            }
            else
                marcaAuxiliar.Pais = null;

            if (!this._ventana.PrioridadCodigo.Equals(""))
            {
                marcaAuxiliar.CPrioridad = this._ventana.PrioridadCodigo;
                _filtroValido = 2;
            }
            else
                marcaAuxiliar.CPrioridad = null;

            if (!this._ventana.PrioridadFecha.Equals(""))
            {
                DateTime fechaPrioridad = DateTime.Parse(this._ventana.PrioridadFecha);
                _filtroValido = 2;
                marcaAuxiliar.FechaPrioridad = fechaPrioridad;
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
            return marcaAuxiliar;
        }


        /// <summary>
        /// Método que devuelve la marca con los datos del check Indicadores
        /// </summary>
        /// <param name="marcaAuxiliar">Marca a cargar</param>
        /// <returns>marca cargada con los datos de Indicadores</returns>
        private Marca TomarDatosMarcaFiltroIndicadores(Marca marcaAuxiliar)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
            return marcaAuxiliar;
        }


        /// <summary>
        /// Método que devuelve la marca con los datos del check Boletines
        /// </summary>
        /// <param name="marcaAuxiliar">Marca a cargar</param>
        /// <returns>marca cargada con los datos de Boletines</returns>
        private Marca TomarDatosMarcaFiltroBoletines(Marca marcaAuxiliar)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion



            if (((Boletin)this._ventana.BoletinConcesion).Id != int.MinValue)
            {
                marcaAuxiliar.BoletinConcesion = new Boletin();
                marcaAuxiliar.BoletinConcesion = ((Boletin)this._ventana.BoletinConcesion);
                _filtroValido = 2;
            }
            else
                marcaAuxiliar.BoletinConcesion = null;

            if (((Boletin)this._ventana.BoletinPublicacion).Id != int.MinValue)
            {
                marcaAuxiliar.BoletinPublicacion = new Boletin();
                marcaAuxiliar.BoletinPublicacion = ((Boletin)this._ventana.BoletinPublicacion);
                _filtroValido = 2;
            }
            else
                marcaAuxiliar.BoletinPublicacion = null;


            if (((Boletin)this._ventana.BoletinOrdenPublicacion).Id != int.MinValue)
            {
                marcaAuxiliar.BoletinOrdenPublicacion = new Boletin();
                marcaAuxiliar.BoletinOrdenPublicacion = ((Boletin)this._ventana.BoletinOrdenPublicacion);
                _filtroValido = 2;
            }
            else
                marcaAuxiliar.BoletinOrdenPublicacion = null;


            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return marcaAuxiliar;
        }


        /// <summary>
        /// Método que devuelve la marca con los datos del check Nacional
        /// </summary>
        /// <param name="marcaAuxiliar">Marca a cargar</param>
        /// <returns>marca cargada con los datos de Nacional</returns>
        private Marca TomarDatosMarcaFiltroNacional(Marca marcaAuxiliar)
        {
            int indexStr;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            try
            {
                marcaAuxiliar.LocalidadMarca = ((ListaDatosValores)this._ventana.TipoBusqueda).Valor;

                if (!this._ventana.Id.Equals(""))
                {
                    _filtroValido = 2;
                    marcaAuxiliar.Id = int.Parse(this._ventana.Id);
                }
                else
                    //marcaAuxiliar.Id = 0;
                    marcaAuxiliar.Id = int.MinValue;

                if ((null != this._ventana.Asociado) && (((Asociado)this._ventana.Asociado).Id != int.MinValue))
                {
                    marcaAuxiliar.Asociado = new Asociado();
                    //marcaAuxiliar.Asociado = (Asociado)this._ventana.Asociado;
                    marcaAuxiliar.Asociado.Id = ((Asociado)this._ventana.Asociado).Id; 
                    _filtroValido = 2;
                }
                else
                    marcaAuxiliar.Asociado = null;

                if ((null != this._ventana.AsociadoInt) && (((Asociado)this._ventana.AsociadoInt).Id != int.MinValue))
                {
                    marcaAuxiliar.AsociadoInternacional = new Asociado();
                    //marcaAuxiliar.Asociado = (Asociado)this._ventana.Asociado;
                    marcaAuxiliar.AsociadoInternacional.Id = ((Asociado)this._ventana.AsociadoInt).Id;
                    _filtroValido = 2;
                }
                else
                    marcaAuxiliar.AsociadoInternacional = null;

                if ((null != this._ventana.OrigenAsociado) && (!((ListaDatosValores)this._ventana.OrigenAsociado).Id.Equals("NGN")))
                {
                    if (marcaAuxiliar.Asociado != null)
                    {
                        marcaAuxiliar.Asociado.OrigenCliente = ((ListaDatosValores)this._ventana.OrigenAsociado).Valor;
                        _filtroValido = 2;
                    }
                    else
                    {
                        marcaAuxiliar.Asociado = new Asociado();
                        marcaAuxiliar.Asociado.Id = int.MinValue;
                        marcaAuxiliar.Asociado.OrigenCliente = ((ListaDatosValores)this._ventana.OrigenAsociado).Valor;
                        _filtroValido = 2;
                    }
                }
                
                


                if ((null != this._ventana.Interesado) && (((Interesado)this._ventana.Interesado).Id != int.MinValue))
                {
                    marcaAuxiliar.Interesado = new Interesado();
                    //marcaAuxiliar.Interesado = (Interesado)this._ventana.Interesado;
                    marcaAuxiliar.Interesado.Id = ((Interesado)this._ventana.Interesado).Id;
                    _filtroValido = 2;
                }
                else
                    marcaAuxiliar.Interesado = null;

                if ((null != this._ventana.OrigenInteresado) && (!((ListaDatosValores)this._ventana.OrigenInteresado).Id.Equals("NGN")))
                {
                    if (marcaAuxiliar.Interesado != null)
                    {
                        marcaAuxiliar.Interesado.OrigenCliente = ((ListaDatosValores)this._ventana.OrigenInteresado).Valor;
                        _filtroValido = 2;
                    }
                    else
                    {
                        marcaAuxiliar.Interesado = new Interesado();
                        marcaAuxiliar.Interesado.Id = int.MinValue;
                        marcaAuxiliar.Interesado.OrigenCliente = ((ListaDatosValores)this._ventana.OrigenInteresado).Valor;
                        _filtroValido = 2;
                    }
                }

                if ((null != this._ventana.OrigenMarca) && (!((ListaDatosValores)this._ventana.OrigenMarca).Id.Equals("NGN")))
                {
                    marcaAuxiliar.OrigenMarca = ((ListaDatosValores)this._ventana.OrigenMarca).Valor;
                    _filtroValido = 2;
                }
                else
                    marcaAuxiliar.OrigenMarca = null;
                

                if ((null != this._ventana.Corresponsal) && (((Corresponsal)this._ventana.Corresponsal).Id != int.MinValue))
                {
                    marcaAuxiliar.Corresponsal = new Corresponsal();
                    marcaAuxiliar.Corresponsal = (Corresponsal)this._ventana.Corresponsal;
                    _filtroValido = 2;
                }
                else
                    marcaAuxiliar.Corresponsal = null;

                if (!this._ventana.DescripcionFiltrar.Equals(""))
                {
                    _filtroValido = 2;
                    if (this._ventana.DescripcionFiltrar.Contains("'"))
                    {
                        indexStr = this._ventana.DescripcionFiltrar.IndexOf("'");
                        string cadena = this._ventana.DescripcionFiltrar.Insert(indexStr + 1, "'");
                        marcaAuxiliar.Descripcion = cadena.ToUpper();
                    }
                    else
                    {
                        marcaAuxiliar.Descripcion = this._ventana.DescripcionFiltrar.ToUpper();
                    }
                }
                else
                    marcaAuxiliar.Descripcion = null;

                
                if (!this._ventana.Otros.Equals(""))
                {

                    marcaAuxiliar.PrimerOtro = this._ventana.Otros;
                    _filtroValido = 2;
                }
                else
                    marcaAuxiliar.PrimerOtro = null;


                if (!this._ventana.Fecha.Equals(""))
                {
                    DateTime fechaPublicacion = DateTime.Parse(this._ventana.Fecha);
                    _filtroValido = 2;
                    marcaAuxiliar.FechaPublicacion = fechaPublicacion;
                }
                else
                    marcaAuxiliar.FechaPublicacion = null;

                if (!((TipoEstado)this._ventana.Detalle).Id.Equals("NGN"))
                {
                    _filtroValido = 2;
                    marcaAuxiliar.TipoEstado = new TipoEstado();
                    marcaAuxiliar.TipoEstado = ((TipoEstado)this._ventana.Detalle);
                }
                else
                    marcaAuxiliar.TipoEstado = null;

                if (!((Servicio)this._ventana.Servicio).Id.Equals("NGN"))
                {
                    _filtroValido = 2;
                    marcaAuxiliar.Servicio = new Servicio();
                    marcaAuxiliar.Servicio = ((Servicio)this._ventana.Servicio);
                }
                else
                    marcaAuxiliar.Servicio = null;

                if ((!this._ventana.ClaseInternacional.Equals("")) && (this._ventana.ClaseInternacional != "0"))
                {
                    _filtroValido = 2;
                    marcaAuxiliar.Internacional = new Internacional();
                    marcaAuxiliar.Internacional.Id = int.Parse(this._ventana.ClaseInternacional);
                }
                else
                    marcaAuxiliar.Internacional = new Internacional();

                if ((!this._ventana.ClaseNacional.Equals("")) && (this._ventana.ClaseNacional != "0"))
                {
                    _filtroValido = 2;
                    marcaAuxiliar.Nacional = new Nacional();
                    marcaAuxiliar.Nacional.Id = int.Parse(this._ventana.ClaseNacional);
                }
                else
                    marcaAuxiliar.Nacional = new Nacional();

                //if (!this._ventana.Distingue.Equals(""))
                //{
                //    _filtroValido = 2;
                //    marcaAuxiliar.Distingue = this._ventana.Distingue.ToUpper();
                //}
                //else
                //    marcaAuxiliar.Distingue = null;

                if (!this._ventana.Distingue.Equals(""))
                {
                    _filtroValido = 2;
                    marcaAuxiliar.XDistingue = this._ventana.Distingue.ToUpper();
                }
                else
                    marcaAuxiliar.XDistingue = null;

                //---
                if (!this._ventana.DistingueEnIngles.Equals(""))
                {
                    _filtroValido = 2;
                    _consultaInfoAdicional = true;
                    marcaAuxiliar.InfoAdicional = new InfoAdicional();
                    ((InfoAdicional)marcaAuxiliar.InfoAdicional).Info = this._ventana.DistingueEnIngles.ToUpper();
                    ((InfoAdicional)marcaAuxiliar.InfoAdicional).Id = "M.";

                }


                //---

                if ((!this._ventana.Solicitud.Equals("")) && (this._ventana.Solicitud != "0"))
                {
                    _filtroValido = 2;
                    marcaAuxiliar.CodigoInscripcion = this._ventana.Solicitud;
                }
                else
                    marcaAuxiliar.CodigoInscripcion = null;

                if (null != marcaAuxiliar.PrimeraReferencia)
                {
                    _filtroValido = 2;
                }
                else
                {
                    _filtroValido = 2;
                    
                }

                if (marcaAuxiliar.LocalidadMarca.Equals("I"))
                {
                    if (!this._ventana.IdInternacional.Equals(string.Empty))
                    {
                        _filtroValido = 2;
                        marcaAuxiliar.CodigoMarcaInternacional = int.Parse(this._ventana.IdInternacional);
                    }
                    else { marcaAuxiliar.CodigoMarcaInternacional = 0; }

                    if (!this._ventana.IdCorrelativoInternacional.Equals(string.Empty))
                    {
                        _filtroValido = 2;
                        marcaAuxiliar.CorrelativoExpediente = int.Parse(this._ventana.IdCorrelativoInternacional);
                    }
                    else { marcaAuxiliar.CorrelativoExpediente = 0; }

                    if ((this._ventana.Pais != null) && !((Pais)this._ventana.Pais).Id.Equals("NGN"))
                    {
                        _filtroValido = 2;
                        marcaAuxiliar.PaisInternacional = new Pais();
                        marcaAuxiliar.PaisInternacional = ((Pais)this._ventana.Pais);
                    }
                    else { marcaAuxiliar.PaisInternacional = null; }

                    if (!this._ventana.ReferenciaAsociado.Equals(string.Empty))
                    {
                        _filtroValido = 2;
                        marcaAuxiliar.ReferenciaAsociadoInternacional = this._ventana.ReferenciaAsociado;
                    }

                    if (!this._ventana.ReferenciaInteresado.Equals(string.Empty))
                    {
                        _filtroValido = 2;
                        marcaAuxiliar.ReferenciaInteresadoInternacional = this._ventana.ReferenciaInteresado;
                    }
                    else
                    {
                        _filtroValido = 2;
                        
                    }

                }

                

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                logger.Debug(ex.Message);
            }

            return marcaAuxiliar;
        }


        /// <summary>
        /// Método que devuelve la marca con los datos del check Internacional
        /// </summary>
        /// <param name="marcaAuxiliar">Marca a cargar</param>
        /// <returns>marca cargada con los datos de Internacional</returns>
        private Marca TomarDatosMarcaFiltroInternacional(Marca marcaAuxiliar)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            //if (!this._ventana.IdMarcaInternacional.Equals(string.Empty))
            //{
            //    _filtroValido = 2;
            //    marcaAuxiliar.Id = int.Parse(this._ventana.IdMarcaInternacional);
            //}

            if (!this._ventana.IdInternacional.Equals(string.Empty))
            {
                _filtroValido = 2;
                marcaAuxiliar.CodigoMarcaInternacional = int.Parse(this._ventana.IdInternacional);
            }

            if (!this._ventana.IdCorrelativoInternacional.Equals(string.Empty))
            {
                _filtroValido = 2;
                marcaAuxiliar.CorrelativoExpediente = int.Parse(this._ventana.IdCorrelativoInternacional);
            }

            if (!this._ventana.IdCorrelativoInternacional.Equals(string.Empty))
            {
                _filtroValido = 2;
                marcaAuxiliar.CorrelativoExpediente = int.Parse(this._ventana.IdCorrelativoInternacional);
            }


            #region trace

            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return marcaAuxiliar;
        }


        /// <summary>
        /// Método que valida que la pantalla de busqueda se encuentre seleccionado al menos un check
        /// </summary>
        /// <returns>true en caso de que haya al menos uno seleccionado, false en caso contrario</returns>
        private bool ValidarBusquedaDeMarcas()
        {

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            bool retorno = true;

            if (!this._ventana.InternacionalEstaSeleccionado)

                if (!this._ventana.NacionalEstaSeleccionado)

                    if (!this._ventana.BoletinesEstaSeleccionado)

                        if (!this._ventana.IndicadoresEstaSeleccionado)

                            if (!this._ventana.PrioridadesEstaSeleccionado)

                                if (!this._ventana.TYREstaSeleccionado)

                                    retorno = false;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }


        /// <summary>
        /// Método que invoca una nueva página "ConsultarPoder" y la instancia con el objeto seleccionado
        /// </summary>
        public void IrConsultarMarca()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            if (this._ventana.MarcaSeleccionada != null)
            {
                Marca marcaParaNavegar = null;
                bool encontrada = false;
                int cont = 0;
                while (!encontrada)
                {
                    Marca marca = this._marcas[cont];
                    if (marca.Id == ((Marca)this._ventana.MarcaSeleccionada).Id)
                    {
                        marcaParaNavegar = marca;
                        encontrada = true;
                    }
                    cont++;
                }


                this.Navegar(new ConsultarMarca(marcaParaNavegar, this._ventana));
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que ordena una columna
        /// </summary>
        public void OrdenarColumna(GridViewColumnHeader column)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            String field = column.Tag as String;

            if (this._ventana.CurSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(this._ventana.CurSortCol).Remove(this._ventana.CurAdorner);
                this._ventana.ListaResultados.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (this._ventana.CurSortCol == column && this._ventana.CurAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            this._ventana.CurSortCol = column;
            this._ventana.CurAdorner = new SortAdorner(this._ventana.CurSortCol, newDir);
            AdornerLayer.GetAdornerLayer(this._ventana.CurSortCol).Add(this._ventana.CurAdorner);
            this._ventana.ListaResultados.Items.SortDescriptions.Add(
                new SortDescription(field, newDir));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }

        #region Codigo Comentado NO BORRAR
        //public void BuscarAsociado()
        //{
        //    IEnumerable<Asociado> asociadosFiltrados = (IList<Asociado>)this._asociados;

        //    if (!string.IsNullOrEmpty(this._ventana.IdAsociadoFiltrar))
        //    {
        //        asociadosFiltrados = from p in asociadosFiltrados
        //                             where p.Id == int.Parse(this._ventana.IdAsociadoFiltrar)
        //                             select p;
        //    }

        //    if (!string.IsNullOrEmpty(this._ventana.NombreAsociadoFiltrar))
        //    {
        //        asociadosFiltrados = from p in asociadosFiltrados
        //                             where p.Nombre != null &&
        //                             p.Nombre.ToLower().Contains(this._ventana.NombreAsociadoFiltrar.ToLower())
        //                             select p;
        //    }

        //    if (asociadosFiltrados.ToList<Asociado>().Count != 0)
        //        this._ventana.Asociados = asociadosFiltrados.ToList<Asociado>();
        //    else
        //        this._ventana.Asociados = this._asociados;
        //}

        //public void BuscarInteresado()
        //{
        //    IEnumerable<Interesado> interesadosFiltrados = (IList<Interesado>)this._interesados;

        //    if (!string.IsNullOrEmpty(this._ventana.IdInteresadoFiltrar))
        //    {
        //        interesadosFiltrados = from p in interesadosFiltrados
        //                             where p.Id == int.Parse(this._ventana.IdInteresadoFiltrar)
        //                             select p;
        //    }

        //    if (!string.IsNullOrEmpty(this._ventana.NombreInteresadoFiltrar))
        //    {
        //        interesadosFiltrados = from p in interesadosFiltrados
        //                             where p.Nombre != null &&
        //                             p.Nombre.ToLower().Contains(this._ventana.NombreInteresadoFiltrar.ToLower())
        //                             select p;
        //    }

        //    if (interesadosFiltrados.ToList<Interesado>().Count != 0)
        //        this._ventana.Interesados = interesadosFiltrados.ToList<Interesado>();
        //    else
        //        this._ventana.Interesados = this._interesados;
        //}
        #endregion

        /// <summary>
        /// Metodo que se utiliza para validar si se hara la consulta SOLO por localidad 
        /// </summary>
        /// <param name="marcaAuxiliar">Marca a consultar</param>
        /// <returns>True si la consulta solo se hara por localidad. False si alguno de los campos de la marca a cosultar esta lleno</returns>
        public bool ConsultaSoloLocalidad(Marca marcaAuxiliar)
        {
            bool retorno = false;
            //marcaAuxiliar.LocalidadMarca = ((ListaDatosValores)this._ventana.TipoBusqueda).Valor;

            if (this._ventana.Id.Equals(""))
                if ((null == this._ventana.Asociado) || (((Asociado)this._ventana.Asociado).Id == int.MinValue))
                    if ((null == this._ventana.AsociadoInt) || (((Asociado)this._ventana.AsociadoInt).Id == int.MinValue))
                    if ((null == this._ventana.OrigenAsociado) || ((ListaDatosValores)this._ventana.OrigenAsociado).Id.Equals("NGN"))
                        if ((null == this._ventana.Interesado) || (((Interesado)this._ventana.Interesado).Id == int.MinValue))
                            if ((null == this._ventana.OrigenInteresado) || ((ListaDatosValores)this._ventana.OrigenInteresado).Id.Equals("NGN"))
                                if ((null == this._ventana.OrigenMarca) || ((ListaDatosValores)this._ventana.OrigenMarca).Id.Equals("NGN"))
                                    if ((null == this._ventana.Corresponsal) || (((Corresponsal)this._ventana.Corresponsal).Id == int.MinValue))
                                        if (this._ventana.DescripcionFiltrar.Equals(""))
                                            if (this._ventana.Otros.Equals(""))
                                                if (this._ventana.Fecha.Equals(""))
                                                    if (((TipoEstado)this._ventana.Detalle).Id.Equals("NGN"))
                                                        if (((Servicio)this._ventana.Servicio).Id.Equals("NGN"))
                                                            if (this._ventana.ClaseInternacional.Equals(""))
                                                                if (this._ventana.ClaseNacional.Equals(""))
                                                                    if (this._ventana.Distingue.Equals(""))
                                                                        if (this._ventana.DistingueEnIngles.Equals(""))
                                                                            if (this._ventana.Solicitud.Equals(""))
                                                                                if (null == marcaAuxiliar.PrimeraReferencia)
                                                                                    if (this._ventana.IdInternacional.Equals(string.Empty))
                                                                                        if (this._ventana.IdCorrelativoInternacional.Equals(string.Empty))
                                                                                            if (this._ventana.Pais == null)
                                                                                                if (this._ventana.ReferenciaAsociado.Equals(string.Empty))
                                                                                                    if (this._ventana.ReferenciaInteresado.Equals(string.Empty))
                                                                                                        if (!this._ventana.BoletinesEstaSeleccionado)
                                                                                                            if (!this._ventana.IndicadoresEstaSeleccionado)
                                                                                                                if (!this._ventana.PrioridadesEstaSeleccionado)
                                                                                                                    if (!this._ventana.TYREstaSeleccionado)
                                                                                                                        {
                                                                                                                           _soloLocalidad = true;
                                                                                                                           retorno = _soloLocalidad;
                                                                                                                        }

                if (marcaAuxiliar.LocalidadMarca.Equals("N"))
                    _localidadAConsultar = "Nacionales";
                else if (marcaAuxiliar.LocalidadMarca.Equals("I"))
                    _localidadAConsultar = "Internacionales";

     
            return retorno;
        }
    




        /// <summary>
        /// Método que limpia los campos de búsqueda
        /// </summary>
        public void LimpiarCampos()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this._ventana.TotalHits = "0";
            this._ventana.Resultados = null;

            this._ventana.GestionarVisibilidadFiltroInternacional(false);

            this._ventana.GestionarVisibilidadFiltroNacional(true);

            //this._ventana.GestionarVisibilidadLimpiarFiltros();

            this._ventana.MarcaParaFiltrar = new Marca();

            #region Internacional

            this._ventana.InternacionalEstaSeleccionado = false;
            this._ventana.IdInternacional = null;
            this._ventana.IdCorrelativoInternacional = null;

            #endregion

            #region Nacional

            this._ventana.NacionalEstaSeleccionado = true;
            this._ventana.Id = null;
            this._ventana.DescripcionFiltrar = null;
            this._ventana.Fecha = null;

            this._ventana.AsociadoFiltro = null;
            this._ventana.IdAsociadoFiltrar = null;
            this._ventana.NombreAsociadoFiltrar = null;
            IList<Asociado> asociadosAux = new List<Asociado>();
            Asociado primerAsociado = new Asociado(int.MinValue);
            asociadosAux.Add(primerAsociado);
            this._ventana.Asociados = asociadosAux;
            this._ventana.Asociado = this.BuscarAsociado(asociadosAux, primerAsociado);

            this._ventana.AsociadoIntFiltro = null;
            this._ventana.IdAsociadoIntFiltrar = null;
            this._ventana.NombreAsociadoIntFiltrar = null;
            IList<Asociado> asociadosintAux = new List<Asociado>();
            Asociado primerAsociadoInt = new Asociado(int.MinValue);
            asociadosintAux.Add(primerAsociadoInt);
            this._ventana.AsociadosInt = asociadosintAux;
            this._ventana.AsociadoInt = this.BuscarAsociado(asociadosintAux, primerAsociadoInt);

            this._ventana.IdInteresadoFiltrar = null;
            this._ventana.NombreInteresadoFiltrar = null;
            this._ventana.InteresadoFiltro = null;
            IList<Interesado> interesadosAux = new List<Interesado>();
            Interesado primerInteresado = new Interesado(int.MinValue);
            interesadosAux.Add(primerInteresado);
            this._ventana.Interesados = interesadosAux;
            this._ventana.Interesado = this.BuscarInteresado(interesadosAux, primerInteresado);

            this._ventana.IdCorresponsalFiltrar = null;
            this._ventana.NombreCorresponsalFiltrar = null;
            this._ventana.CorresponsalFiltro = null;
            this._ventana.Corresponsal = null;

            this._ventana.ClaseNacional = null;
            this._ventana.ClaseInternacional = null;
            this._ventana.Servicio = this.BuscarServicio((IList<Servicio>)this._ventana.Servicios, new Servicio("NGN"));
            this._ventana.OrigenAsociado = this.BuscarListaDeDatosValores((IList<ListaDatosValores>)this._ventana.OrigenesAsociados, new ListaDatosValores("NGN"));
            this._ventana.OrigenInteresado = this.BuscarListaDeDatosValores((IList<ListaDatosValores>)this._ventana.OrigenesInteresados, new ListaDatosValores("NGN"));
            this._ventana.OrigenMarca = this.BuscarListaDeDatosValores((IList<ListaDatosValores>)this._ventana.OrigenesMarca, new ListaDatosValores("NGN"));
            //this._ventana.Detalle = this.BuscarDetalle((IList<TipoEstado>)this._ventana.Detalles, new TipoEstado("NGN"));
            this._ventana.Detalle = ((IList<TipoEstado>)this._ventana.Detalles)[0];

            #endregion

            #region TYR

            this._ventana.TYREstaSeleccionado = false;
            this._ventana.CodigoRegistro = null;
            this._ventana.FechaRegistro = "";
            this._ventana.RenovadoPorOtroTramitante = false;
            this._ventana.Condicion = this.BuscarCondicion((IList<Condicion>)this._ventana.Condiciones, new Condicion(int.MinValue));
            this._ventana.ExpCambioPendiente = null;
            this._ventana.InstruccionesDeRenovacion = false;

            #endregion

            #region Boletines

            this._ventana.BoletinesEstaSeleccionado = false;
            this._ventana.BoletinOrdenPublicacion = this.BuscarBoletin((IList<Boletin>)this._ventana.BoletinesOrdenPublicacion, new Boletin(int.MinValue));
            this._ventana.BoletinPublicacion = this.BuscarBoletin((IList<Boletin>)this._ventana.BoletinesPublicacion, new Boletin(int.MinValue));
            this._ventana.BoletinConcesion = this.BuscarBoletin((IList<Boletin>)this._ventana.BoletinesConcesion, new Boletin(int.MinValue));

            #endregion

            #region Prioridades

            this._ventana.PrioridadesEstaSeleccionado = false;
            this._ventana.PrioridadCodigo = "";
            this._ventana.PrioridadFecha = "";
            this._ventana.PaisPrioridad = BuscarPais((IList<Pais>)this._ventana.Paises, new Pais(int.MinValue));

            #endregion

            #region Indicadores

            this._ventana.IndicadoresEstaSeleccionado = false;

            #endregion

            this._ventana.FocoPredeterminado();

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        #region Interesado


        /// <summary>
        /// Método que se encarga de buscar el interesado definido en el filtro
        /// </summary>
        public void BuscarInteresado()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
                      

            Interesado interesadoABuscar = new Interesado();

            //interesadoABuscar.Id = !this._ventana.IdInteresadoFiltrar.Equals("") ?
            //                       int.Parse(this._ventana.IdInteresadoFiltrar) : 0;

            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                interesadoABuscar.Id = !this._ventana.IdInteresadoFiltrar.Equals("") ?
                                           int.Parse(this._ventana.IdInteresadoFiltrar) : int.MinValue;

                interesadoABuscar.Nombre = !this._ventana.NombreInteresadoFiltrar.Equals("") ?
                                           this._ventana.NombreInteresadoFiltrar.ToUpper() : "";

                //if ((interesadoABuscar.Id != 0) || !(interesadoABuscar.Nombre.Equals("")))
                if ((interesadoABuscar.Id != int.MinValue) || !(interesadoABuscar.Nombre.Equals("")))
                {
                    IList<Interesado> interesados = this._interesadoServicios.ObtenerInteresadosFiltro(interesadoABuscar);

                    if (interesados.Count > 0)
                    {
                        interesados.Insert(0, new Interesado(int.MinValue));
                        this._ventana.Interesados = interesados;
                    }
                    else
                    {
                        interesados.Insert(0, new Interesado(int.MinValue));
                        this._ventana.Interesados = this._interesados;
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                    }
                }
                else
                    this._ventana.Mensaje("Ingrese criterios validos para la busqueda del Interesado", 1);

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

           

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

        }


        /// <summary>
        /// Metodo que cambia el texto del interesado en la interfaz
        /// </summary>
        /// <returns>true en caso de que el interesado haya sido valido, false en caso contrario</returns>
        public bool CambiarInteresado()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            bool retorno = false;

            if (this._ventana.Interesado != null)
            {
                this._ventana.InteresadoFiltro = ((Interesado)this._ventana.Interesado).Nombre;
                retorno = true;
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;

        }

        #endregion


        #region Asociado


        /// <summary>
        /// Método que se encarga de buscar el asociado definido en el filtro
        /// </summary>
        public void BuscarAsociado()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                Asociado asociadoABuscar = new Asociado();

                //asociadoABuscar.Id = !this._ventana.IdAsociadoFiltrar.Equals("") ?
                //                     int.Parse(this._ventana.IdAsociadoFiltrar) : 0;

                asociadoABuscar.Id = !this._ventana.IdAsociadoFiltrar.Equals("") ?
                                    int.Parse(this._ventana.IdAsociadoFiltrar) : int.MinValue;

                asociadoABuscar.Nombre = !this._ventana.NombreAsociadoFiltrar.Equals("") ?
                                         this._ventana.NombreAsociadoFiltrar.ToUpper() : "";

                //if ((asociadoABuscar.Id != 0) || !(asociadoABuscar.Nombre.Equals("")))
                if ((asociadoABuscar.Id != int.MinValue) || !(asociadoABuscar.Nombre.Equals("")))
                {
                    IList<Asociado> asociados = this._asociadoServicios.ObtenerAsociadosFiltro(asociadoABuscar);

                    if (asociados.Count > 0)
                    {
                        asociados.Insert(0, new Asociado(int.MinValue));
                        this._ventana.Asociados = asociados;
                    }
                    else
                    {
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                        this._ventana.Asociados = this._asociados;
                    }
                }

                else
                    this._ventana.Mensaje("Ingrese criterios validos para la busqueda del Asociado", 1);
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

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Metodo que cambia el texto del Asociado en la interfaz
        /// </summary>
        /// <returns>true en caso de que el Asociado haya sido valido, false en caso contrario</returns>
        public bool CambiarAsociado()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            bool retorno = false;

            if (this._ventana.Asociado != null)
            {
                this._ventana.AsociadoFiltro = ((Asociado)this._ventana.Asociado).Nombre;
                retorno = true;
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }

        #endregion

        #region AsociadoInt


        /// <summary>
        /// Método que se encarga de buscar el asociado definido en el filtro
        /// </summary>
        public void BuscarAsociadoInt()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                Asociado asociadoIntABuscar = new Asociado();

                //asociadoABuscar.Id = !this._ventana.IdAsociadoFiltrar.Equals("") ?
                //                     int.Parse(this._ventana.IdAsociadoFiltrar) : 0;

                asociadoIntABuscar.Id = !this._ventana.IdAsociadoIntFiltrar.Equals("") ?
                                    int.Parse(this._ventana.IdAsociadoIntFiltrar) : int.MinValue;

                asociadoIntABuscar.Nombre = !this._ventana.NombreAsociadoIntFiltrar.Equals("") ?
                                         this._ventana.NombreAsociadoIntFiltrar.ToUpper() : "";

                //if ((asociadoABuscar.Id != 0) || !(asociadoABuscar.Nombre.Equals("")))
                if ((asociadoIntABuscar.Id != int.MinValue) || !(asociadoIntABuscar.Nombre.Equals("")))
                {
                    IList<Asociado> asociados = this._asociadoServicios.ObtenerAsociadosFiltro(asociadoIntABuscar);

                    if (asociados.Count > 0)
                    {
                        asociados.Insert(0, new Asociado(int.MinValue));
                        this._ventana.AsociadosInt = asociados;
                    }
                    else
                    {
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                        this._ventana.AsociadosInt = this._asociados;
                    }
                }

                else
                    this._ventana.Mensaje("Ingrese criterios validos para la busqueda del Asociado Int", 1);
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

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Metodo que cambia el texto del Asociado en la interfaz
        /// </summary>
        /// <returns>true en caso de que el Asociado haya sido valido, false en caso contrario</returns>
        public bool CambiarAsociadoInt()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            bool retorno = false;

            if (this._ventana.AsociadoInt != null)
            {
                this._ventana.AsociadoIntFiltro = ((Asociado)this._ventana.AsociadoInt).Nombre;
                retorno = true;
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }

        #endregion


        #region Corresponsal


        /// <summary>
        /// Método que se encarga de buscar el asociado definido en el filtro
        /// </summary>
        public void BuscarCorresponsal()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            Mouse.OverrideCursor = Cursors.Wait;

            Corresponsal corresponsalABuscar = new Corresponsal();

            corresponsalABuscar.Id = !this._ventana.IdCorresponsalFiltrar.Equals("") ?
                                 int.Parse(this._ventana.IdCorresponsalFiltrar) : 0;

            corresponsalABuscar.Descripcion = !this._ventana.NombreCorresponsalFiltrar.Equals("") ?
                                     this._ventana.NombreCorresponsalFiltrar.ToUpper() : "";

            if ((corresponsalABuscar.Id != 0) || !(corresponsalABuscar.Descripcion.Equals("")))
            {
                IList<Corresponsal> corresponsales = this._corresponsalServicios.ConsultarTodos();
                corresponsales.Insert(0, new Corresponsal(int.MinValue));
                this._ventana.Corresponsales = corresponsales;

            }
            else
            {
                this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                this._ventana.Corresponsales = this._asociados;
            }

            Mouse.OverrideCursor = null;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Metodo que cambia el texto del Corresponsal en la interfaz
        /// </summary>
        /// <returns>true en caso de que el Corresponsal haya sido valido, false en caso contrario</returns>
        public bool CambiarCorresponsal()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            bool retorno = false;

            if (this._ventana.Corresponsal != null)
            {
                this._ventana.CorresponsalFiltro = ((Corresponsal)this._ventana.Corresponsal).Descripcion;
                retorno = true;
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }

        #endregion


        #region Marca


        public void BuscarMarca()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Mouse.OverrideCursor = Cursors.Wait;
                Marca marca = new Marca();
                IEnumerable<Marca> marcasFiltradas;
                marca.Descripcion = this._ventana.NombreMarcaFiltrar.ToUpper();
                marca.Id = this._ventana.IdMarcaFiltrar.Equals("") ? 0 : int.Parse(this._ventana.IdMarcaFiltrar);
                if ((!marca.Descripcion.Equals("")) || (marca.Id != 0))
                    marcasFiltradas = this._marcaServicios.ObtenerMarcasFiltro(marca);
                else
                    marcasFiltradas = new List<Marca>();

                if (marcasFiltradas.ToList<Marca>().Count != 0)
                    this._ventana.Marcas = marcasFiltradas.ToList<Marca>();
                else
                    this._ventana.Marcas = this._marcas;

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


        public bool ElegirMarca()
        {
            bool retorno = false;
            if (this._ventana.Marca != null)
            {
                retorno = true;
                this._ventana.NombreMarca = ((Marca)this._ventana.Marca).Descripcion;
            }

            return retorno;
        }


        #endregion

    }
}
