﻿Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
'Imports Diginsoft.Bolet.Cliente.Fac.Ayuda
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.FacOperacionProformas
Imports Diginsoft.Bolet.Cliente.Fac.Presentadores.FacOperacionProformas
Imports Diginsoft.Bolet.Cliente.Fac.Contratos
Imports Trascend.Bolet.Cliente.Ayuda
Namespace Ventanas.FacOperacionProformas
    ''' <summary>
    ''' Interaction logic for ConsultarObjetos.xaml
    ''' </summary>
    Partial Public Class ConsultarDepositoFacOperacionProformas
        Inherits Page
        Implements IConsultarDepositoFacOperacionProformas

        Dim value As Boolean = False
        Private _CurSortCol As GridViewColumnHeader = Nothing
        Private _CurAdorner As SortAdorner = Nothing
        Private _presentador As PresentadorConsultarDepositoFacOperacionProformas
        Private _cargada As Boolean

#Region "IConsultarFacOperacionProformaes"

        Public Property EstaCargada() As Boolean Implements IPaginaBaseFac.EstaCargada
            Get
                Return Me._cargada
            End Get
            Set(ByVal value As Boolean)
                Me._cargada = value
            End Set
        End Property

        Public Sub FocoPredeterminado() Implements IPaginaBaseFac.FocoPredeterminado
            Me._txtNDeposito.Focus()
        End Sub

        Public Property FacOperacionProformaFiltrar() As Object Implements Contratos.FacOperacionProformas.IConsultarDepositoFacOperacionProformas.FacOperacionProformaFiltrar
            Get
                Return Me._splFiltro.DataContext
            End Get
            Set(ByVal value As Object)
                Me._splFiltro.DataContext = value
            End Set
        End Property

        Public ReadOnly Property FacOperacionProformaSeleccionado() As Object Implements Contratos.FacOperacionProformas.IConsultarDepositoFacOperacionProformas.FacOperacionProformaSeleccionado
            Get
                Return Me._lstResultados.SelectedItem
            End Get
        End Property


        Public Property Resultados() As Object Implements Contratos.FacOperacionProformas.IConsultarDepositoFacOperacionProformas.Resultados
            Get
                Return Me._lstResultados.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstResultados.DataContext = value
            End Set
        End Property

        'Public Property Id() As Integer Implements Contratos.FacOperacionProformas.IConsultarDepositoFacOperacionProformas.Id
        '    Get
        '        Return Me._lstId.Text
        '    End Get
        '    Set(ByVal value As Integer)
        '        Me._lstId.Text = value
        '    End Set
        'End Property

        Public Property CurSortCol() As System.Windows.Controls.GridViewColumnHeader Implements Contratos.FacOperacionProformas.IConsultarDepositoFacOperacionProformas.CurSortCol
            Get
                Return _CurSortCol
            End Get
            Set(ByVal value As GridViewColumnHeader)
                _CurSortCol = value
            End Set
        End Property

        Public Property CurAdorner() As SortAdorner Implements Contratos.FacOperacionProformas.IConsultarDepositoFacOperacionProformas.CurAdorner
            Get
                Return _CurAdorner
            End Get
            Set(ByVal value As SortAdorner)
                _CurAdorner = value
            End Set
        End Property

        Public Property ListaResultados() As System.Windows.Controls.ListView Implements Contratos.FacOperacionProformas.IConsultarDepositoFacOperacionProformas.ListaResultados
            Get
                Return Me._lstResultados
            End Get
            Set(ByVal value As ListView)
                Me._lstResultados = value
            End Set
        End Property

#End Region

        Public Sub New()
            InitializeComponent()
            Me._cargada = False

            Me._presentador = New PresentadorConsultarDepositoFacOperacionProformas(Me)
        End Sub

        Private Sub Page_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If Not EstaCargada Then
                Me._presentador.CargarPagina()
                EstaCargada = True
            Else
                Me._presentador.ActualizarTitulo()
            End If
        End Sub

        Private Sub _btnCancelar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.Cancelar()
        End Sub

        Private Sub _btnAplicarDeposito_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._btnAplicarDeposito.Focus()
            Me._presentador.AplicarDeposito()
        End Sub


        Private Sub _Ordenar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.OrdenarColumna(TryCast(sender, GridViewColumnHeader))
        End Sub


        Public Property Banco As Object Implements Contratos.FacOperacionProformas.IConsultarDepositoFacOperacionProformas.Banco
            Get
                Return Me._cbxBanco.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._cbxBanco.SelectedItem = value
            End Set
        End Property

        Public Property Bancos As Object Implements Contratos.FacOperacionProformas.IConsultarDepositoFacOperacionProformas.Bancos
            Get
                Return Me._cbxBanco.DataContext
            End Get
            Set(ByVal value As Object)
                Me._cbxBanco.DataContext = value
            End Set
        End Property


        Public ReadOnly Property FechaDeposito As String Implements Contratos.FacOperacionProformas.IConsultarDepositoFacOperacionProformas.FechaDeposito
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Private Sub _btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles _btnSeleccionar.Click
            If (value = False) Then
                value = True
            Else
                value = False
            End If
            Me._presentador.seleccionar(value)
        End Sub


        Public Property NReg As Integer Implements Contratos.FacOperacionProformas.IConsultarDepositoFacOperacionProformas.NReg
            Get
                Return Me._txtNReg.Text
            End Get
            Set(ByVal value As Integer)
                Me._txtNReg.Text = value
            End Set
        End Property

        Public Property Tmonto As Double Implements Contratos.FacOperacionProformas.IConsultarDepositoFacOperacionProformas.Tmonto
            Get
                Return Me._txtTMonto.Text
            End Get
            Set(ByVal value As Double)
                Me._txtTMonto.Text = value
            End Set
        End Property
    End Class
End Namespace
