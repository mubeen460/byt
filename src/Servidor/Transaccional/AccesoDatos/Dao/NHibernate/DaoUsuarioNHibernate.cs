﻿using System;
using System.Configuration;
using NHibernate;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    public class DaoUsuarioNHibernate : DaoBaseNHibernate<Usuario, string>, IDaoUsuario
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Método que autentica un usuario
        /// </summary>
        /// <param name="usuario">usuario a autenticar</param>
        /// <returns>Usuario autenticado</returns>
        public Usuario Autenticar(Usuario usuario)
        {
            Usuario retorno;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IQuery query = Session.CreateQuery(string.Format(Recursos.ConsultasHQL.ObtenerUsuarioPorIdYPassword, usuario.Id, usuario.Password));
                retorno = query.UniqueResult<Usuario>();

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


        /// <summary>
        /// Metodo que obtiene el usuario solicitado por sus iniciales
        /// </summary>
        /// <param name="iniciales">String de iniciales</param>
        /// <returns>El usuario con esas iniciales</returns>
        public Usuario ObtenerUsuarioPorIniciales(string iniciales)
        {
            Usuario retorno = new Usuario();
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IQuery query = Session.CreateQuery(string.Format(Recursos.ConsultasHQL.ObtenerUsuarioPorIniciales, iniciales));
                IList<Usuario> resultado = query.List<Usuario>();

                //Error de BD ya que no hay constraints por lo tanto el sistema trata de traerse el 
                //responsable de la carta por FK y este no existe
                retorno = resultado.Count != 0 ? resultado[0] : null;

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
