﻿using System;
using System.Configuration;
using NHibernate;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    class DaoInstruccionEnvioOriginalesNHibernate : DaoBaseNHibernate<InstruccionEnvioOriginales,int>, IDaoInstruccionEnvioOriginales
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Metodo que obtiene una instruccion de envio de originales de una marca o de una patente segun su aplicacion, el codigo 
        /// de la marca o de la patente y el concepto utilizado para esta instruccion
        /// </summary>
        /// <param name="instruccion">Instruccion de Envio de Originales a buscar</param>
        /// <returns>Retorna una instruccion de Envio de Originales; en caso contrario retorna NULL</returns>
        public InstruccionEnvioOriginales ObtenerInstruccionDeEnvioDeOriginales(InstruccionEnvioOriginales instruccion)
        {
            InstruccionEnvioOriginales retorno;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IQuery query = Session.CreateQuery(string.Format(Recursos.ConsultasHQL.ObtenerInstruccionDeEnvioOriginalesMarcaOPatente, instruccion.Id, instruccion.AplicadaA, instruccion.Concepto));
                retorno = query.UniqueResult<InstruccionEnvioOriginales>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerArchivoPorId);
            }
            finally
            {
                Session.Close();
            }

            return retorno;
        }

    }
}
