﻿Imports System
Imports System.Configuration
Imports System.Net.Sockets
Imports System.Runtime.Remoting
Imports System.Windows.Input
Imports NLog
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.FacPagoBolivias
Imports Diginsoft.Bolet.Cliente.Fac.Ventanas.FacPagoBolivias
Imports Diginsoft.Bolet.Cliente.Fac.Ventanas.FacGestiones
Imports Diginsoft.Bolet.ObjetosComunes.ContratosServicios
Imports Diginsoft.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.ObjetosComunes.ContratosServicios
Imports Trascend.Bolet.Cliente.Presentadores
Imports Trascend.Bolet.Cliente.Ventanas.Principales

Namespace Presentadores.FacPagoBolivias
    Class PresentadorAgregarFacPagoBolivia
        Inherits PresentadorBase
        Private _ventana As IAgregarFacPagoBolivia
        Private _FacPagoBoliviaServicios As IFacPagoBoliviaServicios
        Private Shared _paginaPrincipal As PaginaPrincipal = PaginaPrincipal.ObtenerInstancia
        Private Shared logger As Logger = LogManager.GetCurrentClassLogger()
        Private _asociadosServicios As IAsociadoServicios
        Private _contactoServicios As IContactoServicios
        Private _facbancosServicios As IFacBancoServicios
        Private _asociados As IList(Of Asociado)
        Private _cartasServicios As ICartaServicios
        ''' <summary>
        ''' Constructor predeterminado
        ''' </summary>
        ''' <param name="ventana">Página que satisface el contrato</param>
        Public Sub New(ByVal ventana As IAgregarFacPagoBolivia)
            Try
                Me._ventana = ventana
                'Me._ventana.FacPagoBolivia = New FacPagoBolivia()
                Me._FacPagoBoliviaServicios = DirectCast(Activator.GetObject(GetType(IFacPagoBoliviaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacPagoBoliviaServicios")), IFacPagoBoliviaServicios)
                Me._asociadosServicios = DirectCast(Activator.GetObject(GetType(IAsociadoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("AsociadoServicios")), IAsociadoServicios)
                Me._facbancosServicios = DirectCast(Activator.GetObject(GetType(IFacBancoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacBancoServicios")), IFacBancoServicios)
                Me._cartasServicios = DirectCast(Activator.GetObject(GetType(ICartaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("CartaServicios")), ICartaServicios)

                Dim FacPagoBolivia As New FacPagoBolivia()
                FacPagoBolivia.FechaBanco = Date.Now
                Me._ventana.FacPagoBolivia = FacPagoBolivia

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

            Me.Navegar(New AgregarFacPagoBolivia())
            'Me.Navegar(New ConsultarFacCobro())
            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region
        End Sub

        ''' <summary>
        ''' Método que carga los datos iniciales a mostrar en la página
        ''' </summary>
        Public Sub CargarPagina()
            Mouse.OverrideCursor = Cursors.Wait

            Try
                Me.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.fac_titleAgregarFacPagoBolivia, Recursos.Ids.AgregarUsuario)

                'Dim asociados As IList(Of Asociado) = Me._asociadosServicios.ConsultarTodos()
                'Dim primeraasociado As New Asociado()
                'primeraasociado.Id = Integer.MinValue
                'asociados.Insert(0, primeraasociado)
                'Me._asociados = Me._asociadosServicios.ConsultarTodos()
                'Me._ventana.Asociados = Me._asociados

                Dim facbancos As IList(Of FacBanco) = Me._facbancosServicios.ConsultarTodos()
                Dim primerfacbanco As New FacBanco()
                primerfacbanco.Id = Integer.MinValue
                facbancos.Insert(0, primerfacbanco)
                Me._ventana.BancosRec = facbancos


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
                Dim FacPagoBolivia As FacPagoBolivia = DirectCast(_ventana.FacPagoBolivia, FacPagoBolivia)

                FacPagoBolivia.Id = If(Not DirectCast(Me._ventana.Asociado, Asociado).Id.Equals("NGN"), DirectCast(Me._ventana.Asociado, Asociado), Nothing)

                'If Not Me._FacPagoBoliviaServicios.VerificarExistencia(FacPagoBolivia) Then
                FacPagoBolivia.BancoRec = If(Not DirectCast(Me._ventana.BancoRec, FacBanco).Id.Equals("NGN"), DirectCast(Me._ventana.BancoRec, FacBanco), Nothing)
                FacPagoBolivia.PagoRec = _ventana.TipoPago
                Dim primerbancog As New BancoG()
                primerbancog.Id = -2
                FacPagoBolivia.BancoPag = Nothing
                'FacPagoBolivia.FechaDeposito = Date.Now
                FacPagoBolivia.IPagado = "0"
                FacPagoBolivia.PagoPag = ""
                FacPagoBolivia.FechaReg = Date.Now
                FacPagoBolivia.FechaPago = CDate("01-01-1900")
                FacPagoBolivia.NumeroPag = ""

                If DirectCast(Me._ventana.Carta, Carta) IsNot Nothing Then
                    If Me._ventana.Carta.id <> Integer.MinValue Then
                        FacPagoBolivia.Carta = DirectCast(Me._ventana.Carta, Carta)
                    Else
                        'MessageBox.Show("Debe especificar Carta Orden", "Error", MessageBoxButton.OK)
                        'Exit Sub
                    End If
                Else
                    'MessageBox.Show("Debe especificar Carta Orden", "Error", MessageBoxButton.OK)
                    'Exit Sub
                End If

                Dim exitoso As Boolean = _FacPagoBoliviaServicios.InsertarOModificar(FacPagoBolivia, UsuarioLogeado.Hash)

                If exitoso Then
                    Me.Navegar(Recursos.MensajesConElUsuario.fac_FacPagoBoliviaInsertado, False)
                End If
                'Else
                'Me._ventana.Mensaje(Recursos.MensajesConElUsuario.fac_ErrorFacPagoBoliviaRepetido)
                ' End If
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

        Public Sub Ver_Carta()

            If DirectCast(Me._ventana.Carta, Carta) IsNot Nothing Then
                If Me._ventana.Carta.id <> Integer.MinValue Then
                    Dim ag As New Mostrar_Detalle_Carta(Me._ventana.Carta)
                    'ag.Owner = Me
                    ag.ShowDialog()
                Else
                    MessageBox.Show("Debe especificar una gestion ", "Error", MessageBoxButton.OK)
                    Mouse.OverrideCursor = Nothing
                    Exit Sub
                End If
            Else
                MessageBox.Show("Debe especificar una gestion ", "Error", MessageBoxButton.OK)
                Mouse.OverrideCursor = Nothing
                Exit Sub
            End If
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
                Me._ventana.NombreAsociado = DirectCast(Me._ventana.Asociado, Asociado).Id & " - " & DirectCast(Me._ventana.Asociado, Asociado).Nombre
                'Me._ventana.Personas = asociado.Contactos
            Catch e As ApplicationException
                'Me._ventana.Personas = Nothing
            End Try
        End Sub

        Public Sub BuscarCarta()
            Mouse.OverrideCursor = Cursors.Wait
            Dim i As Boolean = False
            Dim cartas As List(Of Carta) = Nothing
            'If DirectCast(Me._ventana.Asociado, Asociado) IsNot Nothing Then
            Dim cartaaux As New Carta

            'Dim CartasFiltrados As IEnumerable(Of Carta) = Me._Cartas

            If Not String.IsNullOrEmpty(Me._ventana.idCartaFiltrar) Then
                cartaaux.Id = Integer.Parse(Me._ventana.idCartaFiltrar)
                'CartasFiltrados = From p In CartasFiltrados Where p.Id = Integer.Parse(Me._ventana.idCartaFiltrar)
                i = True
            End If

            If Not String.IsNullOrEmpty(Me._ventana.NombreCartaFiltrar) Then
                cartaaux.Medio = UCase(Me._ventana.NombreCartaFiltrar)
                'CartasFiltrados = From p In CartasFiltrados Where p.Medio IsNot Nothing AndAlso p.Medio.ToLower().Contains(Me._ventana.NombreCartaFiltrar.ToLower())
                i = True
            End If

            If Not String.IsNullOrEmpty(Me._ventana.FechaCartaFiltrar) Then
                cartaaux.Fecha = Me._ventana.FechaCartaFiltrar
            End If

            'cartaaux.Asociado = DirectCast(Me._ventana.Asociado, Asociado)
            If i = True Then
                cartas = Me._cartasServicios.ObtenerCartasFiltro(cartaaux)
                'Else
                If cartas Is Nothing Then
                    Me._ventana.Cartas = Nothing
                    MessageBox.Show("Error: No Existe Carta Relacionado a la Búsqueda")
                Else
                    If cartas.Count < 1 Then
                        Me._ventana.Cartas = Nothing
                        MessageBox.Show("Error: No Existe Carta Relacionado a la Búsqueda")
                    End If
                End If
            Else
                Me._ventana.Cartas = Nothing
                MessageBox.Show("Error: No Existe Carta Relacionado a la Búsqueda")
            End If
            'End If

            Dim primercarta As New Carta()
            primercarta.Id = Integer.MinValue
            cartas.Insert(0, primercarta)

            Me._ventana.Cartas = cartas
            Mouse.OverrideCursor = Nothing
            'If CartasFiltrados.ToList.Count <> 0 Then
            '    Me._ventana.Cartas = CartasFiltrados
            'Else
            '    Me._ventana.Cartas = Me._Cartas
            'End If
        End Sub

        Public Sub CambiarCarta()
            Try
                If DirectCast(Me._ventana.Carta, Carta) IsNot Nothing Then
                    If Me._ventana.Carta.id <> Integer.MinValue Then
                        'Dim carta As List(Of Carta) = Me._cartasServicios.ObtenerCartasFiltro(DirectCast(Me._ventana.Carta, Carta))
                        Me._ventana.NombreCarta = DirectCast(Me._ventana.Carta, Carta).Id & " - " & DirectCast(Me._ventana.Carta, Carta).Medio & " - " & FormatDateTime(DirectCast(Me._ventana.Carta, Carta).Fecha, DateFormat.ShortDate)
                    End If
                Else
                    Me._ventana.NombreCarta = Nothing
                End If
            Catch e As ApplicationException
                'Me._ventana.Personas = Nothing
            End Try
        End Sub
    End Class
End Namespace
