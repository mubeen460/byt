﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Configuration;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.AccesoDatos.Fabrica;

namespace Trascend.Bolet.Comandos.Comandos.ComandosMarcaTercero
{
    public class ComandoEliminarMarcaTercero : ComandoBase<bool>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MarcaTercero _marcaTercero;

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="marcaTercero">marcaTercero a eliminar</param>
        public ComandoEliminarMarcaTercero(MarcaTercero marcaTercero)
        {
            this._marcaTercero = marcaTercero;
        }

        /// <summary>
        /// Método que ejecuta el comando
        /// </summary>
        public override void Ejecutar()
        {
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                IDaoMarcaTercero dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoMarcaTercero();
                this.Receptor = new Receptor<bool>(dao.Eliminar(this._marcaTercero));

                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Saliendo del Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion
            }
            catch (ApplicationException ex)
            {
                logger.Error(ex.Message);
                throw ex;
            }
        }
    }
}