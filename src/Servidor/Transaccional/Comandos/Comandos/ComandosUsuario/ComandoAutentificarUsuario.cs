﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Configuration;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.AccesoDatos.Fabrica;

namespace Trascend.Bolet.Comandos.Comandos.ComandosUsuario
{
    public class ComandoAutentificarUsuario: ComandoBase<Usuario>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Usuario _usuario;


        /// <summary>
        /// Metodo Comando que Autentifica al usuario
        /// </summary>
        /// <param name="usuario">Usuario a autentificar</param>
        public ComandoAutentificarUsuario(Usuario usuario)
        {
            this._usuario = usuario;
        }

        /// <summary>
        /// Método que ejecuta el comando
        /// </summary>
        public override void Ejecutar()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IDaoUsuario dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoUsuario();
                this.Receptor = new Receptor<Usuario>(dao.Autenticar(this._usuario)); ;

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }
    }
}
