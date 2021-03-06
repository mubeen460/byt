﻿using System;
using System.Collections.Generic;
using System.Configuration;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.AccesoDatos.Fabrica;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Comandos.Comandos.ComandosInteresado
{
    class ComandoConsultarInteresadoConTodo : ComandoBase<Interesado>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Interesado _interesado;

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="interesado">Interesado a consultar</param>
        public ComandoConsultarInteresadoConTodo(Interesado interesado)
        {
            this._interesado = interesado;
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

                IDaoInteresado dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoInteresado();
                this.Receptor = new Receptor<Interesado>(dao.ObtenerInteresadoConTodo(this._interesado));

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
