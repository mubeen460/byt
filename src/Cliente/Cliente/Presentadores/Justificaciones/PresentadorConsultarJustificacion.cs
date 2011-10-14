﻿using System;
using System.Configuration;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Windows.Input;
using NLog;
using Trascend.Bolet.Cliente.Contratos.Justificaciones;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;
using Trascend.Bolet.Cliente.Ventanas.Auditorias;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Documents;
using Trascend.Bolet.Cliente.Ayuda;
using System.Linq;
using Trascend.Bolet.Cliente.Ventanas.Poderes;

namespace Trascend.Bolet.Cliente.Presentadores.Justificaciones
{
    class PresentadorConsultarJustificacion : PresentadorBase
    {
        private IConsultarJustificacion _ventana;
        private IConceptoServicios _conceptoServicios;
        private IJustificacionServicios _justificacionServicios;
        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="ventana">Página que satisface el contrato</param>
        public PresentadorConsultarJustificacion(IConsultarJustificacion ventana, object justificacion)
        {
            try
            {
                this._ventana = ventana;
                this._ventana.Justificacion = justificacion;
                this._justificacionServicios = (IJustificacionServicios)Activator.GetObject(typeof(IJustificacionServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["JustificacionServicios"]);
                this._conceptoServicios = (IConceptoServicios)Activator.GetObject(typeof(IConceptoServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["ConceptoServicios"]);
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

                this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleConsultarJustificacion,
                    Recursos.Ids.ConsultarJustificaciones);
                IList<Concepto> conceptos = this._conceptoServicios.ConsultarTodos();
                this._ventana.Conceptos = conceptos;
                this._ventana.Concepto = this.BuscarConcepto(conceptos,((Justificacion)this._ventana.Justificacion).Concepto);
                this._ventana.FocoPredeterminado();

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
            finally
            {
                Mouse.OverrideCursor = null;
            }
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

                //Modifica los datos del Poder
                else
                {
                    Justificacion justificacion = (Justificacion)this._ventana.Justificacion;
                    justificacion.Concepto = (Concepto)this._ventana.Concepto;
                    bool exitoso = this._justificacionServicios.InsertarOModificar(justificacion, UsuarioLogeado.Hash);
                    if (exitoso)
                    {
                        _paginaPrincipal.MensajeUsuario = Recursos.MensajesConElUsuario.JustificacionModificado;
                        this.Navegar(_paginaPrincipal);
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

        //public void Eliminar()
        //{
        //    try
        //    {
        //        #region trace
        //        if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
        //            logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
        //        #endregion
        //        Poder poder = (Poder)this._ventana.Poder;

        //        poder.Boletin = (Boletin)this._ventana.Boletin;
        //        poder.Operacion = "DELETE";

        //        bool exitoso = this._justificacionServicios.Eliminar(poder, UsuarioLogeado.Hash);
        //        if (exitoso)
        //        {
        //            _paginaPrincipal.MensajeUsuario = Recursos.MensajesConElUsuario.PoderEliminado;
        //            this.Navegar(_paginaPrincipal);
        //        }

        //        #region trace
        //        if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
        //            logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
        //        #endregion
        //    }
        //    catch (ApplicationException ex)
        //    {
        //        logger.Error(ex.Message);
        //        this.Navegar(ex.Message, true);
        //    }
        //    catch (RemotingException ex)
        //    {
        //        logger.Error(ex.Message);
        //        this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
        //    }
        //    catch (SocketException ex)
        //    {
        //        logger.Error(ex.Message);
        //        this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
        //    }
        //}

        //public void Auditoria()
        //{
        //    try
        //    {
        //        #region trace
        //        if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
        //            logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
        //        #endregion

        //        Auditoria auditoria = new Auditoria();
        //        auditoria.Fk = ((Poder)this._ventana.Poder).Id;
        //        auditoria.Tabla = "MYP_PODERES";

        //        IList<Auditoria> auditorias = this._justificacionServicios.AuditoriaPorFkyTabla(auditoria);
        //        _paginaPrincipal.MensajeUsuario = Recursos.MensajesConElUsuario.PoderEliminado;
        //        this.Navegar(new ListaAuditorias(auditorias));


        //        #region trace
        //        if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
        //            logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
        //        #endregion
        //    }
        //    catch (ApplicationException ex)
        //    {
        //        logger.Error(ex.Message);
        //        this.Navegar(ex.Message, true);
        //    }
        //    catch (RemotingException ex)
        //    {
        //        logger.Error(ex.Message);
        //        this.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, true);
        //    }
        //    catch (SocketException ex)
        //    {
        //        logger.Error(ex.Message);
        //        this.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
        //    }
        //}

        //public void AbrirPoder()
        //{
        //    try
        //    {
        //        #region trace
        //        if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
        //            logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
        //        #endregion

        //        System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["rutaPoderes"].ToString()+((Poder)this._ventana.Poder).Id+".pdf");

        //        #region trace
        //        if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
        //            logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
        //        #endregion
        //    }
        //    catch (Win32Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        this._ventana.ArchivoNoEncontrado();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        this.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, true);
        //    }
        //}


        ///// <summary>
        ///// Método que ordena una columna
        ///// </summary>
        //public void OrdenarColumna(GridViewColumnHeader column)
        //{
        //    #region trace
        //    if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
        //        logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
        //    #endregion

        //    String field = column.Tag as String;

        //    if (this._ventana.CurSortCol != null)
        //    {
        //        AdornerLayer.GetAdornerLayer(this._ventana.CurSortCol).Remove(this._ventana.CurAdorner);
        //        this._ventana.ListaResultados.Items.SortDescriptions.Clear();
        //    }

        //    ListSortDirection newDir = ListSortDirection.Ascending;
        //    if (this._ventana.CurSortCol == column && this._ventana.CurAdorner.Direction == newDir)
        //        newDir = ListSortDirection.Descending;

        //    this._ventana.CurSortCol = column;
        //    this._ventana.CurAdorner = new SortAdorner(this._ventana.CurSortCol, newDir);
        //    AdornerLayer.GetAdornerLayer(this._ventana.CurSortCol).Add(this._ventana.CurAdorner);
        //    this._ventana.ListaResultados.Items.SortDescriptions.Add(
        //        new SortDescription(field, newDir));

        //    #region trace
        //    if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
        //        logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
        //    #endregion
        //}
    }
}
