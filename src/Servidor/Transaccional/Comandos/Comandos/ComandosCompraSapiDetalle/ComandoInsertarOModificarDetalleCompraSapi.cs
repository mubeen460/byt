﻿using System;
using System.Configuration;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.AccesoDatos.Fabrica;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Comandos.Comandos.ComandosCompraSapiDetalle
{
    public class ComandoInsertarOModificarDetalleCompraSapi : ComandoBase<bool>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        CompraSapiDetalle _detalleCompra;

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        /// <param name="detalleCompra">Renglon de detalle de Compra de Material Sapi</param>
        public ComandoInsertarOModificarDetalleCompraSapi(CompraSapiDetalle detalleCompra)
        {
            this._detalleCompra = detalleCompra;
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

                IDaoCompraSapiDetalle dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoCompraSapiDetalle();
                this.Receptor = new Receptor<bool>(dao.InsertarOModificar(this._detalleCompra));

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
