﻿using System;
using System.Collections.Generic;
using System.Configuration;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.AccesoDatos.Fabrica;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Comandos.Comandos.ComandosPoder
{
    public class ComandoConsultarPoderesPorInteresado : ComandoBase<IList<Poder>>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Interesado _interesado;

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="interesado">Interesado para el filtrado</param>
        public ComandoConsultarPoderesPorInteresado(Interesado interesado)
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
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IDaoPoder dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoPoder();
                this.Receptor = new Receptor<IList<Poder>>(dao.ObtenerPoderesPorInteresado(this._interesado));

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
