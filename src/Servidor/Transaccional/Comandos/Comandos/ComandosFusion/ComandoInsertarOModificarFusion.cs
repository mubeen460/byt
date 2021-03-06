﻿using System;
using System.Configuration;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.AccesoDatos.Fabrica;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Comandos.Comandos.ComandosFusion
{
    public class ComandoInsertarOModificarFusion : ComandoBase<bool>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        Fusion _fusion;

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="fusion">fusion a insertar o modificar</param>
        public ComandoInsertarOModificarFusion(Fusion fusion)
        {
            this._fusion = fusion;
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

                IDaoFusion dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoFusion();
                this.Receptor = new Receptor<bool>(dao.InsertarOModificar(this._fusion));

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
