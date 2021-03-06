﻿using System;
using System.Collections.Generic;
using System.Configuration;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.AccesoDatos.Fabrica;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Comandos.Comandos.ComandosInteresadoMultiple
{
    class ComandoConsultarInteresadosMultiplesDeMarca : ComandoBase<IList<InteresadoMultiple>>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Marca _marca;

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        /// <param name="marca"></param>
        public ComandoConsultarInteresadosMultiplesDeMarca(Marca marca)
        {
            this._marca = marca;
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

                IDaoInteresadoMultiple dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoInteresadoMultiple();
                this.Receptor = new Receptor<IList<InteresadoMultiple>>(dao.ObtenerInteresadosPorMarca(this._marca));

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
