﻿Imports System.Collections.Generic
Imports System.Configuration
Imports NLog
Imports Diginsoft.Bolet.Comandos.Comandos
Imports Diginsoft.Bolet.Comandos.Fabrica
Imports Trascend.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.Comandos.Comandos
Imports Trascend.Bolet.LogicaNegocio.Controladores
Namespace Controladores
    Public Class ControladorFacFacturaPendienteConGru
        Inherits ControladorBase
        Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

        ''' <summary>
        ''' Método que devuelve todos los Usuarios del sistema
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function ConsultarTodos() As IList(Of FacFacturaPendienteConGru)
            Dim retorno As IList(Of FacFacturaPendienteConGru)
            Try
                '#Region "trace"
                If ConfigurationManager.AppSettings("Ambiente").ToString().Equals("Desarrollo") Then
                    logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                End If
                '#End Region                
                Dim comando As ComandoBase(Of IList(Of FacFacturaPendienteConGru)) = FabricaComandosFacFacturaPendienteConGru.ObtenerComandoConsultarTodos()
                comando.Ejecutar()
                retorno = comando.Receptor.ObjetoAlmacenado

                '#Region "trace"
                If ConfigurationManager.AppSettings("Ambiente").ToString().Equals("Desarrollo") Then
                    logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                    '#End Region
                End If
            Catch ex As ApplicationException
                logger.[Error](ex.Message)
                Throw ex
            End Try
            Return retorno
        End Function


        Public Shared Function ConsultarFacFacturaPendienteConGrusFiltro(ByVal FacFacturaPendienteConGru As FacFacturaPendienteConGru) As IList(Of FacFacturaPendienteConGru)
            Dim retorno As IList(Of FacFacturaPendienteConGru)

            Try
                '#Region "trace"
                If ConfigurationManager.AppSettings("Ambiente").ToString().Equals("Desarrollo") Then
                    logger.Debug("Entrando al Método {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                End If
                '#End Region

                Dim comando As ComandoBase(Of IList(Of FacFacturaPendienteConGru)) = FabricaComandosFacFacturaPendienteConGru.ObtenerComandoConsultarFacFacturaPendienteConGrusFiltro(FacFacturaPendienteConGru)
                comando.Ejecutar()
                retorno = comando.Receptor.ObjetoAlmacenado

                '#Region "trace"
                If ConfigurationManager.AppSettings("Ambiente").ToString().Equals("Desarrollo") Then
                    logger.Debug("Saliendo del Método {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                    '#End Region
                End If
            Catch ex As ApplicationException
                logger.[Error](ex.Message)
                Throw ex
            End Try

            Return retorno
        End Function

    End Class
End Namespace