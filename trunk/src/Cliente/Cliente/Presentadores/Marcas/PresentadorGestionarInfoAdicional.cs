﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Windows.Input;
using NLog;
using Trascend.Bolet.Cliente.Contratos.Marcas;
using Trascend.Bolet.Cliente.Ventanas.Auditorias;
using Trascend.Bolet.Cliente.Ventanas.Principales;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.Cliente.Ventanas.Marcas;

namespace Trascend.Bolet.Cliente.Presentadores.Marcas
{
    class PresentadorGestionarInfoAdicional : PresentadorBase
    {
        private IGestionarInfoAdicional _ventana;
        private Marca _marca;
        private static PaginaPrincipal _paginaPrincipal = PaginaPrincipal.ObtenerInstancia;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IInfoAdicionalServicios _infoAdicionalServicios;
        private IList<Auditoria> _auditorias;
        private bool _nuevaInfoAdicional = false;

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="ventana">Página que satisface el contrato</param>
        public PresentadorGestionarInfoAdicional(IGestionarInfoAdicional ventana, object marca)
        {
            try
            {
                this._ventana = ventana;
                this._marca = (Marca)marca;
                this._ventana.InfoAdicional = null != ((Marca)marca).InfoAdicional ? ((Marca)marca).InfoAdicional : new InfoAdicional("M." + this._marca.Id);

                if (null == ((Marca)marca).InfoAdicional)
                    this._nuevaInfoAdicional = true;
      
                 
                this._infoAdicionalServicios = (IInfoAdicionalServicios)Activator.GetObject(typeof(IInfoAdicionalServicios),
                    ConfigurationManager.AppSettings["RutaServidor"] + ConfigurationManager.AppSettings["InfoAdicionalServicios"]);
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
                //cambiar titulo
                this.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.titleAgregarAgente,
                    Recursos.Ids.AgregarAgente);

                if (this._nuevaInfoAdicional)
                {
                    this._ventana.HabilitarCampos = true;
                    this._ventana.TextoBotonModificar = Recursos.Etiquetas.btnAceptar;
                    this._ventana.OculatarControlesAlAgregar();
                }

                Auditoria auditoria = new Auditoria();
                int id = int.Parse(((InfoAdicional)this._ventana.InfoAdicional).Id.Substring(2));
                auditoria.Fk = id;
                auditoria.Tabla = "MYP_ADICIONAL";

                _auditorias = this._infoAdicionalServicios.AuditoriaPorFkyTabla(auditoria);

                if (_auditorias.Count > 0)
                    this._ventana.PintarAuditoria();

                this._ventana.FocoPredeterminado();
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
        /// Método que realiza toda la lógica para agregar al Usuario dentro de la base de datos
        /// </summary>
        public bool Aceptar()
        {
            bool exitoso = false;
            try
            {

                //Habilitar campos
                if (this._ventana.TextoBotonModificar == Recursos.Etiquetas.btnModificar)
                {
                    this._ventana.HabilitarCampos = true;
                    this._ventana.TextoBotonModificar = Recursos.Etiquetas.btnAceptar;
                }

                //Agregar o modificar datos
                else
                {
                    InfoAdicional infoAdicional = (InfoAdicional)this._ventana.InfoAdicional;
                    
                    infoAdicional.Operacion = this._nuevaInfoAdicional ? "CREATE" : "MODIFY";

                    this._marca.InfoAdicional = infoAdicional;

                    exitoso = this._infoAdicionalServicios.InsertarOModificar(infoAdicional, UsuarioLogeado.Hash);

                }
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

            return exitoso;
        }

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
        /// Método que invoca una nueva página "ConsultarMarca" y la instancia con el objeto seleccionado
        /// </summary>
        public void IrConsultarMarca()
        {
            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            this.Navegar(new ConsultarMarca(this._marca, this._ventana.Tab));

            #region trace
            if (ConfigurationManager.AppSettings["ambiente"].ToString().Equals("desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion
        }
    }
}
