﻿using System;
using System.Configuration;
using NHibernate;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Collections.Generic;

namespace Trascend.Bolet.AccesoDatos.Dao.NHibernate
{
    public class DaoContactoNHibernate : DaoBaseNHibernate<Contacto, int>, IDaoContacto
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public IList<Contacto> ObtenerContactosPorAsociado(Asociado asociado)
        {
            IList<Contacto> contactos;

            try
            {
                IQuery query = Session.CreateQuery(string.Format(Recursos.ConsultasHQL.ObtenerContactosPorAsociado, asociado.Id));
                contactos = query.List<Contacto>();
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

            return contactos;
        }
    }
}