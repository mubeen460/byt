﻿Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.FacReportes
Imports Diginsoft.Bolet.Cliente.Fac.Presentadores.FacReportes
Imports Diginsoft.Bolet.Cliente.Fac.Contratos
Imports Trascend.Bolet.ControlesByT
Namespace Ventanas.FacReportes
    Partial Public Class FacturaEncabezado
        Inherits Page
        Implements IFacturaEncabezado

        Private _presentador As PresentadorFacturaEncabezado
        Private _cargada As Boolean

#Region "IFacturaEncabezado"

        Public Property EstaCargada() As Boolean Implements IPaginaBaseFac.EstaCargada
            Get
                Return Me._cargada
            End Get
            Set(ByVal value As Boolean)
                Me._cargada = value
            End Set
        End Property

        Public Sub FocoPredeterminado() Implements IPaginaBaseFac.FocoPredeterminado

        End Sub

        Public Sub Mensaje(ByVal mensaje__1 As String) Implements Contratos.FacReportes.IFacturaEncabezado.Mensaje
            MessageBox.Show(mensaje__1, "Error", MessageBoxButton.OK, MessageBoxImage.[Error])
        End Sub
#End Region

        Public Sub New()
            InitializeComponent()
            Me._cargada = False
            Me._presentador = New PresentadorFacturaEncabezado(Me)
        End Sub

        Private Sub _btnImprimir_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.generar_txt()
        End Sub

        Private Sub _btnRegresar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.Regresar()
        End Sub

        Private Sub Page_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If Not EstaCargada Then
                Me._presentador.CargarPagina()
                EstaCargada = True

            End If
        End Sub

        Public Sub _btnLimpiar_Click()
            Me._presentador.Limpiar()
        End Sub

        Public Property FechaInicio As String Implements Contratos.FacReportes.IFacturaEncabezado.FechaInicio
            Get
                Return _dpkFechaInicio.Text
            End Get
            Set(ByVal value As String)
                _dpkFechaInicio.Text = value
            End Set
        End Property

        Public Property FechaFin As String Implements Contratos.FacReportes.IFacturaEncabezado.FechaFin
            Get
                Return _dpkFechaFin.Text
            End Get
            Set(ByVal value As String)
                _dpkFechaFin.Text = value
            End Set
        End Property

    End Class
End Namespace
