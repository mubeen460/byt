﻿using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;
using NHibernate;
using System.Collections.Generic;
using System;
using System.Configuration;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    public class DaoFusionNHibernate : DaoBaseNHibernate<Fusion, int>, IDaoFusion
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Metodo que consulta las fusiones dado unos parametros
        /// </summary>
        /// <param name="Fusion">Fusion con parametros</param>
        /// <returns>Lista de Fusiones solicitados</returns>
        public IList<Fusion> ObtenerFusionesFiltro(Fusion fusion)
        {
            IList<Fusion> Fusiones = null;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                bool variosFiltros = false;
                string filtro = "";
                string cabecera = string.Format(Recursos.ConsultasHQL.CabeceraObtenerFusion);
                if ((null != fusion) && (fusion.Id != 0))
                {
                    filtro = string.Format(Recursos.ConsultasHQL.FiltroObtenerFusionId, fusion.Id);
                    variosFiltros = true;
                }
                if ((null != fusion.Marca) && (!fusion.Marca.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerFusionIdMarca, fusion.Marca.Id);
                    variosFiltros = true;
                }
                if (null != fusion.CadenaDeCambios)
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerFusionCadenaDeCambios, fusion.CadenaDeCambios);
                    variosFiltros = true;
                }
                //if ((null != fusion.Interesado) && (!fusion.Interesado.Id.Equals("")))
                //{
                //    if (variosFiltros)
                //        filtro += " and ";
                //    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerMarcaIdInteresado, fusion.Interesado.Id);
                //    variosFiltros = true;
                //}
                //if (!string.IsNullOrEmpty(fusion.Fichas))
                //{
                //    if (variosFiltros)
                //        filtro += " and ";
                //    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerMarcaFichas, fusion.Fichas);
                //}
                //if (!string.IsNullOrEmpty(fusion.Descripcion))
                //{
                //    if (variosFiltros)
                //        filtro += " and ";
                //    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerMarcaDescripcion, fusion.Descripcion);
                //}
                if ((null != fusion.Fecha) && (!fusion.Fecha.Equals(DateTime.MinValue)))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    string fecha = String.Format("{0:dd/MM/yyyy}", fusion.Fecha);
                    //string fecha2 = String.Format("{0:dd/MM/yy}", fusion.Fecha.Value.AddDays(1));
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerFusionFecha, fecha);
                }
                IQuery query = Session.CreateQuery(cabecera + filtro);
                //IQuery query = Session.CreateSQLQuery(cabecera + filtro);
                Fusiones = query.List<Fusion>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerFusionPorPatente + ": " + ex.Message);
            }
            finally
            {
                Session.Close();
            }
            return Fusiones;
        }

    }
}
