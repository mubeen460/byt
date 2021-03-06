﻿using NLog;
using System;
using System.Configuration;
using NHibernate;
using Trascend.Bolet.ObjetosComunes.Entidades;
using Trascend.Bolet.AccesoDatos.Contrato;
using System.Collections.Generic;
using Trascend.Bolet.AccesoDatos.Fabrica;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    class DaoCartaOutNHibernate : DaoBaseNHibernate<CartaOut, int>, IDaoCartaOut
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Metodo que consulta las CatasOuts dado unos parametros
        /// </summary>
        /// <param name="carta">CartaOut con parametros establecidos</param>
        /// <returns>Lista de CartasOuts con parametros solicitados</returns>
        public IList<CartaOut> ObtenerCartasOutsFiltro(CartaOut carta)
        {
            IList<CartaOut> cartas = null;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                bool variosFiltros = false;
                string filtro = "";
                string cabecera = string.Format(Recursos.ConsultasHQL.CabeceraObtenerCartaOut);
                if ((null != carta) && (carta.Status != null))
                {
                    filtro = string.Format(Recursos.ConsultasHQL.FiltroObtenerCartaOutStatus, carta.Status);
                    variosFiltros = true;
                }
                if ((null != carta) && (null != carta.Id) && (!carta.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerCartaOutId, carta.Id);
                    variosFiltros = true;
                }
                if (carta.Asociado != 0)
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerCartaIdAsociado, carta.Asociado);
                    variosFiltros = true;
                }
                if ((null != carta.Fecha) && (!carta.Fecha.Equals(DateTime.MinValue)))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    string fecha = String.Format("{0:dd/MM/yy}", carta.Fecha);
                    DateTime fechaPrueba = Convert.ToDateTime(fecha);
                    string fecha2 = String.Format("{0:dd/MM/yy}", fechaPrueba.AddDays(1));
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerCartaOutFecha, fecha, fecha2);
                }

                filtro += " order by c.NRelacion";
                IQuery query = Session.CreateQuery(cabecera + filtro);
                cartas = query.List<CartaOut>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerCartaOutFiltro);
            }
            finally
            {
                Session.Close();
            }
            return cartas;
        }


        /// <summary>
        /// Metodo que se usa para transferir la planilla de cartas
        /// </summary>
        /// <param name="cartas">Cartas que se encuentran en la planilla</param>
        /// <param name="cartasOut">CartasOuts que se encuentran en la planilla</param>
        /// <returns>True si se inserto correctamente, False de lo contrario</returns>
        public bool TransferirPlantilla(IList<Carta> cartas, IList<CartaOut> cartasOut)
        {
            ITransaction transaccion = Session.BeginTransaction();
            int index = 0;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                foreach (Carta carta in cartas)
                {

                    cartasOut[index].Status = 'M';
                    carta.DescripcionResumen = "INFORMACION";
                    Session.SaveOrUpdate(cartasOut[index]);
                    Session.SaveOrUpdate(carta);

                    index++;
                }
                transaccion.Commit();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                transaccion.Rollback();
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exTransferirPlantilla);
            }
            finally { Session.Close(); }

            return transaccion.WasCommitted;
        }
    }
}
