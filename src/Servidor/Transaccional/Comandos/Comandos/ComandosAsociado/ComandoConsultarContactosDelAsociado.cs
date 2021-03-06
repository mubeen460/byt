﻿using System;
using System.Collections.Generic;
using System.Configuration;
using NLog;
using Trascend.Bolet.AccesoDatos.Contrato;
using Trascend.Bolet.AccesoDatos.Fabrica;
using Trascend.Bolet.ObjetosComunes.Entidades;

namespace Trascend.Bolet.Comandos.Comandos.ComandosAsociado
{
    class ComandoConsultarContactosDelAsociado : ComandoBase<IList<ContactosDelAsociadoVista>>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private DateTime[] _fechas;
        private Asociado _asociado;
        private bool _todos;
        


        /// <summary>
        /// Metodo Comando que consulta los recordatorios de marcas
        /// </summary>
        /// <param name="RecordatorioVista">recordatorio con parametros</param>
        /// /// <param name="Fechas">fecha de renovación de marcas a filtrar</param>
        public ComandoConsultarContactosDelAsociado(Asociado asociado, bool todos)
        {
            this._asociado = asociado;
            this._todos = todos;
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

                IDaoAsociado dao = FabricaDaoBase.ObtenerFabricaDao().ObtenerDaoAsociado();
                this.Receptor = new Receptor<IList<ContactosDelAsociadoVista>>(dao.ObtenerContactosDelAsociado(this._asociado,this._todos));

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
