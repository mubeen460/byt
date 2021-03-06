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
using Trascend.Bolet.Cliente.Contratos.Renovaciones;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.Cliente.Ventanas.Renovaciones;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.Cliente.Ventanas.Auditorias;
using Trascend.Bolet.Cliente.Ventanas.Marcas;

namespace Trascend.Bolet.Cliente.Presentadores.Renovaciones
{
    class PresentadorGestionarRenovacion : PresentadorBase
    {
        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private bool _agregar = true;
        private IGestionarRenovacion _ventana;

        private IMarcaServicios _marcaServicios;
        private IAsociadoServicios _asociadoServicios;
        private IAgenteServicios _agenteServicios;
        private IPoderServicios _poderServicios;
        private IPaisServicios _paisServicios;
        private IInteresadoServicios _interesadoServicios;
        private IServicioServicios _servicioServicios;
        private IRenovacionServicios _renovacionServicios;
        private IListaDatosValoresServicios _listaDatosValoresServicios;
        private IListaDatosDominioServicios _listaDatosDominioServicios;

        private IList<Asociado> _asociados;
        private IList<Marca> _marcas;
        private IList<Interesado> _interesados;
        private IList<Agente> _agentes;
        private IList<Poder> _poderes;

        private IList<Poder> _poderesInterseccion;
        private IList<Poder> _poderesInteresado;
        private IList<Poder> _poderesAgente;

        private bool _esMarcaNacional = true;
        private bool _vieneDeConsultarMarca = false;
        private object _ventanaConsultarMarca;

        private bool _nueva = false;

        /// <summary>
        /// Constructor Predeterminado
        /// </summary>
        /// <param name="ventana">página que satisface el contrato</param>
        public PresentadorGestionarRenovacion(IGestionarRenovacion ventana, object renovacion, object ventanaPadre)
        {
            try
            {
                this._ventana = ventana;
                this._ventanaPadre = ventanaPadre;
                if (renovacion != null)
                {
                    this._ventana.HabilitarCampos = true;
                    this._ventana.Renovacion = renovacion;
                    _agregar = false;
                    _nueva = ((Renovacion)renovacion).Nueva;

                }
                else
                {
                    Renovacion renovacionAgregar = new Renovacion();
                    this._ventana.Renovacion = renovacionAgregar;

                    ((Renovacion)this._ventana.Renovacion).Fecha = DateTime.Now;

                    this._ventana.Marca = null;
                    this._ventana.Poder = null;
                    this._ventana.Interesado = null;
                    this._ventana.Agente = null;

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
                this._paisServicios = (IPaisServicios)Activator.GetObject(typeof(IPaisServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["PaisServicios"]);
                this._interesadoServicios = (IInteresadoServicios)Activator.GetObject(typeof(IInteresadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InteresadoServicios"]);
                this._servicioServicios = (IServicioServicios)Activator.GetObject(typeof(IServicioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ServicioServicios"]);
                this._renovacionServicios = (IRenovacionServicios)Activator.GetObject(typeof(IRenovacionServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["RenovacionServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);
                this._listaDatosDominioServicios = (IListaDatosDominioServicios)Activator.GetObject(typeof(IListaDatosDominioServicios),
                     ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosDominioServicios"]);


                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }




        /// <summary>
        /// Constructor predeterminado que recibe una marca adicionalmente a la renovacion y a la ventana padre
        /// </summary>
        /// <param name="ventana">Ventana actual</param>
        /// <param name="renovacion">Renovacion a gestionar</param>
        /// <param name="marca">Marca consultada</param>
        /// <param name="ventanaPadre">Ventana padre que precede a esta ventana</param>
        public PresentadorGestionarRenovacion(IGestionarRenovacion ventana, object renovacion, object marca, object ventanaPadre, object ventanaConsultarMarca)
        {
            try
            {
                this._ventana = ventana;
                this._ventanaPadre = ventanaPadre;
                if (renovacion != null)
                {
                    this._ventana.HabilitarCampos = true;
                    this._ventana.Renovacion = renovacion;
                    _agregar = false;
                }
                else
                {
                    Renovacion renovacionAgregar = new Renovacion();
                    this._ventana.Renovacion = renovacionAgregar;

                    ((Renovacion)this._ventana.Renovacion).Fecha = DateTime.Now;

                    if (marca != null)
                    {
                        this._vieneDeConsultarMarca = true;
                        this._ventanaConsultarMarca = ventanaConsultarMarca;
                        this._ventana.Marca = marca;
                        ((Renovacion)this._ventana.Renovacion).Marca = (Marca)marca;
                        ((Renovacion)this._ventana.Renovacion).Interesado = ((Marca)marca).Interesado;
                        this._ventana.Interesado = ((Marca)marca).Interesado;
                    }
                    else
                        this._ventana.Marca = null;


                    this._ventana.Poder = null;
                    //this._ventana.Interesado = null;
                    this._ventana.Agente = null;

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
                this._paisServicios = (IPaisServicios)Activator.GetObject(typeof(IPaisServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["PaisServicios"]);
                this._interesadoServicios = (IInteresadoServicios)Activator.GetObject(typeof(IInteresadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InteresadoServicios"]);
                this._servicioServicios = (IServicioServicios)Activator.GetObject(typeof(IServicioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ServicioServicios"]);
                this._renovacionServicios = (IRenovacionServicios)Activator.GetObject(typeof(IRenovacionServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["RenovacionServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);
                this._listaDatosDominioServicios = (IListaDatosDominioServicios)Activator.GetObject(typeof(IListaDatosDominioServicios),
                     ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosDominioServicios"]);


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
                this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleAgregarRenovacion,
                Recursos.Ids.GestionarRenovacion);
            else
                this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleGestionarRenovacion,
                Recursos.Ids.GestionarRenovacion);
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



                    Renovacion renovacion = (Renovacion)this._ventana.Renovacion;

                    if (((Renovacion)renovacion).Marca != null)
                        this._ventana.Marca = this._marcaServicios.ConsultarMarcaConTodo(((Renovacion)renovacion).Marca);

                    this._ventana.NombreMarca = ((Marca)this._ventana.Marca).Descripcion;
                    this._ventana.Agente = ((Renovacion)renovacion).Agente;
                    this._ventana.Poder = renovacion.Poder;

                    #region CODIGO COMENTADO NO BORRAR
                    //IList<ListaDatosValores> tiposRenovacion = this._listaDatosValoresServicios.
                    //    ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiTipoRenovacion));
                    //ListaDatosValores primerTipo = new ListaDatosValores();
                    //primerTipo.Descripcion = string.Empty;
                    //primerTipo.Valor = "NGN";
                    //tiposRenovacion.Insert(0, primerTipo);
                    //this._ventana.TiposRenovaciones = tiposRenovacion;
                    //this._ventana.TipoRenovacion = this.BuscarTipoRenovacion(tiposRenovacion,renovacion.TipoRenovacion); 
                    #endregion

                    CargarMarca();

                    if (this._ventana.Marca != null)
                    {
                        if (((Renovacion)renovacion).Marca != null)
                            this._ventana.Marca = this._marcaServicios.ConsultarMarcaConTodo(((Renovacion)renovacion).Marca);

                        this._ventana.NombreMarca = ((Marca)this._ventana.Marca).Descripcion;
                        this._ventana.IdMarca = ((Marca)this._ventana.Marca).Id.ToString();
                    }

                    CargarInteresado();

                    CargarAgente();

                    CargarPoder();

                    //CambiarInteresado();

                    CargarId();


                    ValidarSiEsMarcaNacional();

                    if (_nueva && !_vieneDeConsultarMarca)
                    {
                        this._ventana.MostrarBotonNuevaRenovacion();
                        this._ventana.HabilitarBotonNuevaRenovacion();
                    }

                    if (System.IO.File.Exists(ConfigurationManager.AppSettings["RutaPlanillaPDFRenovacionMarca"].ToString() + ((Renovacion)this._ventana.Renovacion).Id + ".pdf"))
                    {
                        this._ventana.PintarVerPlanilla();
                    }

                    #region CODIGO COMENTADO NO BORRAR
                    //if (((Marca)this._ventana.Marca).LocalidadMarca != null)
                    //{
                    //    this._ventana.EsMarcaNacional(!((Marca)this._ventana.Marca).LocalidadMarca.Equals("I"));

                    //    if (((Marca)this._ventana.Marca).LocalidadMarca.Equals("I"))
                    //    {
                    //        _esMarcaNacional = false;
                    //        this._ventana.EsMarcaNacional(_esMarcaNacional);
                    //        ListaDatosValores itemBuscado = new ListaDatosValores();
                    //        itemBuscado.Valor = ((Marca)this._ventana.Marca).ClasificacionInternacional;
                    //        IList<ListaDatosValores> items = this._listaDatosValoresServicios.
                    //            ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiLocalidadMarca));
                    //        this._ventana.TipoClase = this.BuscarListaDeDatosValores(items, itemBuscado).Descripcion;
                    //    }
                    //    else
                    //    {
                    //        _esMarcaNacional = true;
                    //        this._ventana.EsMarcaNacional(_esMarcaNacional);
                    //        this._ventana.BorrarCerosInternacional();
                    //    }
                    //}
                    //else
                    //{
                    //    _esMarcaNacional = true;
                    //    this._ventana.EsMarcaNacional(_esMarcaNacional);
                    //    this._ventana.BorrarCerosInternacional();
                    //} 
                    #endregion


                }
                else
                {
                    
                    CargarMarca();

                    if (this._ventana.Marca != null)
                    {
                        this._ventana.NombreMarca = ((Marca)this._ventana.Marca).Descripcion;
                        this._ventana.IdMarca = ((Marca)this._ventana.Marca).Id.ToString();
                        this._ventana.IdInteresado = ((Interesado)this._ventana.Interesado).Id.ToString(); 
                    }

                    CargarInteresado();

                    CargarAgente();

                    CargarPoder();

                    if(!_vieneDeConsultarMarca)
                        this._ventana.MostrarBotonNuevaRenovacion();

                    //ActualizarFechaProxima();

                    this._ventana.ConvertirEnteroMinimoABlanco();
                    this._ventana.BorrarCeros();


                }

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


        private void ValidarSiEsMarcaNacional()
        {

            if (((Marca)this._ventana.Marca).LocalidadMarca != null)
            {
                this._ventana.EsMarcaNacional(!((Marca)this._ventana.Marca).LocalidadMarca.Equals("I"));

                if (((Marca)this._ventana.Marca).LocalidadMarca.Equals("I"))
                {
                    _esMarcaNacional = false;
                    this._ventana.EsMarcaNacional(_esMarcaNacional);
                    ListaDatosValores itemBuscado = new ListaDatosValores();
                    itemBuscado.Valor = ((Marca)this._ventana.Marca).ClasificacionInternacional;
                    IList<ListaDatosValores> items = this._listaDatosValoresServicios.
                        ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiLocalidadMarca));
                    this._ventana.TipoClase = this.BuscarListaDeDatosValores(items, itemBuscado).Descripcion;
                }
                else
                {
                    _esMarcaNacional = true;
                    this._ventana.EsMarcaNacional(_esMarcaNacional);
                    this._ventana.BorrarCerosInternacional();
                }
            }
            else
            {
                _esMarcaNacional = true;
                this._ventana.EsMarcaNacional(_esMarcaNacional);
                this._ventana.BorrarCerosInternacional();
            }
        }


        /// <summary>
        /// Método que carga los datos iniciales de TipoRenovación a mostrar en la página
        /// </summary>
        private void CargarTipoRenovacion(ListaDatosValores TipoRenovacion)
        {


            ListaDatosValores filtro = new ListaDatosValores(Recursos.Etiquetas.cbiTipoRenovacion);
            IList<ListaDatosValores> listaTipoRenovacion =
                this._listaDatosValoresServicios.ConsultarListaDatosValoresPorParametro(filtro);
            //   filtro.Valor = ((Renovacion)this._ventana.Renovacion).TipoR.ToString();
            this._ventana.TiposRenovaciones = listaTipoRenovacion;

            //this._ventana.TipoRenovacion = this.BuscarTipoRenovacion(listaTipoRenovacion, TipoRenovacion);
            this._ventana.TipoRenovacion = this.BuscarTipoRenovacion(listaTipoRenovacion, listaTipoRenovacion[0]);

            //ListaDatosValores tipoRenovacion = new ListaDatosValores();
            //tipoRenovacion.Id = ((Renovacion)this._ventana.Renovacion).TipoRenovacion.Id;

            //this._ventana.TipoRenovacion = this.BuscarTipoRenovacion(listaTipoRenovacion, tipoRenovacion);
        }


        /// <summary>
        /// Método que carga la ventana de Consultar Renovaciones
        /// </summary>
        public void IrConsultarRenovaciones()
        {
            this.Navegar(new ConsultarRenovaciones());
        }


        /// <summary>
        /// Método que carga los datos de Renovación para agregar en la Base de Datos
        /// </summary>
        public Renovacion CargarRenovacionDeLaPantalla()
        {

            Renovacion renovacion = (Renovacion)this._ventana.Renovacion;

            if (null != this._ventana.TipoRenovacion)
                renovacion.TipoR = Char.Parse(((ListaDatosValores)this._ventana.TipoRenovacion).Valor);

            if (null != this._ventana.Marca)
            {
                renovacion.Marca = ((Marca)this._ventana.Marca).Id != int.MinValue ? (Marca)this._ventana.Marca : null;

                //Se realizó un cambio de la linea 1 a la linea 2, luego se volvió a cambiar de la 2 a la 3
                /*1*/
                //renovacion.FechaProxima = DateTime.Parse(this._ventana.ProximaRenovacion);

                /*2*/
                //renovacion.FechaProxima = ((Marca)this._ventana.Marca).FechaRenovacion;

                /*3*/
                if (null != (((Marca)this._ventana.Marca).FechaRenovacion))
                {
                    if (_esMarcaNacional)
                    {
                        if (_agregar)
                        {
                            renovacion.FechaProxima = ((Marca)this._ventana.Marca).FechaRenovacion.Value.AddYears(int.Parse(ConfigurationManager.AppSettings["PeriodoRenovacion"]));
                        }
                    }
                    else
                    {
                        if (!this._ventana.ProximaRenovacion.Equals(string.Empty))
                            renovacion.Fecha = Convert.ToDateTime(FormatearFecha(this._ventana.ProximaRenovacion)).Date;
                        else
                            renovacion.FechaProxima = null;
                    }
                }
                else
                    renovacion.FechaProxima = null;
            }

            if ((null != this._ventana.Marca) && (null != ((Marca)this._ventana.Marca).Asociado))
                renovacion.Asociado = ((Marca)this._ventana.Marca).Asociado;

            if (null != this._ventana.Interesado)
                renovacion.Interesado = ((Interesado)this._ventana.Interesado).Id != int.MinValue ? (Interesado)this._ventana.Interesado : null;

            if (null != this._ventana.Agente)
                renovacion.Agente = !((Agente)this._ventana.Agente).Id.Equals("") ? (Agente)this._ventana.Agente : null;

            if (null != this._ventana.Poder)
                renovacion.Poder = ((Poder)this._ventana.Poder).Id != int.MinValue ? (Poder)this._ventana.Poder : null;

            if (!this._ventana.Otros.Equals(""))
                renovacion.OtrosS1 = this._ventana.Otros;

            if (!this._ventana.PeriodoDeGracia.Equals(""))
                renovacion.Observacion = this._ventana.PeriodoDeGracia;

            return renovacion;
        }


        /// <summary>
        /// Método que agrega a la ventana el distingue de la Marca
        /// </summary>
        internal void CopiarDistingue()
        {
            this._ventana.Otros = "";

            if (this._ventana.Marca != null)
                this._ventana.Otros = ((Marca)this._ventana.Marca).Fichas;
        }


        /// <summary>
        /// Método que dependiendo del estado de la página, habilita los campos o 
        /// Agrega los datos de Renovación
        /// </summary>
        public void Agregar()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            String distingueMarca = String.Empty;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Renovacion renovacion = CargarRenovacionDeLaPantalla();
                bool marcaExitoso = false;
                bool exitoso = false;

                if (renovacion.FechaProxima != null)
                {
                    if (null != renovacion.Marca)
                    {
                        int? exitosoIdRenovacion = this._renovacionServicios.InsertarOModificarRenovacion(renovacion, UsuarioLogeado.Hash);


                        if (_agregar)
                        {
                            if (exitosoIdRenovacion != null)
                            {
                                renovacion.Id = exitosoIdRenovacion.Value;
                                Marca marcaAuxiliar = new Marca();
                                marcaAuxiliar = ((Renovacion)this._ventana.Renovacion).Marca;

                                int tiempoConfiguracion = int.Parse(ConfigurationManager.AppSettings["PeriodoRenovacion"].ToString());

                                distingueMarca = marcaAuxiliar.XDistingue;
                                marcaAuxiliar.Distingue = String.Empty;

                                marcaAuxiliar.FechaRenovacion = marcaAuxiliar.FechaRenovacion != null ? ((DateTime)marcaAuxiliar.FechaRenovacion).AddYears(tiempoConfiguracion) : System.DateTime.Now;
                                marcaAuxiliar.Operacion = "MODIFY";
                                marcaAuxiliar.Recordatorio = 0;
                                marcaAuxiliar.NumeroCondiciones = 0;

                                marcaExitoso = this._marcaServicios.InsertarOModificar(marcaAuxiliar, UsuarioLogeado.Hash);
                                if (marcaExitoso)
                                    this._marcaServicios.ActualizarDistingueDeMarca(marcaAuxiliar, distingueMarca);
                            }

                            if (marcaExitoso)
                            {
                                if (this._vieneDeConsultarMarca)
                                    this.Navegar(new ConsultarRenovaciones(this._ventana.Marca, this._ventanaConsultarMarca));
                                else
                                {
                                    renovacion.Nueva = true;
                                    this.Navegar(new GestionarRenovacion(renovacion, null));
                                }
                                //this._ventana.HabilitarCampos = false;
                            }
                            else
                                this.Navegar(Recursos.MensajesConElUsuario.RenovacionInsertada, true); 
                        }
                    }
                    else
                        this._ventana.Mensaje(Recursos.MensajesConElUsuario.AlertaRenovacionSinMarcas, 0);
                }
                else
                    this._ventana.Mensaje(String.Format(Recursos.MensajesConElUsuario.AlertaMarcaSinFechaRenovacion, renovacion.Marca.Id), 0);

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
        /// Metodo que se encarga de eliminar una Renovacion
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

                Renovacion renovacion = CargarRenovacionDeLaPantalla();
                int UltimaReno = this._renovacionServicios.ConsultarUltimaRenovacion(renovacion);
                bool marcaExitoso = false;
                if (renovacion.Id == UltimaReno)
                {
                    if (this._renovacionServicios.Eliminar((Renovacion)this._ventana.Renovacion, UsuarioLogeado.Hash))
                    {
                        Marca marcaAuxiliar = new Marca();
                        marcaAuxiliar = ((Renovacion)this._ventana.Renovacion).Marca;
                        int tiempoConfiguracion = int.Parse(ConfigurationManager.AppSettings["PeriodoRenovacion"].ToString());
                        marcaAuxiliar.FechaRenovacion = ((Renovacion)this._ventana.Renovacion).FechaProxima.Value.AddYears(-tiempoConfiguracion);
                        //marcaAuxiliar.FechaRenovacion = DateTime.Parse(this._ventana.ProximaRenovacion);
                        marcaAuxiliar.Operacion = "MODIFY";
                        marcaAuxiliar.Recordatorio = 0;

                        marcaExitoso = this._marcaServicios.InsertarOModificar(marcaAuxiliar, UsuarioLogeado.Hash);
                        _paginaPrincipal.MensajeUsuario = Recursos.MensajesConElUsuario.RenovacionEliminada;
                        this.Navegar(_paginaPrincipal);
                    }
                }
                else
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.UltimaRenovacion, 1);

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


        /// <summary>
        /// Método que Llena la lista de Agente y de Interesado asociados a un poder
        /// </summary>
        /// <param name="poder">Poder a filtrar</param>
        /// <param name="cargaInicial">True si es carga principal de la ventana o false si es llenado posterior</param>
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

                Agente agente = new Agente();

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
                    this._ventana.InteresadosFiltrados = interesadosFiltrados;

                    if (cargaInicial)
                        this._ventana.InteresadoFiltrado = this.BuscarInteresado(interesadosFiltrados, interesado);
                    else
                        this._ventana.InteresadoFiltrado = primerInteresado;
                }
                else
                {
                    interesadosFiltrados.Insert(0, primerInteresado);
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                    this._ventana.InteresadoFiltrado = primerInteresado;
                }

                if (agentesInteresadoFiltrados.Count != 0)
                {
                    agente = (Agente)this._ventana.AgenteFiltrado;
                    agentesInteresadoFiltrados.Insert(0, primerAgente);
                    this._ventana.AgentesFiltrados = agentesInteresadoFiltrados;
                    this._ventana.AgenteFiltrado = BuscarAgente(agentesInteresadoFiltrados, agente);
                }
                else
                {
                    agentesInteresadoFiltrados.Insert(0, primerAgente);
                    this._ventana.AgentesFiltrados = this._agentes;
                    this._ventana.Mensaje(Recursos.MensajesConElUsuario.NoHayResultados, 1);
                    this._ventana.AgenteFiltrado = primerAgente;
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


        /// <summary>
        /// Metodo que llena la lista de Agentes filtrados por un Poder
        /// </summary>
        /// <param name="poder">Poder a filtrar</param>
        private void LlenarListaAgente(Poder poder)
        {
            Agente primerAgente = new Agente("");

            this._agentes = this._agenteServicios.ObtenerAgentesDeUnPoder(poder);
            this._agentes.Insert(0, primerAgente);
            this._ventana.AgentesFiltrados = this._agentes;
            this._ventana.AgenteFiltrado = primerAgente;
        }


        /// <summary>
        /// Método que carga los Id de la cesion
        /// </summary>
        private void CargarId()
        {

            if ((Marca)this._ventana.Marca != null)
            {
                this._ventana.IdMarca = (((Marca)this._ventana.MarcaFiltrada).Id).ToString();
            }

            if (null != ((Renovacion)this._ventana.Renovacion).Interesado)
            {
                this._ventana.Interesado = ((Renovacion)this._ventana.Renovacion).Interesado;
                this._ventana.IdInteresado = ((Renovacion)this._ventana.Renovacion).Interesado.Id.ToString();
            }

            if (null != ((Renovacion)this._ventana.Renovacion).Agente)
            {
                this._ventana.Agente = ((Renovacion)this._ventana.Renovacion).Agente;
                this._ventana.IdAgente = ((Renovacion)this._ventana.Renovacion).Agente.Id;
            }
            this._ventana.ConvertirEnteroMinimoABlanco();

        }


        #region Marcas

        /// <summary>
        /// Método que carga los datos iniciales de Marca a mostrar en la página
        /// </summary>
        private void CargarMarca()
        {
            this._marcas = new List<Marca>();
            Marca primeraMarca = new Marca(int.MinValue);
            this._marcas.Add(primeraMarca);

            if ((Marca)this._ventana.Marca != null)
            {
                this._marcas.Add((Marca)this._ventana.Marca);
                this._ventana.MarcasFiltradas = this._marcas;
                this._ventana.MarcaFiltrada = (Marca)this._ventana.Marca;

                IList<ListaDatosDominio> tiposMarcas = this._listaDatosDominioServicios.
                ConsultarListaDatosDominioPorParametro(new ListaDatosDominio(Recursos.Etiquetas.cbiCategoriaMarca));
                ListaDatosDominio DatoDominio = new ListaDatosDominio();
                DatoDominio.Id = ((Marca)this._ventana.Marca).Tipo;
                DatoDominio = BuscarListaDeDominio(tiposMarcas, DatoDominio);
                if (null != DatoDominio)
                    this._ventana.Tipo = DatoDominio.Descripcion;
                else
                    this._ventana.Tipo = "";


                ListaDatosValores TipoDeRenovacion = new ListaDatosValores();
                if (((Marca)this._ventana.MarcaFiltrada).Ter == 'T')
                {
                    TipoDeRenovacion.Valor = "T";
                    ((Marca)this._ventana.MarcaFiltrada).Ter = 'T';
                }
                else
                {
                    TipoDeRenovacion.Valor = "I";
                    ((Marca)this._ventana.MarcaFiltrada).Ter = 'I';
                }


                CargarTipoRenovacion(TipoDeRenovacion);

                if (null != ((Marca)this._ventana.Marca).Asociado)
                    if(((Marca)this._ventana.Marca).Asociado.TipoCliente != null)
                        this._ventana.PintarAsociado(((Marca)this._ventana.Marca).Asociado.TipoCliente.Id);

                //    IList<ListaDatosValores> tiposRenovacion = this._listaDatosValoresServicios.
                //    ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiTipoRenovacion));

                //    this._ventana.TiposRenovaciones = tiposRenovacion;

                //    ListaDatosValores tipoRevAux= new ListaDatosValores();
                //    tipoRevAux.Valor = ((Marca)this._ventana.Marca).Ter.ToString();

                //    this._ventana.TipoRenovacion = this.BuscarTipoRenovacion((IList<ListaDatosValores>)this._ventana.TiposRenovaciones,tipoRevAux);
            }
            else
            {
                ListaDatosValores TipoDeRenovacion = new ListaDatosValores();
                CargarTipoRenovacion(TipoDeRenovacion);
                this._ventana.MarcasFiltradas = this._marcas;
                this._ventana.MarcaFiltrada = primeraMarca;
            }

        }

        /// <summary>
        /// Método que se encarga de consultar una Marca en Base de datos
        /// </summary>
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

        /// <summary>
        /// Método que se encarga en cambiar la Marca seleccionada en la lista filtrada
        /// </summary>
        /// <returns>True si se cambió correctamente, false en caso contrario</returns>
        public bool CambiarMarca()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            bool retorno = false;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion


                if (this._ventana.MarcaFiltrada != null)
                {
                    this._ventana.Marca = this._ventana.MarcaFiltrada;
                    this._ventana.IdMarca = ((Marca)this._ventana.MarcaFiltrada).Id.ToString();
                    this._ventana.NombreMarca = ((Marca)this._ventana.MarcaFiltrada).Descripcion;
                    this._marcas.RemoveAt(0);
                    this._marcas.Add((Marca)this._ventana.MarcaFiltrada);

                    //int tiempoConfiguracion = int.Parse(ConfigurationManager.AppSettings["PeriodoRenovacion"].ToString());
                    //this._ventana.ProximaRenovacion = ((Marca)this._ventana.Marca).FechaRenovacion.Value.AddYears(tiempoConfiguracion).ToString();
                    retorno = true;

                    ListaDatosValores TipoDeRenovacion = new ListaDatosValores();
                    #region CODIGO COMENTADO
                    //if ((((Marca)this._ventana.MarcaFiltrada).Ter == 'T')&& (((ListaDatosValores)this._ventana.TipoRenovacion).Valor.Equals('T')))

                    //{
                    //    TipoDeRenovacion.Valor = "T";
                    //    ((Marca)this._ventana.MarcaFiltrada).Ter = 'T';
                    //}
                    //else if ((((Marca)this._ventana.MarcaFiltrada).Ter == 'T'))
                    //{
                       
                    //    this._ventana.Mensaje("Esta marca es renovada por un Tercero", 1);
                    //    TipoDeRenovacion.Valor = "T";
                    //    ((Marca)this._ventana.MarcaFiltrada).Ter = 'T';
                    //}
                    //else
                    //{
                    //    this._ventana.Mensaje("Esta marca no es renovada por un Tercero", 1);
                    //    TipoDeRenovacion.Valor = "I";
                    //    ((Marca)this._ventana.MarcaFiltrada).Ter = 'I';
                    //}
                    #endregion
                    if (((Marca)this._ventana.MarcaFiltrada).Ter.Equals('T'))
                    {
                        if (((ListaDatosValores)this._ventana.TipoRenovacion).Valor.Equals("T"))
                        {
                            TipoDeRenovacion.Valor = "T";
                            ((Marca)this._ventana.MarcaFiltrada).Ter = 'T';
                        }
                        else if (((ListaDatosValores)this._ventana.TipoRenovacion).Valor.Equals("I"))
                        {
                            this._ventana.Mensaje("Esta marca es renovada por un Tercero", 1);
                            TipoDeRenovacion.Valor = "T";
                            ((Marca)this._ventana.MarcaFiltrada).Ter = 'T';
                        }

                    }
                    else if (((Marca)this._ventana.MarcaFiltrada).Ter.Equals('I'))
                    {
                        if (((ListaDatosValores)this._ventana.TipoRenovacion).Valor.Equals("T"))
                        {
                            this._ventana.Mensaje("Esta marca no es renovada por un Tercero", 1);
                            TipoDeRenovacion.Valor = "I";
                            ((Marca)this._ventana.MarcaFiltrada).Ter = 'I';
                        }
                        else
                        {
                            TipoDeRenovacion.Valor = "I";
                            ((Marca)this._ventana.MarcaFiltrada).Ter = 'I';
                        }
                    }



                    //CargarTipoRenovacion(TipoDeRenovacion);

                    this._ventana.TipoRenovacion = this.BuscarTipoRenovacion((IList<ListaDatosValores>)this._ventana.TiposRenovaciones, TipoDeRenovacion);


                    //if (((Marca)this._ventana.Marca).Ter.ToString().Equals("T"))
                    //{
                    //    this._ventana.Mensaje(Recursos.MensajesConElUsuario.RenovacionTercero, 1);
                    //}

                    if (((Marca)this._ventana.MarcaFiltrada).Id != int.MinValue)
                    {
                        this._ventana.Agente = null;
                        //this._ventana.AgenteFiltrado = null;
                        this._ventana.Interesado = null;
                        //this._ventana.InteresadoFiltrado = null;
                        this._ventana.Poder = null;
                        //this._ventana.PoderFiltrado = null;

                        if (((Marca)this._ventana.MarcaFiltrada).Interesado != null)
                        {
                            this._ventana.Interesado = ((Marca)this._ventana.MarcaFiltrada).Interesado;
                            this._ventana.IdInteresado = ((Marca)this._ventana.MarcaFiltrada).Interesado.Id.ToString();

                            IList<Interesado> listaAux = new List<Interesado>();
                            listaAux.Add(new Interesado(int.MinValue));
                            listaAux.Add(((Marca)this._ventana.MarcaFiltrada).Interesado);

                            this._ventana.InteresadosFiltrados = listaAux;

                            this._ventana.InteresadoFiltrado = this.BuscarInteresado((IList<Interesado>)this._ventana.InteresadosFiltrados,
                               ((Marca)this._ventana.MarcaFiltrada).Interesado);
                                                      

                            this.CambiarInteresado();
                        }

                        //if (((Marca)this._ventana.MarcaFiltrada).Poder != null)
                        //{
                        //    this._ventana.Poder = ((Marca)this._ventana.MarcaFiltrada).Poder;
                        //    //this._ventana.IdPoder = ((Poder) this._ventana.Poder).Id.ToString();

                        //    IList<Poder> listaAux = new List<Poder>();
                        //    listaAux.Add(new Poder(int.MinValue));
                        //    listaAux.Add((Poder)this._ventana.Poder);

                        //    this._ventana.PoderesFiltrados = listaAux;

                        //    this._ventana.PoderFiltrado = this.BuscarPoder((IList<Poder>)this._ventana.PoderesFiltrados,
                        //        ((Marca)this._ventana.MarcaFiltrada).Poder);

                        //    this.CambiarPoder();
                        //}

                        //if (((Marca)this._ventana.MarcaFiltrada).Agente != null)
                        //{
                        //    this._ventana.Agente = ((Marca)this._ventana.MarcaFiltrada).Agente;
                        //    this._ventana.IdAgente = ((Marca)this._ventana.MarcaFiltrada).Agente.Id;

                        //    IList<Agente> listaAux = new List<Agente>();
                        //    listaAux.Add(new Agente(""));
                        //    listaAux.Add((Agente)this._ventana.Agente);

                        //    this._ventana.AgentesFiltrados = listaAux;

                        //    this._ventana.AgenteFiltrado = this.BuscarAgente((IList<Agente>)this._ventana.AgentesFiltrados,
                        //        ((Marca)this._ventana.MarcaFiltrada).Agente);

                        //    this.CambiarAgente();
                        //}

                        //CambiarInteresado();
                        IList<ListaDatosDominio> tiposMarcas = this._listaDatosDominioServicios.
                        ConsultarListaDatosDominioPorParametro(new ListaDatosDominio(Recursos.Etiquetas.cbiCategoriaMarca));
                        ListaDatosDominio DatoDominio = new ListaDatosDominio();
                        DatoDominio.Id = ((Marca)this._ventana.Marca).Tipo;
                        DatoDominio = BuscarListaDeDominio(tiposMarcas, DatoDominio);
                        if (null != DatoDominio)
                            this._ventana.Tipo = DatoDominio.Descripcion;
                        else
                            this._ventana.Tipo = "";
                    }
                    else
                    {
                        Interesado primerInteresado = new Interesado(int.MinValue);
                        IList<Interesado> listaInteresadoAux = new List<Interesado>();
                        listaInteresadoAux.Insert(0, primerInteresado);
                        this._ventana.InteresadosFiltrados = listaInteresadoAux;
                        this._ventana.InteresadoFiltrado = primerInteresado;
                        this._ventana.Interesado = null;

                        Agente primerAgente = new Agente("");
                        IList<Agente> listaAgenteAux = new List<Agente>();
                        listaAgenteAux.Insert(0, primerAgente);
                        this._ventana.AgentesFiltrados = listaAgenteAux;
                        this._ventana.AgenteFiltrado = primerAgente;
                        this._ventana.Agente = null;

                        Poder primerPoder = new Poder(int.MinValue);
                        IList<Poder> listaPoderAux = new List<Poder>();
                        listaPoderAux.Insert(0, primerPoder);
                        this._ventana.PoderesFiltrados = listaPoderAux;
                        this._ventana.PoderFiltrado = primerPoder;
                        this._ventana.Poder = null;

                    }

                    ValidarSiEsMarcaNacional();

                }

                this._ventana.ConvertirEnteroMinimoABlanco();
                this._ventana.BorrarCerosInternacional();

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




        public void CambiarMarcaAlIniciar()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion


                if (this._ventana.MarcaFiltrada != null)
                {
                    this._ventana.IdMarca = ((Marca)this._ventana.MarcaFiltrada).Id.ToString();
                    this._ventana.NombreMarca = ((Marca)this._ventana.MarcaFiltrada).Descripcion;

                    ListaDatosValores TipoDeRenovacion = new ListaDatosValores();
                    if (((Marca)this._ventana.MarcaFiltrada).Ter.Equals('T'))
                    {
                        if (((ListaDatosValores)this._ventana.TipoRenovacion).Valor.Equals("T"))
                        {
                            TipoDeRenovacion.Valor = "T";
                            ((Marca)this._ventana.MarcaFiltrada).Ter = 'T';
                        }
                        else if (((ListaDatosValores)this._ventana.TipoRenovacion).Valor.Equals("I"))
                        {
                            this._ventana.Mensaje("Esta marca es renovada por un Tercero", 1);
                            TipoDeRenovacion.Valor = "T";
                            ((Marca)this._ventana.MarcaFiltrada).Ter = 'T';
                        }

                    }
                    else if (((Marca)this._ventana.MarcaFiltrada).Ter.Equals('I'))
                    {
                        if (((ListaDatosValores)this._ventana.TipoRenovacion).Valor.Equals("T"))
                        {
                            this._ventana.Mensaje("Esta marca no es renovada por un Tercero", 1);
                            TipoDeRenovacion.Valor = "I";
                            ((Marca)this._ventana.MarcaFiltrada).Ter = 'I';
                        }
                        else
                        {
                            TipoDeRenovacion.Valor = "I";
                            ((Marca)this._ventana.MarcaFiltrada).Ter = 'I';
                        }
                    }



                    this._ventana.TipoRenovacion = this.BuscarTipoRenovacion((IList<ListaDatosValores>)this._ventana.TiposRenovaciones, TipoDeRenovacion);


                    if (((Marca)this._ventana.MarcaFiltrada).Id != int.MinValue)
                    {
                        this._ventana.Agente = null;
                        //this._ventana.AgenteFiltrado = null;
                        this._ventana.Interesado = null;
                        //this._ventana.InteresadoFiltrado = null;
                        this._ventana.Poder = null;
                        //this._ventana.PoderFiltrado = null;

                        if (((Marca)this._ventana.MarcaFiltrada).Interesado != null)
                        {
                            this._ventana.Interesado = ((Marca)this._ventana.MarcaFiltrada).Interesado;
                            this._ventana.IdInteresado = ((Marca)this._ventana.MarcaFiltrada).Interesado.Id.ToString();

                            IList<Interesado> listaAux = new List<Interesado>();
                            listaAux.Add(new Interesado(int.MinValue));
                            listaAux.Add(((Marca)this._ventana.MarcaFiltrada).Interesado);

                            this._ventana.InteresadosFiltrados = listaAux;
                            this._ventana.Interesado = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.Interesado);

                            this._ventana.InteresadoFiltrado = this.BuscarInteresado((IList<Interesado>)this._ventana.InteresadosFiltrados,
                               (Interesado)this._ventana.Interesado);
                            this._ventana.InteresadoFiltrado = this._ventana.Interesado;

                            this.CambiarInteresado();
                        }


                        IList<ListaDatosDominio> tiposMarcas = this._listaDatosDominioServicios.
                        ConsultarListaDatosDominioPorParametro(new ListaDatosDominio(Recursos.Etiquetas.cbiCategoriaMarca));
                        ListaDatosDominio DatoDominio = new ListaDatosDominio();
                        DatoDominio.Id = ((Marca)this._ventana.Marca).Tipo;
                        DatoDominio = BuscarListaDeDominio(tiposMarcas, DatoDominio);
                        if (null != DatoDominio)
                            this._ventana.Tipo = DatoDominio.Descripcion;
                        else
                            this._ventana.Tipo = "";
                    }
                    else
                    {
                        Interesado primerInteresado = new Interesado(int.MinValue);
                        IList<Interesado> listaInteresadoAux = new List<Interesado>();
                        listaInteresadoAux.Insert(0, primerInteresado);
                        this._ventana.InteresadosFiltrados = listaInteresadoAux;
                        this._ventana.InteresadoFiltrado = primerInteresado;
                        this._ventana.Interesado = null;

                        Agente primerAgente = new Agente("");
                        IList<Agente> listaAgenteAux = new List<Agente>();
                        listaAgenteAux.Insert(0, primerAgente);
                        this._ventana.AgentesFiltrados = listaAgenteAux;
                        this._ventana.AgenteFiltrado = primerAgente;
                        this._ventana.Agente = null;

                        Poder primerPoder = new Poder(int.MinValue);
                        IList<Poder> listaPoderAux = new List<Poder>();
                        listaPoderAux.Insert(0, primerPoder);
                        this._ventana.PoderesFiltrados = listaPoderAux;
                        this._ventana.PoderFiltrado = primerPoder;
                        this._ventana.Poder = null;

                    }

                    ValidarSiEsMarcaNacional();

                }

                this._ventana.ConvertirEnteroMinimoABlanco();
                this._ventana.BorrarCerosInternacional();

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


        #endregion


        #region Interesado

        /// <summary>
        /// Método que carga los datos iniciales de Interesado a mostrar en la página
        /// </summary>
        private void CargarInteresado()
        {
            Interesado primerInteresado = new Interesado(int.MinValue);

            this._interesados = new List<Interesado>();

            this._interesados.Add(primerInteresado);

            if (((Renovacion)this._ventana.Renovacion).Interesado != null)
            {
                this._ventana.Interesado = this._interesadoServicios.ConsultarInteresadoConTodo(((Renovacion)this._ventana.Renovacion).Interesado);
                this._ventana.NombreInteresado = ((Interesado)this._ventana.Interesado).Nombre;

                if ((Interesado)this._ventana.Interesado != null)
                {
                    this._interesados.Add((Interesado)this._ventana.Interesado);
                    this._ventana.InteresadosFiltrados = this._interesados;
                    this._ventana.InteresadoFiltrado = this.BuscarInteresado((IList<Interesado>)this._ventana.InteresadosFiltrados, (Interesado)this._ventana.Interesado);
                }
            }
            else
            {
                this._ventana.Interesado = primerInteresado;
                this._ventana.InteresadosFiltrados = this._interesados;
                this._ventana.InteresadoFiltrado = primerInteresado;

            }
        }

        /// <summary>
        /// Método que hace las validaciones en carga inicial del interesado en la ventana
        /// </summary>
        private void ValidarInteresado()
        {
            if (((Interesado)this._ventana.InteresadoFiltrado).Id == int.MinValue)
            {
                if (((Agente)this._ventana.AgenteFiltrado).Id.Equals(""))
                {
                    if (((Poder)this._ventana.PoderFiltrado).Id != int.MinValue)
                    {
                        LlenarListaAgenteEInteresado((Poder)this._ventana.Poder, true);

                        this._ventana.GestionarBotonConsultarInteresado(false);
                        this._ventana.GestionarBotonConsultarAgente(false);
                    }
                }
                else
                {
                    if (((Cesion)this._ventana.PoderFiltrado).Id == int.MinValue)
                        this._ventana.GestionarBotonConsultarInteresado(false);

                    else
                    {
                        LlenarListaAgenteEInteresado((Poder)this._ventana.Poder, true);

                        this._ventana.GestionarBotonConsultarInteresado(false);
                        this._ventana.GestionarBotonConsultarAgente(false);
                        this._ventana.GestionarBotonConsultarPoder(false);
                    }

                }
            }
            else
            {
                if (((Agente)this._ventana.AgenteFiltrado).Id.Equals(""))
                {
                    if (((Poder)this._ventana.PoderFiltrado).Id == int.MinValue)
                        this._ventana.GestionarBotonConsultarPoder(false);

                    else
                    {
                        LlenarListaAgenteEInteresado((Poder)this._ventana.Poder, true);

                        this._ventana.GestionarBotonConsultarInteresado(false);
                        this._ventana.GestionarBotonConsultarAgente(false);
                        this._ventana.GestionarBotonConsultarPoder(false);

                    }
                }
                else
                {
                    if (((Poder)this._ventana.PoderFiltrado).Id == int.MinValue)
                    {
                        ValidarListaDePoderes(this._poderesInteresado, this._poderes);

                        this._ventana.GestionarBotonConsultarPoder(false);
                    }
                    else
                    {
                        LlenarListaAgenteEInteresado((Poder)this._ventana.Poder, true);
                        ValidarListaDePoderes(this._poderesInteresado, this._poderes);

                        this._ventana.GestionarBotonConsultarInteresado(false);
                        this._ventana.GestionarBotonConsultarAgente(false);
                        this._ventana.GestionarBotonConsultarPoder(false);
                    }
                }
            }
        }

        /// <summary>
        /// Método que se encarga de consultar un Intesesado en Base de datos
        /// </summary>
        public void ConsultarInteresados()
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
                interesado.Nombre = this._ventana.NombreInteresadoFiltrar.ToUpper();
                interesado.Id = this._ventana.IdInteresadoFiltrar.Equals("") ? 0 : int.Parse(this._ventana.IdInteresadoFiltrar);

                if ((!interesado.Nombre.Equals("")) || (interesado.Id != 0))
                    interesadosFiltrados = this._interesadoServicios.ObtenerInteresadosFiltro(interesado);
                else
                    interesadosFiltrados = new List<Interesado>();

                if (interesadosFiltrados.Count != 0)
                {
                    interesadosFiltrados.Insert(0, primerInteresado);
                    this._ventana.InteresadosFiltrados = interesadosFiltrados;
                    this._ventana.InteresadoFiltrado = primerInteresado;
                }
                else
                {
                    interesadosFiltrados.Insert(0, primerInteresado);
                    this._ventana.InteresadosFiltrados = this._interesados;
                    this._ventana.InteresadoFiltrado = primerInteresado;
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

        /// <summary>
        /// Método que se encarga en cambiar el Interesado seleccionada en la lista filtrada
        /// </summary>
        /// <returns>True si se cambió correctamente, false en caso contrario</returns>
        public bool CambiarInteresado()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            bool retorno = false;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if ((null != this._ventana.InteresadoFiltrado) && (((Interesado)this._ventana.InteresadoFiltrado).Id != int.MinValue))
                {
                    if ((null != this._ventana.AgenteFiltrado) && (!((Agente)this._ventana.AgenteFiltrado).Id.Equals("")))
                    {
                        if ((null != this._ventana.PoderFiltrado) && (((Poder)this._ventana.PoderFiltrado).Id != int.MinValue))
                        {
                            this._ventana.Interesado = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.InteresadoFiltrado);
                            this._ventana.NombreInteresado = ((Interesado)this._ventana.Interesado).Nombre;
                            this._ventana.IdInteresado = ((Interesado)this._ventana.Interesado).Id.ToString();

                            this._ventana.NombreAgente = null;
                            this._ventana.IdAgente = null;
                            this._ventana.IdPoder = null;
                            this._ventana.NumPoder = null;

                            Agente primerAgente = new Agente("");
                            this._ventana.Agente = primerAgente;

                            IList<Agente> listaAux = new List<Agente>();
                            listaAux.Add(new Agente(""));

                            this._ventana.AgentesFiltrados = listaAux;
                            this._ventana.AgenteFiltrado = this.BuscarAgente((IList<Agente>)this._ventana.AgentesFiltrados, primerAgente);

                            Poder primerPoder = new Poder(int.MinValue);

                            this._poderesInteresado = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.InteresadoFiltrado));
                            this._poderesInteresado.Insert(0, primerPoder);
                            IList<Poder> poderes = this._poderesInteresado;
                            foreach (Poder poder in poderes)
                            {
                                if (poder.Id != int.MinValue)
                                {
                                    IList<Agente> ListaAgentes = this._agenteServicios.ObtenerAgentesDeUnPoder(poder);
                                    foreach (Agente agente in ListaAgentes)
                                    {
                                        poder.MostrarAgentes += agente.Id + "-" + agente.Nombre + "\n";
                                    }
                                }
                            }
                            this._ventana.PoderesFiltrados = poderes;
                            this._ventana.PoderFiltrado = primerPoder;

                            retorno = true;
                        }
                        else
                        {
                            this._poderesInteresado = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.InteresadoFiltrado));

                            LimpiarListaPoder();

                            if ((this.ValidarListaDePoderes(this._poderesInteresado, this._poderes)))
                            {
                                this._ventana.Interesado = this._ventana.InteresadoFiltrado;
                                this._ventana.NombreInteresado = ((Interesado)this._ventana.InteresadoFiltrado).Nombre;
                                this._ventana.IdInteresado = ((Interesado)this._ventana.Interesado).Id.ToString();
                                retorno = true;
                            }
                            else if (!this.ValidarListaDePoderes(this._poderesInteresado, this._poderes))
                            {
                                this._ventana.ConvertirEnteroMinimoABlanco();
                                this._ventana.Mensaje(string.Format(Recursos.MensajesConElUsuario.ErrorInteresadoNoPoseePoderConAgente, ""), 0);
                            }
                        }
                    }
                    else
                    {
                        if (((Poder)this._ventana.PoderFiltrado).Id == int.MinValue)
                        {
                            Poder primerPoder = new Poder(int.MinValue);

                            this._poderesInteresado = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.InteresadoFiltrado));
                            this._poderesInteresado.Insert(0, primerPoder);
                            IList<Poder> poderes = this._poderesInteresado;
                            foreach (Poder poder in poderes)
                            {
                                if (poder.Id != int.MinValue)
                                {
                                    IList<Agente> ListaAgentes = this._agenteServicios.ObtenerAgentesDeUnPoder(poder);
                                    foreach (Agente agente in ListaAgentes)
                                    {
                                        poder.MostrarAgentes += agente.Id + "-" + agente.Nombre + "\n";
                                    }
                                }
                            }
                            this._ventana.PoderesFiltrados = poderes;
                            this._ventana.PoderFiltrado = primerPoder;

                            this._poderesInteresado = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.InteresadoFiltrado));
                            this._ventana.Interesado = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.InteresadoFiltrado);
                            this._ventana.NombreInteresado = ((Interesado)this._ventana.Interesado).Nombre;
                            this._ventana.IdInteresado = ((Interesado)this._ventana.Interesado).Id.ToString();
                            retorno = true;
                        }
                        else
                        {

                            this._poderesInteresado = this._poderServicios.ConsultarPoderesPorInteresado(((Interesado)_ventana.InteresadoFiltrado));
                            this._ventana.Interesado = this._interesadoServicios.ConsultarInteresadoConTodo((Interesado)this._ventana.InteresadoFiltrado);
                            this._ventana.NombreInteresado = ((Interesado)this._ventana.Interesado).Nombre;
                            this._ventana.IdInteresado = ((Interesado)this._ventana.Interesado).Id.ToString();
                            retorno = true;
                        }
                    }

                }
                else
                {
                    this._ventana.Interesado = this._ventana.InteresadoFiltrado;
                    this._ventana.NombreInteresado = ((Interesado)this._ventana.Interesado).Nombre;
                    this._ventana.IdInteresado = ((Interesado)this._ventana.Interesado).Id.ToString();
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

        /// <summary>
        /// Metodo que se encarga de verificar si el Interesado seleccionado es válido
        /// </summary>
        /// <returns>True si es válido, False en caso contrario</returns>
        public bool VerificarCambioInteresado()
        {
            bool retorno = false;

            if ((((Interesado)this._ventana.InteresadoFiltrado).Id != int.MinValue) || !(((Agente)this._ventana.AgenteFiltrado).Id.Equals("")))
                retorno = true;

            return retorno;
        }

        /// <summary>
        /// Método que se encarga de limpiar las lista de Interesados
        /// </summary>
        public void LimpiarListaInteresado()
        {
            Interesado primerInteresado = new Interesado(int.MinValue);
            IList<Interesado> listaInteresados = new List<Interesado>();
            listaInteresados.Add(primerInteresado);

            this._ventana.InteresadosFiltrados = listaInteresados;
            this._ventana.InteresadoFiltrado = BuscarInteresado(listaInteresados, primerInteresado);
            this._ventana.Interesado = this._ventana.InteresadoFiltrado;
        }

        #endregion


        #region Agente

        /// <summary>
        /// Método que carga los datos iniciales de Agente a mostrar en la página
        /// </summary>
        private void CargarAgente()
        {
            Agente primerAgente = new Agente("");

            this._agentes = new List<Agente>();
            this._agentes.Add(primerAgente);

            if ((Agente)this._ventana.Agente != null)
            {
                this._agentes.Add((Agente)this._ventana.Agente);
                this._ventana.AgentesFiltrados = this._agentes;
                this._ventana.AgenteFiltrado = this.BuscarAgente((IList<Agente>)this._ventana.AgentesFiltrados, (Agente)this._ventana.Agente);
            }
            else
            {
                this._ventana.Agente = primerAgente;
                this._ventana.AgentesFiltrados = this._agentes;
                this._ventana.AgenteFiltrado = primerAgente;
            }

        }

        /// <summary>
        /// Método que se encarga de consultar un Agente en Base de datos
        /// </summary>
        public void ConsultarAgentes()
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
                apoderadoInteresado.Nombre = this._ventana.NombreAgenteFiltrar.ToUpper();
                apoderadoInteresado.Id = this._ventana.IdAgenteFiltrar.ToUpper();

                if ((!apoderadoInteresado.Nombre.Equals("")) || (!apoderadoInteresado.Id.Equals("")))
                    agentesInteresadoFiltrados = this._agenteServicios.ObtenerAgentesFiltro(apoderadoInteresado);
                else
                    agentesInteresadoFiltrados = new List<Agente>();

                if (agentesInteresadoFiltrados.ToList<Agente>().Count != 0)
                {
                    agentesInteresadoFiltrados.Insert(0, primerAgente);
                    this._ventana.AgentesFiltrados = agentesInteresadoFiltrados;
                    this._ventana.AgenteFiltrado = primerAgente;
                }
                else
                {
                    agentesInteresadoFiltrados.Insert(0, primerAgente);
                    this._ventana.AgentesFiltrados = this._agentes;
                    this._ventana.AgenteFiltrado = primerAgente;
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

        /// <summary>
        /// Método que se encarga en cambiar el Agente seleccionado en la lista filtrada
        /// </summary>
        /// <returns>True si se cambió correctamente, false en caso contrario</returns>         
        public bool CambiarAgente()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            bool retorno = false;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if ((null != this._ventana.AgenteFiltrado) && (!((Agente)this._ventana.AgenteFiltrado).Id.Equals("")))
                {
                    if ((null != this._ventana.InteresadoFiltrado) && (((Interesado)this._ventana.Interesado).Id != int.MinValue))
                    {
                        if ((null != this._ventana.PoderFiltrado) && (((Poder)this._ventana.PoderFiltrado).Id != int.MinValue))
                        {
                            this._ventana.Agente = this._ventana.AgenteFiltrado;
                            this._ventana.NombreAgente = ((Agente)this._ventana.AgenteFiltrado).Nombre;
                            retorno = true;
                        }
                        else
                        {
                            //this._poderes = this._poderServicios.ConsultarPoderesPorAgente(((Agente)_ventana.AgenteFiltrado));

                            this._poderes = this._poderServicios.ObtenerPoderesEntreAgenteEInteresado((Agente)this._ventana.AgenteFiltrado, (Interesado)this._ventana.Interesado);
                                                                

                            //LimpiarListaPoder();

                            if (this._poderes.Count != 0)
                            {
                                this._ventana.Agente = this._ventana.AgenteFiltrado;
                                this._ventana.NombreAgente = ((Agente)this._ventana.AgenteFiltrado).Nombre;
                                this._ventana.PoderesFiltrados = this._poderes;
                                this._poderes.Insert(0, new Poder(int.MinValue));
                                retorno = true;
                            }



                            //if (null != this._poderesInteresado)
                            //{

                            //    if ((this.ValidarListaDePoderes(this._poderesInteresado, this._poderes)))
                            //    {
                            //        this._ventana.Agente = this._ventana.AgenteFiltrado;
                            //        this._ventana.NombreAgente = ((Agente)this._ventana.AgenteFiltrado).Nombre;
                            //        retorno = true;
                            //    }
                            //    else if (!this.ValidarListaDePoderes(this._poderesInteresado, this._poderes))
                            //    {
                            //        this._ventana.ConvertirEnteroMinimoABlanco();
                            //        this._ventana.Mensaje(string.Format(Recursos.MensajesConElUsuario.ErrorAgenteNoPoseePoderConInteresado, "Cedente"), 0);
                            //    }
                            //}

                            else
                            {
                                this._ventana.Mensaje("El Apoderado no tiene poderes con el Interesado", 1);
                                this._ventana.Agente = this._ventana.AgenteFiltrado;
                                this._ventana.NombreAgente = ((Agente)this._ventana.AgenteFiltrado).Nombre;
                                retorno = true;
                            }

                        }
                    }
                    else
                    {
                        if (((Poder)this._ventana.PoderFiltrado).Id != int.MinValue)
                        {
                            this._ventana.Agente = this._ventana.AgenteFiltrado;
                            this._ventana.NombreAgente = ((Agente)this._ventana.AgenteFiltrado).Nombre;
                            retorno = true;
                        }
                        else
                        {
                            this._poderes = this._poderServicios.ConsultarPoderesPorAgente(((Agente)_ventana.AgenteFiltrado));
                            this._ventana.Agente = this._ventana.AgenteFiltrado;
                            this._ventana.NombreAgente = ((Agente)this._ventana.AgenteFiltrado).Nombre;
                            retorno = true;
                        }
                    }
                }
                else
                {
                    this._ventana.Agente = this._ventana.AgenteFiltrado;
                    this._ventana.NombreAgente = ((Agente)this._ventana.AgenteFiltrado).Nombre;
                    //this._ventana.PoderesFiltrados = null;
                    //this._ventana.PoderFiltrado = null;
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

        /// <summary>
        /// Metodo que se encarga de verificar si el Agente seleccionado es válido
        /// </summary>
        /// <returns>True si es válido, False en caso contrario</returns>
        public bool VerificarCambioAgente()
        {
            bool retorno = false;

            if (!(((Agente)this._ventana.AgenteFiltrado).Id.Equals("")) || (((Interesado)this._ventana.InteresadoFiltrado).Id != int.MinValue))
                retorno = true;

            return retorno;
        }

        /// <summary>
        /// Método que se encarga de limpiar las lista de Agentes
        /// </summary>
        public void LimpiarListaAgente()
        {
            Agente primerAgente = new Agente("");
            IList<Agente> listaAgentes = new List<Agente>();
            listaAgentes.Add(primerAgente);

            this._ventana.AgentesFiltrados = listaAgentes;
            this._ventana.AgenteFiltrado = BuscarAgente(listaAgentes, primerAgente);
            this._ventana.Agente = this._ventana.AgenteFiltrado;

        }

        #endregion


        #region Poder

        /// <summary>
        /// Método que carga los datos iniciales de Poder a mostrar en la página
        /// </summary>
        private void CargarPoder()
        {
            Poder primerPoder = new Poder(int.MinValue);

            this._poderes = new List<Poder>();
            this._poderes.Add(primerPoder);

            if (((Renovacion)this._ventana.Renovacion).Poder != null)
            {
                this._poderes.Add((Poder)this._ventana.Poder);
                this._ventana.PoderesFiltrados = this._poderes;
                primerPoder = this.BuscarPoder((IList<Poder>)this._ventana.PoderesFiltrados,
                                                                                    (Poder)this._ventana.Poder);
                IList<Agente> ListaAgentes = this._agenteServicios.ObtenerAgentesDeUnPoder(primerPoder);
                foreach (Agente agente in ListaAgentes)
                {
                    primerPoder.MostrarAgentes += agente.Id + "-" + agente.Nombre + "\n";
                }
                this._ventana.PoderFiltrado = primerPoder;
            }
            else
            {
                this._ventana.PoderesFiltrados = this._poderes;
                this._ventana.PoderFiltrado = this.BuscarPoder(this._poderes, this._poderes[0]);
                this._ventana.ConvertirEnteroMinimoABlanco();
            }


        }

        /// <summary>
        /// Método que se encarga de consultar un Poder en Base de datos
        /// </summary>
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
                    //poderesFiltrados = this._poderServicios.ObtenerPoderesFiltro(poder);
                    poderesFiltrados = this._poderServicios.ConsultarPoderesPorInteresado((Interesado)this._ventana.Interesado);

                else
                    poderesFiltrados = new List<Poder>();

                if (poderesFiltrados.ToList<Poder>().Count != 0)
                {
                    Poder poderBuscado = this.BuscarPoder(poderesFiltrados, poder);
                    if (poderBuscado != null)
                    {

                        foreach (Poder aux in poderesFiltrados)
                        {
                            IList<Agente> ListaAgentes = this._agenteServicios.ObtenerAgentesDeUnPoder(aux);
                            foreach (Agente agente in ListaAgentes)
                            {
                                aux.MostrarAgentes += agente.Id + "-" + agente.Nombre + "\n";
                            }

                        }
                        poderesFiltrados.Insert(0, pimerPoder);
                        this._ventana.PoderesFiltrados = poderesFiltrados;
                        this._ventana.PoderFiltrado = pimerPoder;
                    }

                    else
                    {
                        this._ventana.Mensaje("El Poder no corresponde al Interesado", 1);
                        this._ventana.IdPoderFiltrar = null;

                    }
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

        /// <summary>
        /// Método que se encarga en cambiar el Poder seleccionado en la lista filtrada
        /// </summary>
        /// <returns>True si se cambió correctamente, false en caso contrario</returns>   
        public bool CambiarPoder()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            bool retorno = false;

            IList<Poder> poderesObtenidos = new List<Poder>();

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if ((null != this._ventana.PoderFiltrado) && (((Poder)this._ventana.PoderFiltrado).Id != int.MinValue))
                {
                    if ((null != this._ventana.AgenteFiltrado) && (((Agente)this._ventana.AgenteFiltrado).Id.Equals("")))
                    {
                        if ((null != this._ventana.InteresadoFiltrado) && (((Interesado)this._ventana.InteresadoFiltrado).Id != int.MinValue))
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
                        poderesObtenidos = (IList<Poder>)this._ventana.PoderesFiltrados;
                        Poder poderFiltrado = this.BuscarPoder(poderesObtenidos, (Poder)this._ventana.PoderFiltrado);
                        if (poderFiltrado != null)
                            this._ventana.Poder = this._ventana.PoderFiltrado;
                        else
                        {
                            this._ventana.Mensaje("El Poder no pertenece al Interesado", 1);
                            this._ventana.Poder = this._ventana.PoderFiltrado;
                        }

                        this._ventana.IdPoder = ((Poder)this._ventana.PoderFiltrado).Id.ToString();
                        retorno = true;
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

        /// <summary>
        /// Método que se encarga de llenar la lista de poderes de Interesados y Agentes de la Renovacion
        /// </summary>
        /// <param name="renvacion"></param>
        public void LlenarListasPoderes(Renovacion renvacion)
        {
            if (renvacion.Interesado != null)
                this._poderesInteresado = this._poderServicios.ConsultarPoderesPorInteresado(renvacion.Interesado);

            if (renvacion.Agente != null)
                this._poderes = this._poderServicios.ConsultarPoderesPorAgente(renvacion.Agente);
        }

        /// <summary>
        /// Método que se encarga de hacer la intersección entre lista de poderes de Agentes e Interesados
        /// </summary>
        /// <param name="listaPoderesA"></param>
        /// <param name="listaPoderesB"></param>
        /// <returns></returns>
        public bool ValidarListaDePoderes(IList<Poder> listaPoderesA, IList<Poder> listaPoderesB)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            bool retorno = false;
            IList<Poder> listaIntereseccionInteresado = new List<Poder>();
            Poder primerPoder = new Poder(int.MinValue);

            Poder poder = new Poder();

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
                    poder = (Poder)this._ventana.PoderFiltrado;
                    this._poderesInterseccion = listaIntereseccionInteresado;
                    this._ventana.PoderesFiltrados = listaIntereseccionInteresado;
                    this._ventana.PoderFiltrado = BuscarPoder((IList<Poder>)this._ventana.PoderesFiltrados, poder);
                }
                else
                    retorno = false;
            }

            Mouse.OverrideCursor = null;

            return retorno;
        }

        /// <summary>
        /// Metodo que se encarga de verificar si el Poder seleccionado es válido
        /// </summary>
        /// <returns>True si es válido, False en caso contrario</returns>
        public bool VerificarCambioPoder()
        {
            bool retorno = false;

            if (((Poder)this._ventana.PoderFiltrado).Id != int.MinValue)
                retorno = true;

            return retorno;
        }

        /// <summary>
        /// Método que se encarga de limpiar las lista de Poderes
        /// </summary>
        public void LimpiarListaPoder()
        {
            Poder primerPoder = new Poder(int.MinValue);
            IList<Poder> listaPoderes = new List<Poder>();
            listaPoderes.Add(primerPoder);

            this._ventana.PoderesFiltrados = listaPoderes;
            this._ventana.PoderFiltrado = BuscarPoder(listaPoderes, primerPoder);
            this._ventana.Poder = this._ventana.PoderFiltrado;

        }

        #endregion


        public void IrImprimir(string nombreBoton)
        {
            try
            {
                switch (nombreBoton)
                {
                    case "_btnAnexo":
                        ImprimirAnexo();
                        break;
                    case "_btnSolicitud":
                        ImprimirSolicitud();
                        break;
                    case "_btnSolicitudVan":
                        ImprimirSolicitudVan();
                        break;
                    case "_btnCarpeta":
                        ImprimirCarpeta();
                        break;
                    default:
                        break;
                }
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


        private void ImprimirSolicitud()
        {
            if (ValidarMarcaAntesDeImprimirCarpeta())
            {
                string paqueteProcedimiento = "PCK_MYP_RENOVACIONES";
                string procedimiento = "P1";
                ParametroProcedimiento parametro =
                    new ParametroProcedimiento(((Renovacion)this._ventana.Renovacion).Id, UsuarioLogeado, 1, paqueteProcedimiento, procedimiento);

                this.LlamarProcedimientoDeBaseDeDatos(parametro, Recursos.Etiquetas.btnSolicitud);
            }
        }


        private void ImprimirSolicitudVan()
        {
            if (ValidarMarcaAntesDeImprimirCarpeta())
            {
                string paqueteProcedimiento = "PCK_MYP_RENOVACIONES";
                string procedimiento = "P2";
                ParametroProcedimiento parametro =
                    new ParametroProcedimiento(((Renovacion)this._ventana.Renovacion).Id, UsuarioLogeado, 1, paqueteProcedimiento, procedimiento);

                this.LlamarProcedimientoDeBaseDeDatos(parametro, Recursos.Etiquetas.btnSolicitud);
            }
        }


        private void ImprimirCarpeta()
        {
            if (ValidarMarcaAntesDeImprimirCarpeta())
            {
                string paqueteProcedimiento = "PCK_MYP_RENOVACIONES";
                string procedimiento = "P4";
                ParametroProcedimiento parametro =
                    new ParametroProcedimiento(((Renovacion)this._ventana.Renovacion).Id, UsuarioLogeado, 1, paqueteProcedimiento, procedimiento);

                this.LlamarProcedimientoDeBaseDeDatos(parametro, Recursos.Etiquetas.btnCarpeta);
            }
        }


        private void ImprimirAnexo()
        {
            if (ValidarMarcaAntesDeImprimirCarpeta())
            {
                string paqueteProcedimiento = "PCK_MYP_RENOVACIONES";
                string procedimiento = "P3";
                ParametroProcedimiento parametro =
                    new ParametroProcedimiento(((Renovacion)this._ventana.Renovacion).Id, UsuarioLogeado, 1, paqueteProcedimiento, procedimiento);

                this.LlamarProcedimientoDeBaseDeDatos(parametro, Recursos.Etiquetas.btnAnexo);
            }
        }


        private bool ValidarMarcaAntesDeImprimirCarpeta()
        {
            return true;
        }


        internal void ActualizarFechaProxima()
        {
            int tiempoConfiguracion = int.Parse(ConfigurationManager.AppSettings["PeriodoRenovacion"].ToString());
            this._ventana.ProximaRenovacion = ((Marca)this._ventana.Marca).FechaRenovacion.Value.AddYears(tiempoConfiguracion).ToString();

        }


        public void EscribirPeriodoDeGracia()
        {
            this._ventana.PeriodoDeGracia = ConfigurationManager.AppSettings["TextoPeriodoDeGracia"].ToString();
        }

        /// <summary>
        /// Metodo para llamar a una nueva Renovacion de Marca
        /// </summary>
        public void NuevaRenovacionMarca()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                this.Navegar(new GestionarRenovacion(null, null));

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
        /// Metodo que presenta el archivo pdf de la planilla
        /// </summary>
        public void IrVerPlanilla()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                String ruta = ConfigurationManager.AppSettings["RutaPlanillaPDFRenovacionMarca"].ToString() + ((Renovacion)this._ventana.Renovacion).Id.ToString() + ".pdf";
                System.Diagnostics.Process.Start(ruta);

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Win32Exception ex)
            {
                logger.Error(ex.Message);
                this._ventana.ArchivoNoEncontrado(string.Format(Recursos.MensajesConElUsuario.ErrorPlanillaPdfRenovacionNoEncontrada, ((Renovacion)this._ventana.Renovacion).Id.ToString()));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
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

            if (this._ventana.Marca != null)
            {
                Marca marcaParaNavegar = null;
                bool encontrada = false;
                int cont = 0;
                while (!encontrada)
                {
                    Marca marca = this._marcas[cont];
                    if (marca.Id == ((Marca)this._ventana.Marca).Id)
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
    }
}
