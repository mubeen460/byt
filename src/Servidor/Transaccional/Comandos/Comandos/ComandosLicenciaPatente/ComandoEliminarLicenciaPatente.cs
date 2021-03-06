﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Configuration;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.AccesoDatos.Fabrica;

namespace Trascend.Bolet.Comandos.Comandos.ComandosLicenciaPatente
{
    public class ComandoEliminarLicenciaPatente : ComandoBase<bool>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private LicenciaPatente _licencia;

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="licencia">LicenciaPatente a eliminar</param>
        public ComandoEliminarLicenciaPatente(LicenciaPatente licencia)
        {
            this._licencia = licencia;
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

                IDaoLicenciaPatente dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoLicenciaPatente();
                 this.Receptor = new Receptor<bool>(dao.Eliminar(this._licencia));

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