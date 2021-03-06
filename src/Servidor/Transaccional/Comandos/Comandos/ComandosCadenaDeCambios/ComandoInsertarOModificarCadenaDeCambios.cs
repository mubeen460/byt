﻿using System;
using System.Configuration;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.AccesoDatos.Fabrica;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Comandos.Comandos.ComandosCadenaDeCambios
{
    public class ComandoInsertarOModificarCadenaDeCambios : ComandoBase<bool>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        CadenaDeCambios _cadenaCambios;

        /// <summary>
        /// Constructor predeterminado que recibe una cadena de cambios
        /// </summary>
        /// <param name="cadenaCambios">Cadena de Cambios a insertar y/o actualizar</param>
        public ComandoInsertarOModificarCadenaDeCambios(CadenaDeCambios cadenaCambios)
        {
            this._cadenaCambios = cadenaCambios;
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

                //IDaoArchivo dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoArchivo();
                IDaoCadenaDeCambios dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoCadenaDeCambios();
                this.Receptor = new Receptor<bool>(dao.InsertarOModificar(this._cadenaCambios));

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
