﻿using System;
using System.Configuration;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Windows.Input;
using NLog;
using System.Linq;
using Trascend.Bolet.Cliente.Contratos.Cartas;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;
using System.Windows.Controls;
using Trascend.Bolet.Cliente.Ventanas.Auditorias;
using Trascend.Bolet.Cliente.Ventanas.Cartas;
using Trascend.Bolet.Cliente.Ventanas.Asociados;
using Trascend.Bolet.Cliente.Ventanas.Contactos;

namespace Trascend.Bolet.Cliente.Presentadores.Cartas
{
    class PresentadorConsultarCarta : PresentadorBase
    {

        private IConsultarCarta _ventana;
        private ICartaServicios _cartaServicios;
        private IResumenServicios _resumenServicios;
        private IMedioServicios _medioServicios;
        private IAsociadoServicios _asociadoServicios;
        private IUsuarioServicios _usuarioServicios;
        private IAnexoServicios _anexoServicios;
        private IContactoServicios _contactoServicios;
        private IDepartamentoServicios _departamentoServicios;
        private IAsignacionServicios _asignacionServicios;
        private IListaDatosValoresServicios _listaDatosValoresServicios;
        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IList<Asociado> _asociados;
        private IList<Usuario> _receptores;
        private IList<Medio> _medios;
        private IList<Medio> _listaMedioTracking;
        private IList<ListaDatosValores> _listaDatosValores;
        private IList<Anexo> _anexos;
        private IList<Anexo> _anexosConfirmacion;
        private IList<Contacto> _personas = new List<Contacto>();
        private IList<Resumen> _resumenes;
        private IList<Departamento> _departamentos;
        private IList<Usuario> _responsables;
        private IList<Carta> _cartasARecorrer;
        private int _posicion = 1;


        private bool _precargada = false;
        private bool _listaCartasCargada = false;
        private bool _otraCarta = false;
        private bool _volverRefresca = false;
        private bool _presionoBotonPersona = false;


        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="ventana">Página que satisface el contrato</param>
        /// <param name="carta">Carta seleccionada para consultar</param>
        /// <param name="ventanaAVolver">Ventana que antecede a esta ventana</param>
        public PresentadorConsultarCarta(IConsultarCarta ventana, 
                                         object carta, 
                                         object ventanaAVolver)
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                this._ventana = ventana;

                this._ventanaPadre = ventanaAVolver;
                Carta cartaA = (Carta)carta;
                //cartaA.FechaAlt = null != cartaA.FechaAlt ? FormatearFecha(cartaA.FechaAlt) : null;
                //cartaA.FechaReal = null != cartaA.FechaReal ? FormatearFecha(cartaA.FechaReal) : null;
                this._ventana.Carta = cartaA;

                //       this._precargada = ventanaAVolver.Equals(null) ? false : true;
                //       this._ventanaAVolver = ventanaAVolver;

                this._cartaServicios = (ICartaServicios)Activator.GetObject(typeof(ICartaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CartaServicios"]);
                this._resumenServicios = (IResumenServicios)Activator.GetObject(typeof(IResumenServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ResumenServicios"]);
                this._medioServicios = (IMedioServicios)Activator.GetObject(typeof(IMedioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["MedioServicios"]);
                this._usuarioServicios = (IUsuarioServicios)Activator.GetObject(typeof(IUsuarioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["UsuarioServicios"]);
                this._anexoServicios = (IAnexoServicios)Activator.GetObject(typeof(IAnexoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AnexoServicios"]);
                this._contactoServicios = (IContactoServicios)Activator.GetObject(typeof(IContactoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ContactoServicios"]);
                this._departamentoServicios = (IDepartamentoServicios)Activator.GetObject(typeof(IDepartamentoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["DepartamentoServicios"]);
                this._asociadoServicios = (IAsociadoServicios)Activator.GetObject(typeof(IAsociadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsociadoServicios"]);
                this._asignacionServicios = (IAsignacionServicios)Activator.GetObject(typeof(IAsignacionServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsignacionServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);

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
        /// Constructor Con Lista de Resultados
        /// </summary>
        /// <param name="ventana">Página que satisface el contrato</param>
        /// <param name="carta">Carta seleccionada en una lista de Resultados</param>
        /// <param name="cartasConsultadas">Lista de Cartas producto del resultado de la consulta</param>
        /// <param name="posicion">Posicion de la carta en la lista de cartas consultadas</param>
        /// <param name="ventanaPadre">Ventana que antecede a esta ventana</param>
        /// <param name="volverRefresca">Bit de bandera para indicar si se refresca al volver</param>
        public PresentadorConsultarCarta(IConsultarCarta ventana, 
                                         object carta,
                                         object cartasConsultadas, 
                                         int posicion, 
                                         object ventanaPadre, 
                                         bool volverRefresca)
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                this._ventana = ventana;
                this._ventanaPadre = ventanaPadre;
                this._ventana.Carta = carta;
                this._volverRefresca = volverRefresca;
                //this._precargada = ventanaAVolver.Equals(null) ? false : true;
                //this._ventanaAVolver = ventanaAVolver;
                this._listaCartasCargada = true;
                this._cartasARecorrer = (List<Carta>)cartasConsultadas;

                this._posicion += posicion;

                this._cartaServicios = (ICartaServicios)Activator.GetObject(typeof(ICartaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CartaServicios"]);
                this._resumenServicios = (IResumenServicios)Activator.GetObject(typeof(IResumenServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ResumenServicios"]);
                this._medioServicios = (IMedioServicios)Activator.GetObject(typeof(IMedioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["MedioServicios"]);
                this._usuarioServicios = (IUsuarioServicios)Activator.GetObject(typeof(IUsuarioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["UsuarioServicios"]);
                this._anexoServicios = (IAnexoServicios)Activator.GetObject(typeof(IAnexoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AnexoServicios"]);
                this._contactoServicios = (IContactoServicios)Activator.GetObject(typeof(IContactoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ContactoServicios"]);
                this._departamentoServicios = (IDepartamentoServicios)Activator.GetObject(typeof(IDepartamentoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["DepartamentoServicios"]);
                this._asociadoServicios = (IAsociadoServicios)Activator.GetObject(typeof(IAsociadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsociadoServicios"]);
                this._asignacionServicios = (IAsignacionServicios)Activator.GetObject(typeof(IAsignacionServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsignacionServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);


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
        /// Método que carga los datos iniciales a mostrar en la página
        /// </summary>
        public void CargarPagina()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Carta carta = (Carta)this._ventana.Carta;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleConsultarCarta,
                    Recursos.Ids.AgregarCarta);

                if (!_otraCarta)
                    this._medios = this._medioServicios.ConsultarTodos();
                this._ventana.Medios = this._medios;
                Medio medio = new Medio();
                medio.Id = carta.Medio;
                this._ventana.Medio = this.BuscarMedio(this._medios, medio);

                if (!_otraCarta)
                {
                    IList<ListaDatosValores> listaAcuse =
                    this._listaDatosValoresServicios.ConsultarListaDatosValoresPorParametro
                                    (new ListaDatosValores(Recursos.Etiquetas.cbiCategoriaAcuseEntrada));
                    ListaDatosValores primerDatoValor = new ListaDatosValores();
                    primerDatoValor.Id = "NGN";
                    listaAcuse.Insert(0, primerDatoValor);
                    this._ventana.AcuseLista = listaAcuse;
                    this._listaDatosValores = listaAcuse;
                }
                ListaDatosValores DatoValor = new ListaDatosValores();
                DatoValor.Valor = carta.Acuse.ToString();
                this._ventana.Acuse = BuscarListaDeDatosValores(_listaDatosValores, DatoValor);

                if (!_otraCarta)
                {
                    IList<Medio> mediosTracking = this._medioServicios.ConsultarTodos();
                    _listaMedioTracking = mediosTracking;
                    Medio primerosMediosTracking = new Medio();
                    primerosMediosTracking.Id = "NGN";
                    mediosTracking.Insert(0, primerosMediosTracking);
                    this._ventana.MediosTrackingConfirmacion = mediosTracking;
                }
                Medio medioConfirmacion = new Medio();
                if (null != carta.AnexoMedio)
                {
                    medioConfirmacion.Id = carta.AnexoMedio.ToUpper();
                    this._ventana.MedioTrackingConfirmacion = this.BuscarMedio(_listaMedioTracking, medioConfirmacion);
                }
                else
                    this._ventana.MedioTrackingConfirmacion = _listaMedioTracking[0];

                if (!_otraCarta)
                    this._receptores = this._usuarioServicios.ConsultarTodos();
                this._ventana.Receptores = this._receptores;
                this._ventana.Receptor = this.BuscarReceptor(this._receptores, carta.Receptor);

                //if (!_otraCarta)
                //{
                    this._anexos = this._anexoServicios.ConsultarTodos();
                    Anexo primerAnexo = new Anexo();
                    primerAnexo.Id = "NGN";
                    this._anexos.Insert(0, primerAnexo);

                    this._ventana.Anexos = _anexos;
                    this._anexosConfirmacion = this._anexoServicios.ConsultarTodos();
                    this._anexosConfirmacion.Insert(0, primerAnexo);
                    this._ventana.AnexosConfirmacion = _anexosConfirmacion;
                //}

                //if (!_otraCarta)
                //{
                //    //this._asociados = this._asociadoServicios.ConsultarTodos();

                //}

                //this._ventana.Asociados = this._asociados;
                try
                {
                    IList<Asociado> asociados = new List<Asociado>();
                    if (null != carta.Asociado)
                    {
                        this._ventana.Personas = null;
                        this._ventana.Persona = null;
                        this._ventana.NombreAsociado = ((Carta)this._ventana.Carta).Asociado.Nombre;
                        this._ventana.CodigoAsociado = ((Carta)this._ventana.Carta).Asociado.Id.ToString();
                        Asociado asociado = this._asociadoServicios.ConsultarAsociadoConTodo(((Carta)this._ventana.Carta).Asociado);
                        asociado.Contactos = this._contactoServicios.ConsultarContactosPorAsociado(asociado);
                        Contacto primerContacto = new Contacto();
                        Asociado primerAsociadoC = new Asociado();
                        primerAsociadoC.Id = int.MinValue;
                        primerContacto.Nombre = string.Empty;
                        primerContacto.Asociado = primerAsociadoC;
                        IList<Contacto> listaContactos = this._contactoServicios.ConsultarContactosPorAsociado(asociado);
                        listaContactos.Insert(0, primerContacto);
                        this._personas = listaContactos;
                        this._ventana.Personas = listaContactos;
                        this._ventana.Persona = BuscarContacto(this._personas, carta.Persona);
                        asociados.Add(((Carta)this._ventana.Carta).Asociado);
                    }
                    Asociado primerAsociado = new Asociado();
                    primerAsociado.Id = int.MinValue;
                    asociados.Insert(0, primerAsociado);
                    this._ventana.Asociados = asociados;
                    if (null != carta.Asociado)
                        this._ventana.Asociado = this.BuscarAsociado(asociados, carta.Asociado);
                }
                catch (NHibernate.LazyInitializationException)
                {
                    carta.Asociado = null;
                    this._ventana.Asociado = null;
                    this._ventana.Personas = null;
                    this._ventana.Persona = null;
                    this._ventana.NombreAsociado = null;
                    this._ventana.CodigoAsociado = null;
                }

                if (!_otraCarta)
                {
                    _departamentos = this._departamentoServicios.ConsultarTodos();
                    Departamento primeraTarifa = new Departamento();
                    primeraTarifa.Id = "NGN";
                    _departamentos.Insert(0, primeraTarifa);
                }
                this._ventana.Departamentos = _departamentos;
                if (null != carta.Departamento)
                    this._ventana.Departamento = this.BuscarDepartamento(this._departamentos, carta.Departamento);

                //if (!_otraCarta)
                //{
                    this._responsables = this._usuarioServicios.ConsultarTodos();
                    _responsables = FiltrarUsuariosRepetidos(_responsables);
                    Usuario primerResponsable = new Usuario();
                    primerResponsable.Id = "NGN";
                    _responsables.Insert(0, primerResponsable);
                //}
                this._ventana.Responsables = _responsables;

                this._ventana.MediosTrackingConfirmacion = _listaMedioTracking;

                if (!_otraCarta)
                {
                    this._resumenes = this._resumenServicios.ConsultarTodos();
                    Resumen primeraResumen = new Resumen();
                    primeraResumen.Id = "NGN";
                    _resumenes.Insert(0, primeraResumen);
                }
                this._ventana.Resumenes = null;
                this._ventana.Resumenes = this._resumenes;
                if (null != carta.Resumen)
                    this._ventana.Resumen = this.BuscarResumen(this._resumenes, carta.Resumen.Id);

                ((Carta)this._ventana.Carta).Asignaciones = this._asignacionServicios.ObtenerAsignacionesPorCarta((Carta)this._ventana.Carta);

               

                this._ventana.FocoPredeterminado();
                if (_listaCartasCargada)
                    this._ventana.ContadorCartas = this._posicion.ToString() + " de "
                                + ((List<Carta>)this._cartasARecorrer).Count.ToString();

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
        /// Método que carga lista de anexos de la carta
        /// </summary>
        /// <returns>true si se realizó correctamente</returns>
        public bool CargarAnexosCarta()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            bool retorno = false;
            Carta carta = (Carta)this._ventana.Carta;
            if ((null != carta.Anexos) && (carta.Anexos.Count != 0))
            {
                this._ventana.AnexosCarta = carta.Anexos;
                retorno = true;
                this.LimpiarAnexosCarta(carta);
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }


        /// <summary>
        /// Método que limpia lista de anexos de carta
        /// </summary>
        /// <param name="carta"></param>
        private void LimpiarAnexosCarta(Carta carta)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            IList<int> indices = new List<int>();
            foreach (Anexo anexos in carta.Anexos)
            {
                int index = 0;
                foreach (Anexo anexosTotal in this._anexos)
                {
                    if (anexos.Id == anexosTotal.Id)
                    {
                        indices.Insert(0, index);
                    }
                    index++;
                }
            }

            foreach (int indice in indices)
            {
                this._anexos.RemoveAt(indice);
            }

            this._ventana.Anexos = this._anexos;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que carga los anexos confirmación de cartas
        /// </summary>
        /// <returns></returns>
        public bool CargarAnexosCartaConfirmacion()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            bool retorno = false;
            Carta carta = (Carta)this._ventana.Carta;
            if ((null != carta.AnexosConfirmacion) && (carta.AnexosConfirmacion.Count != 0))
            {
                this._ventana.AnexosConfirmacionCarta = carta.AnexosConfirmacion;
                retorno = true;
                this.LimpiarAnexosCartaConfirmacion(carta);
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }


        /// <summary>
        /// Método que limpia lista de anexos confirmación de carta
        /// </summary>
        /// <param name="carta"></param>
        private void LimpiarAnexosCartaConfirmacion(Carta carta)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            IList<int> indices = new List<int>();
            foreach (Anexo anexos in carta.AnexosConfirmacion)
            {
                int index = 0;
                foreach (Anexo anexosTotal in this._anexosConfirmacion)
                {
                    if (anexos.Id == anexosTotal.Id)
                    {
                        indices.Insert(0, index);
                    }
                    index++;
                }
            }

            foreach (int indice in indices)
            {
                this._anexosConfirmacion.RemoveAt(indice);
            }

            this._ventana.AnexosConfirmacion = this._anexosConfirmacion;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        //public bool CargarResponsables()    
        //{
        //    bool retorno = false;
        //    Carta carta = (Carta)this._ventana.Carta;
        //    if ((null != carta.Asignaciones) && (carta.Asignaciones.Count != 0))
        //    {
        //        this._ventana.ResponsablesList = carta.Asignaciones;
        //        retorno = true;
        //        this.LimpiarResponsables(carta);
        //    }
        //    return retorno;
        //}

        //private void LimpiarResponsables(Carta carta)
        //{
        //    IList<int> indices = new List<int>();
        //    foreach (Usuario responsables in carta.Asignaciones)
        //    {
        //        int index = 0;
        //        foreach (Usuario responsablesTotal in this._responsables)
        //        {
        //            if (responsables.Id == responsablesTotal.Id)
        //            {
        //                indices.Insert(0, index);
        //            }
        //            index++;
        //        }
        //    }

        //    foreach (int indice in indices)
        //    {
        //        this._responsables.RemoveAt(indice);
        //    }

        //    this._ventana.Responsables = this._responsables;
        //}

        //public bool verificarFormato()
        //{
        //    bool trackingCorrecto = true;
        //    if (((Medio)this._ventana.MedioTracking).Formato.Length == ((Carta)this._ventana.Carta).Tracking.Length)
        //    {
        //        for (int i = 0; i < ((Medio)this._ventana.MedioTracking).Formato.Length; i++)
        //        {
        //            if (((Medio)this._ventana.MedioTracking).Formato[i] == '9')
        //            {
        //                if (!Char.IsNumber(((Carta)this._ventana.Carta).Tracking[i]))
        //                    trackingCorrecto = false;
        //            }

        //            if (((Medio)this._ventana.MedioTracking).Formato[i] == '-')
        //            {
        //                if (((Carta)this._ventana.Carta).Tracking[i] != '-')
        //                    trackingCorrecto = false;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        trackingCorrecto = false;
        //    }

        //    return trackingCorrecto;
        //}


        /// <summary>
        /// Método que realiza toda la lógica para agregar al Usuario dentro de la base de datos
        /// </summary>
        public void Modificar()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (this._ventana.TextoBotonModificar == Recursos.Etiquetas.btnModificar)
                {
                    this._ventana.HabilitarCampos = true;
                    this._ventana.TextoBotonModificar = Recursos.Etiquetas.btnAceptar;
                }
                else
                {

                    bool tracking = true;

                    if (null != (((Medio)this._ventana.Medio).Formato) &&
                       (!String.IsNullOrEmpty(((Carta)this._ventana.Carta).Tracking)))

                        tracking = this.VerificarFormatoProduccion(((Medio)this._ventana.Medio).Formato, ((Carta)this._ventana.Carta).Tracking);

                    if (null != (((Medio)this._ventana.MedioTrackingConfirmacion)) &&
                       (null != (((Medio)this._ventana.MedioTrackingConfirmacion).Formato)) &&
                       (!String.IsNullOrEmpty(((Carta)this._ventana.Carta).AnexoTracking)))

                        tracking = this.VerificarFormatoProduccion(((Medio)this._ventana.MedioTrackingConfirmacion).Formato, ((Carta)this._ventana.Carta).AnexoTracking);

                    if (tracking)
                    {
                        Carta carta = (Carta)this._ventana.Carta;
                        //carta.FechaAlt = FormatearFecha(carta.FechaAlt);
                        //carta.FechaReal = FormatearFecha(carta.FechaReal);
                        carta.Operacion = "MODIFY";

                        if (null != this._ventana.Departamento)
                            carta.Departamento = !((Departamento)this._ventana.Departamento).Id.Equals("NGN") ?
                                                 (Departamento)this._ventana.Departamento : null;

                        if (null != this._ventana.Asociado)
                            carta.Asociado = !((Asociado)this._ventana.Asociado).Id.Equals("NGN") ? (Asociado)this._ventana.Asociado : null;

                        if (null != this._ventana.Persona)
                            carta.Persona = !((Contacto)this._ventana.Persona).Id.Equals("NGN") ? ((Contacto)this._ventana.Persona).Nombre : null;

                        if (null != this._ventana.Resumen)
                            carta.Resumen = !((Resumen)this._ventana.Resumen).Id.Equals("NGN") ? ((Resumen)this._ventana.Resumen) : null;

                        if (null != this._ventana.MedioTrackingConfirmacion)
                            carta.AnexoMedio = ((Medio)this._ventana.MedioTrackingConfirmacion).Id;
                        if ((null != this._ventana.Acuse) && ((ListaDatosValores)this._ventana.Acuse).Id != "NGN")
                            carta.Acuse = ((ListaDatosValores)this._ventana.Acuse).Valor[0];
                        else
                            carta.Acuse = ' ';
                        carta.Medio = ((Medio)this._ventana.Medio).Id;
                        carta.Receptor = ((Usuario)this._ventana.Receptor).Iniciales;
                        carta.FechaConfirmacion = System.DateTime.Now;
                        this._ventana.SetFechaConfirmacion(carta.FechaConfirmacion.ToString());
                        bool exitoso = this._cartaServicios.InsertarOModificar(carta, UsuarioLogeado.Hash);

                        if (exitoso)
                        {
                            this._ventana.HabilitarCampos = false;
                            this._ventana.TextoBotonModificar = Recursos.Etiquetas.btnModificar;
                            //this.Navegar(Recursos.MensajesConElUsuario.CartaModificada, false);
                        }

                    }
                    else
                        this._ventana.Mensaje(String.Format(Recursos.MensajesConElUsuario.ErrorTrackingErroneo));
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
        /// Método que permite pasar a la carta siguiente
        /// </summary>
        public void SiguienteCarta()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                #region Guarda Antes de ir a la Carta siguiente

                if (this._ventana.TextoBotonModificar != Recursos.Etiquetas.btnModificar)
                {
                    bool tracking = true;

                    if (null != (((Medio)this._ventana.Medio).Formato) &&
                        (!String.IsNullOrEmpty(((Carta)this._ventana.Carta).Tracking)))

                        //tracking = this.VerificarFormato(((Medio)this._ventana.Medio).Formato, ((Carta)this._ventana.Carta).Tracking);
                        tracking = this.VerificarFormatoProduccion(((Medio)this._ventana.Medio).Formato, ((Carta)this._ventana.Carta).Tracking);

                    if (null != (((Medio)this._ventana.MedioTrackingConfirmacion)) &&
                        (null != (((Medio)this._ventana.MedioTrackingConfirmacion).Formato)) &&
                        (!String.IsNullOrEmpty(((Carta)this._ventana.Carta).AnexoTracking)))

                        //tracking = this.VerificarFormato(((Medio)this._ventana.MedioTrackingConfirmacion).Formato, ((Carta)this._ventana.Carta).AnexoTracking);
                        tracking = this.VerificarFormatoProduccion(((Medio)this._ventana.MedioTrackingConfirmacion).Formato, ((Carta)this._ventana.Carta).AnexoTracking);

                    if (tracking)
                    {
                        Carta carta = (Carta)this._ventana.Carta;
                        carta.Operacion = "MODIFY";

                        if (null != this._ventana.Departamento)
                            carta.Departamento = !((Departamento)this._ventana.Departamento).Id.Equals("NGN")
                                                     ? (Departamento)this._ventana.Departamento
                                                     : null;

                        if (null != this._ventana.Asociado)
                            carta.Asociado = !((Asociado)this._ventana.Asociado).Id.Equals("NGN")
                                                 ? (Asociado)this._ventana.Asociado
                                                 : null;

                        if (null != this._ventana.Persona)
                            carta.Persona = !((Contacto)this._ventana.Persona).Id.Equals("NGN")
                                                ? ((Contacto)this._ventana.Persona).Nombre
                                                : null;

                        if (null != this._ventana.Resumen)
                            carta.Resumen = !((Resumen)this._ventana.Resumen).Id.Equals("NGN")
                                                ? ((Resumen)this._ventana.Resumen)
                                                : null;

                        if (null != this._ventana.MedioTrackingConfirmacion)
                            carta.AnexoMedio = ((Medio)this._ventana.MedioTrackingConfirmacion).Id;

                        if ((null != this._ventana.Acuse) && ((ListaDatosValores)this._ventana.Acuse).Id != "NGN")
                            carta.Acuse = ((ListaDatosValores)this._ventana.Acuse).Valor[0];
                        carta.Medio = ((Medio)this._ventana.Medio).Id;
                        carta.Receptor = ((Usuario)this._ventana.Receptor).Iniciales;
                        
                        bool exitoso = this._cartaServicios.InsertarOModificar(carta, UsuarioLogeado.Hash);

                        if (exitoso)
                        {
                            //this._ventana.HabilitarCampos = false;
                            //this._ventana.TextoBotonModificar = Recursos.Etiquetas.btnModificar;
                        }
                    }
                }

                #endregion

                if (_posicion == _cartasARecorrer.Count())
                    _posicion = 0;

                this._ventana.Carta = this._cartasARecorrer[_posicion];
                _posicion += 1;
                _otraCarta = true;
                this.CargarPagina();
                //   CargarResponsables();



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
        /// Método que permite pasar a la carta anterior
        /// </summary>
        public void AnteriorCarta()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                #region Guarda la Carta antes de pasar la anterior

                if (this._ventana.TextoBotonModificar != Recursos.Etiquetas.btnModificar)
                {
                    bool tracking = true;

                    if (null != (((Medio)this._ventana.Medio).Formato) &&
                        (!String.IsNullOrEmpty(((Carta)this._ventana.Carta).Tracking)))

                        //tracking = this.VerificarFormato(((Medio)this._ventana.Medio).Formato,
                        //                                 ((Carta)this._ventana.Carta).Tracking);

                           tracking = this.VerificarFormatoProduccion(((Medio)this._ventana.Medio).Formato,((Carta)this._ventana.Carta).Tracking);

                    if (null != (((Medio)this._ventana.MedioTrackingConfirmacion)) &&
                        (null != (((Medio)this._ventana.MedioTrackingConfirmacion).Formato)) &&
                        (!String.IsNullOrEmpty(((Carta)this._ventana.Carta).AnexoTracking)))

                        //tracking = this.VerificarFormato(((Medio)this._ventana.MedioTrackingConfirmacion).Formato,
                        //                                 ((Carta)this._ventana.Carta).AnexoTracking);
                        tracking = this.VerificarFormatoProduccion(((Medio)this._ventana.MedioTrackingConfirmacion).Formato, ((Carta)this._ventana.Carta).AnexoTracking);

                    if (tracking)
                    {
                        Carta carta = (Carta)this._ventana.Carta;
                        carta.Operacion = "MODIFY";

                        if (null != this._ventana.Departamento)
                            carta.Departamento = !((Departamento)this._ventana.Departamento).Id.Equals("NGN")
                                                     ? (Departamento)this._ventana.Departamento
                                                     : null;

                        if (null != this._ventana.Asociado)
                            carta.Asociado = !((Asociado)this._ventana.Asociado).Id.Equals("NGN")
                                                 ? (Asociado)this._ventana.Asociado
                                                 : null;

                        if (null != this._ventana.Persona)
                            carta.Persona = !((Contacto)this._ventana.Persona).Id.Equals("NGN")
                                                ? ((Contacto)this._ventana.Persona).Nombre
                                                : null;

                        if (null != this._ventana.Resumen)
                            carta.Resumen = !((Resumen)this._ventana.Resumen).Id.Equals("NGN")
                                                ? ((Resumen)this._ventana.Resumen)
                                                : null;


                        if (null != this._ventana.MedioTrackingConfirmacion)
                            carta.AnexoMedio = ((Medio)this._ventana.MedioTrackingConfirmacion).Id;

                        if ((null != this._ventana.Acuse) && ((ListaDatosValores)this._ventana.Acuse).Id != "NGN")
                            carta.Acuse = ((ListaDatosValores)this._ventana.Acuse).Valor[0];
                        carta.Medio = ((Medio)this._ventana.Medio).Id;
                        carta.Receptor = ((Usuario)this._ventana.Receptor).Iniciales;
                        bool exitoso = this._cartaServicios.InsertarOModificar(carta, UsuarioLogeado.Hash);

                        if (exitoso)
                        {
                            //this._ventana.HabilitarCampos = false;
                            //this._ventana.TextoBotonModificar = Recursos.Etiquetas.btnModificar;
                        }
                    }
                }

                #endregion

                if (_posicion == 1)
                    _posicion = _cartasARecorrer.Count();
                else
                    _posicion -= 1;
                this._ventana.Carta = this._cartasARecorrer[_posicion - 1];

                _otraCarta = true;
                this.CargarPagina();
                //  CargarResponsables();



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
        /// Método que cambia el asociado en la ventana
        /// </summary>
        public bool CambiarAsociado()
        {
            bool retorno = false;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
                if ((this._ventana.Asociado != null) && ((Asociado)this._ventana.Asociado).Id != int.MinValue)
                {
                    retorno = true;
                    Asociado asociado = this._asociadoServicios.ConsultarAsociadoConTodo((Asociado)this._ventana.Asociado);
                    asociado.Contactos = this._contactoServicios.ConsultarContactosPorAsociado(asociado);
                    this._ventana.NombreAsociado = ((Asociado)this._ventana.Asociado).Nombre;
                    this._ventana.CodigoAsociado = ((Asociado)this._ventana.Asociado).Id.ToString();
                    Contacto primerContacto = new Contacto();
                    Asociado primerAsociadoC = new Asociado();
                    primerAsociadoC.Id = int.MinValue;
                    primerContacto.Nombre = string.Empty;
                    primerContacto.Asociado = primerAsociadoC;
                    IList<Contacto> listaContactos = this._contactoServicios.ConsultarContactosPorAsociado(asociado);
                    listaContactos.Insert(0, primerContacto);
                    this._personas = listaContactos;
                    this._ventana.Personas = listaContactos;

                    //if (asociado.Contactos.Count != 0)
                    //    this._ventana.Personas = asociado.Contactos;
                }
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException e)
            {
                this._ventana.Personas = null;
            }

            return retorno;
        }


        /// <summary>
        /// Método que se encarga de buscar un asociado con filtros
        /// </summary>
        public void BuscarAsociado()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            Asociado asociadoAFiltrar = new Asociado();

            IList<Asociado> resultados = new List<Asociado>();

            if ((!this._ventana.idAsociadoFiltrar.Equals("")) || (!this._ventana.NombreAsociadoFiltrar.Equals("")))
            {
                asociadoAFiltrar.Id = !this._ventana.idAsociadoFiltrar.Equals("") ? int.Parse(this._ventana.idAsociadoFiltrar) : 0;
                asociadoAFiltrar.Nombre = !this._ventana.NombreAsociadoFiltrar.Equals("")
                    ? this._ventana.NombreAsociadoFiltrar.ToUpper() : "";


                resultados = this._asociadoServicios.ObtenerAsociadosFiltro(asociadoAFiltrar);

            }

            Asociado primerAsociado = new Asociado();
            primerAsociado.Id = int.MinValue;
            resultados.Insert(0, primerAsociado);
            this._ventana.Asociados = resultados;

            //IEnumerable<Asociado> asociadosFiltrados = this._asociados;

            //if (!string.IsNullOrEmpty(this._ventana.idAsociadoFiltrar))
            //{
            //    asociadosFiltrados = from p in asociadosFiltrados
            //                         where p.Id == int.Parse(this._ventana.idAsociadoFiltrar)
            //                         select p;
            //}

            //if (!string.IsNullOrEmpty(this._ventana.NombreAsociadoFiltrar))
            //{
            //    asociadosFiltrados = from p in asociadosFiltrados
            //                         where p.Nombre != null &&
            //                         p.Nombre.ToLower().Contains(this._ventana.NombreAsociadoFiltrar.ToLower())
            //                         select p;
            //}

            //if (asociadosFiltrados.ToList<Asociado>().Count != 0)
            //    this._ventana.Asociados = asociadosFiltrados.ToList<Asociado>();
            //else
            //    this._ventana.Asociados = this._asociados;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que se encarga de agregar un anexo a carta
        /// </summary>
        /// <returns>true si se realizó correctamente</returns>
        public bool AgregarAnexoCarta()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            IList<Anexo> anexosCarta;
            bool retorno = false;
            if ((null != (Anexo)this._ventana.Anexo) && (!((Anexo)this._ventana.Anexo).Id.Equals("NGN")))
            {
                if (null == ((Carta)this._ventana.Carta).Anexos)
                    anexosCarta = new List<Anexo>();
                else
                    anexosCarta = ((Carta)this._ventana.Carta).Anexos;

                anexosCarta.Add((Anexo)this._ventana.Anexo);
                ((Carta)this._ventana.Carta).Anexos = anexosCarta;
                this._ventana.AnexosCarta = anexosCarta.ToList<Anexo>();
                this._anexos.Remove((Anexo)this._ventana.Anexo);
                this._ventana.Anexos = this._anexos.ToList<Anexo>();
                retorno = true;
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }


        /// <summary>
        /// Método que se encarga de agregar un anexo confirmación a carta
        /// </summary>
        /// <returns>true si se realizó correctamente</returns>
        public bool AgregarAnexoCartaConfirmacion()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            IList<Anexo> anexosCarta;
            bool retorno = false;
            if ((null != (Anexo)this._ventana.AnexoConfirmacion) && (!((Anexo)this._ventana.AnexoConfirmacion).Id.Equals("NGN")))
            {
                if (null == ((Carta)this._ventana.Carta).AnexosConfirmacion)
                    anexosCarta = new List<Anexo>();
                else
                    anexosCarta = ((Carta)this._ventana.Carta).AnexosConfirmacion;

                anexosCarta.Add((Anexo)this._ventana.AnexoConfirmacion);
                ((Carta)this._ventana.Carta).AnexosConfirmacion = anexosCarta;
                this._ventana.AnexosConfirmacionCarta = anexosCarta.ToList<Anexo>();
                this._anexosConfirmacion.Remove((Anexo)this._ventana.AnexoConfirmacion);
                this._ventana.AnexosConfirmacion = this._anexosConfirmacion.ToList<Anexo>();
                retorno = true;
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }


        /// <summary>
        /// Metodo que deshabilita los anexos de carta
        /// </summary>
        /// <returns>retorno true si se deshabilitó</returns>
        public bool DeshabilitarAnexosCarta()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            IList<Anexo> anexosCarta;
            bool respuesta = false;

            if (null != ((Anexo)this._ventana.AnexoCarta))
            {
                if (null == ((Carta)this._ventana.Carta).Anexos)
                    anexosCarta = new List<Anexo>();
                else
                    anexosCarta = ((Carta)this._ventana.Carta).Anexos;

                anexosCarta.Remove((Anexo)this._ventana.AnexoCarta);
                ((Carta)this._ventana.Carta).Anexos = anexosCarta;
                this._anexos.Add((Anexo)this._ventana.AnexoCarta);
                this._ventana.AnexosCarta = anexosCarta.ToList<Anexo>();
                this._ventana.Anexos = this._anexos.ToList<Anexo>();

                if (anexosCarta.Count == 0)
                    respuesta = true;

            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return respuesta;
        }


        /// <summary>
        /// Método que deshabilita los anexos confirmación de carta
        /// </summary>
        /// <returns>retorno true si se deshabilitó</returns>
        public bool DeshabilitarAnexosCartaConfirmacion()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            IList<Anexo> anexosCarta;
            bool respuesta = false;

            if (null != ((Anexo)this._ventana.AnexoConfirmacionCarta))
            {
                if (null == ((Carta)this._ventana.Carta).AnexosConfirmacion)
                    anexosCarta = new List<Anexo>();
                else
                    anexosCarta = ((Carta)this._ventana.Carta).AnexosConfirmacion;

                anexosCarta.Remove((Anexo)this._ventana.AnexoConfirmacionCarta);
                ((Carta)this._ventana.Carta).AnexosConfirmacion = anexosCarta;
                this._anexosConfirmacion.Add((Anexo)this._ventana.AnexoConfirmacionCarta);
                this._ventana.AnexosConfirmacionCarta = anexosCarta.ToList<Anexo>();
                this._ventana.AnexosConfirmacion = this._anexosConfirmacion.ToList<Anexo>();

                if (anexosCarta.Count == 0)
                    respuesta = true;

            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return respuesta;
        }


        /// <summary>
        /// Método que cambia el formato de la pantalla entre vacío a
        /// contenido
        /// </summary>
        public void CambiarFormatoTracking()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this._ventana.FormatoTracking = !((Medio)this._ventana.Medio).Id.Equals("NGN") ? "Formato: " + ((Medio)this._ventana.Medio).Formato : "Formato: ";

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que cambia el formato confirmación de la pantalla entre vacío       
        /// a contenido
        /// </summary>
        public void CambiarFormatoTrackingConfirmacion()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this._ventana.FormatoTrackingConfirmacion = !((Medio)this._ventana.MedioTrackingConfirmacion).Id.Equals("NGN") ? "Formato: " + ((Medio)this._ventana.MedioTrackingConfirmacion).Formato : "Formato: ";

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que registra una operación de la entrada para ser auditada
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
                auditoria.Fk = ((Carta)this._ventana.Carta).Id;
                auditoria.Tabla = "ENTRADA";

                IList<Auditoria> auditorias = this._cartaServicios.AuditoriaPorFkyTabla(auditoria);
                _paginaPrincipal.MensajeUsuario = Recursos.MensajesConElUsuario.PoderEliminado;
                this.Navegar(new ListaAuditorias(auditorias));


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
        /// Método que agrega un responsable a una carta
        /// </summary>
        /// <returns>true si se realizó</returns>
        public bool AgregarResponsable()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            bool retorno = false;
            IList<Asignacion> asignacionLista;
            if ((null != (Usuario)_ventana.Responsable) && (!((Usuario)this._ventana.Responsable).Id.Equals("NGN")))
            {
                if (null == ((Carta)this._ventana.Carta).Asignaciones)
                {
                    asignacionLista = new List<Asignacion>();
                }
                else
                {
                    asignacionLista = ((Carta)this._ventana.Carta).Asignaciones;
                }


                Asignacion asignacionInsertar = new Asignacion((Usuario)this._ventana.Responsable, (Carta)this._ventana.Carta);
                asignacionLista.Add(asignacionInsertar);
                ((Carta)this._ventana.Carta).Asignaciones = asignacionLista;
                IList<Usuario> usuariosLista = ListAsignacionesToUsuarios(asignacionLista);
                this._ventana.ResponsablesList = usuariosLista;

                this._responsables.Remove((Usuario)this._ventana.Responsable);
                this._ventana.Responsables = this._responsables.ToList<Usuario>();


                retorno = true;
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }


        /// <summary>
        /// Metodo que convierte una lista de asignaciones a usuarios
        /// </summary>
        /// <param name="asignaciones">lista de asignaciones</param>
        /// <returns>lista de usuarios</returns>
        private IList<Usuario> ListAsignacionesToUsuarios(IList<Asignacion> asignaciones)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            IList<Usuario> usuarios = new List<Usuario>();
            foreach (Asignacion asignacion in asignaciones)
            {
                usuarios.Add(asignacion.Responsable);
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return usuarios;
        }


        /// <summary>
        /// Método que deshabilita un responsable de la carta
        /// </summary>
        /// <returns>retorno true si se realizó correctamente</returns>
        public bool DeshabilitarResponsable()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            bool retorno = false;
            IList<Asignacion> asignacionLista;
            if (null != (Usuario)this._ventana.ResponsableList)
            {
                if (null == ((Carta)this._ventana.Carta).Asignaciones)
                    asignacionLista = new List<Asignacion>();
                else
                    asignacionLista = ((Carta)this._ventana.Carta).Asignaciones;

                IList<Usuario> usuariosLista = ListAsignacionesToUsuarios(asignacionLista);
                this._responsables.Add((Usuario)this._ventana.ResponsableList);

                ((Carta)this._ventana.Carta).Asignaciones = RemoverResponsable(asignacionLista);
                usuariosLista.Remove((Usuario)this._ventana.ResponsableList);

                this._ventana.ResponsablesList = usuariosLista.ToList<Usuario>();
                this._ventana.Responsables = this._responsables.ToList<Usuario>();
            }

            if (((Carta)this._ventana.Carta).Asignaciones.Count == 0)
                retorno = true;

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }


        /// <summary>
        /// Método que elimina un responsable de la carta
        /// </summary>
        /// <param name="asignaciones">lista de asignaciones</param>
        /// <returns>lista con el responsable removido</returns>
        private IList<Asignacion> RemoverResponsable(IList<Asignacion> asignaciones)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            try
            {
                int index = 0;
                int borrar = 0;
                foreach (Asignacion asignacion in asignaciones)
                {
                    if (null != asignacion.Responsable)
                        if (asignacion.Responsable.Id == ((Usuario)this._ventana.ResponsableList).Id)
                            borrar = index;
                    index++;
                }
                asignaciones.RemoveAt(borrar);
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

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return asignaciones;
        }


        /// <summary>
        /// Método que se encarga de cargar los responsables de una carta
        /// </summary>
        /// <returns></returns>
        public bool CargarResponsables()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            bool retorno = false;
            IList<Usuario> responsables;
            if ((null != ((Carta)this._ventana.Carta).Asignaciones) && (((Carta)this._ventana.Carta).Asignaciones).Count > 0)
            {
                responsables = ListAsignacionesToUsuarios(((Carta)this._ventana.Carta).Asignaciones);
                this._ventana.ResponsablesList = responsables.ToList<Usuario>();
                LimpiarResponsables(responsables);
                retorno = true;
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return retorno;
        }


        /// <summary>
        /// Método que se encarga de limpiar la lista de responsables
        /// </summary>
        /// <param name="responsables">lista de usuarios responsables</param>
        public void LimpiarResponsables(IList<Usuario> responsables)
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            IList<int> indice = new List<int>();
            int index = 0;
            foreach (Usuario usuario in this._responsables)
            {
                //index = 0;
                foreach (Usuario usuarioCarta in responsables)
                {
                    if ((usuarioCarta != null) && (usuario.Id == usuarioCarta.Id))
                        indice.Add(index);
                    //index++;
                }
                index++;
            }

            foreach (int posicion in indice)
            {
                this._responsables.RemoveAt(posicion);
            }

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que carga la ventana de consultar cartas
        /// </summary>
        public void IrConsultarCartas()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            if (null != this._cartasARecorrer)
                this.Navegar(new ConsultarCartas(this._cartasARecorrer));
            else
                this.Navegar(new ConsultarCartas());

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        //public bool AgregarResponsable()
        //{
        //    IList<Usuario> usuariosLista;
        //    bool retorno = false;
        //    if ((null != (Usuario)this._ventana.Responsable) && (!((Usuario)this._ventana.Responsable).Id.Equals("NGN")))
        //    {
        //        if (null == ((Carta)this._ventana.Carta).Responsables)
        //            usuariosLista = new List<Usuario>();
        //        else
        //            usuariosLista = ((Carta)this._ventana.Carta).Responsables;

        //        usuariosLista.Add((Usuario)this._ventana.Responsable);
        //        ((Carta)this._ventana.Carta).Responsables = usuariosLista;
        //        this._ventana.ResponsablesList = usuariosLista.ToList<Usuario>();
        //        this._responsables.Remove((Usuario)this._ventana.Responsable);
        //        this._ventana.Responsables = this._responsables.ToList<Usuario>();
        //        retorno = true;
        //    }
        //    return retorno;
        //}

        //public bool DeshabilitarResponsable()
        //{
        //    IList<Usuario> responsables;
        //    bool respuesta = false;

        //    if (null != ((Usuario)this._ventana.ResponsableList))
        //    {
        //        if (null == ((Carta)this._ventana.Carta).Responsables)
        //            responsables = new List<Usuario>();
        //        else
        //            responsables = ((Carta)this._ventana.Carta).Responsables;

        //        responsables.Remove((Usuario)this._ventana.ResponsableList);
        //        ((Carta)this._ventana.Carta).Responsables = responsables;
        //        this._responsables.Add((Usuario)this._ventana.ResponsableList);
        //        this._ventana.ResponsablesList = responsables.ToList<Usuario>();
        //        this._ventana.Responsables = this._responsables.ToList<Usuario>();

        //        if (responsables.Count == 0)
        //            respuesta = true;

        //    }
        //    return respuesta;
        //}


        public void EliminarCarta()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (this._cartaServicios.Eliminar((Carta)this._ventana.Carta, UsuarioLogeado.Hash))
                {
                    if ((_cartasARecorrer != null) && (_cartasARecorrer.Count > 0))
                    {
                        _cartasARecorrer.RemoveAt(_posicion - 1);

                        //int longitudListaCartas = _cartasARecorrer.Count;

                        //if (_posicion >= _cartasARecorrer.Count())
                        if (_posicion > _cartasARecorrer.Count())
                        {
                            _posicion = 0;
                            this._ventana.Carta = this._cartasARecorrer[_posicion];
                        }
                        else
                        {
                            _posicion--;
                            //this._ventana.Carta = this._cartasARecorrer[_posicion - 1];
                            this._ventana.Carta = this._cartasARecorrer[_posicion];
                        }
                        //_otraCarta = true;
                        _otraCarta = false;
                        //this.CargarPagina();
                        this.Navegar(new ConsultarCarta(this._cartasARecorrer[_posicion], this._cartasARecorrer, _posicion, this._ventanaPadre, false));
                    }
                    else
                        this.Navegar(Recursos.MensajesConElUsuario.CartaEliminadaConExito, false);
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


        public void VolverAVentanaPadre()
        {
            if (_volverRefresca)
                ((ConsultarCartas)_ventanaPadre).Refrescar(this._ventana.Carta, this._cartasARecorrer);

            Navegar((Page)_ventanaPadre);
        }


        public void AbrirCorrespondencia()
        {
            string rutaEntrada = ConfigurationManager.AppSettings["RutaVerEntrada"];
            string comando = "START " + rutaEntrada;
            int anoCorrespondencia = ((Carta)this._ventana.Carta).Fecha.Year;
            string mesCorrespondencia = ((Carta)this._ventana.Carta).Fecha.Month.ToString().Length == 2 ? ((Carta)this._ventana.Carta).Fecha.Month.ToString() : "0" + ((Carta)this._ventana.Carta).Fecha.Month.ToString();

            string diaCorrespondencia = ((Carta)this._ventana.Carta).Fecha.Day.ToString().Length == 2 ? ((Carta)this._ventana.Carta).Fecha.Day.ToString() : "0" + ((Carta)this._ventana.Carta).Fecha.Day.ToString();
            int codigoEntrada = ((Carta)this._ventana.Carta).Id;

            string comandoRutaCorrespondencia = rutaEntrada + anoCorrespondencia + "\\" + mesCorrespondencia + "\\" + anoCorrespondencia + mesCorrespondencia + diaCorrespondencia + "\\" + codigoEntrada;
            this.EjecutarArchivoBAT(ConfigurationManager.AppSettings["RutaBatCorrespondencia"].ToString() + ConfigurationManager.AppSettings["ArchivoBatCorrespondencia"].ToString(), ConfigurationManager.AppSettings["RutaBatCorrespondencia"].ToString() + comandoRutaCorrespondencia + ".TIF" + " " + ConfigurationManager.AppSettings["RutaBatCorrespondencia"].ToString() + comandoRutaCorrespondencia + ".MSG");

            //EjecutarComandoDeConsola(comando + "ejecutar.bat " + comandoRutaCorrespondencia + ".TIF" + " " + comandoRutaCorrespondencia + ".MSG", "Lectura de la correspondencia Id: " + codigoEntrada);
        }


        public string ObtenerIdCarta()
        {
            return ((Carta)this._ventana.Carta).Id.ToString();
        }


        public void IrVentanaAsociado()
        {
            if (this._ventana.Asociado != null)
            {
                Asociado asociado = ((Asociado)this._ventana.Asociado).Id != int.MinValue ? (Asociado)this._ventana.Asociado : null;
                Navegar(new ConsultarAsociado(asociado, this._ventana, false));
            }
        }


        public void SeleccionarContactoYAsociado(object asociado, object contacto)
        {
            Asociado asociadoNuevo = (Asociado)asociado;
            Contacto contactoNuevo = (Contacto)contacto;

            this._ventana.Asociados = null;

            IList<Asociado> asociados = new List<Asociado>();
            asociados.Add(asociadoNuevo);
            this._ventana.Asociados = asociados;
            this._ventana.Asociado = asociadoNuevo;
            this.CambiarAsociado();
            this._ventana.Persona = contacto;
        }


        public void ConsultarContacto()
        {
            if (null == ((Contacto)this._ventana.Persona) || (((Contacto)this._ventana.Persona).Nombre.Equals(string.Empty)))
                Navegar(new ConsultarContactosPorAsociado(this._ventana, null));
            else 
            {
                Contacto contactoFiltro = new Contacto();
                contactoFiltro.Nombre = ((Contacto)this._ventana.Persona).Nombre;
                contactoFiltro.Asociado = (Asociado)this._ventana.Asociado;
                IList<Contacto> contactos = this._contactoServicios.ConsultarContactosFiltro(contactoFiltro);
                if (!this._listaCartasCargada)
                {
                    //Navegar(new ConsultarContacto(contactos[0], this._ventana));
                    Navegar(new ConsultarContacto(contactos[0], true, (Carta)this._ventana.Carta, null, 0, this._ventana, this._ventanaPadre));
                }
                else
                {
                    Navegar(new ConsultarContacto(contactos[0], true, (Carta)this._ventana.Carta, this._cartasARecorrer, this._posicion-1, this._ventana, this._ventanaPadre));
                }
                
            }
        }

        /// <summary>
        /// Metodo que refresca la lista de contactos del asociado
        /// NOTA: Este proceso depende de una bandera para poderse hacer
        /// </summary>
        //public void RefrescarContactosCarta()
        //{
        //    if (_presionoBotonPersona)
        //    {

        //    }

        //}
    }
}
