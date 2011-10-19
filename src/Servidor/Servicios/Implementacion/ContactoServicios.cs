﻿using System;
using System.Collections.Generic;
using System.Configuration;
using NLog;
using Trascend.Bolet.LogicaNegocio.Controladores;
using Trascend.Bolet.ObjetosComunes.ContratosServicios;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Servicios.Implementacion
{
    public class ContactoServicios : MarshalByRefObject, IContactoServicios
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Servicio que obtiene todos los conceptos
        /// </summary>
        /// <returns>Lista con todos los conceptos</returns>
        public IList<Contacto> ConsultarTodos()
        {
            IList<Contacto> contactos;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                contactos = ControladorContacto.ConsultarTodos();

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            return contactos;
        }

        public Concepto ConsultarPorId(Concepto concepto)
        {
            throw new NotImplementedException();
        }

        public bool InsertarOModificar(Concepto concepto, int hash)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(Concepto concepto, int hash)
        {
            throw new NotImplementedException();
        }

        public IList<Contacto> ConsultarContactosPorAsociado(Asociado asociado)
        {
            #region trace
            if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                logger.Debug("Entrando al metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            IList<Contacto> contactos = ControladorContacto.ConsultarContactosPorAsociado(asociado);

            #region trace
            if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                logger.Debug("Saliendo del metodo {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
            #endregion

            return contactos;
        }

        public Contacto ConsultarPorId(Contacto entidad)
        {
            throw new NotImplementedException();
        }

        public bool InsertarOModificar(Contacto entidad, int hash)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(Contacto entidad, int hash)
        {
            throw new NotImplementedException();
        }
    }
}
