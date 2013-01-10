﻿Imports System
Imports System.Configuration
Imports System.Net.Sockets
Imports System.Runtime.Remoting
Imports System.Windows.Input
Imports NLog
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.FacFactuDetaAnuladas
'Imports Diginsoft.Bolet.Cliente.Fac.Ventanas.Principales
Imports Diginsoft.Bolet.ObjetosComunes.ContratosServicios
Imports Diginsoft.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.ObjetosComunes.ContratosServicios
Imports Trascend.Bolet.Cliente.Presentadores
Imports Trascend.Bolet.Cliente.Ventanas.Principales


Namespace Presentadores.FacFactuDetaAnuladas
    Class PresentadorAgregarFacFactuDetaAnulada
        Inherits PresentadorBase
        Private _ventana As IAgregarFacFactuDetaAnulada
        Private _FacFactuDetaAnuladaServicios As IFacFactuDetaAnuladaServicios
        Private Shared _paginaPrincipal As PaginaPrincipal = PaginaPrincipal.ObtenerInstancia
        Private Shared logger As Logger = LogManager.GetCurrentClassLogger()
        Private _asociadosServicios As IAsociadoServicios
        'Private _contactoServicios As IContactoServicios        
        Private _facoperacionesServicios As IFacOperacionServicios
        Private _idiomasServicios As IIdiomaServicios
        Private _monedasServicios As IMonedaServicios
        Private _asociados As IList(Of Asociado)
        Private _FacOperaciones As IList(Of FacOperacion)
        Private _tasasServicios As ITasaServicios
        Private _FacCreditoServicios As IFacCreditoServicios
        Private _contadorfacServicios As IContadorFacServicios
        Private _bancosServicios As IFacBancoServicios
        Private _FacFormaServicios As IFacFormaServicios
        'Private _FacFactuDetaAnuladaFacturaServicios As IFacFactuDetaAnuladaFacturaServicios

        Dim xoperacion As String
        ''' <summary>
        ''' Constructor predeterminado
        ''' </summary>
        ''' <param name="ventana">Página que satisface el contrato</param>
        Public Sub New(ByVal ventana As IAgregarFacFactuDetaAnulada)
            Try
                Me._ventana = ventana
                'Me._ventana.FacFactuDetaAnulada = New FacFactuDetaAnulada()
                Me._FacFactuDetaAnuladaServicios = DirectCast(Activator.GetObject(GetType(IFacFactuDetaAnuladaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacFactuDetaAnuladaServicios")), IFacFactuDetaAnuladaServicios)
                Me._asociadosServicios = DirectCast(Activator.GetObject(GetType(IAsociadoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("AsociadoServicios")), IAsociadoServicios)
                Me._facoperacionesServicios = DirectCast(Activator.GetObject(GetType(IFacOperacionServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacOperacionServicios")), IFacOperacionServicios)
                Me._idiomasServicios = DirectCast(Activator.GetObject(GetType(IIdiomaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("IdiomaServicios")), IIdiomaServicios)
                Me._monedasServicios = DirectCast(Activator.GetObject(GetType(IMonedaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("MonedaServicios")), IMonedaServicios)
                Me._tasasServicios = DirectCast(Activator.GetObject(GetType(ITasaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("TasaServicios")), ITasaServicios)
                Me._FacCreditoServicios = DirectCast(Activator.GetObject(GetType(IFacCreditoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacCreditoServicios")), IFacCreditoServicios)
                Me._contadorfacServicios = DirectCast(Activator.GetObject(GetType(IContadorFacServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("ContadorFacServicios")), IContadorFacServicios)
                Me._bancosServicios = DirectCast(Activator.GetObject(GetType(IFacBancoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacBancoServicios")), IFacBancoServicios)
                Me._FacFormaServicios = DirectCast(Activator.GetObject(GetType(IFacFormaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacFormaServicios")), IFacFormaServicios)
                'Me._FacFactuDetaAnuladaFacturaServicios = DirectCast(Activator.GetObject(GetType(IFacFactuDetaAnuladaFacturaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacFactuDetaAnuladaFacturaServicios")), IFacFactuDetaAnuladaFacturaServicios)

                Dim FacFactuDetaAnulada As New FacFactuDetaAnulada()
                Me._ventana.FacFactuDetaAnulada = FacFactuDetaAnulada

            Catch ex As Exception
                logger.[Error](ex.Message)
                Me.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, True)
            End Try
        End Sub

        ''' <summary>
        ''' Método que carga los datos iniciales a mostrar en la página
        ''' </summary>
        Public Sub CargarPagina()
            Mouse.OverrideCursor = Cursors.Wait

            Try
                Me.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.fac_titleAgregarFacFactuDetaAnulada, Recursos.Ids.AgregarUsuario)
                Me._ventana.MensajeErrorCobro = ""

                Dim FacFactuDetaAnulada As New FacFactuDetaAnulada()
                'FacFactuDetaAnulada.FechaCobro = Date.Now
                Me._ventana.FacFactuDetaAnulada = FacFactuDetaAnulada

                Me._asociados = Me._asociadosServicios.ConsultarTodos()
                Me._ventana.Asociados = Me._asociados

                Dim bancos As IList(Of FacBanco) = Me._bancosServicios.ConsultarTodos()
                Dim primerabanco As New FacBanco()
                primerabanco.Id = Integer.MinValue
                bancos.Insert(0, primerabanco)
                Me._ventana.Bancos = bancos


                Me._ventana.FocoPredeterminado()
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

        ''' <summary>
        ''' Método que realiza toda la lógica para agregar al Usuario dentro de la base de datos
        ''' </summary>
        Public Sub Aceptar()
            Try
                Dim FacFactuDetaAnulada As FacFactuDetaAnulada = DirectCast(_ventana.FacFactuDetaAnulada, FacFactuDetaAnulada)

                'FacFactuDetaAnulada.Asociado = If(Not DirectCast(Me._ventana.Asociado, Asociado).Id.Equals("NGN"), DirectCast(Me._ventana.Asociado, Asociado), Nothing)

                ''If Not Me._FacCreditoServicios.VerificarExistencia(FacCredito) Then                
                'FacFactuDetaAnulada.Moneda = If(Not DirectCast(Me._ventana.Moneda, Moneda).Id.Equals("NGN"), DirectCast(Me._ventana.Moneda, Moneda), Nothing)
                'FacFactuDetaAnulada.Idioma = If(Not DirectCast(Me._ventana.Idioma, Idioma).Id.Equals("NGN"), DirectCast(Me._ventana.Idioma, Idioma), Nothing)
                'FacFactuDetaAnulada.Banco = If(Not DirectCast(Me._ventana.Banco, FacBanco).Id.Equals("NGN"), DirectCast(Me._ventana.Banco, FacBanco), Nothing)

                'Dim valido As Boolean = True
                'If (FacFactuDetaAnulada.Moneda.Id = "US") Then
                '    If (Me._ventana.SumaBono <> Me._ventana.SumaBforma) Then
                '        Me._ventana.MensajeErrorCobro = "Error en los montos"
                '        valido = False
                '    End If
                'Else
                '    If (Me._ventana.SumaBonoBf <> Me._ventana.SumaBformaBf) Then
                '        Me._ventana.MensajeErrorCobro = "Error en los montos"
                '        valido = False
                '    End If
                'End If

                'If (valido = True) Then

                'para el contador
                Dim contador As New ContadorFac
                contador.Id = "FAC_COBROS"
                contador = _contadorfacServicios.ConsultarPorId(contador)
                FacFactuDetaAnulada.Id = contador.ProximoValor
                contador.ProximoValor = System.Math.Max(System.Threading.Interlocked.Increment(contador.ProximoValor), contador.ProximoValor - 1)
                Dim exitocontador As Boolean = _contadorfacServicios.InsertarOModificar(contador, UsuarioLogeado.Hash)
                'fin contador                   

                Dim FacFormas As List(Of FacForma) = DirectCast(Me._ventana.ResultadosForma, List(Of FacForma))
                ModificarFacOperacionesForma(FacFormas)

                'Dim FacFactuDetaAnuladaFactura As List(Of FacFactuDetaAnuladaFactura) = DirectCast(Me._ventana.ResultadosFacturaCobro, List(Of FacFactuDetaAnuladaFactura))
                'ModificarFacOperacionesCobroxFactura(FacFactuDetaAnuladaFactura)
                Dim exitoso As Boolean = _FacFactuDetaAnuladaServicios.InsertarOModificar(FacFactuDetaAnulada, UsuarioLogeado.Hash)

                If exitoso Then
                    'AgregarFacFormaCobro(FacFormas, FacFactuDetaAnulada)
                    'AgregarFacFactuDetaAnuladaFacturaCobro(FacFactuDetaAnuladaFactura, FacFactuDetaAnulada)

                    'Dim operacion As New FacOperacion
                    'operacion.Id = "NP"
                    'operacion.CodigoOperacion = FacFactuDetaAnulada.Id
                    'operacion.FechaOperacion = FacFactuDetaAnulada.FechaCobro
                    'operacion.Asociado = FacFactuDetaAnulada.Asociado
                    'operacion.Idioma = FacFactuDetaAnulada.Idioma
                    'operacion.Moneda = FacFactuDetaAnulada.Moneda
                    'operacion.Monto = Me._ventana.SumaBono
                    'operacion.Saldo = Me._ventana.SumaBono
                    'operacion.MontoBf = Me._ventana.SumaBonoBf
                    'operacion.SaldoBf = Me._ventana.SumaBonoBf
                    'operacion.XOperacion = xoperacion
                    '_facoperacionesServicios.InsertarOModificar(operacion, UsuarioLogeado.Hash)

                    Me.Navegar(Recursos.MensajesConElUsuario.fac_FacFactuDetaAnuladaInsertado, False)
                End If
                'Else
                'Me._ventana.Mensaje(Recursos.MensajesConElUsuario.fac_ErrorFacFactuDetaAnuladaRepetido)
                ' End If
                'End If
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
            End Try
        End Sub

        Public Sub BuscarAsociado2()

            Dim asociadosFiltrados As IEnumerable(Of Asociado) = Me._asociados

            If Not String.IsNullOrEmpty(Me._ventana.idAsociadoFiltrar) Then
                asociadosFiltrados = From p In asociadosFiltrados Where p.Id = Integer.Parse(Me._ventana.idAsociadoFiltrar)
            End If

            If Not String.IsNullOrEmpty(Me._ventana.NombreAsociadoFiltrar) Then
                asociadosFiltrados = From p In asociadosFiltrados Where p.Nombre IsNot Nothing AndAlso p.Nombre.ToLower().Contains(Me._ventana.NombreAsociadoFiltrar.ToLower())
            End If

            If asociadosFiltrados.ToList.Count <> 0 Then
                Me._ventana.Asociados = asociadosFiltrados
            Else
                Me._ventana.Asociados = Me._asociados
            End If
        End Sub

        Public Sub VerFacturas()
            Dim FacOperacionAuxiliar As New FacOperacion()
            If (Me._ventana.Asociado IsNot Nothing) AndAlso (DirectCast(Me._ventana.Asociado, Asociado).Id <> Integer.MinValue) Then
                FacOperacionAuxiliar.Asociado = DirectCast(Me._ventana.Asociado, Asociado)
            End If
            FacOperacionAuxiliar.Id = "ND"
            FacOperacionAuxiliar.Saldo = -1
            'Me._FacCreditos = Me._FacCreditoServicios.ConsultarTodos()
            Me._FacOperaciones = Me._facoperacionesServicios.ObtenerFacOperacionesFiltro(FacOperacionAuxiliar)
            Me._ventana.ResultadosFactura2 = Me._FacOperaciones
            Me._ventana.MensajeErrorCobro = ""
        End Sub

        Public Sub MostrarForma()
            Dim FacForma As FacForma = DirectCast(Me._ventana.FacFormaSeleccionada, FacForma)
            Me._ventana.Xforma = FacForma.XForma
            Me._ventana.Credito = FacForma.Credito.Id
            Me._ventana.Bforma = FacForma.BForma
            Me._ventana.BformaBf = FacForma.BFormaBf
            Me._ventana.MensajeErrorCobro = ""
        End Sub

        Public Sub ModificarForma()
            Dim FacFormas As List(Of FacForma) = DirectCast(Me._ventana.ResultadosForma, List(Of FacForma))
            Dim id As Integer = Me._ventana.Credito
            Dim sumabforma As Double = 0
            Dim sumabformabf As Double = 0
            For i As Integer = 0 To FacFormas.Count - 1
                If (FacFormas.Item(i).Credito.Id = id) Then
                    FacFormas.Item(i).XForma = Me._ventana.Xforma
                    FacFormas.Item(i).BForma = Me._ventana.Bforma
                    FacFormas.Item(i).BFormaBf = Me._ventana.BformaBf
                End If
                sumabforma = sumabforma + FacFormas.Item(i).BForma
                sumabformabf = sumabformabf + FacFormas.Item(i).BFormaBf
            Next
            Me._ventana.SumaBforma = sumabforma
            Me._ventana.SumaBformaBf = sumabformabf
            Me._ventana.ResultadosForma = Nothing
            Me._ventana.ResultadosForma = FacFormas
            Me._ventana.MensajeErrorCobro = ""
        End Sub

        Public Sub ModificarFacOperacionesForma(ByVal FacFormas As List(Of FacForma))

            For i As Integer = 0 To FacFormas.Count - 1
                Dim FacOperacionAuxiliar As New FacOperacion()
                FacOperacionAuxiliar.Id = "NC"
                FacOperacionAuxiliar.CodigoOperacion = FacFormas.Item(i).Credito.Id
                Dim FacOperacion As New FacOperacion
                FacOperacion = Me._facoperacionesServicios.ObtenerFacOperacionesFiltro(FacOperacionAuxiliar).Item(0)
                FacOperacion.Saldo = 0
                FacOperacion.SaldoBf = 0
                Dim exitoso As Boolean = Me._facoperacionesServicios.InsertarOModificar(FacOperacion, UsuarioLogeado.Hash)
            Next
        End Sub

        Public Sub AgregarFacFormaCobro(ByVal FacFormas As List(Of FacForma), ByVal Cobro As FacFactuDetaAnulada)
            'For i As Integer = 0 To FacFormas.Count - 1
            '    FacFormas.Item(i).Cobro = Cobro
            '    Dim exitoso As Boolean = Me._FacFormaServicios.InsertarOModificar(FacFormas.Item(i), UsuarioLogeado.Hash)
            'Next
        End Sub

        'Public Sub ModificarFacOperacionesCobroxFactura(ByVal CobroxFacturas As List(Of FacFactuDetaAnuladaFactura))

        '    For i As Integer = 0 To CobroxFacturas.Count - 1
        '        Dim FacOperacionAuxiliar As New FacOperacion()
        '        FacOperacionAuxiliar.Id = "ND"
        '        FacOperacionAuxiliar.CodigoOperacion = CobroxFacturas.Item(i).Factura
        '        Dim FacOperacion As New FacOperacion
        '        FacOperacion = Me._facoperacionesServicios.ObtenerFacOperacionesFiltro(FacOperacionAuxiliar).Item(0)
        '        FacOperacion.Saldo = 0
        '        FacOperacion.SaldoBf = 0
        '        Dim exitoso As Boolean = Me._facoperacionesServicios.InsertarOModificar(FacOperacion, UsuarioLogeado.Hash)
        '    Next
        'End Sub

        'Public Sub AgregarFacFactuDetaAnuladaFacturaCobro(ByVal FacFactuDetaAnuladaFacturas As List(Of FacFactuDetaAnuladaFactura), ByVal Cobro As FacFactuDetaAnulada)
        '    For i As Integer = 0 To FacFactuDetaAnuladaFacturas.Count - 1
        '        FacFactuDetaAnuladaFacturas.Item(i).Id = Cobro
        '        Dim exitoso As Boolean = Me._FacFactuDetaAnuladaFacturaServicios.InsertarOModificar(FacFactuDetaAnuladaFacturas.Item(i), UsuarioLogeado.Hash)
        '    Next
        'End Sub

        Public Sub VerCreditos()
            Dim FacOperacionAuxiliar As New FacOperacion()
            If (Me._ventana.Asociado IsNot Nothing) AndAlso (DirectCast(Me._ventana.Asociado, Asociado).Id <> Integer.MinValue) Then
                FacOperacionAuxiliar.Asociado = DirectCast(Me._ventana.Asociado, Asociado)
            End If
            FacOperacionAuxiliar.Id = "NC"
            FacOperacionAuxiliar.Saldo = -1
            'Me._FacCreditos = Me._FacCreditoServicios.ConsultarTodos()
            Me._FacOperaciones = Me._facoperacionesServicios.ObtenerFacOperacionesFiltro(FacOperacionAuxiliar)
            Me._ventana.ResultadosFactura = Me._FacOperaciones
            Me._ventana.MensajeErrorCobro = ""
        End Sub

        'Public Sub AgregarFacturas2()
        '    Dim FacOperacion As List(Of FacOperacion) = DirectCast(_ventana.ResultadosFactura2, List(Of FacOperacion))
        '   ' Dim FacFactuDetaAnuladaFactura As New List(Of FacFactuDetaAnuladaFactura)
        '    Dim Moneda As Moneda = If(Not DirectCast(Me._ventana.Moneda, Moneda).Id.Equals("NGN"), DirectCast(Me._ventana.Moneda, Moneda), Nothing)
        '    Dim sumabono As Double = 0
        '    Dim sumabonobf As Double = 0
        '    Dim j As Integer = 0
        '    xoperacion = ""
        '    For i As Integer = 0 To FacOperacion.Count - 1
        '        If (FacOperacion.Item(i).Seleccion = True) Then
        '            FacFactuDetaAnuladaFactura.Add(New FacFactuDetaAnuladaFactura)
        '            Dim cobro As New FacFactuDetaAnulada
        '            cobro.Id = 0
        '            FacFactuDetaAnuladaFactura.Item(j).Id = cobro
        '            FacFactuDetaAnuladaFactura.Item(j).Factura = FacOperacion.Item(i).CodigoOperacion
        '            FacFactuDetaAnuladaFactura.Item(j).Bono = FacOperacion.Item(i).Saldo
        '            FacFactuDetaAnuladaFactura.Item(j).BonoBf = FacOperacion.Item(i).SaldoBf
        '            If (Moneda.Id = "US") Then
        '                sumabono = sumabono + FacFactuDetaAnuladaFactura.Item(j).Bono
        '            Else
        '                If (Moneda.Id = "BS") Then
        '                    sumabono = sumabono + FacFactuDetaAnuladaFactura.Item(j).Bono / 2.15
        '                Else
        '                    If (Moneda.Id = "BF") Then
        '                        sumabono = sumabono + FacFactuDetaAnuladaFactura.Item(j).BonoBf
        '                    End If
        '                End If

        '            End If
        '            sumabonobf = sumabonobf + FacFactuDetaAnuladaFactura.Item(j).BonoBf

        '            If (xoperacion = "") Then
        '                xoperacion = FacFactuDetaAnuladaFactura.Item(j).Factura
        '            Else
        '                xoperacion = xoperacion & "," & FacFactuDetaAnuladaFactura.Item(j).Factura
        '            End If

        '            j = j + 1
        '        End If
        '    Next
        '    Me._ventana.ResultadosFacturaCobro = FacFactuDetaAnuladaFactura
        '    Me._ventana.SumaBono = sumabono
        '    Me._ventana.SumaBonoBf = sumabonobf
        'End Sub

        Public Sub AgregarForma()
            Dim FacOperacion As List(Of FacOperacion) = DirectCast(_ventana.ResultadosFactura, List(Of FacOperacion))
            Dim FacForma As New List(Of FacForma)
            Dim sumabforma As Double = 0
            Dim sumabformabf As Double = 0
            Dim j As Integer = 0
            For i As Integer = 0 To FacOperacion.Count - 1
                If (FacOperacion.Item(i).Seleccion = True) Then
                    FacForma.Add(New FacForma)
                    Dim cobro As New FacFactuDetaAnulada
                    cobro.Id = 0
                    'FacForma.Item(j).Cobro = cobro
                    FacForma.Item(j).Id = j

                    Dim FacCreditoAuxiliar As New FacCredito()
                    FacCreditoAuxiliar.Id = FacOperacion.Item(i).CodigoOperacion
                    'Me._FacCreditos = Me._FacCreditoServicios.ConsultarTodos()                    
                    FacForma.Item(j).Credito = Me._FacCreditoServicios.ObtenerFacCreditosFiltro(FacCreditoAuxiliar).Item(0)
                    If (FacOperacion.Item(i).Moneda.Id = "US") Then
                        FacForma.Item(j).BForma = FacOperacion.Item(i).Saldo

                        Dim moneda As Moneda
                        moneda = If(Not DirectCast(Me._ventana.Moneda, Moneda).Id.Equals("NGN"), DirectCast(Me._ventana.Moneda, Moneda), Nothing)
                        Dim FacFactuDetaAnulada As FacFactuDetaAnulada = DirectCast(_ventana.FacFactuDetaAnulada, FacFactuDetaAnulada)
                        'Dim tasa As New Tasa()
                        'tasa.Id = FacFactuDetaAnulada.FechaCobro
                        'tasa.Moneda = moneda.Id

                        'Dim tasas As IList(Of Tasa) = _tasasServicios.ObtenerTasasFiltro(tasa)
                        'If tasas.Count > 0 Then
                        '    FacForma.Item(j).Tasa = tasa.Tasabf
                        '    FacForma.Item(j).BFormaBf = FacForma.Item(j).BForma * tasa.Tasabf
                        'Else
                        '    FacForma.Item(j).Tasa = 1
                        '    FacForma.Item(j).BFormaBf = FacForma.Item(j).BForma
                        'End If
                    Else
                        If (FacOperacion.Item(i).Moneda.Id = "BF") Then
                            FacForma.Item(j).BForma = FacOperacion.Item(i).SaldoBf
                            FacForma.Item(j).BFormaBf = FacOperacion.Item(i).SaldoBf
                        Else
                            If (FacOperacion.Item(i).Moneda.Id = "BS") Then
                                FacForma.Item(j).BForma = FacOperacion.Item(i).SaldoBf
                                FacForma.Item(j).BFormaBf = FacOperacion.Item(i).SaldoBf
                            End If
                        End If
                    End If
                    sumabforma = sumabforma + FacForma.Item(j).BForma
                    sumabformabf = sumabformabf + FacForma.Item(j).BFormaBf
                    j = j + 1
                End If
            Next
            Me._ventana.ResultadosForma = FacForma
            Me._ventana.SumaBforma = sumabforma
            Me._ventana.SumaBformaBf = sumabformabf
        End Sub

        Public Sub CambiarAsociado()
            Try
                Dim asociado As Asociado = Me._asociadosServicios.ConsultarAsociadoConTodo(DirectCast(Me._ventana.Asociado, Asociado))
                Me._ventana.NombreAsociado = DirectCast(Me._ventana.Asociado, Asociado).Nombre

                Dim idiomas As IList(Of Idioma) = Me._idiomasServicios.ConsultarTodos()
                Me._ventana.Idiomas = idiomas
                Me._ventana.Idioma = Me.BuscarIdioma(idiomas, asociado.Idioma)

                Dim Monedas As IList(Of Moneda) = Me._monedasServicios.ConsultarTodos()
                Me._ventana.Monedas = Monedas
                Me._ventana.Moneda = Me.BuscarMoneda(Monedas, asociado.Moneda)

            Catch e As ApplicationException
                'Me._ventana.Personas = Nothing
            End Try
        End Sub
    End Class
End Namespace
