﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Trascend.Bolet.ObjetosComunes.Entidades;
using System.Configuration;
using Trascend.Bolet.Comandos.Comandos;
using Trascend.Bolet.Comandos.Fabrica;

namespace Trascend.Bolet.LogicaNegocio.Controladores
{
    public class ControladorMarca : ControladorBase
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Método que inserta o modifica una Marca
        /// </summary>
        /// <param name="marca">Marca a insertar o modificar</param>
        /// <param name="hash">Hash del usuario que realiza la operacion</param>
        /// <returns>True: si la modificación fue exitosa; false: en caso contrario</returns>
        public static bool InsertarOModificar(ref Marca marca, int hash)
        {
            bool exitoso = false;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                ComandoBase<bool> comandoInteresadoContador = null;

                // si es una insercion
                if (marca.Operacion.Equals("CREATE"))
                {
                    marca.Recordatorio = 1;
                    ComandoBase<Contador> comandoContadorInteresadoProximoValor = FabricaComandosContador.ObtenerComandoConsultarPorId("MYP_MARCAS");
                    comandoContadorInteresadoProximoValor.Ejecutar();
                    Contador contador = comandoContadorInteresadoProximoValor.Receptor.ObjetoAlmacenado;
                    marca.Id = contador.ProximoValor++;
                    comandoInteresadoContador = FabricaComandosContador.ObtenerComandoInsertarOModificar(contador);
                }

                //ComandoBase<bool> comandoAnaqua = FabricaComandosAnaqua.ObtenerComandoInsertarOModificar(marca.Anaqua);
                //comandoAnaqua.Ejecutar();

                Auditoria auditoria = new Auditoria();
                ComandoBase<ContadorAuditoria> comandoContadorAuditoriaPoximoValor = FabricaComandosContadorAuditoria.ObtenerComandoConsultarPorId("SEG_AUDITORIA");

                comandoContadorAuditoriaPoximoValor.Ejecutar();
                ContadorAuditoria contadorAuditoria = comandoContadorAuditoriaPoximoValor.Receptor.ObjetoAlmacenado;


                auditoria.Id = contadorAuditoria.ProximoValor++;
                auditoria.Usuario = ObtenerUsuarioPorHash(hash).Id;
                auditoria.Fecha = System.DateTime.Now;
                auditoria.Operacion = marca.Operacion;
                auditoria.Tabla = "MYP_MARCAS";
                auditoria.Fk = marca.Id;

                ComandoBase<bool> comando = FabricaComandosMarca.ObtenerComandoInsertarOModificar(marca);
                ComandoBase<bool> comandoAuditoria = FabricaComandosAuditoria.ObtenerComandoInsertarOModificar(auditoria);
                ComandoBase<bool> comandoAuditoriaContador = FabricaComandosContadorAuditoria.ObtenerComandoInsertarOModificar(contadorAuditoria);
                
                comando.Ejecutar();
                exitoso = comando.Receptor.ObjetoAlmacenado;

                if (exitoso)
                {
                    comandoAuditoria.Ejecutar();
                    comandoAuditoriaContador.Ejecutar();
                    
                    if (comandoInteresadoContador != null)
                        comandoInteresadoContador.Ejecutar();
                }

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
            return exitoso;
        }


        /// <summary>
        /// Método que elimina una Marca
        /// </summary>
        /// <param name="marca">Marca a eliminar</param>
        /// <param name="hash">Hash del usuario que realiza la operacion</param>
        /// <returns>True si la eliminacion fue exitosa, en caso contrario False</returns>
        public static bool Eliminar(Marca marca, int hash)
        {
            bool exitoso = false;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                ComandoBase<bool> comando = FabricaComandosMarca.ObtenerComandoEliminarObjeto(marca);
                comando.Ejecutar();
                exitoso = true;

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

            return exitoso;
        }


        /// <summary>
        /// Método que consulta la lista de todos las Marcas
        /// </summary>
        /// <returns>Lista con todos las Marcas</returns>
        public static IList<Marca> ConsultarTodos()
        {
            IList<Marca> retorno;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                ComandoBase<IList<Marca>> comando = FabricaComandosMarca.ObtenerComandoConsultarTodos();
                comando.Ejecutar();
                retorno = comando.Receptor.ObjetoAlmacenado;

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

            return retorno;
        }


        /// <summary>
        /// Verifica si la Marca existe
        /// </summary>
        /// <param name="marca">Marca a verificar</param>
        /// <returns>True de existir, false en caso conrario</returns>
        public static bool VerificarExistencia(Marca marca)
        {
            bool existe = false;
            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                ComandoBase<bool> comando = FabricaComandosMarca.ObtenerComandoVerificarExistenciaMarca(marca);
                comando.Ejecutar();
                existe = comando.Receptor.ObjetoAlmacenado;

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

            return existe;
        }


        public static IList<Marca> ConsultarMarcasFiltro(Marca marca)
        {
            IList<Marca> retorno;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                ComandoBase<IList<Marca>> comando = FabricaComandosMarca.ObtenerComandoConsultarMarcasFiltro(marca);
                comando.Ejecutar();
                retorno = comando.Receptor.ObjetoAlmacenado;
                
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

            return retorno;
        }


        /// <summary>
        /// Metodo que obtiene el marcas por fecha de renovacion
        /// </summary>
        /// <param name="marca">marca con NRecordatorio a filtrar</param>
        /// <param name="fechas">Arreglo con las fechas a filtrar [0]FechaInicio y [1]FechaFin</param>
        /// <returns>Lista de Marcas fitlradas</returns>
        public static IList<Marca> ObtenerMarcasPorFechaRenovacion(Marca marca, DateTime[] fechas)
        {
            IList<Marca> retorno;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                ComandoBase<IList<Marca>> comando = FabricaComandosMarca.ObtenerComandoObtenerMarcasPorFechaRenovacion(marca, fechas);
                comando.Ejecutar();
                retorno = comando.Receptor.ObjetoAlmacenado;

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

            return retorno;
        }


        /// <summary>
        /// Método que consulta una marca con todas sus dependencias
        /// </summary>
        /// <returns>marca completo</returns>
        public static Marca ConsultarMarcaConTodo(Marca marca)
        {
            Marca retorno;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                ComandoBase<Marca> comando = FabricaComandosMarca.ObtenerComandoConsultarMarcaConTodo(marca);
                comando.Ejecutar();
                retorno = comando.Receptor.ObjetoAlmacenado;

                ComandoBase<InfoAdicional> comandoInfoAdicional = FabricaComandosInfoAdicional.ObtenerComandoConsultarInfoAdicionalPorId(new InfoAdicional("M." + retorno.Id));
                comandoInfoAdicional.Ejecutar();
                retorno.InfoAdicional = comandoInfoAdicional.Receptor.ObjetoAlmacenado;

                ComandoBase<Anaqua> comandoAnaqua = FabricaComandosAnaqua.ObtenerComandoConsultarPorID(new Anaqua(retorno.Id));
                comandoAnaqua.Ejecutar();
                retorno.Anaqua = comandoAnaqua.Receptor.ObjetoAlmacenado;

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

            return retorno;
        }


        /// <summary>
        /// Metodo que obtiene el marcas por fecha de renovacion
        /// </summary>
        /// <param name="recordatorio">recordatorio a filtrar</param>
        /// <param name="fechas">fechas de renovación de marca a filtrar</param>
        /// <returns>Lista de recordatorios fitlrados</returns>
        public static IList<RecordatorioVista> ConsultarRecordatoriosVista(RecordatorioVista recordatorio, DateTime[] fechas, string localidad)
        {
            IList<RecordatorioVista> retorno;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                ComandoBase<IList<RecordatorioVista>> comando = FabricaComandosMarca.ObtenerComandoConsultarRecordatoriosVista(recordatorio, fechas, localidad);
                comando.Ejecutar();
                retorno = comando.Receptor.ObjetoAlmacenado;

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

            return retorno;
        }


        /// <summary>
        /// Servicio que se encarga de consultar la vista de recordatorios de marca con filtro no automático
        /// </summary>
        /// <param name="RecordatorioVista">recordatorio a consultar</param>
        /// <param name="ano">Ano de fecha renovacion a filtrar</param>
        /// <param name="mes">mes de fecha renovación a filtrar</param>
        /// <param name="fechas">fecha desde y hasta de renovación a filtrar</param>
        /// <returns>Lista de marcas para recordatorio filtradas</returns>
        public static IList<RecordatorioVista> ConsultarRecordatoriosVista(RecordatorioVista recordatorio, string ano, string mes, DateTime?[] fechas, string localidad)
        {
            IList<RecordatorioVista> retorno;

            try
            {
                #region trace
                if (ConfigurationManager.AppSettings["Ambiente"].ToString().Equals("Desarrollo"))
                    logger.Debug("Entrando al Método {0}", (new System.Diagnostics.StackFrame()).GetMethod().Name);
                #endregion

                ComandoBase<IList<RecordatorioVista>> comando = FabricaComandosMarca.ObtenerComandoConsultarRecordatoriosVista(recordatorio, ano, mes, fechas, localidad);
                comando.Ejecutar();
                retorno = comando.Receptor.ObjetoAlmacenado;

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

            return retorno;
        }

    }
}

