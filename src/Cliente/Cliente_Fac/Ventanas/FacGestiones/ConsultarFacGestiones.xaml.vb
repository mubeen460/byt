﻿Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
'Imports Diginsoft.Bolet.Cliente.Fac.Ayuda
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.FacGestiones
Imports Diginsoft.Bolet.Cliente.Fac.Presentadores.FacGestiones
Imports Diginsoft.Bolet.Cliente.Fac.Contratos
Imports Trascend.Bolet.Cliente.Ayuda
Imports Trascend.Bolet.ControlesByT
Namespace Ventanas.FacGestiones
    ''' <summary>
    ''' Interaction logic for ConsultarObjetos.xaml
    ''' </summary>
    Partial Public Class ConsultarFacGestiones
        Inherits Page
        Implements IConsultarFacGestiones

        Private _CurSortCol As GridViewColumnHeader = Nothing
        Private _CurAdorner As SortAdorner = Nothing
        Private _presentador As PresentadorConsultarFacGestiones
        Private _cargada As Boolean

#Region "IConsultarFacGestiones"

        Public Property EstaCargada() As Boolean Implements IPaginaBaseFac.EstaCargada
            Get
                Return Me._cargada
            End Get
            Set(ByVal value As Boolean)
                Me._cargada = value
            End Set
        End Property

        Public Sub FocoPredeterminado() Implements IPaginaBaseFac.FocoPredeterminado
            'Me._lstId.Focus()
        End Sub

        Public Property FacGestionFiltrar() As Object Implements Contratos.FacGestiones.IConsultarFacGestiones.FacGestionFiltrar
            Get
                Return Me._splFiltro.DataContext
            End Get
            Set(ByVal value As Object)
                Me._splFiltro.DataContext = value
            End Set
        End Property

        Public ReadOnly Property FacGestionseleccionado() As Object Implements Contratos.FacGestiones.IConsultarFacGestiones.FacGestionSeleccionado
            Get
                Return Me._lstResultados.SelectedItem
            End Get
        End Property


        Public Property Resultados() As Object Implements Contratos.FacGestiones.IConsultarFacGestiones.Resultados
            Get
                Return Me._lstResultados.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstResultados.DataContext = value
            End Set
        End Property

        'Public Property Id() As Integer Implements Contratos.FacGestiones.IConsultarFacGestiones.Id
        '    Get
        '        Return Me._lstId.Text
        '    End Get
        '    Set(ByVal value As Integer)
        '        Me._lstId.Text = value
        '    End Set
        'End Property

        Public Property CurSortCol() As System.Windows.Controls.GridViewColumnHeader Implements Contratos.FacGestiones.IConsultarFacGestiones.CurSortCol
            Get
                Return _CurSortCol
            End Get
            Set(ByVal value As GridViewColumnHeader)
                _CurSortCol = value
            End Set
        End Property

        Public Property CurAdorner() As SortAdorner Implements Contratos.FacGestiones.IConsultarFacGestiones.CurAdorner
            Get
                Return _CurAdorner
            End Get
            Set(ByVal value As SortAdorner)
                _CurAdorner = value
            End Set
        End Property

        Public Property ListaResultados() As System.Windows.Controls.ListView Implements Contratos.FacGestiones.IConsultarFacGestiones.ListaResultados
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

            Me._presentador = New PresentadorConsultarFacGestiones(Me)
        End Sub

        Private Sub Page_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If Not EstaCargada Then
                Me._presentador.CargarPagina()
                EstaCargada = True
            Else
                Me._presentador.ActualizarTitulo()
            End If
            PintarAsociado()
        End Sub

        Public Sub _btnLimpiar_Click()
            Me._presentador.Limpiar()
        End Sub

        Private Sub _btnCancelar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.Cancelar()
        End Sub

        Private Sub _btnConsultar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._btnConsultar.Focus()
            Me._presentador.Consultar()
        End Sub

        Private Sub _lstResultados_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            If Me._lstResultados.SelectedItem IsNot Nothing Then
                Me._presentador.IrConsultarFacGestion()
            End If
        End Sub

        Private Sub _Ordenar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.OrdenarColumna(TryCast(sender, GridViewColumnHeader))
        End Sub


        Private Sub _dpkFechaUltima_SelectedDateChanged(ByVal sender As Object, ByVal e As SelectionChangedEventArgs)
            'If Not String.IsNullOrEmpty(Me._dpkFechaUltima.Text) Then
            '    Me._presentador.DeshabilitarDias(Me._dpkFechaBoletinVence, Me._dpkFechaBoletin.SelectedDate.Value.AddDays(-1))
            'End If
        End Sub

        Public ReadOnly Property FechaGestion() As String Implements Contratos.FacGestiones.IConsultarFacGestiones.FechaGestion
            Get
                Return Me._dpkFechaGestion.SelectedDate.ToString()
            End Get
        End Property

        'Public ReadOnly Property Id() As String Implements Contratos.FacGestiones.IConsultarFacGestiones.Id
        '    Get
        '        Return Me._txtId.Text
        '    End Get
        'End Property

        Public Property Asociado As Object Implements Contratos.FacGestiones.IConsultarFacGestiones.Asociado

            Get
                Return Me._lstAsociados.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._lstAsociados.SelectedItem = value
                Me._lstAsociados.ScrollIntoView(value)
            End Set
        End Property

        Public Property Asociados As Object Implements Contratos.FacGestiones.IConsultarFacGestiones.Asociados
            Get
                Return Me._lstAsociados.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstAsociados.DataContext = value
            End Set
        End Property

        'Private Sub _btnConsultar(ByVal sender As Object, ByVal e As RoutedEventArgs)
        '    Dim nom As String = DirectCast(sender, Button).Name.ToString
        '    If nom = "_btnConsultarAsociado" Then
        '        Me._presentador.BuscarAsociado2()
        '    ElseIf nom = "_btnCancelar" Then
        '        Me._presentador.Cancelar()
        '    ElseIf nom = "_btnConsulta" Then
        '        Me._presentador.Consultar()
        '    End If
        'End Sub

        Private Sub _btnConsultarAsociado_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.BuscarAsociado2()
        End Sub
        Private Sub _Consultar_Enter(ByVal sender As Object, ByVal e As KeyEventArgs)
            If (e.Key = Key.Enter) Then
                Dim nom As String = DirectCast(sender, ByTTextBox).Name.ToString
                If nom = "_txtIdAsociado" Or nom = "_txtNombreAsociado" Then
                    Me._presentador.BuscarAsociado2()
                Else
                    Me._presentador.Consultar()
                End If
            End If
        End Sub

        Private Sub _txtAsociado_MouseDoubleClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ControlesMostrarAsociado()
        End Sub

        Private Sub ControlesMostrarAsociado()
            Me._txtAsociado.Visibility = System.Windows.Visibility.Collapsed
            Me._lblasociado2.Visibility = System.Windows.Visibility.Collapsed
            'Me._txtAsociadoId.Visibility = System.Windows.Visibility.Collapsed
            Me._lstAsociados.Visibility = System.Windows.Visibility.Visible
            Me._lstAsociados.IsEnabled = True
            Me._btnConsultarAsociado.Visibility = System.Windows.Visibility.Visible
            Me._txtIdAsociado.Visibility = System.Windows.Visibility.Visible
            Me._txtNombreAsociado.Visibility = System.Windows.Visibility.Visible
            Me._lblIdAsociado.Visibility = System.Windows.Visibility.Visible
            Me._lblNombreAsociado.Visibility = System.Windows.Visibility.Visible
            Me._lblasociado.Visibility = System.Windows.Visibility.Visible
        End Sub

        Private Sub _lstAsociados_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            If Me._lstAsociados.SelectedItem IsNot Nothing Then
                Me._presentador.CambiarAsociado()
                ControlesOcultarAsociado()
            End If
        End Sub

        Private Sub ControlesOcultarAsociado()
            Me._lstAsociados.Visibility = System.Windows.Visibility.Collapsed
            Me._btnConsultarAsociado.Visibility = System.Windows.Visibility.Collapsed
            Me._txtIdAsociado.Visibility = System.Windows.Visibility.Collapsed
            Me._txtNombreAsociado.Visibility = System.Windows.Visibility.Collapsed
            Me._lblasociado.Visibility = System.Windows.Visibility.Collapsed
            Me._txtAsociado.Visibility = System.Windows.Visibility.Visible
            Me._lblasociado2.Visibility = System.Windows.Visibility.Visible
            'Me._txtAsociadoId.Visibility = System.Windows.Visibility.Visible
            Me._lblIdAsociado.Visibility = System.Windows.Visibility.Collapsed
            Me._lblNombreAsociado.Visibility = System.Windows.Visibility.Collapsed
        End Sub

        Public Sub PintarAsociado()
            Me._txtAsociado.BorderBrush = New SolidColorBrush(Colors.LightGreen)
        End Sub

        Public Property NombreAsociado() As String Implements Contratos.FacGestiones.IConsultarFacGestiones.NombreAsociado
            Get
                Return Me._txtAsociado.Text
            End Get
            Set(ByVal value As String)
                Me._txtAsociado.Text = value
            End Set
        End Property

        Public Property idAsociadoFiltrar() As String Implements Contratos.FacGestiones.IConsultarFacGestiones.idAsociadoFiltrar
            Get
                Return Me._txtIdAsociado.Text
            End Get
            Set(ByVal value As String)
                Me._txtIdAsociado.Text = value
            End Set
        End Property

        Public Property NombreAsociadoFiltrar() As String Implements Contratos.FacGestiones.IConsultarFacGestiones.NombreAsociadoFiltrar
            Get
                Return Me._txtNombreAsociado.Text
            End Get
            Set(ByVal value As String)
                Me._txtNombreAsociado.Text = value
            End Set
        End Property


        Public Property Medio As Object Implements Contratos.FacGestiones.IConsultarFacGestiones.Medio
            Get
                Return Me._cbxMedioGestion.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._cbxMedioGestion.SelectedItem = value
            End Set
        End Property

        Public Property Medios As Object Implements Contratos.FacGestiones.IConsultarFacGestiones.Medios
            Get
                Return Me._cbxMedioGestion.DataContext
            End Get
            Set(ByVal value As Object)
                Me._cbxMedioGestion.DataContext = value
            End Set
        End Property

        Public Property Concepto As Object Implements Contratos.FacGestiones.IConsultarFacGestiones.Concepto
            Get
                Return Me._cbxConceptoGestion.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._cbxConceptoGestion.SelectedItem = value
            End Set
        End Property

        Public Property Conceptos As Object Implements Contratos.FacGestiones.IConsultarFacGestiones.Conceptos
            Get
                Return Me._cbxConceptoGestion.DataContext
            End Get
            Set(ByVal value As Object)
                Me._cbxConceptoGestion.DataContext = value
            End Set
        End Property

        Public Property Id As String Implements Contratos.FacGestiones.IConsultarFacGestiones.Id
            Get
                Return (_txtId.Text)
            End Get
            Set(ByVal value As String)
                _txtId.Text = value
            End Set
        End Property

        Public WriteOnly Property Count As Integer Implements Contratos.FacGestiones.IConsultarFacGestiones.Count
            Set(value As Integer)
                _lblHits.Text = value
            End Set
        End Property

        Public Property Inicial As String Implements Contratos.FacGestiones.IConsultarFacGestiones.Inicial
            Get
                Return _txtInicial.Text
            End Get
            Set(value As String)
                _txtInicial.Text = value
            End Set
        End Property

        Public Property Observacion As String Implements Contratos.FacGestiones.IConsultarFacGestiones.Observacion
            Get
                Return _txtObservacion.Text
            End Get
            Set(value As String)
                _txtObservacion.Text = value
            End Set
        End Property
    End Class
End Namespace
