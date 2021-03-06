﻿using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;
using NHibernate;
using System.Collections.Generic;
using System;
using System.Configuration;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    public class DaoCambioPeticionarioPatenteNHibernate : DaoBaseNHibernate<CambioPeticionarioPatente, int>, IDaoCambioPeticionarioPatente
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Metodo que Consulta los CambiosDePeticionarioPatente dado unos parametros
        /// </summary>
        /// <param name="cambioPeticionario">CambioDePeticionarioPatente con parametros</param>
        /// <returns>Lista de CambioDePeticionariosPatente</returns>
        public IList<CambioPeticionarioPatente> ObtenerCambiosPeticionarioPatenteFiltro(CambioPeticionarioPatente cambioPeticionario)
        {
            IList<CambioPeticionarioPatente> CambioPeticionarios = null;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                bool variosFiltros = false;
                string filtro = "";
                string cabecera = string.Format(Recursos.ConsultasHQL.CabeceraObtenerCambioPeticionarioPatente);
                if ((null != cambioPeticionario) && (cambioPeticionario.Id != 0))
                {
                    filtro = string.Format(Recursos.ConsultasHQL.FiltroObtenerCambioPeticionarioPatenteId, cambioPeticionario.Id);
                    variosFiltros = true;
                }
                if ((null != cambioPeticionario.Patente) && (!cambioPeticionario.Patente.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerCambioPeticionarioPatenteIdPatente, cambioPeticionario.Patente.Id);
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
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerCambioPeticionarioPatenteFecha, fecha);
                }
                IQuery query = Session.CreateQuery(cabecera + filtro);
                CambioPeticionarios = query.List<CambioPeticionarioPatente>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerCambioPeticionarioPatente);
            }
            finally
            {
                Session.Close();
            }
            return CambioPeticionarios;
        }
    }
}
