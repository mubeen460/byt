﻿using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;
using NHibernate;
using System.Collections.Generic;
using System;
using System.Configuration;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    public class DaoCambioPeticionarioNHibernate : DaoBaseNHibernate<CambioPeticionario, int>, IDaoCambioPeticionario
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Metodo que Consulta los CambiosDePeticionarios dado unos parametros
        /// </summary>
        /// <param name="CambioPeticionario">CambioDePeticionario con parametros</param>
        /// <returns>Lista de CambioDePeticionarios</returns>
        public IList<CambioPeticionario> ObtenerCambiosPeticionarioFiltro(CambioPeticionario cambioPeticionario)
        {
            IList<CambioPeticionario> CambioPeticionarios = null;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                bool variosFiltros = false;
                string filtro = "";
                string cabecera = string.Format(Recursos.ConsultasHQL.CabeceraObtenerCambioPeticionario);
                if ((null != cambioPeticionario) && (cambioPeticionario.Id != 0))
                {
                    filtro = string.Format(Recursos.ConsultasHQL.FiltroObtenerCambioPeticionarioId, cambioPeticionario.Id);
                    variosFiltros = true;
                }
                if ((null != cambioPeticionario.Marca) && (!cambioPeticionario.Marca.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerCambioPeticionarioIdMarca, cambioPeticionario.Marca.Id);
                    variosFiltros = true;
                }
                if (null != cambioPeticionario.CadenaDeCambios)
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerCambioPeticionarioCadenaDeCambios, cambioPeticionario.CadenaDeCambios);
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
                if ((null != cambioPeticionario.FechaPeticionario) && (!cambioPeticionario.FechaPeticionario.Equals(DateTime.MinValue)))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    string fecha = String.Format("{0:dd/MM/yyyy}", cambioPeticionario.FechaPeticionario);
                    //string fecha2 = String.Format("{0:dd/MM/yy}", cambioPeticionario.FechaPeticionario.Value.AddDays(1));
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerCambioPeticionarioFecha, fecha);
                }
                IQuery query = Session.CreateQuery(cabecera + filtro);
                CambioPeticionarios = query.List<CambioPeticionario>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerCambioPeticionarioMarca);
            }
            finally
            {
                Session.Close();
            }
            return CambioPeticionarios;
        }
    }
}
