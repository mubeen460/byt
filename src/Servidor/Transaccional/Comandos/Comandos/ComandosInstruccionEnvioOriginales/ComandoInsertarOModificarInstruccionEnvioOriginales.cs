﻿using System;
using System.Configuration;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.AccesoDatos.Fabrica;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Comandos.Comandos.ComandosInstruccionEnvioOriginales
{
    public class ComandoInsertarOModificarInstruccionEnvioOriginales : ComandoBase<bool>
    {
        
        private static Logger logger = LogManager.GetCurrentClassLogger();

        InstruccionEnvioOriginales _instruccion;

        /// <summary>
        /// Constructor predeterminado
        /// </summary>
        /// <param name="infoBol">Archivo a insertar o modificar</param>
        public ComandoInsertarOModificarInstruccionEnvioOriginales(InstruccionEnvioOriginales instruccion)
        {
            this._instruccion = instruccion;
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

                IDaoInstruccionEnvioOriginales dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoInstruccionEnvioOriginales();
                this.Receptor = new Receptor<bool>(dao.InsertarOModificar(this._instruccion));

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
