﻿Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Configuration
Imports System.Linq
Imports System.Net.Sockets
Imports System.Runtime.Remoting
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports NLog
Imports Trascend.Bolet.Cliente.Ayuda
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.FacInternacionales
Imports Diginsoft.Bolet.Cliente.Fac.Ventanas.FacInternacionales
Imports Diginsoft.Bolet.Cliente.Fac.Ventanas.FacFacturaProformas
'Imports Diginsoft.Bolet.Cliente.Fac.Ventanas.Principales
Imports Diginsoft.Bolet.ObjetosComunes.ContratosServicios
Imports Diginsoft.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.ObjetosComunes.ContratosServicios
Imports Trascend.Bolet.Cliente.Presentadores
Imports Trascend.Bolet.Cliente.Ventanas.Principales

Namespace Presentadores.FacInternacionales
    Class PresentadorConsultarFacInternacionalesProforma
        Inherits PresentadorBase
        Private Shared _paginaPrincipal As PaginaPrincipal = PaginaPrincipal.ObtenerInstancia
        Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

        Private _ventana As IConsultarFacInternacionalesProforma
        Private _FacFacturaProformaServicios As IFacFacturaProformaServicios
        'Private _FacFacturaProformas As IList(Of FacFacturaProforma)
        'Private _asociados As IAsociadoServicios
        Private _asociadosServicios As IAsociadoServicios
        Private _asociados As IList(Of Asociado)
        Private _facbancos As IFacBancoServicios
        Private _facbancosServicios As IFacBancoServicios
        Private _idiomasServicios As IIdiomaServicios
        Private _monedasServicios As IMonedaServicios
        Private _facOperacionProformasServicios As IFacOperacionProformaServicios
        Private _FacInternacionalesServicios As IFacInternacionalServicios
        Private _PaisServicios As IPaisServicios
        Private _datosInternacionales As Boolean

        ''' <summary>
        ''' Constructor Predeterminado
        ''' </summary>
        ''' <param name="ventana">página que satisface el contrato</param>
        Public Sub New(ByVal ventana As IConsultarFacInternacionalesProforma)
            Try
                Me._ventana = ventana
                Me._FacFacturaProformaServicios = DirectCast(Activator.GetObject(GetType(IFacFacturaProformaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacFacturaProformaServicios")), IFacFacturaProformaServicios)
                Me._asociadosServicios = DirectCast(Activator.GetObject(GetType(IAsociadoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("AsociadoServicios")), IAsociadoServicios)
                Me._facbancosServicios = DirectCast(Activator.GetObject(GetType(IFacBancoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacBancoServicios")), IFacBancoServicios)
                Me._idiomasServicios = DirectCast(Activator.GetObject(GetType(IIdiomaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("IdiomaServicios")), IIdiomaServicios)
                Me._monedasServicios = DirectCast(Activator.GetObject(GetType(IMonedaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("MonedaServicios")), IMonedaServicios)
                Me._facOperacionProformasServicios = DirectCast(Activator.GetObject(GetType(IFacOperacionProformaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacOperacionProformaServicios")), IFacOperacionProformaServicios)
                Me._FacInternacionalesServicios = DirectCast(Activator.GetObject(GetType(IFacInternacionalServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacInternacionalServicios")), IFacInternacionalServicios)
                Me._PaisServicios = DirectCast(Activator.GetObject(GetType(IPaisServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("PaisServicios")), IPaisServicios)
            Catch ex As Exception
                logger.[Error](ex.Message)
                Me.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, True)
            End Try
        End Sub

        Public Sub Limpiar()
            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region

            Me.Navegar(New ConsultarFacInternacionalesProforma())
            'Me.Navegar(New ConsultarFacCobro())
            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region
        End Sub

        Public Sub ActualizarTitulo()
            Me.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.fac_titleConsultarFacInternacionalesProforma, Recursos.Ids.fac_ConsultarFacInternacionalesProforma)
        End Sub

        ''' <summary>
        ''' Método que carga los datos iniciales a mostrar en la página
        ''' </summary>
        Public Sub CargarPagina()
            Mouse.OverrideCursor = Cursors.Wait

            Try
                '#Region "trace"
                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                End If
                '#End Region

                ActualizarTitulo()

                CargarPaises()

                'Me._FacFacturaProformas = Me._FacFacturaProformaServicios.ConsultarTodos()
                'Dim FacFacturaProformaAuxiliar As New FacFacturaProforma
                'FacFacturaProformaAuxiliar.Inicial = UsuarioLogeado.Iniciales
                'FacFacturaProformaAuxiliar.Status = 1 ' esto es para el campo auto porque es in

                'Dim FacFacturaProformas As IList(Of FacFacturaProforma)
                'FacFacturaProformas = Me._FacFacturaProformaServicios.ObtenerFacFacturaProformasFiltro(FacFacturaProformaAuxiliar)

                'Me._ventana.Resultados = FacFacturaProformas
                'Consultar()
                Me._ventana.FacFacturaProformaFiltrar = New FacFacturaProforma
                'sumar(FacFacturaProformas)

                'Me._asociados = Me._asociadosServicios.ConsultarTodos()
                'Me._ventana.Asociados = Me._asociados

                'Dim facbancos As IList(Of FacBanco) = Me._facbancosServicios.ObtenerFacBancosFiltro(Nothing)()
                'Dim primerafacbanco As New FacBanco()
                'primerafacbanco.Id = Integer.MinValue
                'facbancos.Insert(0, primerafacbanco)
                'Me._ventana.Bancos = facbancos

                'Dim idiomas As IList(Of Idioma) = Me._idiomasServicios.ConsultarTodos()
                'Dim primeridioma As New Idioma
                'primeridioma.Id = ""
                'idiomas.Insert(0, primeridioma)
                'Me._ventana.Idiomas = idiomas

                'Dim monedas As IList(Of Moneda) = Me._monedasServicios.ConsultarTodos()
                'Dim primeramoneda As New Moneda
                'primeramoneda.Id = ""
                'monedas.Insert(0, primeramoneda)
                'Me._ventana.Monedas = monedas

                Me._ventana.FocoPredeterminado()

                '#Region "trace"
                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                    '#End Region
                End If
            Catch ex As ApplicationException
                logger.[Error](ex.Message)
                Me.Navegar(ex.Message, True)
            Catch ex As RemotingException
                logger.[Error](ex.Message)
                Me.Navegar(Recursos.MensajesConElUsuario.ErrorRemoting, True)
            Catch ex As SocketException
                logger.[Error](ex.Message)
                Me.Navegar(Recursos.MensajesConElUsuario.ErrorConexionServidor, True)
            Catch ex As Exception
                logger.[Error](ex.Message)
                Me.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, True)
            Finally
                Mouse.OverrideCursor = Nothing
            End Try
        End Sub


        Public Function consultar_OperacionProformas(ByVal id As Integer) As FacOperacionProforma
            Try
                Dim FacOperacionProformaAuxiliar As New FacOperacionProforma
                Dim FacOperacionProforma As List(Of FacOperacionProforma)

                FacOperacionProformaAuxiliar.CodigoOperacion = id
                FacOperacionProformaAuxiliar.Id = "ND"
                FacOperacionProforma = Me._facOperacionProformasServicios.ObtenerFacOperacionProformasFiltro(FacOperacionProformaAuxiliar)
                If FacOperacionProforma IsNot Nothing Then
                    If FacOperacionProforma.Count > 0 Then
                        Return (FacOperacionProforma(0))
                    Else
                        Return (Nothing)
                    End If
                Else
                    Return (Nothing)
                End If
            Catch ex As Exception
                logger.[Error](ex.Message)
                Me.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, True)
                Return (Nothing)
            End Try
        End Function

        Public Function buscar_facinternacional(ByVal proforma As Integer) As FacInternacional
            Dim facinternacionalaux As New FacInternacional
            facinternacionalaux.Id = proforma
            Dim facinternacionales As List(Of FacInternacional) = Me._FacInternacionalesServicios.ObtenerFacInternacionalesFiltro(facinternacionalaux)
            If facinternacionales IsNot Nothing Then
                If facinternacionales.Count > 0 Then
                    Return (facinternacionales(0))
                Else
                    Return (Nothing)
                End If
            Else
                Return (Nothing)
            End If
        End Function

        Public Sub Asignar_proforma(ByRef proforma As IList(Of FacFacturaProforma))
            Try
                'Dim operacion As FacOperacionProforma
                Dim internacional As FacInternacional
                For i As Integer = 0 To proforma.Count - 1
                    'operacion = consultar_OperacionProformas(proforma(i).Id)
                    'If operacion IsNot Nothing Then
                    '    proforma(i).Mttotal = operacion.Saldo
                    'End If
                    internacional = buscar_facinternacional(proforma(i).Id)
                    If internacional IsNot Nothing Then
                        proforma(i).SelecReg = True
                        If internacional.FechaPago IsNot Nothing Then
                            proforma(i).Selecpag = True
                        Else
                            proforma(i).Selecpag = False
                        End If
                    Else
                        proforma(i).SelecReg = False
                        proforma(i).Selecpag = False
                    End If

                    ''' Asignacion de los datos de FacInternacional a los campos de FacFacturaProforma
                    If internacional IsNot Nothing Then
                        If internacional.Asociado_o IsNot Nothing Then
                            proforma(i).AsociadoO = internacional.Asociado_o
                        Else
                            proforma(i).AsociadoO = Nothing
                        End If

                        If Not String.IsNullOrEmpty(internacional.Numerofactura) Then
                            proforma(i).NumeroFacInt = internacional.Numerofactura
                        Else
                            proforma(i).NumeroFacInt = Nothing
                        End If

                        proforma(i).MontoFacInt = internacional.Monto
                        proforma(i).FechaFacAsocInt = internacional.Fecha
                        proforma(i).PaisAsocInt = internacional.Pais
                        proforma(i).DetalleFacAsocInt = internacional.Detalle
                        proforma(i).FechaRecepcionFacAsocInt = internacional.FechaRecepcion
                    End If

                Next





            Catch ex As Exception
                logger.[Error](ex.Message)
                Me.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, True)
            End Try
        End Sub
        ''' <summary>
        ''' Método que realiza una consulta al servicio, con el fin de filtrar los datos que se muestran 
        ''' por pantalla
        ''' </summary>
        Public Sub Consultar()

            Dim facProformas As New List(Of FacFacturaProforma)()

            Mouse.OverrideCursor = Cursors.Wait
            Try
                '#Region "trace"
                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                End If
                '#End Region
                'Dim filtroValido As Boolean = False
                'Dim filtroValido As Integer = 0
                'Variable utilizada para limitar a que el filtro se ejecute solo cuando 
                'dos filtros sean utilizados

                Dim FacFacturaProformaAuxiliar As New FacFacturaProforma()
                Dim FacFacturaProformasfiltro As FacFacturaProforma = DirectCast(_ventana.FacFacturaProformaFiltrar, FacFacturaProforma)

                If Not Me._ventana.Id.Equals("") Then
                    FacFacturaProformaAuxiliar.Id = Integer.Parse(Me._ventana.Id)
                End If

                'If Not Me._ventana.CreditoSent.Equals("") Then
                '    FacFacturaProformaAuxiliar.CreditoSent = Integer.Parse(Me._ventana.CreditoSent)
                'End If

                If (Me._ventana.Asociado IsNot Nothing) AndAlso (DirectCast(Me._ventana.Asociado, Asociado).Id <> Integer.MinValue) Then
                    FacFacturaProformaAuxiliar.Asociado = DirectCast(Me._ventana.Asociado, Asociado)
                End If

                'If (Me._ventana.Banco IsNot Nothing) AndAlso (DirectCast(Me._ventana.Banco, FacBanco).Id <> Integer.MinValue) Then
                '    'FacFacturaProformaAuxiliar.Banco = DirectCast(Me._ventana.Banco, FacBanco)
                'End If

                'If (Me._ventana.Idioma IsNot Nothing) AndAlso (DirectCast(Me._ventana.Idioma, Idioma).Id <> "") Then
                '    FacFacturaProformaAuxiliar.Idioma = DirectCast(Me._ventana.Idioma, Idioma)
                'End If

                'If (Me._ventana.Moneda IsNot Nothing) AndAlso (DirectCast(Me._ventana.Moneda, Moneda).Id <> "") Then
                '    FacFacturaProformaAuxiliar.Moneda = DirectCast(Me._ventana.Moneda, Moneda)
                'End If

                If Not Me._ventana.FechaFactura.Equals("") Then
                    Dim FechaFacFacturaProforma As DateTime = DateTime.Parse(Me._ventana.FechaFactura)
                    FacFacturaProformaAuxiliar.FechaFactura = FechaFacFacturaProforma
                End If

                'If (filtroValido = True) Then

                'FacFacturaProformaAuxiliar.Inicial = UsuarioLogeado.Iniciales
                'FacFacturaProformaAuxiliar.Status = 1 ' esto es para el campo auto porque es in
                If FacFacturaProformasfiltro IsNot Nothing Then
                    If FacFacturaProformasfiltro.FechaDesde IsNot Nothing And FacFacturaProformasfiltro.FechaHasta IsNot Nothing Then
                        If FacFacturaProformasfiltro.FechaDesde < FacFacturaProformasfiltro.FechaHasta Then
                            FacFacturaProformaAuxiliar.FechaDesde = FacFacturaProformasfiltro.FechaDesde
                            FacFacturaProformaAuxiliar.FechaHasta = FacFacturaProformasfiltro.FechaHasta
                        End If
                    End If
                End If
                FacFacturaProformaAuxiliar.Local = "I"

                If Me._ventana.AsociadoInternacional IsNot Nothing Then
                    _datosInternacionales = True
                ElseIf Not Me._ventana.NumeroFactInternacional.Equals("") Then
                    _datosInternacionales = True
                ElseIf Me._ventana.PaisAsocInt IsNot Nothing Then
                    _datosInternacionales = True
                ElseIf Not String.IsNullOrEmpty(Me._ventana.DetalleFacAsocInt) Then
                    _datosInternacionales = True
                End If



                If Me._datosInternacionales Then

                    Dim facturaInternacional As New FacInternacional()

                    facturaInternacional = CargarDatosFacturaInternacional(facturaInternacional)


                    'facturaInternacional.Asociado_o = DirectCast(Me._ventana.AsociadoInternacional, Asociado)

                    Dim listaFacturasInternacionales As IList(Of FacInternacional)
                    Dim proformas As New List(Of FacFacturaProforma)()
                    Dim proformaAux As New FacFacturaProforma()

                    listaFacturasInternacionales = Me._FacInternacionalesServicios.ObtenerFacInternacionalesFiltro(facturaInternacional)

                    For Each facInternacional As FacInternacional In listaFacturasInternacionales
                        Dim codigoProforma As Integer
                        codigoProforma = facInternacional.Id
                        proformaAux.Id = codigoProforma
                        proformas = Me._FacFacturaProformaServicios.ObtenerFacFacturaProformasFiltro(proformaAux)
                        'Asignar_proforma(proformas)
                        Dim aux As FacFacturaProforma = proformas(0)
                        facProformas.Add(aux)
                    Next
                    Asignar_proforma(facProformas)
                    Me._ventana.Resultados = Nothing
                    Me._ventana.Count = facProformas.Count
                    Me._ventana.Resultados = facProformas


                Else
                    Dim FacFacturaProformas As IList(Of FacFacturaProforma)
                    FacFacturaProformas = Me._FacFacturaProformaServicios.ObtenerFacFacturaProformasFiltro(FacFacturaProformaAuxiliar)
                    Asignar_proforma(FacFacturaProformas)
                    Me._ventana.Resultados = Nothing
                    Me._ventana.Count = FacFacturaProformas.Count
                    Me._ventana.Resultados = FacFacturaProformas
                End If

                'CODIGO ORIGINAL COMENTADO NO BORRAR
                'Dim FacFacturaProformas As IList(Of FacFacturaProforma)
                'FacFacturaProformas = Me._FacFacturaProformaServicios.ObtenerFacFacturaProformasFiltro(FacFacturaProformaAuxiliar)
                'Asignar_proforma(FacFacturaProformas)
                'Me._ventana.Resultados = Nothing
                'Me._ventana.Count = FacFacturaProformas.Count
                'Me._ventana.Resultados = FacFacturaProformas
                'FIN CODIGO ORIGINAL COMENTADO NO BORRAR

                'Else
                '    Me._ventana.Mensaje(Recursos.MensajesConElUsuario.ErrorFiltroIncompleto)
                'End If

                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                    '#End Region
                End If
            Catch ex As Exception
                logger.[Error](ex.Message)
                Me.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, True)
                Mouse.OverrideCursor = Nothing
            End Try
            Mouse.OverrideCursor = Nothing
        End Sub

        ''' <summary>
        ''' Método que invoca una nueva página "ConsultarFacFacturaProforma" y la instancia con el objeto seleccionado
        ''' </summary>
        Public Sub IrConsultarFacFacturaProforma()
            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region
            Me._ventana.FacFacturaProformaSeleccionado.Accion = 2 'no modificar
            Me.Navegar(New ConsultarFacFacturaProforma(Me._ventana.FacFacturaProformaSeleccionado))
            'Me.Navegar(New ConsultarFacFacturaProforma())
            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region
        End Sub

        'aqui la pantalla de registro
        Public Sub IrConsultarRegistrointernacional(ByVal cproforma As Integer)
            '#Region "trace"            
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region

            Dim proforma As FacFacturaProforma
            proforma = ConsultarProforma(cproforma)
            If proforma IsNot Nothing Then
                'aqui va la pantalla de facinternacionales regisro
                Me.Navegar(New FacInternacionalRegistro(proforma))
            Else

            End If

            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region
        End Sub


        ' aqui la pantalla de pago 
        Public Sub IrConsultarPagointernacional(ByVal cproforma As Integer)
            '#Region "trace"            
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region

            Dim proforma As FacFacturaProforma
            proforma = ConsultarProforma(cproforma)
            If proforma IsNot Nothing Then
                Dim internacional As FacInternacional
                internacional = buscar_facinternacional(proforma.Id)
                If Internacional IsNot Nothing Then
                    'aqui va la pantalla de facinternacionales Pago
                    Me.Navegar(New FacInternacionalPago(proforma))
                Else
                    MessageBox.Show("Debe realizar registro", "", MessageBoxButton.OK)
                    Mouse.OverrideCursor = Nothing
                    Exit Sub
                End If
            Else

            End If
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region
        End Sub

        Public Function ConsultarProforma(ByVal cproforma As Integer) As FacFacturaProforma
            Dim proforma As FacFacturaProforma = Nothing
            Try
                '#Region "trace"
                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                End If
                '#End Region

                Dim FacFacturaProformaAuxiliar As New FacFacturaProforma()
                FacFacturaProformaAuxiliar.Id = cproforma

                Dim FacFacturaProformas As IList(Of FacFacturaProforma)
                FacFacturaProformas = Me._FacFacturaProformaServicios.ObtenerFacFacturaProformasFiltro(FacFacturaProformaAuxiliar)

                If FacFacturaProformas IsNot Nothing Then
                    If FacFacturaProformas.Count > 0 Then
                        proforma = FacFacturaProformas(0)
                    Else
                        proforma = Nothing
                    End If
                Else
                    proforma = Nothing
                End If

                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                    '#End Region
                End If
            Catch ex As Exception
                logger.[Error](ex.Message)
                Me.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, True)
            End Try

            Return proforma
        End Function

        ''' <summary>
        ''' Método que ordena una columna
        ''' </summary>
        Public Sub OrdenarColumna(ByVal column As GridViewColumnHeader)
            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region

            Dim field As [String] = TryCast(column.Tag, [String])

            If Me._ventana.CurSortCol IsNot Nothing Then
                AdornerLayer.GetAdornerLayer(Me._ventana.CurSortCol).Remove(Me._ventana.CurAdorner)
                Me._ventana.ListaResultados.Items.SortDescriptions.Clear()
            End If

            Dim newDir As ListSortDirection = ListSortDirection.Ascending
            'If Me._ventana.CurSortCol = column AndAlso Me._ventana.CurAdorner.Direction = newDir Then
            If Me._ventana.CurAdorner IsNot Nothing Then
                If Me._ventana.CurSortCol.Equals(column) AndAlso Me._ventana.CurAdorner.Direction = newDir Then
                    newDir = ListSortDirection.Descending
                End If
            End If

            Me._ventana.CurSortCol = column
            Me._ventana.CurAdorner = New SortAdorner(Me._ventana.CurSortCol, newDir)
            AdornerLayer.GetAdornerLayer(Me._ventana.CurSortCol).Add(Me._ventana.CurAdorner)
            Me._ventana.ListaResultados.Items.SortDescriptions.Add(New SortDescription(field, newDir))

            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region
        End Sub

        Public Sub BuscarAsociado2()
            Mouse.OverrideCursor = Cursors.Wait
            Dim asociadoaux As New Asociado
            Dim asociados As List(Of Asociado) = Nothing
            Dim i As Boolean = False
            'asociadoaux = Nothing
            'Dim asociadosFiltrados As IEnumerable(Of Asociado) = Me._asociados
            Mouse.OverrideCursor = Cursors.Wait

            If Not String.IsNullOrEmpty(Me._ventana.idAsociadoFiltrar) And Me._ventana.idAsociadoFiltrar <> "0" Then
                asociadoaux.Id = Integer.Parse(Me._ventana.idAsociadoFiltrar)
                '
                '    asociadosFiltrados = From p In asociadosFiltrados Where p.Id = Integer.Parse(Me._ventana.idAsociadoFiltrar)
                i = True
            End If

            If Not String.IsNullOrEmpty(Me._ventana.NombreAsociadoFiltrar) Then
                asociadoaux.Nombre = UCase(Me._ventana.NombreAsociadoFiltrar)
                '    asociadosFiltrados = From p In asociadosFiltrados Where p.Nombre IsNot Nothing AndAlso p.Nombre.ToLower().Contains(Me._ventana.NombreAsociadoFiltrar.ToLower())
                i = True
            End If
            If i = True Then
                asociados = Me._asociadosServicios.ObtenerAsociadosFiltro(asociadoaux)
            Else
                Me._ventana.Asociados = Nothing
                Mouse.OverrideCursor = Nothing
                MessageBox.Show("Error: No Existe Asociado Relacionado a la Búsqueda")
                Exit Sub
            End If

            Dim primerasociado As New Asociado()
            primerasociado.Id = Integer.MinValue
            asociados.Insert(0, primerasociado)

            Me._ventana.Asociados = asociados

            Mouse.OverrideCursor = Nothing

            'If asociadosFiltrados.ToList.Count <> 0 Then
            '    Me._ventana.Asociados = asociadosFiltrados
            'Else
            '    Me._ventana.Asociados = Me._asociados
            'End If
            Mouse.OverrideCursor = Nothing
        End Sub

        Public Sub CambiarAsociado()
            Try
                'Dim asociado As Asociado = Me._asociadosServicios.ConsultarAsociadoConTodo(DirectCast(Me._ventana.Asociado, Asociado))
                'asociado.Contactos = Me._contactoServicios.ConsultarContactosPorAsociado(asociado)
                If DirectCast(Me._ventana.Asociado, Asociado) IsNot Nothing Then
                    If Me._ventana.Asociado.id <> Integer.MinValue Then
                        ' Dim asociado As Asociado = Me._asociadosServicios.ConsultarAsociadoConTodo(DirectCast(Me._ventana.Asociado, Asociado))
                        Me._ventana.NombreAsociado = DirectCast(Me._ventana.Asociado, Asociado).Id & " - " & DirectCast(Me._ventana.Asociado, Asociado).Nombre
                    Else
                        Me._ventana.NombreAsociado = Nothing
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If
                'Me._ventana.Personas = asociado.Contactos
            Catch e As ApplicationException
                'Me._ventana.Personas = Nothing
            End Try
        End Sub


        '''<sumary>
        ''' Metodo para buscar un Asociado Internacional
        '''</sumary>
        Sub BuscarAsociadoInternacional()

            Dim asociadoaux As New Asociado
            Dim asociados As List(Of Asociado) = Nothing
            Dim i As Boolean = False

            Mouse.OverrideCursor = Cursors.Wait

            If Not String.IsNullOrEmpty(Me._ventana.idAsociadoIntFiltrar) And Me._ventana.idAsociadoIntFiltrar <> "0" Then
                asociadoaux.Id = Integer.Parse(Me._ventana.idAsociadoIntFiltrar)
                i = True
            End If

            If Not String.IsNullOrEmpty(Me._ventana.NombreAsociadoIntFiltrar) Then
                asociadoaux.Nombre = UCase(Me._ventana.NombreAsociadoIntFiltrar)
                i = True
            End If
            If i = True Then
                asociados = Me._asociadosServicios.ObtenerAsociadosFiltro(asociadoaux)
            Else
                Me._ventana.Asociados = Nothing
                Mouse.OverrideCursor = Nothing
                MessageBox.Show("Error: No Existe Asociado Relacionado a la Búsqueda")
                Exit Sub
            End If

            Dim primerasociado As New Asociado()
            primerasociado.Id = Integer.MinValue
            asociados.Insert(0, primerasociado)

            Me._ventana.AsociadosInternacionales = asociados

            Mouse.OverrideCursor = Nothing

        End Sub

        '''<sumary>
        ''' Metodo para cambiar un Asociado Internacional
        '''</sumary>
        Public Sub CambiarAsociadoInternacional()
            Try
                If DirectCast(Me._ventana.AsociadoInternacional, Asociado) IsNot Nothing Then
                    If Me._ventana.AsociadoInternacional.id <> Integer.MinValue Then
                        Me._ventana.NombreAsociadoInt = DirectCast(Me._ventana.AsociadoInternacional, Asociado).Id & " - " & DirectCast(Me._ventana.AsociadoInternacional, Asociado).Nombre
                    Else
                        Me._ventana.NombreAsociadoInt = Nothing
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If
            Catch e As ApplicationException

            End Try
        End Sub

        '''<summary>
        '''Metodo que recoge los valores para buscar una factura internacional
        '''</summary>
        Private Function CargarDatosFacturaInternacional(facturaInternacional As FacInternacional) As FacInternacional

            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region


            If Me._ventana.AsociadoInternacional IsNot Nothing Then
                facturaInternacional.Asociado_o = DirectCast(Me._ventana.AsociadoInternacional, Asociado)
            Else
                facturaInternacional.Asociado_o = Nothing
            End If

            If Not String.IsNullOrEmpty(Me._ventana.NumeroFactInternacional) Then
                facturaInternacional.Numerofactura = Me._ventana.NumeroFactInternacional
            Else
                facturaInternacional.Numerofactura = Nothing
            End If

            If Me._ventana.PaisesAsocInt IsNot Nothing Then
                facturaInternacional.Pais = DirectCast(Me._ventana.PaisAsocInt, Pais)
            Else
                facturaInternacional.Pais = Nothing
            End If

            If Not String.IsNullOrEmpty(Me._ventana.DetalleFacAsocInt) Then
                facturaInternacional.Detalle = Me._ventana.DetalleFacAsocInt
            Else
                facturaInternacional.Detalle = Nothing
            End If

            Return facturaInternacional

            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region

        End Function

        Private Sub CargarPaises()

            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region

            Dim paises As IList(Of Pais) = Me._PaisServicios.ConsultarTodos()
            Me._ventana.PaisesAsocInt = paises

            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region

        End Sub

    End Class
End Namespace