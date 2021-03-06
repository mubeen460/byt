﻿using NLog;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.AccesoDatos.Contrato;
using System.Collections.Generic;
using System;
using NHibernate;
using System.Configuration;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    class DaoPatenteNHibernate : DaoBaseNHibernate<Patente, int>, IDaoPatente
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Método que obtiene las patentes que cumplan con un filtro determinado
        /// </summary>
        /// <param name="Patente">Patente filtro</param>
        /// <returns>Lista de patentes que cumplan con el filtro</returns>
        public IList<Patente> ObtenerPatentesFiltro(Patente Patente)
        {
            IList<Patente> Patentes = null;
            bool variosFiltros = false;
            int[] consultaGrid; ;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                string filtro = "";
                string cabecera = string.Format(Recursos.ConsultasHQL.CabeceraObtenerPatente);

                //if ((null != Patente) && (Patente.Id != 0))
                if ((null != Patente) && (Patente.Id != int.MinValue))
                {
                    filtro = string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteId, Patente.Id);
                    variosFiltros = true;
                }

                if (!string.IsNullOrEmpty(Patente.OrigenPatente))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteOrigenPatente, Patente.OrigenPatente);
                    variosFiltros = true;
                }

                if ((null != Patente) && (Patente.LocalidadPatente != null) && (!Patente.LocalidadPatente.Equals(string.Empty)))
                {
                    //if (!Patente.LocalidadPatente.Equals("N"))
                    //{
                        if (variosFiltros)
                            filtro += " and ";
                        filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteLocalidad, Patente.LocalidadPatente);
                        variosFiltros = true;
                    //}
                }

                if (!string.IsNullOrEmpty(Patente.Descripcion))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteDescripcion, Patente.Descripcion);
                }

                //if ((null != Patente.Asociado) && (!Patente.Asociado.Id.Equals("")))
                if ((null != Patente.Asociado) && (Patente.Asociado.Id != int.MinValue))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteIdAsociado, Patente.Asociado.Id);
                    variosFiltros = true;
                }

                if ((null != Patente.Asociado) && (!string.IsNullOrEmpty(Patente.Asociado.OrigenCliente)) && (!Patente.Asociado.OrigenCliente.Equals("NGN")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteOrigenAsociado, Patente.Asociado.OrigenCliente);
                    variosFiltros = true;
                }

                //if ((null != Patente.Interesado) && (!Patente.Interesado.Id.Equals("")))
                if ((null != Patente.Interesado) && (Patente.Interesado.Id != int.MinValue))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteIdInteresado, Patente.Interesado.Id);
                    variosFiltros = true;
                }

                if ((null != Patente.Interesado) && (!string.IsNullOrEmpty(Patente.Interesado.OrigenCliente)) && (!Patente.Interesado.OrigenCliente.Equals("NGN")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteOrigenInteresado, Patente.Interesado.OrigenCliente);
                    variosFiltros = true;
                }

                if ((null != Patente.Poder) && (Patente.Poder.Id != int.MinValue))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteIdPoder, Patente.Poder.Id);
                    variosFiltros = true;
                }

                if ((null != Patente.TipoEstado) && (!Patente.TipoEstado.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteTipoEstado, Patente.TipoEstado.Id);
                    variosFiltros = true;
                }

                if ((null != Patente.Servicio) && (!Patente.Servicio.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteServicio, Patente.Servicio.Id);
                    variosFiltros = true;
                }

                if ((null != Patente.BoletinPublicacion) && (!Patente.BoletinPublicacion.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteBoletinPublicacion, Patente.BoletinPublicacion.Id);
                    variosFiltros = true;
                }

                if ((null != Patente.BoletinConcesion) && (!Patente.BoletinConcesion.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteBoletinConcesion, Patente.BoletinConcesion.Id);
                    variosFiltros = true;
                }

                if ((null != Patente.BoletinOrdenPublicacion) && (!Patente.BoletinOrdenPublicacion.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteBoletinOrdenPublicacion, Patente.BoletinOrdenPublicacion.Id);
                    variosFiltros = true;
                }

                if ((null != Patente.CPrioridad) && (!Patente.CPrioridad.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteCodigoPrioridad, Patente.CPrioridad);
                    variosFiltros = true;
                }

                if ((null != Patente.FechaPrioridad) && (!Patente.FechaPrioridad.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    string fecha = String.Format("{0:dd/MM/yy}", Patente.FechaPrioridad);
                    string fecha2 = String.Format("{0:dd/MM/yy}", Patente.FechaPrioridad.Value.AddDays(1));
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteFechaPrioridad, fecha, fecha2);
                }

                if ((null != Patente.Pais) && (Patente.Pais.Id != int.MinValue))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatentePaisPrioridad, Patente.Pais.Id);
                    variosFiltros = true;
                }


                //if ((null != Patente.FechaPublicacion) && (!Patente.FechaPublicacion.Equals(DateTime.MinValue)))
                //{
                //    if (variosFiltros)
                //        filtro += " and ";
                //    string fecha = String.Format("{0:dd/MM/yy}", Patente.FechaPublicacion);
                //    string fecha2 = String.Format("{0:dd/MM/yy}", Patente.FechaPublicacion.Value.AddDays(1));
                //    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteFecha, fecha, fecha2);
                //}


                if ((null != Patente.FechaInscripcion) && (!Patente.FechaInscripcion.Equals(DateTime.MinValue)))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    string fecha = String.Format("{0:dd/MM/yy}", Patente.FechaInscripcion);
                    //string fecha2 = String.Format("{0:dd/MM/yy}", Patente.FechaPublicacion.Value.AddDays(1));
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteFecha, fecha);
                }

                if ((null != Patente.CodigoInscripcion) && (!Patente.CodigoInscripcion.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteSolicitud, Patente.CodigoInscripcion);
                    variosFiltros = true;
                }

                if (null != Patente.PrimeraReferencia)
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteReferencia, Patente.PrimeraReferencia.ToUpper());
                    variosFiltros = true;
                }

                if ((null != Patente.Observacion) && (!Patente.Observacion.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteObservacion, Patente.Observacion);
                    variosFiltros = true;
                }


                #region TYR

                if ((null != Patente.CodigoRegistro) && (!Patente.CodigoRegistro.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteRegistro, Patente.CodigoRegistro);
                    variosFiltros = true;
                }

                if ((null != Patente.FechaRegistro) && (!Patente.FechaRegistro.Equals(DateTime.MinValue)))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    string fecha = String.Format("{0:dd/MM/yy}", Patente.FechaRegistro);
                    //string fecha2 = String.Format("{0:dd/MM/yy}", Patente.FechaPublicacion.Value.AddDays(1));
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteFechaRegistro, fecha);
                }

                if (!string.IsNullOrEmpty(Patente.ExpCambioPendiente))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteExpCambioPendiente, Patente.ExpCambioPendiente);
                    variosFiltros = true;
                }

                #endregion


                #region Filtro de Marca Internacional

                if ((Patente.LocalidadPatente != null) && (Patente.LocalidadPatente.Equals("I")))
                {

                    if (Patente.CodigoPatenteInternacional != 0)
                    {
                        if (variosFiltros)
                            filtro += " and ";
                        filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteIntIdInternacional, Patente.CodigoPatenteInternacional);
                        variosFiltros = true;

                    }

                    if (Patente.CorrelativoExpediente != 0)
                    {
                        if (variosFiltros)
                            filtro += " and ";
                        filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteIntIdCorrelativoExp, Patente.CorrelativoExpediente);
                        variosFiltros = true;
                    }

                    if (Patente.PaisInternacional != null)
                    {
                        if (variosFiltros)
                            filtro += " and ";
                        filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatentePaisInternacional, Patente.PaisInternacional.Id);
                        variosFiltros = true;
                    }

                    if (!string.IsNullOrEmpty(Patente.ReferenciaAsociadoInternacional))
                    {
                        if (variosFiltros)
                            filtro += " and ";
                        filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteReferenciaAsociadoInternacional, Patente.ReferenciaAsociadoInternacional.ToUpper());
                        variosFiltros = true;
                    }

                    if (!string.IsNullOrEmpty(Patente.ReferenciaInteresadoInternacional))
                    {
                        if (variosFiltros)
                            filtro += " and ";
                        filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteReferenciaInteresadoInternacional, Patente.ReferenciaInteresadoInternacional.ToUpper());
                        variosFiltros = true;
                    }

                    if ((null != Patente.AsociadoInternacional) && (Patente.AsociadoInternacional.Id != int.MinValue))
                    {
                        if (variosFiltros)
                            filtro += " and ";
                        filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteIdAsociadoInternacional, Patente.AsociadoInternacional.Id);
                        variosFiltros = true;
                    }
                }

                #endregion


                //Validacion que se realiza cuando el codigo de la Patente = 0
                //if ((filtro.Equals(String.Empty) || filtro.Equals("")) && (Patente.Id == 0))
                //{
                //    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerPatenteId, Patente.Id);
                //}

                filtro += " order by p.Id desc";

                IQuery query = Session.CreateQuery(cabecera + filtro);
                Patentes = query.List<Patente>();


                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.ExPatentesFiltro);
            }
            finally
            {
                Session.Close();
            }
            return Patentes;
        }


        /// <summary>
        /// Método que obtiene una patente con todos sus objetos
        /// </summary>
        /// <param name="Patente">Patente a consultar</param>
        /// <returns>Patente con todos los objetos que la componen</returns>
        public Patente ObtenerPatenteConTodo(Patente Patente)
        {
            Patente retorno;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IQuery query = Session.CreateQuery(string.Format(Recursos.ConsultasHQL.ObtenerPatenteConTodo, Patente.Id));
                retorno = query.UniqueResult<Patente>();

                string CabeceraBase = string.Format(Recursos.ConsultasHQL.CabeceraObtenerAnualidadPorIdPatente, retorno.Id);
                IQuery query2 = Session.CreateQuery(CabeceraBase + " order by a.QAnualidad asc");
                retorno.Anualidades = query2.List<Anualidad>();



                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.ExObtenerPatenteConTodo);
            }
            finally
            {
                Session.Close();
            }

            return retorno;
        }


        /// <summary>
        /// Método que obtiene las fechas de una patente
        /// </summary>
        /// <param name="Patente">Patente a consultarle las fechas</param>
        /// <returns>Lista de fechas de la patente</returns>
        public IList<Fecha> ObtenerFechasPatente(Patente Patente)
        {
            IList<Fecha> retorno;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IQuery query = Session.CreateQuery(string.Format(Recursos.ConsultasHQL.ObtenerFechaPatente, Patente.Id));
                retorno = query.List<Fecha>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.ExObtenerFechasPatente);
            }
            finally
            {
                Session.Close();
            }

            return retorno;
        }


        /// <summary>
        /// Metodo que obtiene las patentes las cuales esta por vencer su prioridad de acuerdo a una cantidad de dias de recordatorio
        /// </summary>
        /// <param name="cantidadDiasRecordatorio">Cantidad de dias usadas para el recordatorio</param>
        /// <returns>Lista de patentes que estan por vencer su prioridad</returns>
        public IList<VencimientoPrioridadPatente> ObtenerPatentesPorVencerPrioridad(int cantidadDiasRecordatorio)
        {
            IList<VencimientoPrioridadPatente> patentesPrioridadVencida = new List<VencimientoPrioridadPatente>();

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                string filtro = "";
                string cabecera = string.Format(Recursos.ConsultasHQL.CabeceraObtenerPatentesPorVencerPrioridad);

                filtro = string.Format(Recursos.ConsultasHQL.FiltroObtenerPatentePorVencerPrioridadCantDias, cantidadDiasRecordatorio);

                filtro += " order by vpp.VencimientoDias asc"; 

                IQuery query = Session.CreateQuery(cabecera + filtro);
                patentesPrioridadVencida = query.List<VencimientoPrioridadPatente>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerPatentesPorVencerPrioridad + ": " + ex.Message);
            }
            finally
            {
                Session.Close();
            }

            return patentesPrioridadVencida;
        }


    }
}
