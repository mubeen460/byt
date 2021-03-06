﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Configuration;
using Trascend.Bolet.AccesoDatos.Fabrica;
using Trascend.Bolet.AccesoDatos.Contrato;

namespace Trascend.Bolet.Comandos.Comandos.ComandosTipoBase
{
    public class ComandoInsertarOModificarTipoBase : ComandoBase<bool>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private TipoBase _tipoBase;

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="tipoBase">TipoBase a insertar o modificar</param>
        public ComandoInsertarOModificarTipoBase(TipoBase tipoBase)
        {
            this._tipoBase = tipoBase;
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

                IDaoTipoBase dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoTipoBase();
                dao.InsertarOModificar(this._tipoBase);

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
