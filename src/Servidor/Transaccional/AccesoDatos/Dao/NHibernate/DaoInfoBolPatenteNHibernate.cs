﻿using System;
using System.Configuration;
using NHibernate;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    public class DaoInfoBolPatenteNHibernate : DaoBaseNHibernate<InfoBolPatente, int>, IDaoInfoBolPatente
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Metodo que consulta todos los InfoBoles por patente
        /// </summary>
        /// <param name="marca">Patente</param>
        /// <returns>Lista de Infoboles de la patente solicitada</returns>
        public IList<InfoBolPatente> ObtenerInfoBolesPorPatente(Patente patente)
        {
            IList<InfoBolPatente> InfoBoles;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IQuery query = Session.CreateQuery(string.Format(Recursos.ConsultasHQL.ObtenerInfoBolesPorPatente, patente.Id));
                InfoBoles = query.List<InfoBolPatente>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerInfobolesPorPatente);
            }
            finally
            {
                Session.Close();
            }

            return InfoBoles;
        }
    }
}
