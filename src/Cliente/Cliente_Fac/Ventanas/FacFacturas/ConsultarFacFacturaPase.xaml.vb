﻿Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.FacFacturas
Imports Diginsoft.Bolet.Cliente.Fac.Presentadores.FacFacturas
Imports Diginsoft.Bolet.Cliente.Fac.Contratos
Imports Trascend.Bolet.ControlesByT
Namespace Ventanas.FacFacturas
    Partial Public Class ConsultarFacFacturaPase
        Inherits Page
        Implements IConsultarFacFacturaPase

        Private _presentador As PresentadorConsultarFacFacturaPase
        Private _cargada As Boolean

#Region "IConsultarFacFacturaPase"

        Public Property EstaCargada() As Boolean Implements IPaginaBaseFac.EstaCargada
            Get
                Return Me._cargada
            End Get
            Set(ByVal value As Boolean)
                Me._cargada = value
            End Set
        End Property

        Public Property TextoBotonModificar() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.TextoBotonModificar
            Get
                Return Me._txbModificar.Text
            End Get
            Set(ByVal value As String)
                Me._txbModificar.Text = value
            End Set
        End Property

        Public WriteOnly Property AccionRealizar As Integer? Implements Contratos.FacFacturas.IConsultarFacFacturaPase.AccionRealizar
            Set(ByVal value As Integer?)
                Select Case value
                    Case 1 'modificar sin el boton regresar
                        _btnModificar.Visibility = Windows.Visibility.Visible
                        '_btnEliminar.Visibility = Windows.Visibility.Visible
                        _btnRegresar.Visibility = Windows.Visibility.Collapsed
                    Case 2 'no modificar
                        '_btnModificar.Visibility = Windows.Visibility.Collapsed
                        '_btnEliminar.Visibility = Windows.Visibility.Collapsed
                        _btnRegresar.Visibility = Windows.Visibility.Visible
                    Case 3 'modificar con el boton regresar                   
                        _btnModificar.Visibility = Windows.Visibility.Visible
                        '_btnEliminar.Visibility = Windows.Visibility.Visible
                        _btnRegresar.Visibility = Windows.Visibility.Visible
                End Select
            End Set
        End Property

        Public WriteOnly Property SetLocalidad As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.SetLocalidad
            Set(ByVal value As String)
                Me._cbxLocal.Text = value
            End Set
        End Property

        Public WriteOnly Property HabilitarCampos() As Boolean Implements Contratos.FacFacturas.IConsultarFacFacturaPase.HabilitarCampos
            Set(ByVal value As Boolean)
                'Me._txtDetalle.IsEnabled = value
                '_txtXiauto.IsEnabled = value
                '_txtXCausaRec.IsEnabled = value
                _txtdesc.IsEnabled = value
                _txtCaso.IsEnabled = value
                '_txtId.IsEnabled = value
                _dpkFechaFactura.IsEnabled = value
                _txtInicial.IsEnabled = value
                _txtSeniat.IsEnabled = value
                _txtCodeti.IsEnabled = value
                _txtAsociado.IsEnabled = value
                _txtXAsociado.IsEnabled = value
                _txtTarifa.IsEnabled = value
                _txtRif.IsEnabled = value
                _txtXNit.IsEnabled = value
                '_txtProforma.IsEnabled = value
                _cbxXterrero.IsEnabled = value
                _cbxMoneda.IsEnabled = value
                _cbxIdioma.IsEnabled = value
                _txtOurref.IsEnabled = value
                _txtInstruc.IsEnabled = value
                _txtAsociadoImp.IsEnabled = value
                _txtInteresado.IsEnabled = value
                _cbxDetalleEnvio.IsEnabled = value
                _chkBIMulmon.IsEnabled = value
                _txtCarta.IsEnabled = value
                _cbxLocal.IsEnabled = value
                '_btnConsultarDepartamentoServicio2.IsEnabled = False
                '_btnEliminarDepartamentoServicio2.IsEnabled = False
                _lstDetalle.IsEnabled = value
            End Set
        End Property

        Public Sub FocoPredeterminado() Implements IPaginaBaseFac.FocoPredeterminado
            Me._lstAsociados.Focus()
        End Sub

        Public Property FacFactura As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.FacFactura
            Get
                Return Me._gridDatos.DataContext
            End Get
            Set(ByVal value As Object)
                Me._gridDatos.DataContext = value
            End Set
        End Property

        Public Property OrigenesFactura As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.OrigenesFactura
            Get
                Return Me._cbxOrigenFactura.DataContext
            End Get
            Set(value As Object)
                Me._cbxOrigenFactura.DataContext = value
            End Set
        End Property

        Public Property OrigenFactura As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.OrigenFactura
            Get
                Return Me._cbxOrigenFactura.SelectedItem
            End Get
            Set(value As Object)
                Me._cbxOrigenFactura.SelectedItem = value
            End Set
        End Property

        Public Sub Mensaje(ByVal mensaje__1 As String) Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Mensaje
            MessageBox.Show(mensaje__1, "Error", MessageBoxButton.OK, MessageBoxImage.[Error])
        End Sub
#End Region

#Region "Constructores de la Clase"

        Public Sub New(ByVal FacFactura As Object)
            InitializeComponent()
            Me._cargada = False
            Me._presentador = New PresentadorConsultarFacFacturaPase(Me, FacFactura)
        End Sub

        Public Sub New(ByVal FacFactura As Object, filtroFacFacturaProforma As Object, ventanaPadre As Object)
            InitializeComponent()
            Me._cargada = False
            Me._presentador = New PresentadorConsultarFacFacturaPase(Me, FacFactura, filtroFacFacturaProforma, ventanaPadre)
        End Sub

#End Region


        Private Sub _btnCancelar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.Cancelar()
        End Sub

        Private Sub _btnsalir_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.Salir()
        End Sub

        Private Sub _btnModificar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.Modificar()
        End Sub

        Private Sub _btnEliminar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If MessageBoxResult.Yes = MessageBox.Show(Recursos.MensajesConElUsuario.fac_ConfirmacionEliminarFacFactura, "Eliminar FacFactura", MessageBoxButton.YesNo, MessageBoxImage.Question) Then
                Me._presentador.Eliminar()
            End If
        End Sub

        Private Sub _btnRegresar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.regresar_con()
        End Sub

        Private Sub _btnVerCarta_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.Ver_Carta()
        End Sub

        Private Sub Page_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If Not EstaCargada Then
                Me._presentador.CargarPagina()
                EstaCargada = True
                '_chkSeleccion.IsChecked = True
                '_txtAnulada.Text = "NO"
                PintarAsociado()
                PintarAsociadoImp()
                PintarCarta()
                PintarInteresado()
            End If
        End Sub

        Public Property Asociado As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Asociado

            Get
                Return Me._lstAsociados.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._lstAsociados.SelectedItem = value
                Me._lstAsociados.ScrollIntoView(value)
            End Set
        End Property

        Public Property Interesado As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Interesado

            Get
                Return Me._lstInteresados.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._lstInteresados.SelectedItem = value
                Me._lstInteresados.ScrollIntoView(value)
            End Set
        End Property

        Public ReadOnly Property Localidad() As Char Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Localidad
            Get
                If Not String.IsNullOrEmpty(Me._cbxLocal.Text) Then
                    Return (Me._cbxLocal.Text)(0)
                Else
                    Return " "c
                End If
            End Get
        End Property

        Public WriteOnly Property Departamento As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Departamento

            Set(ByVal value As String)
                Me._txtDepartamento.Text = value
            End Set
        End Property

        Public ReadOnly Property GetXterrero As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.GetXterrero
            Get
                Dim valor As String = ""
                If _cbxXterrero.Text = "F" Then
                    valor = "1"
                Else
                    If _cbxXterrero.Text = "S-F" Then
                        valor = "2"
                    Else
                        If _cbxXterrero.Text = "S " Then
                            valor = "3"
                        End If
                    End If
                End If
                Return valor
            End Get
        End Property

        Public WriteOnly Property SetXterrero As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.SetXterrero
            Set(ByVal value As String)
                _cbxXterrero.Text = value
            End Set
        End Property

        Public Property Asociados As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Asociados
            Get
                Return Me._lstAsociados.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstAsociados.DataContext = value
            End Set
        End Property

        Public Property Interesados As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Interesados
            Get
                Return Me._lstInteresados.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstInteresados.DataContext = value
            End Set
        End Property

        Public Property AsociadoImp As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.AsociadoImp

            Get
                Return Me._lstAsociadosImp.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._lstAsociadosImp.SelectedItem = value
                Me._lstAsociadosImp.ScrollIntoView(value)
            End Set
        End Property

        Public Property AsociadosImp As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.AsociadosImp
            Get
                Return Me._lstAsociadosImp.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstAsociadosImp.DataContext = value
            End Set
        End Property

        Public Property Carta As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Carta

            Get
                Return Me._lstCartas.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._lstCartas.SelectedItem = value
                Me._lstCartas.ScrollIntoView(value)
            End Set
        End Property

        Public Property Cartas As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Cartas
            Get
                Return Me._lstCartas.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstCartas.DataContext = value
            End Set
        End Property

        Public Property DetalleEnvio As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.DetalleEnvio
            Get
                Return Me._cbxDetalleEnvio.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._cbxDetalleEnvio.SelectedItem = value
            End Set
        End Property

        Public Property DetalleEnvios As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.DetalleEnvios
            Get
                Return Me._cbxDetalleEnvio.DataContext
            End Get
            Set(ByVal value As Object)
                Me._cbxDetalleEnvio.DataContext = value
            End Set
        End Property

        Public Property Idioma As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Idioma
            Get
                Return Me._cbxIdioma.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._cbxIdioma.SelectedItem = value
            End Set
        End Property

        Public Property Idiomas As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Idiomas
            Get
                Return Me._cbxIdioma.DataContext
            End Get
            Set(ByVal value As Object)
                Me._cbxIdioma.DataContext = value
            End Set
        End Property

        Public Property Moneda As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Moneda
            Get
                Return Me._cbxMoneda.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._cbxMoneda.SelectedItem = value
            End Set
        End Property

        Public Property Monedas As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Monedas
            Get
                Return Me._cbxMoneda.DataContext
            End Get
            Set(ByVal value As Object)
                Me._cbxMoneda.DataContext = value
            End Set
        End Property

        Public Property Guia As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Guia
            Get
                Return Me._cbxguia.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._cbxguia.SelectedItem = value
            End Set
        End Property

        Public Property Guias As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Guias
            Get
                Return Me._cbxguia.DataContext
            End Get
            Set(ByVal value As Object)
                Me._cbxguia.DataContext = value
            End Set
        End Property

        Public Property Tarifa As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Tarifa
            Get
                Return Me._txtTarifa.Text
            End Get
            Set(ByVal value As Object)
                Me._txtTarifa.Text = value
            End Set
        End Property

        Public Property Codeti As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Codeti
            Get
                Return Me._txtCodeti.Text
            End Get
            Set(ByVal value As Object)
                Me._txtCodeti.Text = value
            End Set
        End Property

        Public Property Rif As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Rif
            Get
                Return Me._txtRif.Text
            End Get
            Set(ByVal value As Object)
                Me._txtRif.Text = value
            End Set
        End Property

        Public Property XNit As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.XNit
            Get
                Return Me._txtXNit.Text
            End Get
            Set(ByVal value As Object)
                Me._txtXNit.Text = value
            End Set
        End Property

        Public Property XAsociado As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.XAsociado
            Get
                Return Me._txtXAsociado.Text
            End Get
            Set(ByVal value As Object)
                Me._txtXAsociado.Text = value
            End Set

        End Property

        'Private Sub _btnConsultarAsociado_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        '    Me._presentador.BuscarAsociado2()
        'End Sub
        Private Sub _btnConsultar(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim nom As String = DirectCast(sender, Button).Name.ToString
            If nom = "_btnConsultarAsociado" Then
                Me._presentador.BuscarAsociado2()
            ElseIf nom = "_btnConsultarAsociadoImp" Then
                Me._presentador.BuscarAsociadoImp()
            ElseIf nom = "_btnConsultarCarta" Then
                Me._presentador.BuscarCarta()
            ElseIf nom = "_btnConsultarInteresado" Then
                Me._presentador.BuscarInteresado2()
            ElseIf nom = "_btnBuscarDepartamentoServicio" Then
                Me._presentador.VerDepartamentoServicios()
            ElseIf nom = "_btnConsultarMarca" Then
                Me._presentador.BuscarMarca()
            ElseIf nom = "_btnConsultarPatente" Then
                Me._presentador.BuscarPatente()
            End If
        End Sub

        Private Sub _Consultar_Enter(ByVal sender As Object, ByVal e As KeyEventArgs)
            If (e.Key = Key.Enter) Then
                Dim nom As String = DirectCast(sender, ByTTextBox).Name.ToString
                If nom = "_txtIdAsociado" Or nom = "_txtNombreAsociado" Then
                    Me._presentador.BuscarAsociado2()
                ElseIf nom = "_txtIdAsociadoImp" Or nom = "_txtNombreAsociadoImp" Then
                    Me._presentador.BuscarAsociadoImp()
                ElseIf nom = "_txtIdInteresado" Or nom = "_txtNombreInteresado" Then
                    Me._presentador.BuscarInteresado2()
                ElseIf nom = "_txtIdCarta" Or nom = "_txtNombreCarta" Or nom = "_dpkFechaCarta" Then
                    Me._presentador.BuscarCarta()
                ElseIf nom = "_txtServicioId" Or nom = "_txtServicioCod_Cont" Or nom = "_txtServicioXreferencia" Then
                    Me._presentador.VerDepartamentoServicios()
                ElseIf nom = "_txtNombreMarca" Or nom = "_txtIdMarca" Then
                    Me._presentador.BuscarMarca()
                ElseIf nom = "_txtNombrePatente" Or nom = "_txtIdPatente" Then
                    Me._presentador.BuscarPatente()
                ElseIf nom = "_txtCguia" Then
                    Me._presentador.ConsultarGuia(DirectCast(sender, ByTTextBox).Text)
                End If
            Else
                If e.Key = Key.Tab Then
                    Dim nom As String = DirectCast(sender, ByTTextBox).Name.ToString
                    If nom = "_txtCguia" Then

                        Me._presentador.ConsultarGuia(DirectCast(sender, ByTTextBox).Text)
                    End If
                End If
            End If

        End Sub

        'Private Sub _ConsultarInteresado_Enter(ByVal sender As Object, ByVal e As KeyEventArgs)
        '    If (e.Key = Key.Enter) Then
        '        Me._presentador.BuscarInteresado2()
        '    End If
        'End Sub

        'Private Sub _ConsultarAsociadoImp_Enter(ByVal sender As Object, ByVal e As KeyEventArgs)
        '    If (e.Key = Key.Enter) Then
        '        Me._presentador.BuscarAsociadoImp()
        '    End If
        'End Sub

        'Private Sub _ConsultarCarta_Enter(ByVal sender As Object, ByVal e As KeyEventArgs)
        '    If (e.Key = Key.Enter) Then
        '        Me._presentador.BuscarCarta()
        '    End If
        'End Sub

        Private Sub _btnrecalcular_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.focus()
        End Sub

        Private Sub _modificar_valor_LostFocus()
            Me._presentador.focus2()
        End Sub

        'Private Sub _modificar_valor2_LostFocus()            
        '    Me._presentador.focus2()
        'End Sub

        Private Sub _lstDetalle_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Me._presentador.SeleccionarDetalle()

        End Sub


        Private Sub _btnVerDepartamentoServicios_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            '    'Me._lstDepartamentoServicio_2.Visibility = Windows.Visibility.Visible
            '    'Me._btnagregarServicio2.Visibility = Windows.Visibility.Visible
            '    'Me._lstDetalleProforma.Visibility = Windows.Visibility.Collapsed
            '    'Me._btnConsultarDepartamentoServicio2.Visibility = Windows.Visibility.Collapsed
            '    'Me._txtSumaBono.Visibility = Windows.Visibility.Collapsed
            '    'Me._txtSumaBonoBf.Visibility = Windows.Visibility.Collapsed

            '    'Me._GbDepartamentoServicio.Visibility = Windows.Visibility.Visible
            '    'Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
            '    'Me._GbDetalleProforma.Visibility = Windows.Visibility.Collapsed
            '    ''Me._GbDepartamentoServicio.Visibility = Windows.Visibility.Collapsed
            '    'Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Collapsed
            '    'Me._GbMarca.Visibility = Windows.Visibility.Collapsed
            '    'Me._GbPatente.Visibility = Windows.Visibility.Collapsed
            '    'Me._GbCantidades.Visibility = Windows.Visibility.Collapsed
            '    'Me._GbRecursos.Visibility = Windows.Visibility.Collapsed
            '    'Me._GbMateriales.Visibility = Windows.Visibility.Collapsed
            '    'Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
            '    'Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
            '    'Me._GbAnualidades.Visibility = Windows.Visibility.Collapsed
            '    'Me._gridDatos.Visibility = Windows.Visibility.Collapsed
            '    '_Wp_Btn.Visibility = Windows.Visibility.Collapsed
            '    'Me._Wp_Salir.Visibility = Windows.Visibility.Visible

            '    'Me._presentador.VerDepartamentoServicios()
            VerDepartamentoDesglose()
        End Sub
        Public Sub VerDepartamentoDesglose()
            Me._GbDepartamentoServicio.Visibility = Windows.Visibility.Visible
            Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
            Me._GbDetalle.Visibility = Windows.Visibility.Collapsed
            'Me._GbDepartamentoServicio.Visibility = Windows.Visibility.Collapsed
            Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Collapsed
            Me._GbMarca.Visibility = Windows.Visibility.Collapsed
            Me._GbPatente.Visibility = Windows.Visibility.Collapsed
            Me._GbCantidades.Visibility = Windows.Visibility.Collapsed
            Me._GbRecursos.Visibility = Windows.Visibility.Collapsed
            Me._GbMateriales.Visibility = Windows.Visibility.Collapsed
            Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
            Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
            Me._GbAnualidades.Visibility = Windows.Visibility.Collapsed
            Me._gridDatos.Visibility = Windows.Visibility.Collapsed
            _Wp_Btn.Visibility = Windows.Visibility.Collapsed
            Me._Wp_Salir.Visibility = Windows.Visibility.Visible

            Me._presentador.VerDepartamentoServicios()
        End Sub

        Private Sub _btnElimDepartamentoServicios_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.ElimDepartamentoServicios()
        End Sub

        Private Sub _btnAgregarServicio2_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            'Me._lstDepartamentoServicio_2.Visibility = Windows.Visibility.Collapsed
            Me._btnagregarServicio2.Visibility = Windows.Visibility.Collapsed
            Me._lstDetalle.Visibility = Windows.Visibility.Visible
            ' Me._btnConsultarDepartamentoServicio2.Visibility = Windows.Visibility.Visible
            'Me._txtSumaBono.Visibility = Windows.Visibility.Visible
            'Me._txtSumaBonoBf.Visibility = Windows.Visibility.Visible
            'Me._presentador.AgregarFacturas2()
        End Sub

        'Private Sub _btnAgregarForma_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        '    Me._lstfactura.Visibility = Windows.Visibility.Collapsed
        '    Me._btnagregarforma.Visibility = Windows.Visibility.Collapsed
        '    Me._btnConsultarfactura.Visibility = Windows.Visibility.Visible
        '    Me._lstForma.Visibility = Windows.Visibility.Visible
        '    Me._txtSumaBForma.Visibility = Windows.Visibility.Visible
        '    Me._txtSumaBFormaBf.Visibility = Windows.Visibility.Visible
        '    'Me._presentador.AgregarForma()
        'End Sub

        'Private Sub _lstForma_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
        '    Me._txtBForma.Visibility = Windows.Visibility.Visible
        '    Me._txtBFormaBf.Visibility = Windows.Visibility.Visible
        '    Me._txtXForma.Visibility = Windows.Visibility.Visible
        '    Me._btnmodificarforma.Visibility = Windows.Visibility.Visible
        '    'Me._presentador.MostrarForma()
        'End Sub

        'Private Sub _btnModificarForma_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        '    Me._txtBForma.Visibility = Windows.Visibility.Collapsed
        '    Me._txtBFormaBf.Visibility = Windows.Visibility.Collapsed
        '    Me._txtXForma.Visibility = Windows.Visibility.Collapsed
        '    Me._btnmodificarforma.Visibility = Windows.Visibility.Collapsed
        '    'Me._presentador.ModificarForma()
        'End Sub

        'Public ReadOnly Property FacFormaSeleccionada As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.FacFormaSeleccionada
        '    Get
        '        Return Me._lstForma.SelectedItem
        '    End Get
        'End Property

        Public ReadOnly Property FechaFactura As DateTime Implements Contratos.FacFacturas.IConsultarFacFacturaPase.FechaFactura
            Get
                Return Me._dpkFechaFactura.Text
            End Get
        End Property

        Public ReadOnly Property Cantidad As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Cantidad
            Get
                Return Me._txtCantidad.Text
            End Get
        End Property

        Public Property Ourref As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Ourref
            Get
                Return Me._txtOurref.Text
            End Get

            Set(ByVal value As String)
                Me._txtOurref.Text = value
            End Set
        End Property

        Public Property Caso As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Caso
            Get
                Return Me._txtCaso.Text
            End Get

            Set(ByVal value As String)
                Me._txtCaso.Text = value
            End Set
        End Property

        'Private Sub _btnVerCreditos_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        '    Me._lstfactura.Visibility = Windows.Visibility.Visible
        '    Me._btnagregarforma.Visibility = Windows.Visibility.Visible
        '    Me._btnConsultarfactura.Visibility = Windows.Visibility.Collapsed
        '    Me._lstForma.Visibility = Windows.Visibility.Collapsed
        '    Me._txtSumaBForma.Visibility = Windows.Visibility.Collapsed
        '    Me._txtSumaBFormaBf.Visibility = Windows.Visibility.Collapsed

        '    'Me._presentador.VerCreditos()
        'End Sub


        Private Sub _txtAsociado_MouseDoubleClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ControlesMostrarAsociado()
        End Sub

        Private Sub ControlesMostrarAsociado()
            Me._txtAsociado.Visibility = System.Windows.Visibility.Collapsed
            'Me._txtAsociadoId.Visibility = System.Windows.Visibility.Collapsed
            Me._lstAsociados.Visibility = System.Windows.Visibility.Visible
            Me._lstAsociados.IsEnabled = True
            Me._btnConsultarAsociado.Visibility = System.Windows.Visibility.Visible
            Me._txtIdAsociado.Visibility = System.Windows.Visibility.Visible
            Me._txtNombreAsociado.Visibility = System.Windows.Visibility.Visible
            Me._lblIdAsociado.Visibility = System.Windows.Visibility.Visible
            Me._lblNombreAsociado.Visibility = System.Windows.Visibility.Visible
            ControlesOcultarAsociadoImp()
            ControlesOcultarInteresado()
            ControlesOcultarCarta()
        End Sub


        Private Sub _txtInteresado_MouseDoubleClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ControlesMostrarInteresado()
        End Sub

        Private Sub ControlesMostrarInteresado()
            Me._txtInteresado.Visibility = System.Windows.Visibility.Collapsed
            'Me._txtInteresadoId.Visibility = System.Windows.Visibility.Collapsed
            Me._lstInteresados.Visibility = System.Windows.Visibility.Visible
            Me._lstInteresados.IsEnabled = True
            Me._btnConsultarInteresado.Visibility = System.Windows.Visibility.Visible
            Me._txtIdInteresado.Visibility = System.Windows.Visibility.Visible
            Me._txtNombreInteresado.Visibility = System.Windows.Visibility.Visible
            Me._lblIdInteresado.Visibility = System.Windows.Visibility.Visible
            Me._lblNombreInteresado.Visibility = System.Windows.Visibility.Visible
            ControlesOcultarAsociadoImp()
            ControlesOcultarAsociado()
            ControlesOcultarCarta()
        End Sub

        Private Sub _txtAsociadoImp_MouseDoubleClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ControlesMostrarAsociadoImp()
        End Sub

        Private Sub ControlesMostrarAsociadoImp()
            Me._txtAsociadoImp.Visibility = System.Windows.Visibility.Collapsed
            Me._lstAsociadosImp.Visibility = System.Windows.Visibility.Visible
            Me._lstAsociadosImp.IsEnabled = True
            Me._btnConsultarAsociadoImp.Visibility = System.Windows.Visibility.Visible
            Me._txtIdAsociadoImp.Visibility = System.Windows.Visibility.Visible
            Me._txtNombreAsociadoImp.Visibility = System.Windows.Visibility.Visible
            Me._lblIdAsociadoImp.Visibility = System.Windows.Visibility.Visible
            Me._lblNombreAsociadoImp.Visibility = System.Windows.Visibility.Visible
            ControlesOcultarInteresado()
            ControlesOcultarAsociado()
            ControlesOcultarCarta()
        End Sub

        Private Sub _txtCarta_MouseDoubleClick(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ControlesMostrarCarta()
        End Sub

        Private Sub ControlesMostrarCarta()
            Me._txtCarta.Visibility = System.Windows.Visibility.Collapsed
            Me._btnVerCarta.Visibility = System.Windows.Visibility.Collapsed
            Me._lstCartas.Visibility = System.Windows.Visibility.Visible
            Me._lstCartas.IsEnabled = True
            Me._btnConsultarCarta.Visibility = System.Windows.Visibility.Visible
            Me._txtIdCarta.Visibility = System.Windows.Visibility.Visible
            'Me._txtNombreCarta.Visibility = System.Windows.Visibility.Visible
            Me._lblIdCarta.Visibility = System.Windows.Visibility.Visible
            Me._dpkFechaCarta.Visibility = System.Windows.Visibility.Visible
            'Me._lblNombreCarta.Visibility = System.Windows.Visibility.Visible
            ControlesOcultarInteresado()
            ControlesOcultarAsociado()
            ControlesOcultarAsociadoImp()
        End Sub

        Public Sub PintarInteresado()
            Me._txtInteresado.BorderBrush = New SolidColorBrush(Colors.LightGreen)
        End Sub

        Public Sub PintarAsociado()
            Me._txtAsociado.BorderBrush = New SolidColorBrush(Colors.LightGreen)
        End Sub

        Public Sub PintarCarta()
            Me._txtCarta.BorderBrush = New SolidColorBrush(Colors.LightGreen)
        End Sub

        Public Sub PintarAsociadoImp()
            Me._txtAsociadoImp.BorderBrush = New SolidColorBrush(Colors.LightGreen)
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
            Me._txtAsociado.Visibility = System.Windows.Visibility.Visible
            'Me._txtAsociadoId.Visibility = System.Windows.Visibility.Visible
            Me._lblIdAsociado.Visibility = System.Windows.Visibility.Collapsed
            Me._lblNombreAsociado.Visibility = System.Windows.Visibility.Collapsed
        End Sub

        Private Sub _lstInteresados_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Me._presentador.CambiarInteresado()
            ControlesOcultarInteresado()
        End Sub

        Private Sub ControlesOcultarInteresado()
            Me._lstInteresados.Visibility = System.Windows.Visibility.Collapsed
            Me._btnConsultarInteresado.Visibility = System.Windows.Visibility.Collapsed
            Me._txtIdInteresado.Visibility = System.Windows.Visibility.Collapsed
            Me._txtNombreInteresado.Visibility = System.Windows.Visibility.Collapsed
            Me._txtInteresado.Visibility = System.Windows.Visibility.Visible
            'Me._txtInteresadoId.Visibility = System.Windows.Visibility.Visible
            Me._lblIdInteresado.Visibility = System.Windows.Visibility.Collapsed
            Me._lblNombreInteresado.Visibility = System.Windows.Visibility.Collapsed
        End Sub

        Private Sub _lstAsociadosImp_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Me._presentador.CambiarAsociadoImp()
            ControlesOcultarAsociadoImp()
        End Sub

        Private Sub ControlesOcultarAsociadoImp()
            Me._lstAsociadosImp.Visibility = System.Windows.Visibility.Collapsed
            Me._btnConsultarAsociadoImp.Visibility = System.Windows.Visibility.Collapsed
            Me._txtIdAsociadoImp.Visibility = System.Windows.Visibility.Collapsed
            Me._txtNombreAsociadoImp.Visibility = System.Windows.Visibility.Collapsed
            Me._txtAsociadoImp.Visibility = System.Windows.Visibility.Visible
            Me._lblIdAsociadoImp.Visibility = System.Windows.Visibility.Collapsed
            Me._lblNombreAsociadoImp.Visibility = System.Windows.Visibility.Collapsed
        End Sub

        Private Sub _lstCartas_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Me._presentador.CambiarCarta()
            ControlesOcultarCarta()
        End Sub

        Private Sub ControlesOcultarCarta()
            Me._lstCartas.Visibility = System.Windows.Visibility.Collapsed
            Me._btnConsultarCarta.Visibility = System.Windows.Visibility.Collapsed
            Me._txtIdCarta.Visibility = System.Windows.Visibility.Collapsed
            'Me._txtNombreCarta.Visibility = System.Windows.Visibility.Collapsed
            Me._txtCarta.Visibility = System.Windows.Visibility.Visible
            Me._btnVerCarta.Visibility = System.Windows.Visibility.Visible
            Me._lblIdCarta.Visibility = System.Windows.Visibility.Collapsed
            Me._dpkFechaCarta.Visibility = System.Windows.Visibility.Collapsed
            'Me._lblNombreCarta.Visibility = System.Windows.Visibility.Collapsed
        End Sub


        Public Property NombreAsociado() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.NombreAsociado
            Get
                Return Me._txtAsociado.Text
            End Get
            Set(ByVal value As String)
                Me._txtAsociado.Text = value
            End Set
        End Property

        Public Property NombreInteresado() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.NombreInteresado
            Get
                Return Me._txtInteresado.Text
            End Get
            Set(ByVal value As String)
                Me._txtInteresado.Text = value
            End Set
        End Property
        'Public Property IdAsociado() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.IdAsociado
        '    Get
        '        Return Me._txtAsociadoId.Text
        '    End Get
        '    Set(ByVal value As String)
        '        Me._txtAsociadoId.Text = value
        '    End Set
        'End Property

        Public Property NombreAsociadoImp() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.NombreAsociadoImp
            Get
                Return Me._txtAsociadoImp.Text
            End Get
            Set(ByVal value As String)
                Me._txtAsociadoImp.Text = value
            End Set
        End Property

        Public Property NombreCarta() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.NombreCarta
            Get
                Return Me._txtCarta.Text
            End Get
            Set(ByVal value As String)
                Me._txtCarta.Text = value
            End Set
        End Property

        Public Property Seleccion() As Boolean Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Seleccion
            Get
                Return Me._chkSeleccion.IsChecked
            End Get
            Set(ByVal value As Boolean)
                Me._chkSeleccion.IsChecked = value
            End Set
        End Property

        Public Property BIMulmon() As Boolean Implements Contratos.FacFacturas.IConsultarFacFacturaPase.BIMulmon
            Get
                Return Me._chkBIMulmon.IsChecked
            End Get
            Set(ByVal value As Boolean)
                Me._chkBIMulmon.IsChecked = value
            End Set
        End Property


        Public Property Desglose() As Boolean Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Desglose
            Get
                Return Me._chkDesglose.IsChecked
            End Get
            Set(ByVal value As Boolean)
                Me._chkDesglose.IsChecked = value
            End Set
        End Property

        Public Property idAsociadoFiltrar() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.idAsociadoFiltrar
            Get
                Return Me._txtIdAsociado.Text
            End Get
            Set(ByVal value As String)
                Me._txtIdAsociado.Text = value
            End Set
        End Property

        Public Property idInteresadoFiltrar() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.idInteresadoFiltrar
            Get
                Return Me._txtIdInteresado.Text
            End Get
            Set(ByVal value As String)
                Me._txtIdInteresado.Text = value
            End Set
        End Property

        Public Property idAsociadoFiltrarImp() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.idAsociadoFiltrarImp
            Get
                Return Me._txtIdAsociadoImp.Text
            End Get
            Set(ByVal value As String)
                Me._txtIdAsociadoImp.Text = value
            End Set
        End Property

        Public Property idCartaFiltrar() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.idCartaFiltrar
            Get
                Return Me._txtIdCarta.Text
            End Get
            Set(ByVal value As String)
                Me._txtIdCarta.Text = value
            End Set
        End Property

        Public Property NombreAsociadoFiltrar() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.NombreAsociadoFiltrar
            Get
                Return Me._txtNombreAsociado.Text
            End Get
            Set(ByVal value As String)
                Me._txtNombreAsociado.Text = value
            End Set
        End Property

        Public Property NombreInteresadoFiltrar() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.NombreInteresadoFiltrar
            Get
                Return Me._txtNombreInteresado.Text
            End Get
            Set(ByVal value As String)
                Me._txtNombreInteresado.Text = value
            End Set
        End Property

        Public Property NombreAsociadoFiltrarImp() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.NombreAsociadoFiltrarImp
            Get
                Return Me._txtNombreAsociadoImp.Text
            End Get
            Set(ByVal value As String)
                Me._txtNombreAsociadoImp.Text = value
            End Set
        End Property

        'Public Property NombreCartaFiltrar() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.NombreCartaFiltrar
        '    Get
        '        Return Me._txtNombreCarta.Text
        '    End Get
        '    Set(ByVal value As String)
        '        Me._txtNombreCarta.Text = value
        '    End Set
        'End Property

        Public Property FechaCartaFiltrar() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.FechaCartaFiltrar
            Get
                Return Me._dpkFechaCarta.Text
            End Get
            Set(ByVal value As String)
                Me._dpkFechaCarta.Text = value
            End Set
        End Property

        'Public Property ResultadosFactura() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosFactura
        '    Get
        '        Return Me._lstfactura.DataContext
        '    End Get
        '    Set(ByVal value As Object)
        '        Me._lstfactura.DataContext = value
        '    End Set
        'End Property

        'Public Property ResultadosFacturaCobro() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosFacturaCobro
        '    Get
        '        Return Me._lstFacturaCobro.DataContext
        '    End Get
        '    Set(ByVal value As Object)
        '        Me._lstFacturaCobro.DataContext = value
        '    End Set
        'End Property

        'Public Property ResultadosForma() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosForma
        '    Get
        '        Return Me._lstForma.DataContext
        '    End Get
        '    Set(ByVal value As Object)
        '        Me._lstForma.DataContext = value
        '    End Set
        'End Property

        Public Property MensajeError As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.MensajeError
            Get
                Return Me._txtMensajeError.Text
            End Get
            Set(ByVal value As String)
                Me._txtMensajeError.Text = value
            End Set

        End Property

        Private Sub _lstDepartamentoServicio_2_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            If Me._lstDepartamentoServicio_2.SelectedItem IsNot Nothing Then
                Me._GbDepartamentoServicio.Visibility = Windows.Visibility.Collapsed
                Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
                Me._GbDetalle.Visibility = Windows.Visibility.Collapsed
                'Me._GbDepartamentoServicio.Visibility = Windows.Visibility.Collapsed
                Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Collapsed
                Me._GbMarca.Visibility = Windows.Visibility.Collapsed
                Me._GbPatente.Visibility = Windows.Visibility.Collapsed
                Me._GbCantidades.Visibility = Windows.Visibility.Collapsed
                Me._GbRecursos.Visibility = Windows.Visibility.Collapsed
                Me._GbMateriales.Visibility = Windows.Visibility.Collapsed
                Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
                Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
                Me._GbAnualidades.Visibility = Windows.Visibility.Collapsed
                Me._gridDatos.Visibility = Windows.Visibility.Collapsed
                Me._presentador.VerTipoMarcaPatente()
                _Wp_Btn.Visibility = Windows.Visibility.Collapsed
                Me._Wp_Salir.Visibility = Windows.Visibility.Collapsed
            End If
        End Sub

        Private Sub _lstDocumentoMarca_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Me._presentador.VerTipoTraduccion()
        End Sub
        Private Sub _lstDocumentoPatente_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Me._presentador.VerTipoTraduccion()
        End Sub
        Private Sub _btnCantidad_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.VerTipoTraduccion()
        End Sub
        Private Sub _lstDocumentoTraduccion_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Me._presentador.VerTipoRecurso()
        End Sub
        Private Sub _lstRecurso_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Me._presentador.VerTipoMaterial()
        End Sub
        Private Sub _lstMaterial_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Me._presentador.VerTipoMultiplesMarcas()
        End Sub
        Private Sub _btnSalirMarca_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.VerTipoAnualidad()
        End Sub

        Private Sub _btnSalirPatente_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.VerTipoAnualidad()
        End Sub

        Private Sub _lstMarcas_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            If (Me._txtVerTipo.Text = "7") Then
                Me._presentador.CambiarMarca()
            Else
                'Me._presentador.AgregarDetalleProforma()
                Me._presentador.VerTipoAnualidad()
            End If
        End Sub

        Private Sub _lstPatentes_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            If (Me._txtVerTipo.Text = "9") Then
                Me._presentador.CambiarPatente()
            Else
                'Me._presentador.AgregarDetalleProforma()
                Me._presentador.VerTipoAnualidad()
            End If
        End Sub

        Private Sub _lstAnualidad_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Me._presentador.VerTipoDesgloseServicio()
        End Sub

        Private Sub _lstDegloseServicio_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            Me._presentador.AgregarDetalle()
        End Sub

        Private Sub _btnEliminarMultiplesMarca_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.EliminarMultiplesMarca() 'para eliminar las marcas tildadas en multiples marcas
        End Sub

        Private Sub _btnEliminarMultiplesPatente_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.EliminarMultiplesPatente() 'para eliminar las Patentes tildadas en multiples Patentes
        End Sub

        Public Property ResultadosDesgloseServicio2() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosDesgloseServicio2
            Get
                Return Me._lstDesgloseServicio_2.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstDesgloseServicio_2.DataContext = value
            End Set
        End Property

        Public Property ResultadosDepartamentoServicio2() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosDepartamentoServicio2
            Get
                Return Me._lstDepartamentoServicio_2.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstDepartamentoServicio_2.DataContext = value
            End Set
        End Property

        Public Property ResultadosDocumentosMarca() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosDocumentosMarca
            Get
                Return Me._lstDocumentoMarca.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstDocumentoMarca.DataContext = value
            End Set
        End Property

        Public Property ResultadosDocumentosPatente() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosDocumentosPatente
            Get
                Return Me._lstDocumentoPatente.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstDocumentoPatente.DataContext = value
            End Set
        End Property

        Public Property ResultadosDocumentosTraduccion() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosDocumentosTraduccion
            Get
                Return Me._lstDocumentoTraduccion.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstDocumentoTraduccion.DataContext = value
            End Set
        End Property

        Public Property ResultadosRecurso() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosRecurso
            Get
                Return Me._lstRecurso.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstRecurso.DataContext = value
            End Set
        End Property

        Public Property ResultadosMaterial() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosMaterial
            Get
                Return Me._lstMaterial.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstMaterial.DataContext = value
            End Set
        End Property

        Public Property ResultadosFacFactuDeta() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosFacFactuDeta
            Get
                Return Me._lstDetalle.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstDetalle.DataContext = value
            End Set
        End Property


        Public Property ResultadosMarca() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosMarca
            Get
                Return Me._lstMarcas.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstMarcas.DataContext = value
            End Set
        End Property

        Public Property ResultadosMultiplesMarcas() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosMultiplesMarcas
            Get
                Return Me._lstMultiplesMarcas.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstMultiplesMarcas.DataContext = value
            End Set
        End Property

        Public Property ResultadosPatente() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosPatente
            Get
                Return Me._lstPatentes.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstPatentes.DataContext = value
            End Set
        End Property

        Public Property ResultadosMultiplesPatentes() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosMultiplesPatentes
            Get
                Return Me._lstMultiplesPatentes.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstMultiplesPatentes.DataContext = value
            End Set
        End Property

        Public Property ResultadosAnualidad() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ResultadosAnualidad
            Get
                Return Me._lstDocumentoTraduccion.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstDocumentoTraduccion.DataContext = value
            End Set
        End Property

        Public ReadOnly Property DesgloseServicio_2Seleccionado() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.DepartamentoServicio_2Seleccionado
            Get
                Return Me._lstDepartamentoServicio_2.SelectedItem
            End Get
        End Property

        Public ReadOnly Property FacFactuDeta_2Seleccionado() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.FacFactuDeta_2Seleccionado
            Get
                Return Me._lstDetalle.SelectedItem
            End Get
        End Property

        Public ReadOnly Property Marcas_Seleccionado() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Marcas_Seleccionado
            Get
                Return Me._lstMarcas.SelectedItem
            End Get
        End Property

        Public ReadOnly Property Patentes_Seleccionado() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Patentes_Seleccionado
            Get
                Return Me._lstPatentes.SelectedItem
            End Get
        End Property

        Public ReadOnly Property DocumentoMarcas_Seleccionado() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.DocumentoMarca_Seleccionado
            Get
                Return Me._lstDocumentoMarca.SelectedItem
            End Get
        End Property

        Public ReadOnly Property DocumentoPatente_Seleccionado() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.DocumentoPatente_Seleccionado
            Get
                Return Me._lstDocumentoPatente.SelectedItem
            End Get
        End Property

        Public ReadOnly Property DesgloseServicio_Seleccionado() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.DesgloseServicio_Seleccionado
            Get
                Return Me._lstDesgloseServicio_2.SelectedItem
            End Get
        End Property

        Public ReadOnly Property DocumentoTraduccion_Seleccionado() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.DocumentoTraduccion_Seleccionado
            Get
                Return Me._lstDocumentoTraduccion.SelectedItem
            End Get
        End Property

        Public ReadOnly Property Recurso_Seleccionado() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Recurso_Seleccionado
            Get
                Return Me._lstRecurso.SelectedItem
            End Get
        End Property

        Public ReadOnly Property Material_Seleccionado() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Material_Seleccionado
            Get
                Return Me._lstMaterial.SelectedItem
            End Get
        End Property

        Public ReadOnly Property Anualidad_Seleccionado() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Anualidad_Seleccionado
            Get
                Return Me._lstAnualidad.SelectedItem
            End Get
        End Property

        Public Property FacFactuDeta_Seleccionado() As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.FacFactuDeta_Seleccionado
            Get
                Return Me._lstDetalle.SelectedItem
            End Get

            Set(ByVal value As Object)
                Me._lstDetalle.SelectedItem = value
            End Set
        End Property



        Public Property VerTipo As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.VerTipo
            Set(ByVal value As String)

                Me._GbDepartamentoServicio.Visibility = Windows.Visibility.Collapsed
                'Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
                Me._GbDetalle.Visibility = Windows.Visibility.Collapsed
                Me._gridDatos.Visibility = Windows.Visibility.Collapsed
                _Wp_Btn.Visibility = Windows.Visibility.Collapsed
                Me._txtVerTipo.Text = value
                Select Case value
                    Case "1" 'Marca
                        Me._presentador.VerDocumentoMarcas()

                        Me._GbMarca.Visibility = Windows.Visibility.Visible
                        Me._GbPatente.Visibility = Windows.Visibility.Collapsed
                        Me._GbCantidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Collapsed
                        Me._GbRecursos.Visibility = Windows.Visibility.Collapsed
                        Me._GbMateriales.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
                        Me._GbAnualidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
                        Me._Wp_Salir.Visibility = Windows.Visibility.Visible
                    Case "2" 'Patente
                        Me._presentador.VerDocumentoPatentes()

                        Me._GbMarca.Visibility = Windows.Visibility.Collapsed
                        Me._GbPatente.Visibility = Windows.Visibility.Visible
                        Me._GbCantidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Collapsed
                        Me._GbRecursos.Visibility = Windows.Visibility.Collapsed
                        Me._GbMateriales.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
                        Me._GbAnualidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
                        Me._Wp_Salir.Visibility = Windows.Visibility.Visible
                    Case "3" 'Cantidad
                        '    Me._presentador.VerDocumentoTraducciones()

                        Me._GbMarca.Visibility = Windows.Visibility.Collapsed
                        Me._GbPatente.Visibility = Windows.Visibility.Collapsed
                        Me._GbCantidades.Visibility = Windows.Visibility.Visible
                        Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Collapsed
                        Me._GbRecursos.Visibility = Windows.Visibility.Collapsed
                        Me._GbMateriales.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
                        Me._GbAnualidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
                        Me._Wp_Salir.Visibility = Windows.Visibility.Visible
                    Case "4" 'Traduccion
                        Me._presentador.VerDocumentoTraducciones()

                        Me._GbMarca.Visibility = Windows.Visibility.Collapsed
                        Me._GbPatente.Visibility = Windows.Visibility.Collapsed
                        Me._GbCantidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Visible
                        Me._GbRecursos.Visibility = Windows.Visibility.Collapsed
                        Me._GbMateriales.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
                        Me._GbAnualidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
                        Me._Wp_Salir.Visibility = Windows.Visibility.Visible
                    Case "5" 'recursos
                        Me._presentador.VerFacRecursos()

                        Me._GbMarca.Visibility = Windows.Visibility.Collapsed
                        Me._GbPatente.Visibility = Windows.Visibility.Collapsed
                        Me._GbCantidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Collapsed
                        Me._GbRecursos.Visibility = Windows.Visibility.Visible
                        Me._GbMateriales.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
                        Me._GbAnualidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
                        Me._Wp_Salir.Visibility = Windows.Visibility.Visible
                    Case "6" 'Material
                        Me._presentador.VerMateriales()

                        Me._GbMarca.Visibility = Windows.Visibility.Collapsed
                        Me._GbPatente.Visibility = Windows.Visibility.Collapsed
                        Me._GbCantidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Collapsed
                        Me._GbRecursos.Visibility = Windows.Visibility.Collapsed
                        Me._GbMateriales.Visibility = Windows.Visibility.Visible
                        Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
                        Me._GbAnualidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
                        Me._Wp_Salir.Visibility = Windows.Visibility.Visible
                    Case "7" 'Multiples Marcas
                        'Me._presentador.VerMarcas()

                        Me._GbMarca.Visibility = Windows.Visibility.Collapsed
                        Me._GbPatente.Visibility = Windows.Visibility.Collapsed
                        Me._GbCantidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Collapsed
                        Me._GbRecursos.Visibility = Windows.Visibility.Collapsed
                        Me._GbMateriales.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Visible
                        Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
                        Me._GbAnualidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
                        Me._Wp_Salir.Visibility = Windows.Visibility.Visible
                        ocultar_mostrar_marcas(1)

                    Case "8" 'Marcas
                        'Me._presentador.VerMarcas()

                        Me._GbMarca.Visibility = Windows.Visibility.Collapsed
                        Me._GbPatente.Visibility = Windows.Visibility.Collapsed
                        Me._GbCantidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Collapsed
                        Me._GbRecursos.Visibility = Windows.Visibility.Collapsed
                        Me._GbMateriales.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Visible
                        Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
                        Me._GbAnualidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
                        Me._Wp_Salir.Visibility = Windows.Visibility.Visible
                        ocultar_mostrar_marcas(2)

                    Case "9" 'Marcas
                        'Me._presentador.VerMarcas()

                        Me._GbMarca.Visibility = Windows.Visibility.Collapsed
                        Me._GbPatente.Visibility = Windows.Visibility.Collapsed
                        Me._GbCantidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Collapsed
                        Me._GbRecursos.Visibility = Windows.Visibility.Collapsed
                        Me._GbMateriales.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Visible
                        Me._GbAnualidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
                        Me._Wp_Salir.Visibility = Windows.Visibility.Visible
                        ocultar_mostrar_Patentes(1)

                    Case "10" 'Marcas
                        'Me._presentador.VerMarcas()

                        Me._GbMarca.Visibility = Windows.Visibility.Collapsed
                        Me._GbPatente.Visibility = Windows.Visibility.Collapsed
                        Me._GbCantidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Collapsed
                        Me._GbRecursos.Visibility = Windows.Visibility.Collapsed
                        Me._GbMateriales.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Visible
                        Me._GbAnualidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
                        Me._Wp_Salir.Visibility = Windows.Visibility.Visible
                        ocultar_mostrar_Patentes(2)

                    Case "11" 'Anualidad
                        Me._presentador.VerAnualidades()

                        Me._GbMarca.Visibility = Windows.Visibility.Collapsed
                        Me._GbPatente.Visibility = Windows.Visibility.Collapsed
                        Me._GbCantidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Collapsed
                        Me._GbRecursos.Visibility = Windows.Visibility.Collapsed
                        Me._GbMateriales.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
                        Me._GbAnualidades.Visibility = Windows.Visibility.Visible
                        Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
                        Me._Wp_Salir.Visibility = Windows.Visibility.Visible

                    Case "12" 'DesgloseServicio
                        Me._presentador.VerDesgloseServicios()

                        Me._GbMarca.Visibility = Windows.Visibility.Collapsed
                        Me._GbPatente.Visibility = Windows.Visibility.Collapsed
                        Me._GbCantidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Collapsed
                        Me._GbRecursos.Visibility = Windows.Visibility.Collapsed
                        Me._GbMateriales.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
                        Me._GbAnualidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDesgloseServicio.Visibility = Windows.Visibility.Visible
                        Me._Wp_Salir.Visibility = Windows.Visibility.Visible

                    Case "13" 'despues de agregar detalle 
                        Me._presentador.VerDesgloseServicios()

                        Me._GbMarca.Visibility = Windows.Visibility.Collapsed
                        Me._GbPatente.Visibility = Windows.Visibility.Collapsed
                        Me._GbCantidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDocumentoTraduccion.Visibility = Windows.Visibility.Collapsed
                        Me._GbRecursos.Visibility = Windows.Visibility.Collapsed
                        Me._GbMateriales.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
                        Me._GbMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
                        Me._GbAnualidades.Visibility = Windows.Visibility.Collapsed
                        Me._GbDesgloseServicio.Visibility = Windows.Visibility.Collapsed
                        Me._Wp_Salir.Visibility = Windows.Visibility.Collapsed

                        Me._GbDetalle.Visibility = Windows.Visibility.Visible
                        Me._gridDatos.Visibility = Windows.Visibility.Visible
                        _Wp_Btn.Visibility = Windows.Visibility.Visible

                        'Me._lstDesgloseServicio_2.DataContext = Nothing
                        Me._lstDepartamentoServicio_2.DataContext = Nothing
                        Me._lstDocumentoMarca.DataContext = Nothing
                        Me._lstDocumentoPatente.DataContext = Nothing
                        Me._lstDocumentoTraduccion.DataContext = Nothing
                        Me._lstRecurso.DataContext = Nothing
                        Me._lstMaterial.DataContext = Nothing
                        Me._lstMarcas.DataContext = Nothing
                        Me._lstMultiplesMarcas.DataContext = Nothing
                        Me._lstPatentes.DataContext = Nothing
                        Me._lstMultiplesPatentes.DataContext = Nothing
                        Me._lstDocumentoTraduccion.DataContext = Nothing


                        'Case Else
                        '    Debug.WriteLine("Not between 1 and 10, inclusive")
                End Select
            End Set
            Get
                Return Me._txtVerTipo.Text
            End Get
        End Property


        Public Property Marca As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Marca

            Get
                Return Me._lstMarcas.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._lstMarcas.SelectedItem = value
                Me._lstMarcas.ScrollIntoView(value)
            End Set
        End Property

        Public Property Marcas As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Marcas
            Get
                Return Me._lstMarcas.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstMarcas.DataContext = value
            End Set
        End Property

        Public Property Patente As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Patente

            Get
                Return Me._lstPatentes.SelectedItem
            End Get
            Set(ByVal value As Object)
                Me._lstPatentes.SelectedItem = value
                Me._lstPatentes.ScrollIntoView(value)
            End Set
        End Property

        Public Property Patentes As Object Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Patentes
            Get
                Return Me._lstPatentes.DataContext
            End Get
            Set(ByVal value As Object)
                Me._lstPatentes.DataContext = value
            End Set
        End Property

        'Private Sub _btnConsultarMarca_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        '    Me._presentador.BuscarMarca()
        'End Sub

        'Private Sub _btnConsultarPatente_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        '    Me._presentador.BuscarPatente()
        'End Sub

        'Private Sub _txtMarca_GotFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
        '    Me._txtMarca.Visibility = System.Windows.Visibility.Collapsed
        '    Me._lstMarcas.Visibility = System.Windows.Visibility.Visible
        '    Me._lstMarcas.IsEnabled = True
        '    Me._btnConsultarMarca.Visibility = System.Windows.Visibility.Visible
        '    Me._txtIdMarca.Visibility = System.Windows.Visibility.Visible
        '    Me._txtNombreMarca.Visibility = System.Windows.Visibility.Visible
        '    Me._lblIdMarca.Visibility = System.Windows.Visibility.Visible
        '    Me._lblNombreMarca.Visibility = System.Windows.Visibility.Visible
        'End Sub

        'Public Property NombreMarca() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.NombreMarca
        '    Get
        '        Return Me._txtMarca.Text
        '    End Get
        '    Set(ByVal value As String)
        '        Me._txtMarca.Text = value
        '    End Set
        'End Property

        Public Property idMarcaFiltrar() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.idMarcaFiltrar
            Get
                Return Me._txtIdMarca.Text
            End Get
            Set(ByVal value As String)
                Me._txtIdMarca.Text = value
            End Set
        End Property

        Public Property NombreMarcaFiltrar() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.NombreMarcaFiltrar
            Get
                Return Me._txtNombreMarca.Text
            End Get
            Set(ByVal value As String)
                Me._txtNombreMarca.Text = value
            End Set
        End Property

        Public Property idPatenteFiltrar() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.idPatenteFiltrar
            Get
                Return Me._txtIdPatente.Text
            End Get
            Set(ByVal value As String)
                Me._txtIdPatente.Text = value
            End Set
        End Property

        Public Property NombrePatenteFiltrar() As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.NombrePatenteFiltrar
            Get
                Return Me._txtNombrePatente.Text
            End Get
            Set(ByVal value As String)
                Me._txtNombrePatente.Text = value
            End Set
        End Property


        Public Sub ocultar_mostrar_marcas(ByVal valor As Integer)
            If (valor = 1) Then
                _WPMarcas.Visibility = Windows.Visibility.Visible
                _WPListaMarcas.Visibility = Windows.Visibility.Visible
                _WPEBtnMultiplesMarcas.Visibility = Windows.Visibility.Visible
                _WPLtvMultiplesMarcas.Visibility = Windows.Visibility.Visible
                _WPLblMultiplesMarcas.Visibility = Windows.Visibility.Visible
            Else
                If (valor = 2) Then
                    _WPMarcas.Visibility = Windows.Visibility.Visible
                    _WPListaMarcas.Visibility = Windows.Visibility.Visible
                    _WPEBtnMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
                    _WPLtvMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
                    _WPLblMultiplesMarcas.Visibility = Windows.Visibility.Collapsed
                End If
            End If
        End Sub

        Public Sub ocultar_mostrar_Patentes(ByVal valor As Integer)
            If (valor = 1) Then
                _WPPatentes.Visibility = Windows.Visibility.Visible
                _WPListaPatentes.Visibility = Windows.Visibility.Visible
                _WPEBtnMultiplesPatentes.Visibility = Windows.Visibility.Visible
                _WPLtvMultiplesPatentes.Visibility = Windows.Visibility.Visible
                _WPLblMultiplesPatentes.Visibility = Windows.Visibility.Visible
            Else
                If (valor = 2) Then
                    _WPPatentes.Visibility = Windows.Visibility.Visible
                    _WPListaPatentes.Visibility = Windows.Visibility.Visible
                    _WPEBtnMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
                    _WPLtvMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
                    _WPLblMultiplesPatentes.Visibility = Windows.Visibility.Collapsed
                End If
            End If
        End Sub

        Public Property NCantidad As Integer Implements Contratos.FacFacturas.IConsultarFacFacturaPase.NCantidad
            Get
                Return Me._txtNCantidad.Text
            End Get
            Set(ByVal value As Integer)
                Me._txtNCantidad.Text = value
            End Set
        End Property

        Public Property BDetalle As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.BDetalle
            Get
                Return GetFormatoDouble(Me._txtBDetalle.Text)
            End Get
            Set(ByVal value As Double)
                'Convert.ToDecimal(value).ToString("N2")
                Me._txtBDetalle.Text = SetFormatoDouble(value)
                'Me._txtBDetalle.Text = Format(value, "0.00##")
            End Set
        End Property

        Public Property pu As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Pu
            Get
                Return GetFormatoDouble(Me._txtPu.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtPu.Text = SetFormatoDouble(value)
                'Dim a As Double = Convert.ToDecimal(value)
                'Dim b As Double = Format(value, "#,0.00##")
                'Me._txtPu.Text = Convert.ToDecimal(value)
            End Set
        End Property

        Public Property Descuento As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Descuento
            Get
                Return GetFormatoDouble(Me._txtDescuento.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtDescuento.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property MSubtimpo As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.MSubtimpo
            Get
                Return GetFormatoDouble(Me._txtMSubtimpo.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtMSubtimpo.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property MSubtimpoBf As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.MSubtimpoBf
            Get
                Return GetFormatoDouble(Me._txtMSubtimpoBf.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtMSubtimpoBf.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property PDescuento As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.PDescuento
            Get
                Return GetFormatoDouble(Me._txtPDescuento.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtPDescuento.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property MDescuento As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.MDescuento
            Get
                Return GetFormatoDouble(Me._txtMDescuento.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtMDescuento.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property MDescuentoBf As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.MDescuentoBf
            Get
                Return GetFormatoDouble(Me._txtMDescuentoBf.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtMDescuentoBf.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property MTbimp As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.MTbimp
            Get
                Return GetFormatoDouble(Me._txtMTbimp.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtMTbimp.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property MTbimpBf As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.MTbimpBf
            Get
                Return GetFormatoDouble(Me._txtMTbimpBf.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtMTbimpBf.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property Mtbexc As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Mtbexc
            Get
                Return GetFormatoDouble(Me._txtMtbexc.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtMtbexc.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property MtbexcBf As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.MtbexcBf
            Get
                Return GetFormatoDouble(Me._txtMtbexcBf.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtMtbexcBf.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property Msubtotal As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Msubtotal
            Get
                Return GetFormatoDouble(Me._txtMsubtotal.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtMsubtotal.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property MsubtotalBf As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.MsubtotalBf
            Get
                Return GetFormatoDouble(Me._txtMsubtotalBf.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtMsubtotalBf.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property Impuesto As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Impuesto
            Get
                Return Me._txtImpuesto.Text
            End Get
            Set(ByVal value As String)
                Me._txtImpuesto.Text = value
            End Set
        End Property

        Public Property Mtimp As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Mtimp
            Get
                Return GetFormatoDouble(Me._txtMtimp.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtMtimp.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property MtimpBf As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.MtimpBf
            Get
                Return GetFormatoDouble(Me._txtMtimpBf.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtMtimpBf.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property Mttotal As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Mttotal
            Get
                Return GetFormatoDouble(Me._txtMttotal.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtMttotal.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property MttotalBf As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.MttotalBf
            Get
                Return GetFormatoDouble(Me._txtMttotalBf.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtMttotalBf.Text = SetFormatoDouble(value)
            End Set
        End Property

        Public Property Desc As Double Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Desc
            Get
                Return GetFormatoDouble(Me._txtdesc.Text)
            End Get
            Set(ByVal value As Double)
                Me._txtdesc.Text = SetFormatoDouble(value)
            End Set
        End Property


        Public WriteOnly Property Activar_Desactivar As Boolean Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Activar_Desactivar
            Set(ByVal value As Boolean)
                _txtNCantidad.IsEnabled = value
                _txtPu.IsEnabled = value
                _txtDescuento.IsEnabled = value
                _txtBDetalle.IsEnabled = value
                _btnrecalcular.IsEnabled = value
            End Set
        End Property

        Public WriteOnly Property Desactivar_Descuento As Boolean Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Desactivar_Descuento
            Set(ByVal value As Boolean)

                _txtDescuento.IsEnabled = value
            End Set
        End Property

        Public WriteOnly Property Desactivar_Precio As Boolean Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Desactivar_Precio
            Set(ByVal value As Boolean)
                _txtPu.IsEnabled = value
            End Set
        End Property

        Public Sub _btnLimpiar_Click()
            InitializeComponent()
            Me._cargada = False
            Me._presentador = New PresentadorConsultarFacFacturaPase(Me, FacFactura)
        End Sub

        Public ReadOnly Property Tipo As Char Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Tipo
            Get
                If Not String.IsNullOrEmpty(Me._cbxItipo.Text) Then
                    Return (Me._cbxItipo.Text)(0)
                Else
                    Return " "c
                End If
            End Get
        End Property

        Public ReadOnly Property Localidad2 As Char Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Localidad2
            Get
                If Not String.IsNullOrEmpty(Me._cbxLocal2.Text) Then
                    Return (Me._cbxLocal2.Text)(0)
                Else
                    Return " "c
                End If
            End Get
        End Property

        Public ReadOnly Property ServicioCod_Cont As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ServicioCod_Cont
            Get
                Return _txtServicioCod_Cont.Text
            End Get
        End Property

        Public ReadOnly Property ServicioId As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ServicioId
            Get
                Return _txtServicioId.Text
            End Get
        End Property

        Public ReadOnly Property ServicioXreferencia As String Implements Contratos.FacFacturas.IConsultarFacFacturaPase.ServicioXreferencia
            Get
                Return _txtServicioXreferencia.Text
            End Get
        End Property

        Public Function GetFormatoDouble(ByVal texto As String) As String
            'Dim valor As String = Replace(texto, ",", "")
            'valor = Replace(valor, ".", ",")

            'Return valor
            Return _presentador.GetFormatoDouble2(texto)
        End Function

        Public Function SetFormatoDouble(ByVal value As Double) As String

            'Console.WriteLine("{0,-6} {1}", Name + ":", value.ToString("N3", System.Globalization.CultureInfo.CreateSpecificCulture("en-US").NumberFormat))

            'Return value.ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("en-US").NumberFormat)
            Return _presentador.SetFormatoDouble2(value)
            'Return Replace(value.ToString("C2", New System.Globalization.CultureInfo("en-us")).Remove(0, 1), ",", "")
        End Function

        Public Sub Al_Mostrar_Detalle_Servicio(ByVal sender As Object, ByVal args As ExecutedRoutedEventArgs)
            Dim cproformadetalle As Integer = args.Parameter
            Me._presentador.buscar_departamento_servicio_esp(cproformadetalle)
        End Sub
        Public Shared Mostrar_Detalle_Servicio As New RoutedCommand("Mostrar_Detalle_Servicio", GetType(ConsultarFacFacturaPase))

        Public WriteOnly Property Ocultar_Botones As Boolean Implements Contratos.FacFacturas.IConsultarFacFacturaPase.Ocultar_Botones
            Set(ByVal value As Boolean)
                If value = True Then
                    _btnModificar.Visibility = Windows.Visibility.Collapsed
                    _btnEliminar.Visibility = Windows.Visibility.Collapsed
                    _btnRegresar.IsEnabled = True
                    _btnRegresar.Visibility = Windows.Visibility.Visible
                End If
            End Set
        End Property

        Private Sub _btnIrAsociado_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
            Dim parametro As String
            parametro = String.Empty
            If (DirectCast(sender, Button)).Name.Equals("_btnIrAsociadoFacFactura") Then
                parametro = "_btnIrAsociadoFacFactura"
            ElseIf (DirectCast(sender, Button)).Name.Equals("_btnIrAsociadoImpresion") Then
                parametro = "_btnIrAsociadoImpresion"
            End If
            Me._presentador.ConsultarAsociado(parametro)
        End Sub

        Private Sub _btnInteresadoFacFactura_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
            Me._presentador.ConsultarInteresadoFactura()
        End Sub

        Private Sub _btnOurref_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
            Me._presentador.ConsultarMarcasPatentes()
        End Sub
    End Class
End Namespace
