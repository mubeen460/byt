﻿using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;
using NHibernate;
using System.Collections.Generic;
using System;
using System.Configuration;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    class DaoRenovacionNHibernate : DaoBaseNHibernate<Renovacion, int>, IDaoRenovacion
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public IList<Renovacion> ObtenerRenovacionesFiltro(Renovacion renovacion)
        {
            IList<Renovacion> renovaciones = null;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                bool variosFiltros = false;
                string filtro = "";
                string cabecera = string.Format(Recursos.ConsultasHQL.CabeceraObtenerRenovacion);

                if ((null != renovacion) && (renovacion.Id != 0))
                {
                    filtro = string.Format(Recursos.ConsultasHQL.FiltroObtenerRenovacionId, renovacion.Id);
                    variosFiltros = true;
                }
                if ((null != renovacion.Marca) && (!renovacion.Marca.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerRenovacionIdMarca, renovacion.Marca.Id);
                    variosFiltros = true;
                }

                if ((null != renovacion.Fecha) && (!renovacion.Fecha.Equals(DateTime.MinValue)))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    string fecha = String.Format("{0:dd/MM/yy}", renovacion.Fecha);
                    string fecha2 = String.Format("{0:dd/MM/yy}", renovacion.Fecha.Value.AddDays(1));
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerRenovacionFecha, fecha, fecha2);
                }
                IQuery query = Session.CreateQuery(cabecera + filtro);

                renovaciones = query.List<Renovacion>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.ExConsultarTodos);
            }
            finally
            {
                Session.Close();
            }

            return renovaciones;
        }
    }
}