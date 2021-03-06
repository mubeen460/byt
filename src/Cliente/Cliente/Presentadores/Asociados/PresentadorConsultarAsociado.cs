﻿using System;
using System.Configuration;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Windows.Input;
using NLog;
using Trascend.Bolet.Cliente.Contratos.Asociados;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.Cliente.Ventanas.Asociados;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;
using Trascend.Bolet.Cliente.Ventanas.Auditorias;
using Trascend.Bolet.Cliente.Ventanas.Cartas;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Documents;
using Trascend.Bolet.Cliente.Ayuda;
using System.Text.RegularExpressions;
using Trascend.Bolet.ControlesByT.Ventanas;
using Trascend.Bolet.Cliente.Ventanas.EmailsAsociado;
using Trascend.Bolet.Cliente.Ventanas.Contactos;

using Diginsoft.Bolet.Cliente.Fac.Ventanas.FacReportes;
using Diginsoft.Bolet.Cliente.Fac.Ventanas.FacAsociadoMarcaPatentes;
using Diginsoft.Bolet.Cliente.Fac.Ventanas.ViGestionAsociados;
using Diginsoft.Bolet.ObjetosComunes.ContratosServicios;

namespace Trascend.Bolet.Cliente.Presentadores.Asociados
{
    class PresentadorConsultarAsociado : PresentadorBase
    {
        private IConsultarAsociado _ventana;
        private IAsociadoServicios _asociadoServicios;
        private IDetallePagoServicios _detallePagoServicios;
        private IEtiquetaServicios _etiquetaServicios;
        private IIdiomaServicios _idiomaServicios;
        private IMonedaServicios _monedaServicios;
        private ITarifaServicios _tarifaServicios;
        private ITipoClienteServicios _tipoClienteServicios;
        private IPaisServicios _paisServicios;
        private IContactoServicios _contactoServicios;
        private ITipoEmailAsociadoServicios _tipoEmailAsociadoServicios;
        private IEmailAsociadoServicios _emailAsociadoServicios;
        private ICartaServicios _cartaServicios;
        private IListaDatosDominioServicios _listaDatosDominioServicios;
        private IDatosTransferenciaServicios _datosTransferenciaServicios;
        private IFacGestionServicios _facGestionServicios;
        private IListaDatosValoresServicios _listaDatosValoresServicios;
        private IFacVistaFacturacionCxpInternaServicios _facVistaFacturacionCxpInternaServicios;
        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private IList<Auditoria> _auditorias;
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="ventana">Página que satisface el contrato</param>
        /// <param name="asociado">Asociado a mostrar</param>
        public PresentadorConsultarAsociado(IConsultarAsociado ventana, object asociado, object ventanaPadre)
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                this._ventanaPadre = ventanaPadre;
                this._ventana = ventana;
                this._ventana.Asociado = asociado;
                this._asociadoServicios = (IAsociadoServicios)Activator.GetObject(typeof(IAsociadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsociadoServicios"]);
                this._detallePagoServicios = (IDetallePagoServicios)Activator.GetObject(typeof(IDetallePagoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["DetallePagoServicios"]);
                this._etiquetaServicios = (IEtiquetaServicios)Activator.GetObject(typeof(IEtiquetaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["EtiquetaServicios"]);
                this._idiomaServicios = (IIdiomaServicios)Activator.GetObject(typeof(IIdiomaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["IdiomaServicios"]);
                this._monedaServicios = (IMonedaServicios)Activator.GetObject(typeof(IMonedaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["MonedaServicios"]);
                this._tarifaServicios = (ITarifaServicios)Activator.GetObject(typeof(ITarifaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["TarifaServicios"]);
                this._tipoClienteServicios = (ITipoClienteServicios)Activator.GetObject(typeof(ITipoClienteServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["TipoClienteServicios"]);
                this._paisServicios = (IPaisServicios)Activator.GetObject(typeof(IPaisServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["PaisServicios"]);
                this._contactoServicios = (IContactoServicios)Activator.GetObject(typeof(IContactoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ContactoServicios"]);
                this._datosTransferenciaServicios = (IDatosTransferenciaServicios)Activator.GetObject(typeof(IDatosTransferenciaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["DatosTransferenciaServicios"]);
                this._listaDatosDominioServicios = (IListaDatosDominioServicios)Activator.GetObject(typeof(IListaDatosDominioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosDominioServicios"]);
                this._cartaServicios = (ICartaServicios)Activator.GetObject(typeof(ICartaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CartaServicios"]);
                this._tipoEmailAsociadoServicios = (ITipoEmailAsociadoServicios)Activator.GetObject(typeof(ITipoEmailAsociadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["TipoEmailAsociadoServicios"]);
                this._facGestionServicios = (IFacGestionServicios)Activator.GetObject(typeof(IFacGestionServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["FacGestionServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);
                this._facVistaFacturacionCxpInternaServicios = (IFacVistaFacturacionCxpInternaServicios)Activator.GetObject(typeof(IFacVistaFacturacionCxpInternaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["FacVistaFacturacionCxpInternaServicios"]);
                

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

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleConsultarAsociado,
                    Recursos.Ids.ConsultarAsociado);

                this._ventana.Asociado = this._asociadoServicios.ConsultarAsociadoConTodo((Asociado)this._ventana.Asociado);

                Asociado asociado = (Asociado)this._ventana.Asociado;

                asociado.Contactos = this._contactoServicios.ConsultarContactosPorAsociado(asociado);
                //asociado.Emails = FiltrarEmailsPorDepartamento(UsuarioLogeado.Departamento, this._asociadoServicios.ConsultarEmailsDelAsociado(asociado));
                asociado.Emails = this._asociadoServicios.ConsultarEmailsDelAsociado(asociado);
                asociado.DatosTransferencias = this._datosTransferenciaServicios.ConsultarDatosTransferenciaPorAsociado(asociado);

                CalcularSaldos();

                IList<Pais> paises = this._paisServicios.ConsultarTodos();

                this._ventana.Paises = paises;
                this._ventana.Pais = this.BuscarPais(paises, asociado.Pais);

                IList<Idioma> idiomas = this._idiomaServicios.ConsultarTodos();
                this._ventana.Idiomas = idiomas;
                this._ventana.Idioma = this.BuscarIdioma(idiomas, asociado.Idioma);

                IList<Moneda> monedas = this._monedaServicios.ConsultarTodos();
                this._ventana.Monedas = monedas;
                this._ventana.Moneda = this.BuscarMoneda(monedas, asociado.Moneda);

                IList<TipoCliente> tiposClientes = this._tipoClienteServicios.ConsultarTodos();
                this._ventana.TiposClientes = tiposClientes;
                this._ventana.TipoCliente = this.BuscarTipoCliente(tiposClientes, asociado.TipoCliente);


                IList<ListaDatosValores> origenClientes = 
                    this._listaDatosValoresServicios.ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiOrigenClienteAsociado));
                this._ventana.OrigenClientes = origenClientes;
                ListaDatosValores origenAux = new ListaDatosValores();
                if (asociado.OrigenCliente != null)
                {
                    origenAux.Valor = asociado.OrigenCliente;
                    this._ventana.OrigenCliente = this.BuscarListaDeDatosValores(origenClientes, origenAux);
                }
                else
                {
                    origenAux.Valor = "BOLET";
                    this._ventana.OrigenCliente = this.BuscarListaDeDatosValores(origenClientes, origenAux);
                }
                   


                IList<Tarifa> tarifas = this._tarifaServicios.ConsultarTodos();
                Tarifa primeraTarifa = new Tarifa();
                primeraTarifa.Id = "NGN";
                tarifas.Insert(0, primeraTarifa);
                this._ventana.Tarifas = tarifas;
                this._ventana.Tarifa = this.BuscarTarifa(tarifas, asociado.Tarifa);

                IList<Etiqueta> etiquetas = this._etiquetaServicios.ConsultarTodos();
                Etiqueta primeraEtiqueta = new Etiqueta();
                primeraEtiqueta.Id = string.Empty;
                etiquetas.Insert(0, primeraEtiqueta);
                this._ventana.Etiquetas = etiquetas;
                this._ventana.Etiqueta = this.BuscarEtiqueta(etiquetas, asociado.Etiqueta);

                IList<DetallePago> detallesPagos = this._detallePagoServicios.ConsultarTodos();
                DetallePago primerDetallePago = new DetallePago();
                primerDetallePago.Id = "NGN";
                detallesPagos.Insert(0, primerDetallePago);
                this._ventana.DetallesPagos = detallesPagos;
                this._ventana.DetallePago = this.BuscarDetallePago(detallesPagos, asociado.DetallePago);

                Auditoria auditoria = new Auditoria();
                auditoria.Fk = ((Asociado)this._ventana.Asociado).Id;
                auditoria.Tabla = "FAC_ASOCIADOS";

                IList<ListaDatosDominio> tiposPersona = this._listaDatosDominioServicios.ConsultarListaDatosDominioPorParametro(new ListaDatosDominio("PERSONA"));
                this._ventana.TipoPersonas = tiposPersona;

                this._ventana.TipoPersona = BuscarTipoPersona(asociado.TipoPersona, (IList<ListaDatosDominio>)this._ventana.TipoPersonas);

                _auditorias = this._asociadoServicios.AuditoriaPorFkyTabla(auditoria);

                if (asociado.Justificaciones.Count > 0)
                    this._ventana.pintarJustificacion();
                if (asociado.Contactos.Count > 0)
                    this._ventana.pintarContacto();
                if (asociado.DatosTransferencias.Count > 0)
                    this._ventana.pintarDatosTransferencia();
                if (_auditorias.Count > 0)
                    this._ventana.pintarAuditoria();
                if (asociado.Emails.Count > 0)
                    this._ventana.pintarEmails();
                if (this._asociadoServicios.VerificarCartasPorAsociado(asociado))
                    this._ventana.pintarCorrespondencia();
                if ((asociado.Conectividad != null) && (asociado.Conectividad.Count != 0))
                    this._ventana.pintarConectividad();


                //codigo de prueba 

                if (this._ventana.ChkVerContactos.Value)
                {

                    cargarListaDeContactos(asociado);

                    /*******************************Esta es la consulta que tarda mas en el modulo de asociado*******************************/
                    //IList<ContactosDelAsociadoVista> listaContactos = this._asociadoServicios.ConsultarContactosDelAsociado(asociado, false);
                    //this._ventana.ListaContactos = listaContactos;
                    /*******************************Esta es la consulta que tarda mas en el modulo de asociado*******************************/
                }

                else
                {
                    this._ventana.DesactivarVerListaContactos();
                }

                FacGestion facGestionAuxiliar = new FacGestion();
                facGestionAuxiliar.Asociado = (Asociado)this._ventana.Asociado;
                IList<FacGestion> gestionesAsociado = this._facGestionServicios.ObtenerFacGestionesFiltro(facGestionAuxiliar);
                if (gestionesAsociado.Count > 0)
                {
                    this._ventana.pintarGestiones();
                }

                if ((!UsuarioLogeado.Rol.Id.Equals("ADMINISTRADOR")) && (!UsuarioLogeado.Rol.Id.Equals("OPR_CORRESPONDEN")) && (!UsuarioLogeado.Rol.Id.Equals("OPR_FACTURACION")))
                {
                    this._ventana.DesactivarBotonesParaModificar();
                }

                
                IList<FacVistaFacturacionCxpInterna> FacVistaFacturacionCxpInterna = null;
                FacVistaFacturacionCxpInterna FacVistaFacturacionCxpInternaaux = new FacVistaFacturacionCxpInterna();
                FacVistaFacturacionCxpInternaaux.Asociado_o = asociado;
                FacVistaFacturacionCxpInternaaux.Pagada = "NO";
                FacVistaFacturacionCxpInterna = 
                    this._facVistaFacturacionCxpInternaServicios.ObtenerFacVistaFacturacionCxpInternasFiltro(FacVistaFacturacionCxpInternaaux);

                if (FacVistaFacturacionCxpInterna.Count > 0)
                    this._ventana.PintarBotonesCxPInternacional();

                if (asociado.CartaDomicilio == 0)
                    this._ventana.CartaDomicilioDatos = String.Empty;

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


        public void cargarListaDeContactos(Asociado asociado)
        {


            Mouse.OverrideCursor = Cursors.Wait;

            
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            try
            {
                IList<ContactosDelAsociadoVista> listaContactos = this._asociadoServicios.ConsultarContactosDelAsociado(asociado, false);
                this._ventana.ListaContactos = listaContactos;
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
        /// Método que dependiendo del estado de la página, habilita los campos o 
        /// modifica los datos del usuario
        /// </summary>
        public void Modificar()
        {
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

                //Modifica los datos del Agente
                else
                {
                    Asociado asociado = (Asociado)this._ventana.Asociado;

                    asociado.Operacion = "MODIFY";
                    asociado.TipoPersona = ((ListaDatosDominio)this._ventana.TipoPersona).Id[0];
                    asociado.Pais = (Pais)this._ventana.Pais;
                    asociado.Idioma = (Idioma)this._ventana.Idioma;
                    asociado.Moneda = (Moneda)this._ventana.Moneda;
                    asociado.TipoCliente = (TipoCliente)this._ventana.TipoCliente;
                    asociado.OrigenCliente = this._ventana.OrigenCliente != null ? ((ListaDatosValores)this._ventana.OrigenCliente).Valor : null;

                    if ((Tarifa)this._ventana.Tarifa != null)
                        asociado.Tarifa = !((Tarifa)this._ventana.Tarifa).Id.Equals("NGN") ? (Tarifa)this._ventana.Tarifa : null;

                    if ((Etiqueta)this._ventana.Etiqueta != null)
                        asociado.Etiqueta = !((Etiqueta)this._ventana.Etiqueta).Id.Equals("NGN") ? (Etiqueta)this._ventana.Etiqueta : null;

                    if ((DetallePago)this._ventana.DetallePago != null)
                        asociado.DetallePago = !((DetallePago)this._ventana.DetallePago).Id.Equals("NGN") ? (DetallePago)this._ventana.DetallePago : null;

                    if (!String.IsNullOrEmpty(this._ventana.CartaDomicilioDatos))
                        asociado.CartaDomicilio = Int32.Parse(this._ventana.CartaDomicilioDatos);


                    int? exitoso = this._asociadoServicios.InsertarOModificarAsociado(asociado, UsuarioLogeado.Hash);

                    if (exitoso != null)
                    {
                        //this.Navegar(Recursos.MensajesConElUsuario.AsociadoModificado, false);
                        this._ventana.HabilitarCampos = false;
                        this._ventana.TextoBotonModificar = Recursos.Etiquetas.btnModificar;
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


        /// <summary>
        /// Método que elimina un asociado
        /// </summary>
        public void Eliminar()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (this._asociadoServicios.Eliminar((Asociado)this._ventana.Asociado, UsuarioLogeado.Hash))
                {
                    this.Navegar(Recursos.MensajesConElUsuario.AsociadoEliminado, false);
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
        /// Método que muestra la ventana de Justificaciones de un Asociado
        /// </summary>
        public void IrListaJustificaciones()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            //this.Navegar(new ListaJustificaciones(this._ventana.Asociado));
            this.Navegar(new ListaJustificaciones(this._ventana.Asociado,this._ventana));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

        }


        /// <summary>
        /// Método que muestra la ventana de Contactos de un Asociado
        /// </summary>
        public void IrListaContactos()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this.Navegar(new ListaContactos(this._ventana.Asociado, this._ventana));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que muestra la ventana de transferencia de un Asociado
        /// </summary>
        public void IrListaDatosTransferencia()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            //this.Navegar(new ListaDatosTransferencias(this._ventana.Asociado));
            this.Navegar(new ListaDatosTransferencias(this._ventana.Asociado,this._ventana));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        /// <summary>
        /// Método que muestra la ventana de Auditoría de un Asociado
        /// </summary>
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
        /// Metodo que muestra el expediente de un asociado en PDF
        /// </summary>
        public void AbrirExpediente()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["rutaAsociados"].ToString() + ((Asociado)this._ventana.Asociado).Id + ".pdf");

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Win32Exception ex)
            {
                logger.Error(ex.Message);
                this._ventana.ArchivoNoEncontrado();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }



        public void IrACorrespondencia()
        {
            Navegar(new ConsultarCartas(this._ventana, this._ventana.Asociado));
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
                ((ListView)this._ventana.ListaContactos).Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (this._ventana.CurSortCol == column && this._ventana.CurAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            this._ventana.CurSortCol = column;
            this._ventana.CurAdorner = new SortAdorner(this._ventana.CurSortCol, newDir);
            AdornerLayer.GetAdornerLayer(this._ventana.CurSortCol).Add(this._ventana.CurAdorner);
            ((ListView)this._ventana.ListaContactos).Items.SortDescriptions.Add(
                new SortDescription(field, newDir));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }


        public void ConsultarUltimaCorrespondenciaEnviada()
        {
            if ((ContactosDelAsociadoVista)this._ventana.ContactoSeleccionado != null)
            {
                if (((ContactosDelAsociadoVista)this._ventana.ContactoSeleccionado).UltimaCartaSalida != 0)
                {
                    Carta ultimaCorrespondenciaEnviada = this._cartaServicios.ObtenerCartasFiltro(
                        new Carta(((ContactosDelAsociadoVista)this._ventana.ContactoSeleccionado).UltimaCartaSalida))[0];
                    Navegar(new ConsultarCarta(ultimaCorrespondenciaEnviada, this._ventana));
                }
            }
        }


        public void ConsultarCorrespondenciaCreacion()
        {
            if ((ContactosDelAsociadoVista)this._ventana.ContactoSeleccionado != null)
            {
                if (((ContactosDelAsociadoVista)this._ventana.ContactoSeleccionado).CartaCreacion != 0)
                {
                    Carta correspondenciaCreacion = this._cartaServicios.ObtenerCartasFiltro(
                        new Carta(((ContactosDelAsociadoVista)this._ventana.ContactoSeleccionado).CartaCreacion))[0];
                    Navegar(new ConsultarCarta(correspondenciaCreacion, this._ventana));
                }
            }
        }


        public void ConsultarUltimaCorrespondenciaEntrada()
        {
            if ((ContactosDelAsociadoVista)this._ventana.ContactoSeleccionado != null)
            {
                if (((ContactosDelAsociadoVista)this._ventana.ContactoSeleccionado).UltimaCartaEntrada != 0)
                {
                    Carta ultimaCorrespondenciaEntrada = this._cartaServicios.ObtenerCartasFiltro(
                        new Carta(((ContactosDelAsociadoVista)this._ventana.ContactoSeleccionado).UltimaCartaEntrada))[0];
                    Navegar(new ConsultarCarta(ultimaCorrespondenciaEntrada, this._ventana));
                }
            }
        }


        public void ConsultarCorrespondenciaDeDomicilio()
        {
            if (!string.IsNullOrEmpty(this._ventana.CartaDomicilioDatos))
            {
                Carta correspondencia = new Carta();
                correspondencia.Id = Int32.Parse(this._ventana.CartaDomicilioDatos);
                IList<Carta> listaCorrespondencias = this._cartaServicios.ObtenerCartasFiltro(correspondencia);
                if (listaCorrespondencias.Count > 0)
                {
                    Navegar(new ConsultarCarta(listaCorrespondencias[0], this._ventana));
                }
                else
                    this._ventana.Mensaje("La Correspondencia de Domicilio no existe");
            }
        }


        public void IrWebAsociado()
        {
            if (!string.IsNullOrEmpty(((Asociado)this._ventana.Asociado).Web))
            {
        //        Match match = Regex.Match(((Asociado)this._ventana.Asociado).Web, @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$",
        //RegexOptions.IgnoreCase);
        //        if (match.Success)
        //            IrURL(((Asociado)this._ventana.Asociado).Web);
        //        else
        //            this._ventana.Mensaje("Disculpe, URL errónea");

                IrURL(((Asociado)this._ventana.Asociado).Web);
            }
        }


        public void VerEtiqueta()
        {
            if (((Etiqueta)this._ventana.Etiqueta).Id != string.Empty)
            {
                string stringAMostrar = "Español:" +
                                        Environment.NewLine +
                                        ((Etiqueta)this._ventana.Etiqueta).Descripcion1 +
                                        Environment.NewLine + "Inglés: " +
                                        Environment.NewLine +
                                        ((Etiqueta)this._ventana.Etiqueta).Descripcion2;

                ChildWindow detalle = new ChildWindow(stringAMostrar);
                detalle.ShowDialog();
            }

        }


        public void RefrescarVentanaPadre()
        {
            ((ConsultarContactosPorAsociado)_ventanaPadre).Refrescar();
        }


        public void VerEmailsAsociado()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                ((Asociado)this._ventana.Asociado).Emails = this._asociadoServicios.ConsultarEmailsDelAsociado((Asociado)this._ventana.Asociado);
                //Navegar(new ListaEmails(this._ventana.Asociado));
                Navegar(new ListaEmails(this._ventana.Asociado,this._ventana));

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


        public void VerContacto()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (this._ventana.ContactoSeleccionado != null)
                {
                    Asociado asociado = (Asociado)this._ventana.Asociado;

                    Contacto contactoAConsultar = new Contacto(((ContactosDelAsociadoVista)this._ventana.ContactoSeleccionado).Contacto);
                    contactoAConsultar.Asociado = asociado;
                    Contacto contacto = this._contactoServicios.ConsultarPorId(contactoAConsultar);
                    contacto.Asociado = asociado;
                    this.Navegar(new ConsultarContacto(contacto, this._ventana));
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

        /*public void IrVentanaImprimirEdoCuenta()
        {
            if ((Asociado)this._ventana.Asociado != null)
            {
                Asociado Asociado = ((Asociado)this._ventana.Asociado).Id != int.MinValue ? (Asociado)this._ventana.Asociado : null;
                Navegar(new EstadoCuentas("2", Asociado));

            }
        }*/


        public void IrVentanaImprimirEdoCuenta()
        {
            if ((Asociado)this._ventana.Asociado != null)
            {
                Asociado Asociado = ((Asociado)this._ventana.Asociado).Id != int.MinValue ? (Asociado)this._ventana.Asociado : null;
                Navegar(new PendientesRpt("2", Asociado));

            }
        }

        public void IrVentanaCXPINTDatos()
        {
            if ((Asociado)this._ventana.Asociado != null)
            {
                Asociado Asociado = ((Asociado)this._ventana.Asociado).Id != int.MinValue ? (Asociado)this._ventana.Asociado : null;
                Navegar(new ConsultarFacVistaFacturacionCxpInternas(Asociado));

            }
        }

        public void VerCargarListaContactosAsociados()
        {
            Asociado asociado = ((Asociado)this._ventana.Asociado);
            cargarListaDeContactos(asociado);
        }

        public void IrListaConectividad()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this.Navegar(new ListaConectividad(this._ventana.Asociado, this._ventana));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

        }

        /// <summary>
        /// Metodo que recoge el URL del archivo NDP y lo ejecuta desde el boton NDP
        /// </summary>
        public void AbrirArchivoNDP()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["RutaArchivoNDP"].ToString());

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Win32Exception ex)
            {
                logger.Error(ex.Message);
                this._ventana.ArchivoNoEncontrado(string.Format(Recursos.MensajesConElUsuario.ErrorAsociadoArchivoNDP + ": ") + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }

        public void CalcularSaldos()
        {

            Decimal saldoPendiente;

            if ((Asociado)this._ventana.Asociado != null)
            {
                Asociado Asociado = ((Asociado)this._ventana.Asociado).Id != int.MinValue ? (Asociado)this._ventana.Asociado : null;

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
                    this._ventana.SaldoPorVencerSolicitud = System.Convert.ToString(w_4);
                    this._ventana.TotalSolicitud = System.Convert.ToString(w_2 + w_4);
                    //this._ventana.MSaldoPendiente = System.Convert.ToString(msaldope);
                    saldoPendiente = System.Convert.ToDecimal(msaldope);
                    this._ventana.MSaldoPendiente = saldoPendiente.ToString("N");

                }
                else
                {
                    this._ventana.SaldoVencidoSolicitud = System.Convert.ToString(w_1);
                    this._ventana.SaldoPorVencerSolicitud = System.Convert.ToString(w_3);
                    this._ventana.TotalSolicitud = System.Convert.ToString(w_1 + w_3);
                    //this._ventana.MSaldoPendiente = System.Convert.ToString(msaldope);
                    saldoPendiente = System.Convert.ToDecimal(msaldope);
                    this._ventana.MSaldoPendiente = saldoPendiente.ToString("N");
                }
            }

        }


        public void VerGestionesDeAsociado()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Asociado asociadoAux = (Asociado)this._ventana.Asociado;
                this.Navegar(new ConsultarFacGestionesAsociado(asociadoAux));
                
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
    }
}
