﻿Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports NLog
Imports Diginsoft.Bolet.ObjetosComunes.Entidades
Imports System.Configuration
Imports Diginsoft.Bolet.AccesoDatos.Contrato
Imports Diginsoft.Bolet.AccesoDatos.Fabrica
Imports Trascend.Bolet.Comandos.Comandos
Namespace Comandos.ComandosTarifaServicio
    Public Class ComandoEliminarTarifaServicio
        Inherits ComandoBase(Of Boolean)
        Private Shared logger As Logger = LogManager.GetCurrentClassLogger()
        Private _TarifaServicio As TarifaServicio

        ''' <summary>
        ''' Constructor predeterminado
        ''' </summary>
        ''' <param name="TarifaServicio">TarifaServicio a eliminar</param>
        Public Sub New(ByVal TarifaServicio As TarifaServicio)
            Me._TarifaServicio = TarifaServicio
        End Sub

        ''' <summary>
        ''' Método que ejecuta el comando
        ''' </summary>
        Public Overrides Sub Ejecutar()
            Try
                '#Region "trace"
                If ConfigurationManager.AppSettings("Ambiente").ToString().Equals("Desarrollo") Then
                    logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                End If
                '#End Region

                Dim dao As IDaoTarifaServicio = FabricaDaoBaseFac.ObtenerFabricaDao().ObtenerDaoTarifaServicio()
                Me.Receptor = New Receptor(Of Boolean)(dao.Eliminar(Me._TarifaServicio))

                '#Region "trace"
                If ConfigurationManager.AppSettings("Ambiente").ToString().Equals("Desarrollo") Then
                    logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                    '#End Region
                End If
            Catch ex As ApplicationException
                logger.[Error](ex.Message)
                Throw ex
            End Try
        End Sub
    End Class
End Namespace