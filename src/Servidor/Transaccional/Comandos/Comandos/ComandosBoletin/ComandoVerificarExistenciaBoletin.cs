﻿using System;
using System.Configuration;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.AccesoDatos.Fabrica;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Comandos.Comandos.ComandosBoletin
{
    public class ComandoVerificarExistenciaBoletin : ComandoBase<bool>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Boletin _boletin;

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="boletin">Boletin a verificar</param>
        public ComandoVerificarExistenciaBoletin(Boletin boletin)
        {
            this._boletin = boletin;
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
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IDaoBoletin dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoBoletin();
                this.Receptor = new Receptor<bool>(dao.VerificarExistencia(this._boletin.Id));

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
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