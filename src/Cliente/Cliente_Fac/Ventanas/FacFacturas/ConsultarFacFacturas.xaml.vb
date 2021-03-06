﻿Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
'Imports Diginsoft.Bolet.Cliente.Fac.Ayuda
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.FacFacturas
Imports Diginsoft.Bolet.Cliente.Fac.Presentadores.FacFacturas
Imports Diginsoft.Bolet.Cliente.Fac.Contratos
Imports Trascend.Bolet.Cliente.Ayuda
Imports Trascend.Bolet.ControlesByT
Namespace Ventanas.FacFacturas
    ''' <summary>
    ''' Interaction logic for ConsultarObjetos.xaml
    ''' </summary>
    Partial Public Class ConsultarFacFacturas
        Inherits Page
        Implements IConsultarFacFacturas

        Private _CurSortCol As GridViewColumnHeader = Nothing
        Private _CurAdorner As SortAdorner = Nothing
        Private _presentador As PresentadorConsultarFacFacturas
        Private _cargada As Boolean

#Region "IConsultarFacFacturas"

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

        Public Property FacFacturaFiltrar() As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.FacFacturaFiltrar
            Get
                Return Me._splFiltro.DataContext
            End Get
            Set(ByVal value As Object)
                Me._splFiltro.DataContext = value
            End Set
        End Property

        Public ReadOnly Property FacFacturaSeleccionado() As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.FacFacturaSeleccionado
            Get
                Return Me._lstResultados.SelectedItem
            End Get
        End Property


        Public Property Resultados() As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.Resultados
            Get
                Return Me._lstResultados.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstResultados.DataContext = value
            End Set
        End Property

        'Public Property Id() As Integer Implements Contratos.FacFacturas.IConsultarFacFacturas.Id
        '    Get
        '        Return Me._lstId.Text
        '    End Get
        '    Set(ByVal value As Integer)
        '        Me._lstId.Text = value
        '    End Set
        'End Property

        Public Property CurSortCol() As System.Windows.Controls.GridViewColumnHeader Implements Contratos.FacFacturas.IConsultarFacFacturas.CurSortCol
            Get
                Return _CurSortCol
            End Get
            Set(ByVal value As GridViewColumnHeader)
                _CurSortCol = value
            End Set
        End Property

        Public Property CurAdorner() As SortAdorner Implements Contratos.FacFacturas.IConsultarFacFacturas.CurAdorner
            Get
                Return _CurAdorner
            End Get
            Set(ByVal value As SortAdorner)
                _CurAdorner = value
            End Set
        End Property

        Public Property ListaResultados() As System.Windows.Controls.ListView Implements Contratos.FacFacturas.IConsultarFacFacturas.ListaResultados
            Get
                Return Me._lstResultados
            End Get
            Set(ByVal value As ListView)
                Me._lstResultados = value
            End Set
        End Property

        Public Property OrigenesFactura As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.OrigenesFactura
            Get
                Return Me._cbxOrigenFactura.DataContext
            End Get
            Set(value As Object)
                Me._cbxOrigenFactura.DataContext = value
            End Set
        End Property

        Public Property OrigenFactura As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.OrigenFactura
            Get
                Return Me._cbxOrigenFactura.SelectedItem
            End Get
            Set(value As Object)
                Me._cbxOrigenFactura.SelectedItem = value
            End Set
        End Property

#End Region

        Public Sub New()
            InitializeComponent()
            Me._cargada = False

            Me._presentador = New PresentadorConsultarFacFacturas(Me)
        End Sub

        Private Sub Page_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If Not EstaCargada Then
                Me._presentador.CargarPagina()
                EstaCargada = True
            Else
                Me._presentador.ActualizarTitulo()
            End If
            PintarAsociado()
            PintarCarta()
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
                Me._presentador.IrConsultarFacFactura()
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

        Public Property Asociado As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.Asociado

            Get
                Return Me._lstAsociados.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._lstAsociados.SelectedItem = value
                Me._lstAsociados.ScrollIntoView(value)
            End Set
        End Property

        Public Property Asociados As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.Asociados
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
            ElseIf nom = "_btnConsultarCarta" Then
                Me._presentador.BuscarCarta()
            ElseIf nom = "_btnCancelar" Then
                Me._presentador.Cancelar()
                'Me._presentador.reporte()
            ElseIf nom = "_btnConsulta" Then
                Me._presentador.Consultar()
                validarCamposVacios()
                Me._btnConsulta.Focus()
            End If
        End Sub
        'Public Property Banco As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.Banco
        '    Get
        '        Return Me._cbxBanco.SelectedItem
        '    End Get
        '    Set(ByVal value As Object)
        '        Me._cbxBanco.SelectedItem = value
        '    End Set
        'End Property

        'Public Property Bancos As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.Bancos
        '    Get
        '        Return Me._cbxBanco.DataContext
        '    End Get
        '    Set(ByVal value As Object)
        '        Me._cbxBanco.DataContext = value
        '    End Set
        'End Property

        'Public Property Idioma As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.Idioma
        '    Get
        '        Return Me._cbxIdioma.SelectedItem
        '    End Get
        '    Set(ByVal value As Object)
        '        Me._cbxIdioma.SelectedItem = value
        '    End Set
        'End Property

        'Public Property Idiomas As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.Idiomas
        '    Get
        '        Return Me._cbxIdioma.DataContext
        '    End Get
        '    Set(ByVal value As Object)
        '        Me._cbxIdioma.DataContext = value
        '    End Set
        'End Property

        'Public Property Moneda As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.Moneda
        '    Get
        '        Return Me._cbxMoneda.SelectedItem
        '    End Get
        '    Set(ByVal value As Object)
        '        Me._cbxMoneda.SelectedItem = value
        '    End Set
        'End Property

        'Public Property Monedas As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.Monedas
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
                ElseIf nom = "_txtIdCarta" Or nom = "_txtNombreCarta" Or nom = "_dpkFechaCarta" Then
                    Me._presentador.BuscarCarta()
                Else
                    Me._presentador.Consultar()
                    validarCamposVacios()
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
            ControlesOcultarCarta()
        End Sub

        Private Sub _lstAsociados_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            If Me._lstAsociados.SelectedItem IsNot Nothing Then
                Me._presentador.CambiarAsociado()
                ControlesOcultarAsociado()
                Me._btnConsulta.IsDefault = True
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

        Public Property NombreAsociado() As String Implements Contratos.FacFacturas.IConsultarFacFacturas.NombreAsociado
            Get
                Return Me._txtAsociado.Text
            End Get
            Set(ByVal value As String)
                Me._txtAsociado.Text = value
            End Set
        End Property

        Public Property idAsociadoFiltrar() As String Implements Contratos.FacFacturas.IConsultarFacFacturas.idAsociadoFiltrar
            Get
                Return Me._txtIdAsociado.Text
            End Get
            Set(ByVal value As String)
                Me._txtIdAsociado.Text = value
            End Set
        End Property

        Public Property NombreAsociadoFiltrar() As String Implements Contratos.FacFacturas.IConsultarFacFacturas.NombreAsociadoFiltrar
            Get
                Return Me._txtNombreAsociado.Text
            End Get
            Set(ByVal value As String)
                Me._txtNombreAsociado.Text = value
            End Set
        End Property

        Public ReadOnly Property FechaFactura() As String Implements Contratos.FacFacturas.IConsultarFacFacturas.FechaFactura
            Get
                Return Me._dpkFechaFactura.SelectedDate.ToString()
            End Get
        End Property

        Public ReadOnly Property Id() As String Implements Contratos.FacFacturas.IConsultarFacFacturas.Id
            Get
                Return Me._txtId.Text
            End Get
        End Property

        'Public ReadOnly Property CreditoSent As String Implements Contratos.FacFacturas.IConsultarFacFacturas.CreditoSent
        '    Get
        '        Return Me._txtCreditoSent.Text
        '    End Get
        'End Property


        Public WriteOnly Property Cba As Double Implements Contratos.FacFacturas.IConsultarFacFacturas.Cba
            Set(ByVal value As Double)
                'Me._txtCba.Text = value
            End Set
        End Property

        Public WriteOnly Property Cbf As Double Implements Contratos.FacFacturas.IConsultarFacFacturas.Cbf
            Set(ByVal value As Double)
                'Me._txtCbf.Text = value
            End Set
        End Property

        Public WriteOnly Property Cbp As Double Implements Contratos.FacFacturas.IConsultarFacFacturas.Cbp
            Set(ByVal value As Double)
                'Me._txtCbp.Text = value
            End Set
        End Property

        Public WriteOnly Property Cbr As Double Implements Contratos.FacFacturas.IConsultarFacFacturas.Cbr
            Set(ByVal value As Double)
                ' Me._txtCbr.Text = value
            End Set
        End Property

        Public WriteOnly Property Cda As Double Implements Contratos.FacFacturas.IConsultarFacFacturas.Cda
            Set(ByVal value As Double)
                'Me._txtCda.Text = value
            End Set
        End Property

        Public WriteOnly Property Cdf As Double Implements Contratos.FacFacturas.IConsultarFacFacturas.Cdf
            Set(ByVal value As Double)
                ' Me._txtCdf.Text = value
            End Set
        End Property

        Public WriteOnly Property Cdp As Double Implements Contratos.FacFacturas.IConsultarFacFacturas.Cdp
            Set(ByVal value As Double)
                ' Me._txtCdp.Text = value
            End Set
        End Property

        Public WriteOnly Property Cdr As Double Implements Contratos.FacFacturas.IConsultarFacFacturas.Cdr
            Set(ByVal value As Double)
                'Me._txtCdr.Text = value
            End Set
        End Property


        Public Property Carta As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.Carta

            Get
                Return Me._lstCartas.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._lstCartas.SelectedItem = value
                Me._lstCartas.ScrollIntoView(value)
            End Set
        End Property

        Public Property Cartas As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.Cartas
            Get
                Return Me._lstCartas.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstCartas.DataContext = value
            End Set
        End Property

        Public Property DetalleEnvio As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.DetalleEnvio
            Get
                Return Me._cbxDetalleEnvio.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._cbxDetalleEnvio.SelectedItem = value
            End Set
        End Property

        Public Property DetalleEnvios As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.DetalleEnvios
            Get
                Return Me._cbxDetalleEnvio.DataContext
            End Get
            Set(ByVal value As Object)
                Me._cbxDetalleEnvio.DataContext = value
            End Set
        End Property

        Public Property Guia As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.Guia
            Get
                Return Me._cbxguia.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._cbxguia.SelectedItem = value
            End Set
        End Property

        Public Property Guias As Object Implements Contratos.FacFacturas.IConsultarFacFacturas.Guias
            Get
                Return Me._cbxguia.DataContext
            End Get
            Set(ByVal value As Object)
                Me._cbxguia.DataContext = value
            End Set
        End Property

        Private Sub _txtCarta_MouseDoubleClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ControlesMostrarCarta()
        End Sub

        Private Sub ControlesMostrarCarta()
            Me._txtCarta.Visibility = System.Windows.Visibility.Collapsed
            Me._lstCartas.Visibility = System.Windows.Visibility.Visible
            Me._lstCartas.IsEnabled = True
            Me._btnConsultarCarta.Visibility = System.Windows.Visibility.Visible
            Me._txtIdCarta.Visibility = System.Windows.Visibility.Visible
            'Me._txtNombreCarta.Visibility = System.Windows.Visibility.Visible
            Me._lblIdCarta.Visibility = System.Windows.Visibility.Visible
            Me._dpkFechaCarta.Visibility = System.Windows.Visibility.Visible
            'Me._lblNombreCarta.Visibility = System.Windows.Visibility.Visible            
            ControlesOcultarAsociado()
        End Sub

        Public Sub PintarCarta()
            Me._txtCarta.BorderBrush = New SolidColorBrush(Colors.LightGreen)
        End Sub

        Private Sub _lstCartas_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Me._presentador.CambiarCarta()
            ControlesOcultarCarta()
            Me._btnConsulta.IsDefault = True
            Me._btnConsulta.Focus()
        End Sub

        Private Sub ControlesOcultarCarta()
            Me._lstCartas.Visibility = System.Windows.Visibility.Collapsed
            Me._btnConsultarCarta.Visibility = System.Windows.Visibility.Collapsed
            Me._txtIdCarta.Visibility = System.Windows.Visibility.Collapsed
            'Me._txtNombreCarta.Visibility = System.Windows.Visibility.Collapsed
            Me._txtCarta.Visibility = System.Windows.Visibility.Visible
            Me._lblIdCarta.Visibility = System.Windows.Visibility.Collapsed
            Me._dpkFechaCarta.Visibility = System.Windows.Visibility.Collapsed
            'Me._lblNombreCarta.Visibility = System.Windows.Visibility.Collapsed
        End Sub

        Public Property NombreCarta() As String Implements Contratos.FacFacturas.IConsultarFacFacturas.NombreCarta
            Get
                Return Me._txtCarta.Text
            End Get
            Set(ByVal value As String)
                Me._txtCarta.Text = value
            End Set
        End Property

        Public Property idCartaFiltrar() As String Implements Contratos.FacFacturas.IConsultarFacFacturas.idCartaFiltrar
            Get
                Return Me._txtIdCarta.Text
            End Get
            Set(ByVal value As String)
                Me._txtIdCarta.Text = value
            End Set
        End Property

        Public Property FechaCartaFiltrar() As String Implements Contratos.FacFacturas.IConsultarFacFacturas.FechaCartaFiltrar
            Get
                Return Me._dpkFechaCarta.Text
            End Get
            Set(ByVal value As String)
                Me._dpkFechaCarta.Text = value
            End Set
        End Property

        Public WriteOnly Property Count As Integer Implements Contratos.FacFacturas.IConsultarFacFacturas.Count
            Set(value As Integer)
                _lblHits.Text = value
            End Set
        End Property

        Public Property Proforma As String Implements Contratos.FacFacturas.IConsultarFacFacturas.Proforma
            Get
                Return _txtProforma.Text
            End Get
            Set(value As String)
                _txtProforma.Text = value
            End Set
        End Property

        Public Property NumeroControl As String Implements Contratos.FacFacturas.IConsultarFacFacturas.NumeroControl
            Get
                Return _txtNumeroControl.Text
            End Get
            Set(value As String)
                _txtNumeroControl.Text = value
            End Set
        End Property

        Private Sub validarCamposVacios()

            Dim todosCamposVacios As Boolean
            todosCamposVacios = True

            If Me._txtId.Text <> "" Then
                todosCamposVacios = False
                Me._txtId.Focus()
            End If

            If Me._txtInicial.Text <> "" Then
                todosCamposVacios = False
                Me._txtInicial.Focus()
            End If

            If Me._txtInstruc.Text <> "" Then
                todosCamposVacios = False
                Me._txtInstruc.Focus()
            End If

            If Me._txtNumeroControl.Text <> "" Then
                todosCamposVacios = False
                Me._txtNumeroControl.Focus()
            End If

            If Me._txtProforma.Text <> "" Then
                todosCamposVacios = False
                Me._txtProforma.Focus()
            End If

            If Me._txtSeniat.Text <> "" Then
                todosCamposVacios = False
                Me._txtSeniat.Focus()
            End If

            If Me._txtOurref.Text <> "" Then
                todosCamposVacios = False
                Me._txtOurref.Focus()
            End If

            If Me._txtCaso.Text <> "" Then
                todosCamposVacios = False
                Me._txtCaso.Focus()
            End If

            If (Me._cbxDetalleEnvio.SelectedIndex <> 0) AndAlso (Me._cbxDetalleEnvio.SelectedIndex <> -1) Then
                todosCamposVacios = False
                Me._cbxDetalleEnvio.Focus()
            End If

            If (Me._cbxguia.SelectedIndex <> 0) AndAlso (Me._cbxguia.SelectedIndex <> -1) Then
                todosCamposVacios = False
                Me._cbxguia.Focus()
            End If

            If (Me._cbxOrigenFactura.SelectedIndex <> 0) AndAlso (Me._cbxOrigenFactura.SelectedIndex <> -1) Then
                todosCamposVacios = False
                Me._cbxOrigenFactura.Focus()
            End If

            If Me._dpkFechaFactura.Text <> "" Then
                todosCamposVacios = False
                Me._dpkFechaFactura.Focus()
            End If

            If Me._dpkFechaSeniat.Text <> "" Then
                todosCamposVacios = False
                Me._dpkFechaSeniat.Focus()
            End If

            If todosCamposVacios Then
                Me._txtId.Focus()
            End If

        End Sub

        Private Sub _Activar_Consultar(sender As System.Object, e As System.Windows.Input.KeyEventArgs)

            If (e.Key = Key.Enter) Then
                Mouse.OverrideCursor = Cursors.Wait
                Dim nom As String = DirectCast(sender, ByTTextBox).Name.ToString
                Me._btnConsulta.Focus()
                Me._presentador.Consultar()
                Mouse.OverrideCursor = Nothing
                validarCamposVacios()
            End If

        End Sub

        Private Sub _dpkFechaFactura_SelectedDateChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs)
            If (Me._dpkFechaFactura.SelectedDate IsNot Nothing) Then
                Me._btnConsulta.IsDefault = True
                Me._btnConsulta.Focus()
            End If
        End Sub

        Private Sub _dpkFechaSeniat_SelectedDateChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs)
            If (Me._dpkFechaSeniat.SelectedDate IsNot Nothing) Then
                Me._btnConsulta.IsDefault = True
                Me._btnConsulta.Focus()
            End If
        End Sub

        Private Sub _cbxDetalleEnvio_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs)
            Me._btnConsulta.Focus()
        End Sub

        Private Sub _cbxguia_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs)
            Me._btnConsulta.Focus()
        End Sub

        Private Sub _cbxOrigenFactura_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs)
            Me._btnConsulta.Focus()
        End Sub
    End Class
End Namespace
