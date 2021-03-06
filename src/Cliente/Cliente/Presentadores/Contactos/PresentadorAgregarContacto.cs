﻿using System;
using System.Configuration;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Windows.Input;
using NLog;
using Trascend.Bolet.Cliente.Contratos.Contactos;
using Trascend.Bolet.Cliente.Ventanas.Asociados;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;
using Trascend.Bolet.Cliente.Ventanas.Cartas;

namespace Trascend.Bolet.Cliente.Presentadores.Contactos
{
    class PresentadorAgregarContacto : PresentadorBase
    {

        private IAgregarContacto _ventana;
        private IContactoServicios _contactoServicios;
        private IAsociadoServicios _asociadoServicios;
        private ICartaServicios _cartaServicios;
        private IListaDatosValoresServicios _listaDatosValoresServicios;
        private Asociado _asociado;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private bool _regresarRefresca = false;
        private object _ventanaPrevia; //Ventana anterior a la ventana ListaContactos

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="ventana">Página que satisface el contrato</param>
        public PresentadorAgregarContacto(IAgregarContacto ventana, 
                                          object asociado, 
                                          object ventanaPadre, 
                                          object ventanaPrevia,
                                          bool regresarRefresca)
        {
            try
            {
                this._ventana = ventana;
                this._ventanaPadre = ventanaPadre;
                if (ventanaPrevia != null)
                    this._ventanaPrevia = ventanaPrevia;

                this._asociado = (Asociado)asociado;
                this._ventana.Contacto = new Contacto();
                this._regresarRefresca = regresarRefresca;
                //((Contacto)this._ventana.Contacto).Carta = this._carta;
                ((Contacto)this._ventana.Contacto).Asociado = this._asociado;

                this._contactoServicios = (IContactoServicios)Activator.GetObject(typeof(IContactoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ContactoServicios"]);
                this._asociadoServicios = (IAsociadoServicios)Activator.GetObject(typeof(IAsociadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsociadoServicios"]);
                this._cartaServicios = (ICartaServicios)Activator.GetObject(typeof(ICartaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["CartaServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);
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
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Mouse.OverrideCursor = Cursors.Wait;

                IList<ListaDatosValores> departamentos = this._listaDatosValoresServicios.ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiDepartamentoDeContactos));
                ListaDatosValores primerDepartamento = new ListaDatosValores();
                primerDepartamento.Id = "NGN";
                departamentos.Insert(0, primerDepartamento);
                this._ventana.Departamentos = departamentos;

                this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleAgregarContacto,
                        Recursos.Ids.Contacto);
                this._ventana.borrarId();
                this._ventana.AsignarAsociado(this._asociado.Id, this._asociado.Nombre);
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
        /// Método que realiza toda la lógica para agregar al contacto dentro de la base de datos
        /// </summary>
        public void Aceptar()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                bool exitoso = false;
                Contacto contacto = (Contacto)this._ventana.Contacto;
                contacto.Departamento = ((ListaDatosValores)this._ventana.Departamento).Valor;
                contacto.Funcion = this.transformarFuncion(this._ventana.getFuncion);
                if (!string.IsNullOrEmpty(contacto.Email))
                {
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
                        this._ventana.mensaje(Recursos.MensajesConElUsuario.AlertaDebeAgregarUnaCorrespondencia);
                    }

                    if (exitoso)
                    {
                        this._asociado.Contactos.Insert(0, contacto);

                        if (_regresarRefresca)
                            RegresarVentanaPadreContacto();
                        else
                            //this.Navegar(new ListaContactos(this._asociado, this._ventanaPadre));
                            this.Navegar(new ListaContactos(this._asociado, this._ventanaPrevia));
                    }
                }
                else
                {
                    //this._ventana.mensaje(Recursos.MensajesConElUsuario.ErrorEmailObligatorio);
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


        private void RefrescarVentanaPadre()
        {
            ((ConsultarContactosPorAsociado)this._ventanaPadre).Refrescar();
        }

        public void RegresarVentanaPadreContacto()
        {
            if (_regresarRefresca)
                RefrescarVentanaPadre();
            this.RegresarVentanaPadre();

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
    }
}
