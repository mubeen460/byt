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
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.Consultas
Imports Diginsoft.Bolet.Cliente.Fac.Ventanas.FacReportes
Imports Diginsoft.Bolet.Cliente.Fac.Ventanas.Consultas
'Imports Diginsoft.Bolet.Cliente.Fac.Ventanas.FacReportes
'Imports Diginsoft.Bolet.Cliente.Fac.Ventanas.Principales
Imports Diginsoft.Bolet.ObjetosComunes.ContratosServicios
Imports Diginsoft.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.ObjetosComunes.ContratosServicios
Imports Trascend.Bolet.Cliente.Presentadores
Imports Trascend.Bolet.Cliente.Ventanas.Principales

Namespace Presentadores.Consultas
    Class PresentadorConsultaFacturasAnuladas
        Inherits PresentadorBase
        Private Shared _paginaPrincipal As PaginaPrincipal = PaginaPrincipal.ObtenerInstancia
        Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

        Private _ventana As IConsultaFacturasAnuladas
        Private _FacFacturaAnuladaServicios As IFacFacturaAnuladaServicios
        Private _FacFacturaServicios As IFacFacturaServicios
        'Private _FacFacturaAnuladas As IList(Of FacFacturaAnulada)
        'Private _asociados As IAsociadoServicios
        Private _asociadosServicios As IAsociadoServicios
        Private _asociados As IList(Of Asociado)
        Private _facbancos As IFacBancoServicios
        Private _facbancosServicios As IFacBancoServicios
        Private _idiomasServicios As IIdiomaServicios
        Private _monedasServicios As IMonedaServicios

        ''' <summary>
        ''' Constructor Predeterminado
        ''' </summary>
        ''' <param name="ventana">página que satisface el contrato</param>
        Public Sub New(ByVal ventana As IConsultaFacturasAnuladas)
            Try
                Me._ventana = ventana
                Me._FacFacturaAnuladaServicios = DirectCast(Activator.GetObject(GetType(IFacFacturaAnuladaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacFacturaAnuladaServicios")), IFacFacturaAnuladaServicios)
                Me._FacFacturaServicios = DirectCast(Activator.GetObject(GetType(IFacFacturaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacFacturaServicios")), IFacFacturaServicios)
                Me._asociadosServicios = DirectCast(Activator.GetObject(GetType(IAsociadoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("AsociadoServicios")), IAsociadoServicios)
                Me._facbancosServicios = DirectCast(Activator.GetObject(GetType(IFacBancoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacBancoServicios")), IFacBancoServicios)
                Me._idiomasServicios = DirectCast(Activator.GetObject(GetType(IIdiomaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("IdiomaServicios")), IIdiomaServicios)
                Me._monedasServicios = DirectCast(Activator.GetObject(GetType(IMonedaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("MonedaServicios")), IMonedaServicios)
            Catch ex As Exception
                logger.[Error](ex.Message)
                Me.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, True)
            End Try
        End Sub

        Public Sub ActualizarTitulo()
            Me.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.fac_menuItemConsultaFacturasAnuladas, Recursos.Ids.fac_menuItemConsultaFacturasAnuladas)
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
                Me._ventana.FacFacturaAnuladaFiltrar = New FacFacturaAnulada
                'sumar(FacFacturaAnuladas)


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

        Public Sub Limpiar()
            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region

            Me.Navegar(New ConsultaFacturasAnuladas())
            'Me.Navegar(New ConsultarFacCobro())
            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region
        End Sub

        ''' <summary>
        ''' Método que realiza una consulta al servicio, con el fin de filtrar los datos que se muestran 
        ''' por pantalla
        ''' </summary>
        Public Sub Consultar()
            Try
                '#Region "trace"
                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                End If
                '#End Region


                Dim FacFacturaAnuladaAuxiliar As New FacFacturaAnulada()
                'Dim FacFacturaAnuladas As FacFacturaAnulada = DirectCast(_ventana.FacFacturaAnuladaFiltrar, FacFacturaAnulada)

                If Not Me._ventana.Id.Equals("") Then
                    FacFacturaAnuladaAuxiliar.Id = Integer.Parse(Me._ventana.Id)
                End If

                'If Not Me._ventana.CreditoSent.Equals("") Then
                '    FacFacturaAnuladaAuxiliar.CreditoSent = Integer.Parse(Me._ventana.CreditoSent)
                'End If

                If (Me._ventana.Asociado IsNot Nothing) AndAlso (DirectCast(Me._ventana.Asociado, Asociado).Id <> Integer.MinValue) Then
                    FacFacturaAnuladaAuxiliar.Asociado = DirectCast(Me._ventana.Asociado, Asociado)
                End If

                'If (Me._ventana.Banco IsNot Nothing) AndAlso (DirectCast(Me._ventana.Banco, FacBanco).Id <> Integer.MinValue) Then
                '    'FacFacturaAnuladaAuxiliar.Banco = DirectCast(Me._ventana.Banco, FacBanco)
                'End If

                'If (Me._ventana.Idioma IsNot Nothing) AndAlso (DirectCast(Me._ventana.Idioma, Idioma).Id <> "") Then
                '    FacFacturaAnuladaAuxiliar.Idioma = DirectCast(Me._ventana.Idioma, Idioma)
                'End If

                'If (Me._ventana.Moneda IsNot Nothing) AndAlso (DirectCast(Me._ventana.Moneda, Moneda).Id <> "") Then
                '    FacFacturaAnuladaAuxiliar.Moneda = DirectCast(Me._ventana.Moneda, Moneda)
                'End If

                If Not Me._ventana.FechaFacturaAnulada.Equals("") Then
                    Dim FechaFacFacturaAnulada As DateTime = DateTime.Parse(Me._ventana.FechaFacturaAnulada)
                    FacFacturaAnuladaAuxiliar.FechaFactura = FechaFacFacturaAnulada
                End If

                'If (filtroValido = True) Then
                'FacFacturaAnuladaAuxiliar.Inicial = UsuarioLogeado.Iniciales
                'FacFacturaAnuladaAuxiliar.Anulada = "SI"
                Dim FacFacturaAnuladas As IList(Of FacFacturaAnulada)
                FacFacturaAnuladas = Me._FacFacturaAnuladaServicios.ObtenerFacFacturaAnuladasFiltro(FacFacturaAnuladaAuxiliar)
                Me._ventana.Resultados = Nothing                
                Me._ventana.Count = FacFacturaAnuladas.Count
                If FacFacturaAnuladas.Count <= 0 Then
                    MessageBox.Show("Mensaje: No se encontraron registros")
                End If
                Me._ventana.Resultados = FacFacturaAnuladas

                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                    '#End Region
                End If
            Catch ex As Exception
                logger.[Error](ex.Message)
                Me.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, True)
            End Try
        End Sub

        ''' <summary>
        ''' Método que invoca una nueva página "ConsultarFacFacturaAnulada" y la instancia con el objeto seleccionado
        ''' </summary>
        Public Sub IrConsultarFacFacturaAnulada()
            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region
            'Me._ventana.FacFacturaAnuladaSeleccionado.Accion = 2 'no modificar
            Dim factura As FacFactura = consultar_factura(DirectCast(Me._ventana.FacFacturaAnuladaSeleccionado, FacFacturaAnulada).Id)
            If factura IsNot Nothing Then
                Me.Navegar(New FacturaAnuladaRpt(factura, Nothing))
            End If
            'Me.Navegar(New ConsultarFacFacturaAnulada())
            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region
        End Sub

        Public Function consultar_factura(ByVal id As Integer) As FacFactura
            Dim FacFacturaAuxiliar As New FacFactura()
            Dim FacFacturas As List(Of FacFactura)
            FacFacturaAuxiliar.Id = id
            FacFacturas = Me._FacFacturaServicios.ObtenerFacFacturasFiltro(FacFacturaAuxiliar)
            If FacFacturas IsNot Nothing Then
                If FacFacturas.Count > 0 Then
                    Return (FacFacturas(0))
                Else
                    Return (Nothing)
                End If
            Else
                Return (Nothing)
            End If
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
    End Class
End Namespace