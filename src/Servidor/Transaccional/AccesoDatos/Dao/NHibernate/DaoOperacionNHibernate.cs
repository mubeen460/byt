﻿using System;
using System.Configuration;
using NHibernate;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    public class DaoOperacionNHibernate : DaoBaseNHibernate<Operacion, int>, IDaoOperacion
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Metodo que consulta las operaciones por una marca
        /// </summary>
        /// <param name="marca">Marca</param>
        /// <returns>Lista de operaciones solicitadas</returns>
        public IList<Operacion> ObtenerOperacionesPorMarca(Marca marca)
        {
            IList<Operacion> Operaciones;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IQuery query = Session.CreateQuery(string.Format(Recursos.ConsultasHQL.ObtenerOperacionesPorMarcas, marca.Id));
                Operaciones = query.List<Operacion>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerOperacionesPorMarca);
            }
            finally
            {
                Session.Close();
            }

            return Operaciones;
        }


        /// <summary>
        /// Metodo que consulta las operaciones por una patente
        /// </summary>
        /// <param name="patente">Patente</param>
        /// <returns>Lista de operaciones solicitadas</returns>
        public IList<Operacion> ObtenerOperacionesPorPatente(Patente patente)
        {
            IList<Operacion> Operaciones;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IQuery query = Session.CreateQuery(string.Format(Recursos.ConsultasHQL.ObtenerOperacionesPorPatentes, patente.Id));
                Operaciones = query.List<Operacion>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerOperacionesPorPatente);
            }
            finally
            {
                Session.Close();
            }

            return Operaciones;
        }


        /// <summary>
        /// Metodo que obtiene las Marcas y servicios de esa operacion
        /// </summary>
        /// <param name="operacion">Operacion ha solicitar</param>
        /// <returns>Lista de operaciones solicitadas</returns>
        public IList<Operacion> ObtenerOperacionesPorMarcaYServicio(Operacion operacion)
        {
            IList<Operacion> Operaciones;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IQuery query = Session.CreateQuery(string.Format(Recursos.ConsultasHQL.ObtenerOperacionesPorMarcasYServicio, operacion.Marca.Id, operacion.Servicio.Id));
                Operaciones = query.List<Operacion>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerOperacionesPorMarcaYServicio);
            }
            finally
            {
                Session.Close();
            }

            return Operaciones;
        }


        /// <summary>
        /// Metodo que consulta las operaciones por parametros
        /// </summary>
        /// <param name="operacion">Operacion con parameteros</param>
        /// <returns>Lista de operaciones solicitaos</returns>
        public IList<Operacion> ObtenerOperacionesFiltro(Operacion operacion)
        {
            IList<Operacion> operaciones = null;

            try
            {

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                bool variosFiltros = false;
                string filtro = "";
                string cabecera = string.Format(Recursos.ConsultasHQL.CabeceraObtenerOperacion);

                if ((null != operacion) && (operacion.Id != 0))
                {
                    filtro = string.Format(Recursos.ConsultasHQL.FiltroObtenerOperacionId, operacion.Id);
                    variosFiltros = true;
                }

                if ((null != operacion) && !(operacion.Aplicada.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerOperacionMarca, operacion.Aplicada);
                    variosFiltros = true;
                }

                if ((null != operacion) && !(operacion.Servicio.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerOperacionServicio, operacion.Servicio.Id);
                    variosFiltros = true;
                }

                if ((null != operacion.Marca) && (!operacion.Marca.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerOperacionIdMarca, operacion.Marca.Id);
                    variosFiltros = true;
                }

                if ((null != operacion.Patente) && (!operacion.Patente.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerOperacionIdPatente, operacion.Patente.Id);
                    variosFiltros = true;
                }

                if (null != operacion.CadenaDeCambios)
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerOperacionCadenaCambios, operacion.CadenaDeCambios);
                    variosFiltros = true;
                }

                if ((null != operacion.Fecha) && (!operacion.Fecha.Equals(DateTime.MinValue)))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    string fecha = String.Format("{0:dd/MM/yy}", operacion.Fecha);
                    string fecha2 = String.Format("{0:dd/MM/yy}", operacion.Fecha.Value.AddDays(1));
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerOperacionFecha, fecha, fecha2);
                }

                IQuery query = Session.CreateQuery(cabecera + filtro);
                operaciones = query.List<Operacion>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerOperacionesFiltro);
            }
            finally
            {
                Session.Close();
            }

            return operaciones;
        }
    }
}
