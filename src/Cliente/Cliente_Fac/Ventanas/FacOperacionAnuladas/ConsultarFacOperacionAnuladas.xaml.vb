﻿Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
'Imports Diginsoft.Bolet.Cliente.Fac.Ayuda
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.FacOperacionAnuladas
Imports Diginsoft.Bolet.Cliente.Fac.Presentadores.FacOperacionAnuladas
Imports Diginsoft.Bolet.Cliente.Fac.Contratos
Imports Trascend.Bolet.Cliente.Ayuda
Namespace Ventanas.FacOperacionAnuladas
    ''' <summary>
    ''' Interaction logic for ConsultarObjetos.xaml
    ''' </summary>
    Partial Public Class ConsultarFacOperacionAnuladas
        Inherits Page
        Implements IConsultarFacOperacionAnuladas


        Private _CurSortCol As GridViewColumnHeader = Nothing
        Private _CurAdorner As SortAdorner = Nothing
        Private _presentador As PresentadorConsultarFacOperacionAnuladas
        Private _cargada As Boolean

#Region "IConsultarFacOperacionAnuladaes"

        Public Property EstaCargada() As Boolean Implements IPaginaBaseFac.EstaCargada
            Get
                Return Me._cargada
            End Get
            Set(ByVal value As Boolean)
                Me._cargada = value
            End Set
        End Property

        Public Sub FocoPredeterminado() Implements IPaginaBaseFac.FocoPredeterminado
            Me._lstId.Focus()
        End Sub

        Public Property FacOperacionAnuladaFiltrar() As Object Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.FacOperacionAnuladaFiltrar
            Get
                Return Me._splFiltro.DataContext
            End Get
            Set(ByVal value As Object)
                Me._splFiltro.DataContext = value
            End Set
        End Property

        Public ReadOnly Property FacOperacionAnuladaSeleccionado() As Object Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.FacOperacionAnuladaSeleccionado
            Get
                Return Me._lstResultados.SelectedItem
            End Get
        End Property


        Public Property Resultados() As Object Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.Resultados
            Get
                Return Me._lstResultados.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstResultados.DataContext = value
            End Set
        End Property

        'Public Property Id() As Integer Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.Id
        '    Get
        '        Return Me._lstId.Text
        '    End Get
        '    Set(ByVal value As Integer)
        '        Me._lstId.Text = value
        '    End Set
        'End Property

        Public Property CurSortCol() As System.Windows.Controls.GridViewColumnHeader Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.CurSortCol
            Get
                Return _CurSortCol
            End Get
            Set(ByVal value As GridViewColumnHeader)
                _CurSortCol = value
            End Set
        End Property

        Public Property CurAdorner() As SortAdorner Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.CurAdorner
            Get
                Return _CurAdorner
            End Get
            Set(ByVal value As SortAdorner)
                _CurAdorner = value
            End Set
        End Property

        Public Property ListaResultados() As System.Windows.Controls.ListView Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.ListaResultados
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

            Me._presentador = New PresentadorConsultarFacOperacionAnuladas(Me)
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

        Private Sub _btnConsultar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._btnConsultar.Focus()
            Me._presentador.Consultar()
        End Sub

        Private Sub _lstResultados_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Me._presentador.IrConsultarFacOperacionAnulada()
        End Sub

        Private Sub _Ordenar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.OrdenarColumna(TryCast(sender, GridViewColumnHeader))
        End Sub


        Private Sub _dpkFechaUltima_SelectedDateChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
            'If Not String.IsNullOrEmpty(Me._dpkFechaUltima.Text) Then
            '    Me._presentador.DeshabilitarDias(Me._dpkFechaBoletinVence, Me._dpkFechaBoletin.SelectedDate.Value.AddDays(-1))
            'End If
        End Sub

        Public Property Asociado As Object Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.Asociado

            Get
                Return Me._lstId.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._lstId.SelectedItem = value
                Me._lstId.ScrollIntoView(value)
            End Set
        End Property

        Public Property Asociados As Object Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.Asociados
            Get
                Return Me._lstId.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstId.DataContext = value
            End Set
        End Property

        Public Property Banco As Object Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.Banco
            Get
                Return Me._cbxBanco.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._cbxBanco.SelectedItem = value
            End Set
        End Property

        Public Property Bancos As Object Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.Bancos
            Get
                Return Me._cbxBanco.DataContext
            End Get
            Set(ByVal value As Object)
                Me._cbxBanco.DataContext = value
            End Set
        End Property

        Public Property Idioma As Object Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.Idioma
            Get
                Return Me._cbxIdioma.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._cbxIdioma.SelectedItem = value
            End Set
        End Property

        Public Property Idiomas As Object Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.Idiomas
            Get
                Return Me._cbxIdioma.DataContext
            End Get
            Set(ByVal value As Object)
                Me._cbxIdioma.DataContext = value
            End Set
        End Property

        Public Property Moneda As Object Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.Moneda
            Get
                Return Me._cbxMoneda.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._cbxMoneda.SelectedItem = value
            End Set
        End Property

        Public Property Monedas As Object Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.Monedas
            Get
                Return Me._cbxMoneda.DataContext
            End Get
            Set(ByVal value As Object)
                Me._cbxMoneda.DataContext = value
            End Set
        End Property

        Private Sub _btnConsultarAsociado_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.BuscarAsociado2()
        End Sub

        Private Sub _txtAsociado_GotFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._txtAsociado.Visibility = System.Windows.Visibility.Collapsed
            Me._lstId.Visibility = System.Windows.Visibility.Visible
            Me._lstId.IsEnabled = True
            Me._btnConsultarAsociado.Visibility = System.Windows.Visibility.Visible
            Me._txtIdAsociado.Visibility = System.Windows.Visibility.Visible
            Me._txtNombreAsociado.Visibility = System.Windows.Visibility.Visible
            Me._lblIdAsociado.Visibility = System.Windows.Visibility.Visible
            Me._lblNombreAsociado.Visibility = System.Windows.Visibility.Visible
        End Sub

        Private Sub _lstAsociados_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Me._presentador.CambiarAsociado()
            Me._lstId.Visibility = System.Windows.Visibility.Collapsed
            Me._btnConsultarAsociado.Visibility = System.Windows.Visibility.Collapsed
            Me._txtIdAsociado.Visibility = System.Windows.Visibility.Collapsed
            Me._txtNombreAsociado.Visibility = System.Windows.Visibility.Collapsed
            Me._txtAsociado.Visibility = System.Windows.Visibility.Visible
            Me._lblIdAsociado.Visibility = System.Windows.Visibility.Collapsed
            Me._lblNombreAsociado.Visibility = System.Windows.Visibility.Collapsed
        End Sub
        Public Property NombreAsociado() As String Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.NombreAsociado
            Get
                Return Me._txtAsociado.Text
            End Get
            Set(ByVal value As String)
                Me._txtAsociado.Text = value
            End Set
        End Property

        Public Property idAsociadoFiltrar() As String Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.idAsociadoFiltrar
            Get
                Return Me._txtIdAsociado.Text
            End Get
            Set(ByVal value As String)
                Me._txtIdAsociado.Text = value
            End Set
        End Property

        Public Property NombreAsociadoFiltrar() As String Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.NombreAsociadoFiltrar
            Get
                Return Me._txtNombreAsociado.Text
            End Get
            Set(ByVal value As String)
                Me._txtNombreAsociado.Text = value
            End Set
        End Property

        Public ReadOnly Property FechaCobro() As String Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.FechaCobro
            Get
                Return Me._dpkFechaCobro.SelectedDate.ToString()
            End Get
        End Property

        Public ReadOnly Property Id() As String Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.Id
            Get
                Return Me._txtId.Text
            End Get
        End Property

        'Public ReadOnly Property CreditoSent As String Implements Contratos.FacOperacionAnuladas.IConsultarFacOperacionAnuladas.CreditoSent
        '    Get
        '        Return Me._txtCreditoSent.Text
        '    End Get
        'End Property
    End Class
End Namespace
