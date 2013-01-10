﻿Imports System.Windows
Imports System.Windows.Controls
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.TipoPatentes
Imports Diginsoft.Bolet.Cliente.Fac.Presentadores.TipoPatentes
Imports Diginsoft.Bolet.Cliente.Fac.Contratos

Namespace Ventanas.TipoPatentes
    Partial Public Class AgregarTipoPatente
        Inherits Page
        Implements IAgregarTipoPatente

        Private _presentador As PresentadorAgregarTipoPatente
        Private _cargada As Boolean

#Region "IAgregarTipoPatente"

        Public Property EstaCargada() As Boolean Implements IPaginaBaseFac.EstaCargada
            Get
                Return Me._cargada
            End Get
            Set(ByVal value As Boolean)
                Me._cargada = value
            End Set
        End Property

        Public Sub FocoPredeterminado() Implements IPaginaBaseFac.FocoPredeterminado
            Me._txtId.Focus()
        End Sub

        Public Property TipoPatente() As Object Implements Contratos.TipoPatentes.IAgregarTipoPatente.TipoPatente
            Get
                Return Me._gridDatos.DataContext
            End Get
            Set(ByVal value As Object)
                Me._gridDatos.DataContext = value
            End Set
        End Property

        Public Sub Mensaje(ByVal mensaje__1 As String) Implements Contratos.TipoPatentes.IAgregarTipoPatente.Mensaje
            MessageBox.Show(mensaje__1, "Error", MessageBoxButton.OK, MessageBoxImage.[Error])
        End Sub


#End Region

        Public Sub New()
            InitializeComponent()
            Me._cargada = False
            Me._presentador = New PresentadorAgregarTipoPatente(Me)
        End Sub

        Private Sub _btnCancelar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.Cancelar()
        End Sub

        Private Sub _btnAceptar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.Aceptar()
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

    End Class
End Namespace
