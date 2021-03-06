﻿using System;
using System.Configuration;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Windows.Input;
using NLog;
using Trascend.Bolet.Cliente.Contratos.Contactos;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.Cliente.Ventanas.Asociados;
using System.Collections.Generic;
using Trascend.Bolet.Cliente.Ventanas.Cartas;
using Trascend.Bolet.Cliente.Ventanas.Auditorias;

namespace Trascend.Bolet.Cliente.Presentadores.Contactos
{
    class PresentadorConsultarContacto : PresentadorBase
    {

        private IConsultarContacto _ventana;
        private IAsociadoServicios _asociadoServicios;
        private ICartaServicios _cartaServicios;
        private IContactoServicios _contactoServicios;
        private IListaDatosValoresServicios _listaDatosValoresServicios;
        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Contacto _contacto;
        private IList<Auditoria> _auditorias;
        private object _ventanaPadrePrevia; //Ventana inmediatamente anterior a la ventana que llamo a esta ventana
        private object _carta;
        private object _listaCartas;
        private int _posicionCarta;
        private bool   _vieneDeConsultarCarta;

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="ventana">Página que satisface el contrato</param>
        /// <param name="contacto">Contacto a mostrar</param>
        public PresentadorConsultarContacto(IConsultarContacto ventana,
                                            object contacto,
                                            object ventanaPadre,
                                            object ventanaPadrePrevia)
        {
            try
            {
                this._ventana = ventana;
                this._ventana.Contacto = contacto;
                this._ventanaPadre = ventanaPadre;
                if (ventanaPadrePrevia != null)
                    this._ventanaPadrePrevia = ventanaPadrePrevia;

                this._contacto = (Contacto)contacto;
                this._contactoServicios = (IContactoServicios)Activator.GetObject(typeof(IContactoServicios),
                     ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ContactoServicios"]);
                this._asociadoServicios = (IAsociadoServicios)Activator.GetObject(typeof(IAsociadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsociadoServicios"]);
                this._cartaServicios = (ICartaServicios)Activator.GetObject(typeof(ICartaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CartaServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);

                ListaDatosValores valorBuscado = new ListaDatosValores();
                valorBuscado.Valor = ((Contacto)this._ventana.Contacto).Departamento;
                this._ventana.Departamento = this.BuscarDepartamentoContacto(this._listaDatosValoresServicios.ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiDepartamentoDeContactos)), valorBuscado);
                this._ventana.setFuncion = this.BuscarFuncionContacto(((Contacto)this._ventana.Contacto).Funcion);
                this._ventana.setCorrespondencia = ((Contacto)this._ventana.Contacto).Carta == null ? "" : ((Contacto)this._ventana.Contacto).Carta.Id.ToString();


            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }


        
        public PresentadorConsultarContacto(IConsultarContacto ventana,
                                            object contacto,
                                            bool vieneDeConsultarCarta,
                                            object carta,
                                            object listaCartas,
                                            int posicion,
                                            object ventanaPadre,
                                            object ventanaPadrePrevia)
        {
            try
            {
                this._ventana = ventana;
                this._ventana.Contacto = contacto;
                this._ventanaPadre = ventanaPadre;
                if (ventanaPadrePrevia != null)
                    this._ventanaPadrePrevia = ventanaPadrePrevia;

                this._carta = carta;

                if (listaCartas != null)
                {
                    this._listaCartas = listaCartas;
                    this._posicionCarta = posicion;
                }

                this._vieneDeConsultarCarta = vieneDeConsultarCarta;


                this._contacto = (Contacto)contacto;
                this._contactoServicios = (IContactoServicios)Activator.GetObject(typeof(IContactoServicios),
                     ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ContactoServicios"]);
                this._asociadoServicios = (IAsociadoServicios)Activator.GetObject(typeof(IAsociadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsociadoServicios"]);
                this._cartaServicios = (ICartaServicios)Activator.GetObject(typeof(ICartaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CartaServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);

                ListaDatosValores valorBuscado = new ListaDatosValores();
                valorBuscado.Valor = ((Contacto)this._ventana.Contacto).Departamento;
                this._ventana.Departamento = this.BuscarDepartamentoContacto(this._listaDatosValoresServicios.ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiDepartamentoDeContactos)), valorBuscado);
                this._ventana.setFuncion = this.BuscarFuncionContacto(((Contacto)this._ventana.Contacto).Funcion);
                this._ventana.setCorrespondencia = ((Contacto)this._ventana.Contacto).Carta == null ? "" : ((Contacto)this._ventana.Contacto).Carta.Id.ToString();


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


                IList<ListaDatosValores> departamentos = this._listaDatosValoresServicios.ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiDepartamentoDeContactos));
                ListaDatosValores primerDepartamento = new ListaDatosValores();
                primerDepartamento.Id = "NGN";
                departamentos.Insert(0, primerDepartamento);
                this._ventana.Departamentos = departamentos;
                //--
                if (!((Contacto)this._ventana.Contacto).Departamento.Equals(""))
                {
                    ListaDatosValores departamentoBuscado = new ListaDatosValores();
                    departamentoBuscado.Id = Recursos.Etiquetas.cbiDepartamentoDeContactos;
                    departamentoBuscado.Valor = ((Contacto)this._ventana.Contacto).Departamento;
                    this._ventana.Departamento = BuscarDepartamentoContacto(departamentos,departamentoBuscado);
                }
                //--

                this._ventana.AsignarAsociado(this._contacto.Asociado.Id, this._contacto.Asociado.Nombre);
                this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleConsultarContacto,
                    Recursos.Ids.ConsultarContacto);

                Auditoria auditoria = new Auditoria();
                auditoria.Fk = ((Contacto)this._ventana.Contacto).Id;
                auditoria.Tabla = "FAC_ASOCIADOS";
                IList<Auditoria> auditorias = this._contactoServicios.AuditoriaPorFkyTabla(auditoria);
                if (auditorias.Count > 0)
                {
                    this._ventana.PintarAuditoria();
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


        /// <summary>
        /// Método que dependiendo del estado de la página, habilita los campos o 
        /// modifica los datos del contacto
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
                //Modifica los datos del Contacto
                else
                {
                    bool exitoso = false;
                    Contacto contacto = (Contacto)this._ventana.Contacto;
                    contacto.Departamento = ((ListaDatosValores)this._ventana.Departamento).Valor;
                    contacto.Funcion = this.transformarFuncion(this._ventana.getFuncion);

                    if (!string.IsNullOrEmpty(this._ventana.getCorrespondencia))
                    {
                        Carta carta = new Carta();
                        carta.Id = int.Parse(this._ventana.getCorrespondencia);

                        if (this._cartaServicios.VerificarExistencia(carta))
                        {
                            contacto.Carta = carta;
                            exitoso = this._contactoServicios.InsertarOModificar(contacto, UsuarioLogeado.Hash);
                        }
                        else
                        {
                            this._ventana.mensaje(Recursos.MensajesConElUsuario.ErrorCorrespondenciaNoEncontrada);
                        }
                    }
                    else
                    {
                        contacto.Carta = null;
                        exitoso = this._contactoServicios.InsertarOModificar(contacto, UsuarioLogeado.Hash);
                    }
                    if (exitoso)
                        if (this._ventanaPadre == null)
                            this.Navegar(new ListaContactos(((Contacto)this._ventana.Contacto).Asociado, null));
                        else
                            RegresarVentanaPadre();
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
        /// Método que se activa al presionar el boton de eliminar al contacto. Es el encargado de eliminar el
        /// contacto
        /// </summary>
        public void Eliminar()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                if (this._contactoServicios.Eliminar((Contacto)this._ventana.Contacto, UsuarioLogeado.Hash))
                {
                    Asociado asociado = ((Contacto)this._ventana.Contacto).Asociado;
                    asociado.Contactos.Remove((Contacto)this._ventana.Contacto);

                    if ((this._ventanaPadrePrevia != null) && (!this._vieneDeConsultarCarta))
                        this.Navegar(new ListaContactos(asociado, this._ventanaPadrePrevia));
                    else if ((this._ventanaPadrePrevia != null) && (this._vieneDeConsultarCarta))
                    {
                        ((Carta)this._carta).Persona = null;
                        if (this._listaCartas != null)
                        {
                            IList<Carta> cartasObtenidas = (IList<Carta>)this._listaCartas;
                            this.Navegar(new ConsultarCarta((Carta)this._carta, cartasObtenidas, this._posicionCarta, this._ventanaPadrePrevia, true));
                        }
                        else
                        {
                            this.Navegar(new ConsultarCarta((Carta)this._carta, this._ventanaPadrePrevia));
                        }
                    }
                    else
                        this.Navegar(new ListaContactos(asociado, this._ventanaPadre));
                    
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

        public void ConsultarCarta()
        {
            if (!this._ventana.getCorrespondencia.Equals(string.Empty))
            {
                Carta carta = new Carta(int.Parse(this._ventana.getCorrespondencia));
                if (this._cartaServicios.VerificarExistencia(carta))
                {
                    this.Navegar(new ConsultarCarta(this._cartaServicios.ObtenerCartasFiltro(carta)[0], this._ventana));
                }
                else
                    this._ventana.mensaje(Recursos.MensajesConElUsuario.ErrorCorrespondenciaNoEncontrada);
            }
        }


        /// <summary>
        /// Metodo que muestra la auditoria para un Contacto consultado
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
                auditoria.Fk = ((Contacto)this._ventana.Contacto).Id;
                auditoria.Tabla = "FAC_ASOCIADOS";
                this._auditorias = this._contactoServicios.AuditoriaPorFkyTabla(auditoria);
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
    }
}
