﻿using System;
using System.Configuration;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Windows.Input;
using NLog;
using Trascend.Bolet.Cliente.Contratos.DatosTransferencias;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.ComponentModel;
using Trascend.Bolet.Cliente.Ayuda;
using Trascend.Bolet.Cliente.Ventanas.Asociados;

namespace Trascend.Bolet.Cliente.Presentadores.DatosTransferencias
{
    class PresentadorAgregarDatosTransferencia : PresentadorBase
    {

        private IAgregarDatosTransferencia _ventana;
        private IDatosTransferenciaServicios _datosTransferenciaServicios;
        private IAsociadoServicios _asociadoServicios;
        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private object _ventanaConsultarAsociado;


        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="ventana">Página que satisface el contrato</param>
        public PresentadorAgregarDatosTransferencia(IAgregarDatosTransferencia ventana, object asociado)
        {
            try
            {
                this._ventana = ventana;
                DatosTransferencia datosTransferencia = new DatosTransferencia();
                datosTransferencia.Asociado = (Asociado)asociado;
                this._ventana.DatosTransferencia = datosTransferencia;
                this._datosTransferenciaServicios = (IDatosTransferenciaServicios)Activator.GetObject(typeof(IDatosTransferenciaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["DatosTransferenciaServicios"]);
                this._asociadoServicios = (IAsociadoServicios)Activator.GetObject(typeof(IAsociadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsociadoServicios"]);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
            }
        }

        /// <summary>
        /// Constructor predeterminado que recibe una ventana padre y la ventana anterior a esa ventana padre
        /// </summary>
        /// <param name="ventana">Ventana actual</param>
        /// <param name="asociado">Asociado a relacionar con los nuevos datos de transferencia</param>
        /// <param name="ventanaPadre">Ventana anterior a esta ventana</param>
        /// <param name="ventanaConsultarAsociado">Ventana de Consultar Asociado</param>
        public PresentadorAgregarDatosTransferencia(IAgregarDatosTransferencia ventana, object asociado, object ventanaPadre, object ventanaConsultarAsociado)
        {
            try
            {
                this._ventana = ventana;
                this._ventanaPadre = ventanaPadre;
                this._ventanaConsultarAsociado = ventanaConsultarAsociado;

                DatosTransferencia datosTransferencia = new DatosTransferencia();
                datosTransferencia.Asociado = (Asociado)asociado;
                this._ventana.DatosTransferencia = datosTransferencia;
                this._datosTransferenciaServicios = (IDatosTransferenciaServicios)Activator.GetObject(typeof(IDatosTransferenciaServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["DatosTransferenciaServicios"]);
                this._asociadoServicios = (IAsociadoServicios)Activator.GetObject(typeof(IAsociadoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["AsociadoServicios"]);
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


                this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleAgregarJustificacion,
                Recursos.Ids.AgregarJustificacion);
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
        /// Método que realiza toda la lógica para agregar los DatosTransferencia dentro de la base de datos
        /// </summary>
        public void Aceptar()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                int codigoActual, nuevoCodigo;
                DatosTransferencia datosTransferencia = (DatosTransferencia)this._ventana.DatosTransferencia;

                Asociado asociado = datosTransferencia.Asociado;
                IList<DatosTransferencia> cuentasAsociado = this._datosTransferenciaServicios.ConsultarDatosTransferenciaPorAsociado(asociado);

                if (cuentasAsociado.Count > 0)
                {
                    codigoActual = cuentasAsociado.Count;
                    nuevoCodigo = codigoActual + 1;
                    datosTransferencia.Id = nuevoCodigo;
                }
                else
                {
                    datosTransferencia.Id = 1;
                }

                bool exitoso = this._datosTransferenciaServicios.InsertarOModificar(datosTransferencia, UsuarioLogeado.Hash);
                if (exitoso)
                {
                    ((DatosTransferencia)this._ventana.DatosTransferencia).Asociado.DatosTransferencias.Add(datosTransferencia);
                    //this.Navegar(new ListaDatosTransferencias(((DatosTransferencia)this._ventana.DatosTransferencia).Asociado));
                    this.Navegar(new ListaDatosTransferencias(((DatosTransferencia)this._ventana.DatosTransferencia).Asociado, this._ventanaConsultarAsociado));
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



    }
}
