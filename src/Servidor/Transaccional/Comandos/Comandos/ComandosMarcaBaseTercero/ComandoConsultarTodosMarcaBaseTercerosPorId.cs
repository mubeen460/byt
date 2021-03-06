﻿using System.Collections.Generic;
using System.Configuration;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.AccesoDatos.Fabrica;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System;

namespace Trascend.Bolet.Comandos.Comandos.ComandosMarcaBaseTercero
{
    public class ComandoConsultarTodosMarcaBaseTerceroPorId : ComandoBase<List<MarcaBaseTercero>>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MarcaBaseTercero _marcaBaseTercero;

        /// <summary>
        /// Metodo que consulta las marcaBaseTercero segun IdMarca y Anexo
        /// </summary>
        /// <param name="idMarcaTercero">Marca a eliminar</param>
        ///  <param name="idAnexo">idAnexo a eliminar</param>
        public ComandoConsultarTodosMarcaBaseTerceroPorId(string idMarcaTercero,int idAnexo)
        {
            _marcaBaseTercero = new MarcaBaseTercero();
            this._marcaBaseTercero.MarcaTercero = new MarcaTercero(idMarcaTercero);
            this._marcaBaseTercero.MarcaTercero.Anexo = idAnexo;
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
                this.Receptor = new Receptor<List<MarcaBaseTercero>>(dao.ObtenerTodosPorId(this._marcaBaseTercero));

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
