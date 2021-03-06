﻿Imports System.Collections.Generic
Imports System.Configuration
Imports NLog
Imports Diginsoft.Bolet.AccesoDatos.Contrato
Imports Diginsoft.Bolet.AccesoDatos.Fabrica
Imports Trascend.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.Comandos.Comandos

Namespace Comandos.ComandosDesgloseServicioTarifa
    Public Class ComandoConsultarFacDesgloseServicioTarifaFiltro
        Inherits ComandoBase(Of IList(Of FacDesgloseServicioTarifa))
        Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

        Private _FacDesgloseServicioTarifa As FacDesgloseServicioTarifa

        ''' <summary>
        ''' Constructor por defecto
        ''' </summary>
        ''' <param name="FacDesgloseServicioTarifa">FacDesgloseTarifa que sirve como filtro para la consulta</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal FacDesgloseServicioTarifa As FacDesgloseServicioTarifa)
            Me._FacDesgloseServicioTarifa = FacDesgloseServicioTarifa
        End Sub

        ''' <summary>
        ''' Metodo que ejecuta el comando
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Sub Ejecutar()
            Try
                '#Region "trace"
                If ConfigurationManager.AppSettings("Ambiente").ToString().Equals("Desarrollo") Then
                    logger.Debug("Entrando al Método {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                End If
                '#End Region

                Dim dao As IDaoDesgloseServicioTarifa = FabricaDaoBaseFac.ObtenerFabricaDao().ObtenerDaoDesgloseServicioTarifa()
                Me.Receptor = New Receptor(Of IList(Of FacDesgloseServicioTarifa))(dao.ObtenerFacDesgloseServiciosTarifaFiltro(Me._FacDesgloseServicioTarifa))

                '#Region "trace"
                If ConfigurationManager.AppSettings("Ambiente").ToString().Equals("Desarrollo") Then
                    logger.Debug("Saliendo del Método {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                    '#End Region
                End If
            Catch ex As ApplicationException
                logger.[Error](ex.Message)
                Throw ex
            End Try
        End Sub

    End Class

End Namespace
