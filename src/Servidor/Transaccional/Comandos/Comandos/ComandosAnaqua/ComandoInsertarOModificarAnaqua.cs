﻿using System;
using System.Configuration;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.AccesoDatos.Fabrica;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Comandos.Comandos.ComandosAnaqua
{
    public class ComandoInsertarOModificarAnaqua : ComandoBase<bool>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        Anaqua _anaqua;

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="anaqua">anaqua a insertar o modificar</param>
        public ComandoInsertarOModificarAnaqua(Anaqua anaqua)
        {
            this._anaqua = anaqua;
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

                IDaoAnaqua dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoAnaqua();
                this.Receptor = new Receptor<bool>(dao.InsertarOModificar(this._anaqua));

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
