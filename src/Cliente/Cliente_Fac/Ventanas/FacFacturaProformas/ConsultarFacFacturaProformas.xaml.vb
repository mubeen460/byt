﻿Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
'Imports Diginsoft.Bolet.Cliente.Fac.Ayuda
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.FacFacturaProformas
Imports Diginsoft.Bolet.Cliente.Fac.Presentadores.FacFacturaProformas
Imports Diginsoft.Bolet.Cliente.Fac.Contratos
Imports Trascend.Bolet.Cliente.Ayuda
Imports Trascend.Bolet.ControlesByT
Namespace Ventanas.FacFacturaProformas
    ''' <summary>
    ''' Interaction logic for ConsultarObjetos.xaml
    ''' </summary>
    Partial Public Class ConsultarFacFacturaProformas
        Inherits Page
        Implements IConsultarFacFacturaProformas

        Private _CurSortCol As GridViewColumnHeader = Nothing
        Private _CurAdorner As SortAdorner = Nothing
        Private _presentador As PresentadorConsultarFacFacturaProformas
        Private _cargada As Boolean

#Region "IConsultarFacFacturaProformas"

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

        Public Property FacFacturaProformaFiltrar() As Object Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.FacFacturaProformaFiltrar
            Get
                Return Me._splFiltro.DataContext
            End Get
            Set(ByVal value As Object)
                Me._splFiltro.DataContext = value
            End Set
        End Property

        Public ReadOnly Property FacFacturaProformaSeleccionado() As Object Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.FacFacturaProformaSeleccionado
            Get
                Return Me._lstResultados.SelectedItem
            End Get
        End Property


        Public Property Resultados() As Object Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Resultados
            Get
                Return Me._lstResultados.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstResultados.DataContext = value
            End Set
        End Property

        'Public Property Id() As Integer Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Id
        '    Get
        '        Return Me._lstId.Text
        '    End Get
        '    Set(ByVal value As Integer)
        '        Me._lstId.Text = value
        '    End Set
        'End Property

        Public Property CurSortCol() As System.Windows.Controls.GridViewColumnHeader Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.CurSortCol
            Get
                Return _CurSortCol
            End Get
            Set(ByVal value As GridViewColumnHeader)
                _CurSortCol = value
            End Set
        End Property

        Public Property CurAdorner() As SortAdorner Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.CurAdorner
            Get
                Return _CurAdorner
            End Get
            Set(ByVal value As SortAdorner)
                _CurAdorner = value
            End Set
        End Property

        Public Property ListaResultados() As System.Windows.Controls.ListView Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.ListaResultados
            Get
                Return Me._lstResultados
            End Get
            Set(ByVal value As ListView)
                Me._lstResultados = value
            End Set
        End Property

        Public Property OrigenesProforma As Object Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.OrigenesProforma
            Get
                Return Me._cbxOrigenProforma.DataContext
            End Get
            Set(value As Object)
                Me._cbxOrigenProforma.DataContext = value
            End Set
        End Property

        Public Property OrigenProforma As Object Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.OrigenProforma
            Get
                Return Me._cbxOrigenProforma.SelectedItem
            End Get
            Set(value As Object)
                Me._cbxOrigenProforma.SelectedItem = value
            End Set
        End Property

        Public Sub Mensaje(ByVal mensaje__1 As String) Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Mensaje
            MessageBox.Show(mensaje__1, "Error", MessageBoxButton.OK, MessageBoxImage.[Error])
        End Sub

#End Region

        Public Sub New()
            InitializeComponent()
            Me._cargada = False

            Me._presentador = New PresentadorConsultarFacFacturaProformas(Me)
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

        'Private Sub _btnCancelar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        '    Me._presentador.Cancelar()
        'End Sub

        'Private Sub _btnConsultar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        '    'Me._btnConsultar.Focus()
        '    Me._presentador.Consultar()
        'End Sub

        Private Sub _lstResultados_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            If Me._lstResultados.SelectedItem IsNot Nothing Then
                Me._presentador.IrConsultarFacFacturaProforma()
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

        Public Property Asociado As Object Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Asociado

            Get
                Return Me._lstAsociados.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._lstAsociados.SelectedItem = value
                Me._lstAsociados.ScrollIntoView(value)
            End Set
        End Property

        Public Property Asociados As Object Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Asociados
            Get
                Return Me._lstAsociados.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstAsociados.DataContext = value
            End Set
        End Property

        Private Sub _btnConsultar(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim nom As String = DirectCast(sender, Button).Name.ToString
            If nom = "_btnConsultarAsociado" Then
                Me._presentador.BuscarAsociado2()
            ElseIf nom = "_btnCancelar" Then
                Me._presentador.Cancelar()
            ElseIf nom = "_btnConsulta" Then
                'Me._btnConsulta.Focus()
                Me._presentador.Consultar()
                validarCamposVacios()
            End If
            validarCamposVacios()
        End Sub
        'Public Property Banco As Object Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Banco
        '    Get
        '        Return Me._cbxBanco.SelectedItem
        '    End Get
        '    Set(ByVal value As Object)
        '        Me._cbxBanco.SelectedItem = value
        '    End Set
        'End Property

        'Public Property Bancos As Object Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Bancos
        '    Get
        '        Return Me._cbxBanco.DataContext
        '    End Get
        '    Set(ByVal value As Object)
        '        Me._cbxBanco.DataContext = value
        '    End Set
        'End Property

        'Public Property Idioma As Object Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Idioma
        '    Get
        '        Return Me._cbxIdioma.SelectedItem
        '    End Get
        '    Set(ByVal value As Object)
        '        Me._cbxIdioma.SelectedItem = value
        '    End Set
        'End Property

        'Public Property Idiomas As Object Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Idiomas
        '    Get
        '        Return Me._cbxIdioma.DataContext
        '    End Get
        '    Set(ByVal value As Object)
        '        Me._cbxIdioma.DataContext = value
        '    End Set
        'End Property

        'Public Property Moneda As Object Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Moneda
        '    Get
        '        Return Me._cbxMoneda.SelectedItem
        '    End Get
        '    Set(ByVal value As Object)
        '        Me._cbxMoneda.SelectedItem = value
        '    End Set
        'End Property

        'Public Property Monedas As Object Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Monedas
        '    Get
        '        Return Me._cbxMoneda.DataContext
        '    End Get
        '    Set(ByVal value As Object)
        '        Me._cbxMoneda.DataContext = value
        '    End Set
        'End Property

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
                Me._btnConsulta.Focus()
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

        Public Property NombreAsociado() As String Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.NombreAsociado
            Get
                Return Me._txtAsociado.Text
            End Get
            Set(ByVal value As String)
                Me._txtAsociado.Text = value
            End Set
        End Property

        Public Property idAsociadoFiltrar() As String Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.idAsociadoFiltrar
            Get
                Return Me._txtIdAsociado.Text
            End Get
            Set(ByVal value As String)
                Me._txtIdAsociado.Text = value
            End Set
        End Property

        Public Property NombreAsociadoFiltrar() As String Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.NombreAsociadoFiltrar
            Get
                Return Me._txtNombreAsociado.Text
            End Get
            Set(ByVal value As String)
                Me._txtNombreAsociado.Text = value
            End Set
        End Property

        Public ReadOnly Property FechaFactura() As String Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.FechaFactura
            Get
                Return Me._dpkFechaFactura.SelectedDate.ToString()
            End Get
        End Property

        Public ReadOnly Property Id() As String Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Id
            Get
                Return Me._txtId.Text
            End Get
        End Property

        'Public ReadOnly Property CreditoSent As String Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.CreditoSent
        '    Get
        '        Return Me._txtCreditoSent.Text
        '    End Get
        'End Property


        Public WriteOnly Property Cba As Double Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Cba
            Set(ByVal value As Double)
                Me._txtCba.Text = value
            End Set
        End Property

        Public WriteOnly Property Cbf As Double Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Cbf
            Set(ByVal value As Double)
                Me._txtCbf.Text = value
            End Set
        End Property

        Public WriteOnly Property Cbp As Double Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Cbp
            Set(ByVal value As Double)
                Me._txtCbp.Text = value
            End Set
        End Property


        Public WriteOnly Property Cbr As Double Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Cbr
            Set(ByVal value As Double)
                Me._txtCbr.Text = value
            End Set
        End Property

        Public WriteOnly Property Cda As Double Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Cda
            Set(ByVal value As Double)
                Me._txtCda.Text = value
            End Set
        End Property

        Public WriteOnly Property Cdf As Double Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Cdf
            Set(ByVal value As Double)
                Me._txtCdf.Text = value
            End Set
        End Property

        Public WriteOnly Property Cdp As Double Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Cdp
            Set(ByVal value As Double)
                Me._txtCdp.Text = value
            End Set
        End Property

        Public WriteOnly Property Cdr As Double Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Cdr
            Set(ByVal value As Double)
                Me._txtCdr.Text = value
            End Set
        End Property

        Public WriteOnly Property Count As Integer Implements Contratos.FacFacturaProformas.IConsultarFacFacturaProformas.Count
            Set(value As Integer)
                _lblHits.Text = value
            End Set
        End Property

        Private Sub validarCamposVacios()

            Dim todosCamposVacios As Boolean
            todosCamposVacios = True

            If Me._txtId.Text <> "" Then
                todosCamposVacios = False
                Me._txtId.Focus()
            End If

            If (Me._cbxOrigenProforma.SelectedIndex <> 0) AndAlso (Me._cbxOrigenProforma.SelectedIndex <> -1) Then
                todosCamposVacios = False
                Me._cbxOrigenProforma.Focus()
            End If

            If Me._dpkFechaFactura.Text <> "" Then
                todosCamposVacios = False
                Me._dpkFechaFactura.Focus()
            End If

            If todosCamposVacios Then
                Me._txtId.Focus()
            End If

        End Sub

        Private Sub _cbxOrigenProforma_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs)
            Me._btnConsulta.Focus()
        End Sub

        
        
        Private Sub _dpkFechaFactura_SelectedDateChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs)
            'MessageBox.Show("Escogi una fecha")
            If (Me._dpkFechaFactura.SelectedDate IsNot Nothing) Then
                'MessageBox.Show("La fecha no es nula")
                Me._btnConsulta.IsDefault = True
                Me._btnConsulta.Focus()
                'MessageBox.Show("El boton consultar se resalto")
            End If

        End Sub

        

    End Class
End Namespace
