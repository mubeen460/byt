﻿using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;
using NHibernate;
using System.Collections.Generic;
using System;
using System.Configuration;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    public class DaoCambioDeDomicilioPatenteNHibernate : DaoBaseNHibernate<CambioDeDomicilioPatente, int>, IDaoCambioDeDomicilioPatente
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Metodo que consulta los Cambios de Domicilios Patentes dado unos parametros
        /// </summary>
        /// <param name="cambioDeDomicilio">CambioDeDomicilioPatentes con parametros</param>
        /// <returns>una lista de CambioDeDomicilioPatentes</returns>
        public IList<CambioDeDomicilioPatente> ObtenerCambiosDeDomicilioPatenteFiltro(CambioDeDomicilioPatente cambioDeDomicilio)
        {
            IList<CambioDeDomicilioPatente> CambioDeDomicilios = null;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                bool variosFiltros = false;
                string filtro = "";
                string cabecera = string.Format(Recursos.ConsultasHQL.CabeceraObtenerCambioDeDomicilioPatente);

                if ((null != cambioDeDomicilio) && (cambioDeDomicilio.Id != 0))
                {
                    filtro = string.Format(Recursos.ConsultasHQL.FiltroObtenerCambioDeDomicilioPatenteId, cambioDeDomicilio.Id);
                    variosFiltros = true;
                }

                if ((null != cambioDeDomicilio.Patente) && (!cambioDeDomicilio.Patente.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerCambioDeDomicilioPatenteIdPatente, cambioDeDomicilio.Patente.Id);
                    variosFiltros = true;
                }

                //if ((null != cambioDeDomicilio.Interesado) && (!cambioDeDomicilio.Interesado.Id.Equals("")))
                //{
                //    if (variosFiltros)
                //        filtro += " and ";
                //    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerMarcaIdInteresado, cambioDeDomicilio.Interesado.Id);
                //    variosFiltros = true;
                //}
                //if (!string.IsNullOrEmpty(cambioDeDomicilio.Fichas))
                //{
                //    if (variosFiltros)
                //        filtro += " and ";
                //    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerMarcaFichas, cambioDeDomicilio.Fichas);
                //}
                //if (!string.IsNullOrEmpty(cambioDeDomicilio.Descripcion))
                //{
                //    if (variosFiltros)
                //        filtro += " and ";
                //    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerMarcaDescripcion, cambioDeDomicilio.Descripcion);
                //}

                if ((null != cambioDeDomicilio.FechaPublicacion) && (!cambioDeDomicilio.FechaPublicacion.Equals(DateTime.MinValue)))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    string fecha = String.Format("{0:dd/MM/yy}", cambioDeDomicilio.FechaPublicacion);
                    string fecha2 = String.Format("{0:dd/MM/yy}", cambioDeDomicilio.FechaPublicacion.Value.AddDays(1));
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerCambioDeDomicilioPatenteFecha, fecha, fecha2);
                }

                IQuery query = Session.CreateQuery(cabecera + filtro);
                CambioDeDomicilios = query.List<CambioDeDomicilioPatente>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerCambioDeDomicilioPatente);
            }
            finally
            {
                Session.Close();
            }
            return CambioDeDomicilios;
        }
    }
}
