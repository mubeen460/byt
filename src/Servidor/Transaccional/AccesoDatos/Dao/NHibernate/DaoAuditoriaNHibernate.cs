﻿using System;
using System.Collections.Generic;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;
using NLog;
using NHibernate;
using System.Configuration;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    class DaoAuditoriaNHibernate : DaoBaseNHibernate<Auditoria, string>, IDaoAuditoria
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Consulta las Auditorias por cada FKyTabla
        /// </summary>
        /// <param name="auditoria">Auditoria con parametros</param>
        /// <returns>lista de auditorias</returns>
        public IList<Auditoria> AuditoriaPorFkYTabla(Auditoria auditoria)
        {
            IList<Auditoria> retorno;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IQuery query;
                if (auditoria.Tabla != "MYP_MARCAS_TER")
                    query = Session.CreateQuery(string.Format(Recursos.ConsultasHQL.ObtenerAuditoriaPorFKYTabla, auditoria.Fk, auditoria.Tabla));
                else
                    query = Session.CreateQuery(string.Format(Recursos.ConsultasHQL.ObtenerAuditoriaPorFKYTablaMarcaTer, auditoria.Fks, auditoria.Tabla, auditoria.Fk));

                retorno = query.List<Auditoria>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.ExConsultarTodosUsuariosPorUsuario);
            }
            finally
            {
                Session.Close();
            }

            return retorno;
        }
    }
}
