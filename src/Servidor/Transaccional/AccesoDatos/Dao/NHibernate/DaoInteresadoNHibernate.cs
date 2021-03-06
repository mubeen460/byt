﻿using System;
using System.Configuration;
using NHibernate;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    public class DaoInteresadoNHibernate : DaoBaseNHibernate<Interesado, int>, IDaoInteresado
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Método que autentica un usuario
        /// </summary>
        /// <param name="interesado">usuario a autenticar</param>
        /// <returns>Usuario autenticado</returns>
        public Interesado ObtenerInteresadoConTodo(Interesado interesado)
        {
            Interesado retorno;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IQuery query = Session.CreateQuery(string.Format(Recursos.ConsultasHQL.ObtenerInteresadoConTodo, interesado.Id));
                retorno = query.UniqueResult<Interesado>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerInteresadoConTodo);
            }
            finally
            {
                Session.Close();
            }

            return retorno;
        }


        /// <summary>
        /// Método que obtiene un interesado con uno o mas filtros
        /// </summary>
        /// <param name="interesado">filtros de interesado</param>
        /// <returns>interesado filtrado</returns>
        public IList<Interesado> ObtenerInteresadosFiltro(Interesado interesado)
        {
            IList<Interesado> interesados = null;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                bool variosFiltros = false;
                string filtro = "";
                string cabecera = string.Format(Recursos.ConsultasHQL.CabeceraObtenerInteresado);

                if ((null != interesado) && (interesado.Id != 0) && (interesado.Id != int.MinValue))
                //if ((null != interesado) && (interesado.Id != int.MinValue))
                {
                    filtro = string.Format(Recursos.ConsultasHQL.FiltroObtenerInteresadoId, interesado.Id);
                    variosFiltros = true;
                }

                if (!string.IsNullOrEmpty(interesado.Nombre))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerInteresadoNombre, interesado.Nombre.ToUpper());
                    variosFiltros = true;
                }

                if ((null != interesado.TipoPersona) && (!interesado.TipoPersona.Equals(char.MinValue)))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerInteresadoTipoPersona, interesado.TipoPersona);
                    variosFiltros = true;
                }

                if ((null != interesado.Ciudad) && (!interesado.Ciudad.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerInteresadoCiudad, interesado.Ciudad);
                    variosFiltros = true;
                }

                if ((null != interesado.Estado) && (!interesado.Estado.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerInteresadoEstado, interesado.Estado);
                    variosFiltros = true;
                }

                if ((null != interesado.Pais) && (!interesado.Pais.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerInteresadoPais, ((Pais)interesado.Pais).Id);
                    variosFiltros = true;
                }

                if ((null != interesado.Nacionalidad) && (!((Pais)interesado.Nacionalidad).Nacionalidad.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerInteresadoNacionalidad, ((Pais)interesado.Nacionalidad).Nacionalidad);
                    variosFiltros = true;
                }

                if ((null != interesado.Idioma) && (!interesado.Idioma.Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerInteresadoIdioma, ((Idioma)interesado.Idioma).Id);
                }

                if ((null != interesado.Corporacion) && (!((Estado)interesado.Corporacion).Id.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerInteresadoCorporacion, ((Estado)interesado.Corporacion).Id);
                    variosFiltros = true;
                }

                if ((null != interesado.Ci) && (!interesado.Ci.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerInteresadoCi, interesado.Ci);
                    variosFiltros = true;
                }

                if ((null != interesado.RegMercantil) && (!interesado.RegMercantil.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerInteresadoRegMercantil, interesado.RegMercantil);
                    variosFiltros = true;
                }

                if ((null != interesado.Domicilio) && (!interesado.Domicilio.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerInteresadoDomicilio, interesado.Domicilio);
                    variosFiltros = true;
                }


                if ((null != interesado.Alerta) && (!interesado.Alerta.Equals("")))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerInteresadoAlerta, interesado.Alerta.ToUpper());
                    variosFiltros = true;
                }

                if (!string.IsNullOrEmpty(interesado.OrigenCliente))
                {
                    if (variosFiltros)
                        filtro += " and ";
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerInteresadoOrigenCliente, interesado.OrigenCliente);
                    variosFiltros = true;
                }


                //Validacion que se realiza cuando el codigo del Interesado = 0 
                if ((filtro.Equals(String.Empty) || filtro.Equals("")) && (interesado.Id == 0))
                {
                    filtro += string.Format(Recursos.ConsultasHQL.FiltroObtenerInteresadoId, interesado.Id);
                }

                IQuery query = Session.CreateQuery(cabecera + filtro);
                interesados = query.List<Interesado>();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerInteresadosFiltro);
            }
            finally
            {
                Session.Close();
            }
            return interesados;
        }


        /// <summary>
        /// Metodo que obtiene el interesado de un poder
        /// </summary>
        /// <param name="poder">Poder solicitado</param>
        /// <returns>Interesado que tiene ese poder</returns>
        public Interesado ObtenerInteresadosDeUnPoder(Poder poder)
        {
            Interesado retorno;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IQuery query = Session.CreateQuery(string.Format(Recursos.ConsultasHQL.ObtenerInteresadosDeUnPoder, poder.Id));
                Poder poderAux = query.UniqueResult<Poder>();
                retorno = poderAux.Interesado;

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.exObtenerInteresadosDeUnPoder);
            }
            finally
            {
                Session.Close();
            }

            return retorno;
        }
    }
}
