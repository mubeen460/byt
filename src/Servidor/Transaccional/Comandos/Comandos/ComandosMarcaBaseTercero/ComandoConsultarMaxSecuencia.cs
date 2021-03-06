﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Configuration;
using Trascend.Bolet.AccesoDatos.Fabrica;
using Trascend.Bolet.AccesoDatos.Contrato;

namespace Trascend.Bolet.Comandos.Comandos.ComandosMarcaBaseTercero
{
    class ComandoConsultarMaxSecuencia : ComandoBase<int>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private string _secuencia;

    /// <summary>
    /// Metodo que regresa la ultima secuencia de una MarcaBaseTercero
    /// </summary>
        public ComandoConsultarMaxSecuencia()
        {
           
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

                IDaoMarcaBaseTercero dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoMarcaBaseTercero();
                this.Receptor = new Receptor<int>(dao.ObtenerMaxSecuencia());

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
