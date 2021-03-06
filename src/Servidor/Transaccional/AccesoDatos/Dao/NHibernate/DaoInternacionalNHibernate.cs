﻿using NLog;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.AccesoDatos.Contrato;
using NHibernate;
using System;
using System.Configuration;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    class DaoInternacionalNHibernate : DaoBaseNHibernate<Internacional, int>, IDaoInternacional
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Metodo con el que se obtiene el objeto Internacional
        /// </summary>
        /// <param name="id">entero con el que se busca el objeto</param>
        /// <returns>Objeto Internacional</returns>
        public Internacional ObtenerPorId(int id)
        {
            Internacional retorno;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IQuery query = Session.CreateQuery(string.Format(Recursos.ConsultasHQL.ObtenerInternacionalPorId, id));
                Internacional internacionalAux = query.UniqueResult<Internacional>();
                retorno = internacionalAux;

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerInternacionalPorId);
            }
            finally
            {
                Session.Close();
            }

            return retorno;
        }
    }
}
