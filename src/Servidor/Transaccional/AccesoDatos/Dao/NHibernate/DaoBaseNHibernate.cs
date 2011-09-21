using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    public abstract class DaoBaseNHibernate<T, Id> : IDaoBase<T, Id>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ISession _session;

        /// <summary>
        /// Propiedad para obtener la sesion
        /// </summary>
        public ISession Session
        {
            get
            {
                if (null == _session)
                    _session = NHibernateHelper.OpenSession();
                return _session;
            }

            set { _session = value; }
        }

        /// <summary>
        /// M�todo que obtiene todos los registros de la entidad
        /// </summary>
        /// <returns>Lista con todos los elementos de la entidad</returns>
        public IList<T> ObtenerTodos()
        {
            IList<T> listaEntidad;

            try
            {
                listaEntidad = Session.CreateCriteria(typeof(T)).AddOrder(Order.Asc("Id")).List<T>();
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

            return listaEntidad;
        }

        /// <summary>
        /// M�todo que obtiene el elemento de la entidad por su id
        /// </summary>
        /// <param name="id">Id de la entidad a requerida</param>
        /// <returns>Entidad requerida</returns>
        public T ObtenerPorId(Id id)
        {
            T entidad;

            try
            {
                entidad = Session.Get<T>(id);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.ExObtenerPorId);
            }
            finally
            {
                Session.Close();
            }

            return entidad;
        }

        /// <summary>
        /// M�todo que obtiene el elemento de la entidad por su id y 
        /// lo bloquea hasta que sea actualizado
        /// </summary>
        /// <param name="id">Id de la entidad a requerida</param>
        /// <returns>Entidad requerida</returns>
        public T ObtenerPorIdYBloquear(Id id)
        {
            T entidad;

            try
            {
                entidad = Session.Get<T>(id, LockMode.Upgrade);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.ExObtenerPorId);
            }
            finally
            {
                Session.Close();
            }

            return entidad;
        }

        /// <summary>
        /// M�todo que inserta o modifica una entidad
        /// </summary>
        /// <param name="entidad">Entidad a modificar</param>
        /// <returns>True si fue �xitoso la inserci�n o modificaci�n, en caso contrario False</returns>
        public bool InsertarOModificar(T entidad)
        {
            bool exitoso;

            try
            {
                ITransaction transaccion = Session.BeginTransaction();
                Session.SaveOrUpdate(entidad);
                transaccion.Commit();
                exitoso = transaccion.WasCommitted;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.ExInsertarOModificar);
            }
            finally
            {
                Session.Close();
            }

            return exitoso;
        }

        /// <summary>
        /// M�todo que elimina una entidad
        /// </summary>
        /// <param name="entidad">Entidad a eliminar</param>
        /// <returns>True si fue �xitoso la eliminaci�n, en caso contrario False</returns>
        public bool Eliminar(T entidad)
        {
            bool exitoso;

            try
            {
                ITransaction transaccion = Session.BeginTransaction();
                Session.Delete(entidad);
                transaccion.Commit();
                exitoso = transaccion.WasCommitted;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException(Recursos.Errores.ExEliminando);
            }
            finally
            {
                Session.Close();
            }

            return exitoso;
        }
    }
}