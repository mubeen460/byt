﻿using System;
using System.Configuration;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Windows.Input;
using NLog;
using Trascend.Bolet.Cliente.Contratos.Interesados;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;
using Trascend.Bolet.Cliente.Ventanas.Interesados;

namespace Trascend.Bolet.Cliente.Presentadores.Interesados
{
    class PresentadorAgregarInteresado : PresentadorBase
    {
        private IAgregarInteresado _ventana;
        private IPaisServicios _paisServicios;
        private IEstadoServicios _estadoServicios;
        private IInteresadoServicios _interesadoServicios;
        private IListaDatosDominioServicios _listaDatosDominioServicios;
        private IListaDatosValoresServicios _listaDatosValoresServicios;
        private IIdiomaServicios _idiomaServicios;
        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="ventana">Página que satisface el contrato</param>
        public PresentadorAgregarInteresado(IAgregarInteresado ventana)
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                this._ventana = ventana;
                this._ventana.Interesado = new Interesado();
                this._interesadoServicios = (IInteresadoServicios)Activator.GetObject(typeof(IInteresadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InteresadoServicios"]);
                this._paisServicios = (IPaisServicios)Activator.GetObject(typeof(IPaisServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["PaisServicios"]);
                this._estadoServicios = (IEstadoServicios)Activator.GetObject(typeof(IEstadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["EstadoServicios"]);
                this._listaDatosDominioServicios = (IListaDatosDominioServicios)Activator.GetObject(typeof(IListaDatosDominioServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosDominioServicios"]);
                this._listaDatosValoresServicios = (IListaDatosValoresServicios)Activator.GetObject(typeof(IListaDatosValoresServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ListaDatosValoresServicios"]);
                this._idiomaServicios = (IIdiomaServicios)Activator.GetObject(typeof(IIdiomaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["IdiomaServicios"]);

                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado,true);
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

                this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleAgregarInteresado,
                    Recursos.Ids.AgregarInteresado);
                this._ventana.FocoPredeterminado();

                Interesado interesado = (Interesado)this._ventana.Interesado;

                IList<Pais> paises = this._paisServicios.ConsultarTodos();
                IList<Estado> estados = this._estadoServicios.ConsultarTodos();
                this._ventana.Paises = paises;
                IList<ListaDatosDominio> tiposPersona = this._listaDatosDominioServicios.ConsultarListaDatosDominioPorParametro(new ListaDatosDominio("PERSONA"));
                this._ventana.TipoPersonas = tiposPersona;
                this._ventana.Nacionalidades = paises;
                this._ventana.Corporaciones = estados;

                IList<ListaDatosValores> origenesClientes = 
                    this._listaDatosValoresServicios.ConsultarListaDatosValoresPorParametro(new ListaDatosValores(Recursos.Etiquetas.cbiOrigenClienteAsociado));
                this._ventana.OrigenesClientes = origenesClientes;
                ListaDatosValores origenClienteAux = new ListaDatosValores();
                origenClienteAux.Valor = "BOLET";
                this._ventana.OrigenCliente = this.BuscarListaDeDatosValores(origenesClientes, origenClienteAux);

                IList<Idioma> idiomas = this._idiomaServicios.ConsultarTodos();
                Idioma primerIdioma = new Idioma();
                primerIdioma.Id = "NGN";
                idiomas.Insert(0, primerIdioma);
                this._ventana.Idiomas = idiomas;

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
        /// Método que realiza toda la lógica para agregar al Interesado dentro de la base de datos
        /// </summary>
        public void Aceptar()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                Interesado interesado = (Interesado)this._ventana.Interesado;

                interesado.Pais = (Pais)this._ventana.Pais;
                interesado.Nacionalidad = (Pais)this._ventana.Nacionalidad;
                interesado.Corporacion = (Estado)this._ventana.Corporacion;
                interesado.TipoPersona = ((ListaDatosDominio)this._ventana.TipoPersona).Id[0];
                interesado.OrigenCliente = ((ListaDatosValores)this._ventana.OrigenCliente).Valor;
                interesado.Idioma = !((Idioma)this._ventana.Idioma).Id.Equals("NGN") ? (Idioma)this._ventana.Idioma : null;
                interesado.Operacion = "CREATE";

                //if ((interesado.Estado != null) && (!interesado.Estado.Equals("")))
                //{
                    if ((interesado.Pais != null) && (interesado.Pais.Id != 0))
                    {
                        //if ((interesado.Nacionalidad != null) && (!interesado.Nacionalidad.Equals("")))
                        //{
                            int? exitoso = this._interesadoServicios.InsertarOModificarInteresado(interesado, UsuarioLogeado.Hash);

                            if (null != exitoso)
                            {
                                interesado.Id = exitoso.Value;
                                this.Navegar(new ConsultarInteresado(interesado, null));
                            }
                            else
                                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
                        //}
                        //else
                        //{
                        //    this._ventana.Mensaje(Recursos.MensajesValidaciones.InteresadoNacionalidad, 1);
                        //}
                    }
                    else
                    {
                        this._ventana.Mensaje(Recursos.MensajesValidaciones.InteresadoPais, 1);
                    }
                //}
                //else
                //{
                //    this._ventana.Mensaje(Recursos.MensajesValidaciones.InteresadoEstado, 1);
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
    }
}
