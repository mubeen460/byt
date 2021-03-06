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
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.FacOperacionDetaProformas
'Imports Diginsoft.Bolet.Cliente.Fac.Ventanas.FacOperacionDetaProformas
'Imports Diginsoft.Bolet.Cliente.Fac.Ventanas.Principales
Imports Diginsoft.Bolet.ObjetosComunes.ContratosServicios
Imports Diginsoft.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.ObjetosComunes.ContratosServicios
Imports Trascend.Bolet.Cliente.Presentadores
Imports Trascend.Bolet.Cliente.Ventanas.Principales

Namespace Presentadores.FacOperacionDetaProformas
    Class PresentadorConsultarDepositoFacOperacionDetaProformas
        Inherits PresentadorBase
        Private Shared _paginaPrincipal As PaginaPrincipal = PaginaPrincipal.ObtenerInstancia
        Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

        Private _ventana As IConsultarDepositoFacOperacionDetaProformas
        Private _FacOperacionDetaProformaServicios As IFacOperacionDetaProformaServicios
        Private _FacOperacionDetaProformas As IList(Of FacOperacionDetaProforma)
        Dim FacOperacionDetaProformaselect As IList(Of FacOperacionDetaProformaSelec)
        'Private _asociados As IAsociadoServicios
        Private _asociadosServicios As IAsociadoServicios
        Private _asociados As IList(Of Asociado)
        Private _facbancos As IFacBancoServicios
        Private _facbancosServicios As IFacBancoServicios

        ''' <summary>
        ''' Constructor Predeterminado
        ''' </summary>
        ''' <param name="ventana">página que satisface el contrato</param>
        Public Sub New(ByVal ventana As IConsultarDepositoFacOperacionDetaProformas)
            Try
                Me._ventana = ventana
                Me._FacOperacionDetaProformaServicios = DirectCast(Activator.GetObject(GetType(IFacOperacionDetaProformaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacOperacionDetaProformaServicios")), IFacOperacionDetaProformaServicios)
                'Me._asociadosServicios = DirectCast(Activator.GetObject(GetType(IAsociadoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("AsociadoServicios")), IAsociadoServicios)
                Me._facbancosServicios = DirectCast(Activator.GetObject(GetType(IFacBancoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacBancoServicios")), IFacBancoServicios)
            Catch ex As Exception
                logger.[Error](ex.Message)
                Me.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, True)
            End Try
        End Sub

        Public Sub ActualizarTitulo()
            Me.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.fac_titleConsultarFacOperacionDetaProformas, Recursos.Ids.fac_ConsultarFacOperacionDetaProforma)
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


                Dim FacOperacionDetaProformaAuxiliar As New FacOperacionDetaProforma()
                FacOperacionDetaProformaAuxiliar.Deposito = "1"
                'Me._FacOperacionDetaProformas = Me._FacOperacionDetaProformaServicios.ConsultarTodos()
                Me._FacOperacionDetaProformas = Me._FacOperacionDetaProformaServicios.ObtenerFacOperacionDetaProformasFiltro(FacOperacionDetaProformaAuxiliar)
                FacOperacionDetaProformaselect = convertir_FacOperacionDetaProformaSelec(Me._FacOperacionDetaProformas)
                Me._ventana.Resultados = FacOperacionDetaProformaselect
                Dim chequevacio As New FacOperacionDetaProforma
                chequevacio.FechaDeposito = Date.Now
                Me._ventana.FacOperacionDetaProformaFiltrar = chequevacio


                'Dim asociados As IList(Of Asociado) = Me._asociadosServicios.ConsultarTodos()
                'Dim primeraasociado As New Asociado()
                'primeraasociado.Id = Integer.MinValue
                'asociados.Insert(0, primeraasociado)
                'Me._ventana.Asociados = asociados

                Dim facbancos As IList(Of FacBanco) = Me._facbancosServicios.ConsultarTodos()
                Dim primerafacbanco As New FacBanco()
                primerafacbanco.Id = Integer.MinValue
                facbancos.Insert(0, primerafacbanco)
                Me._ventana.Bancos = facbancos

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


        ''' <summary>
        ''' Método que realiza una consulta al servicio, con el fin de filtrar los datos que se muestran 
        ''' por pantalla
        ''' </summary>
        Public Sub AplicarDeposito()
            Try
                '#Region "trace"
                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                End If
                '#End Region

                Dim v_FacOperacionDetaProforma As List(Of FacOperacionDetaProformaSelec) = DirectCast(_ventana.Resultados, List(Of FacOperacionDetaProformaSelec))
                Dim v_FacOperacionDetaProforma2 As FacOperacionDetaProforma = DirectCast(_ventana.FacOperacionDetaProformaFiltrar, FacOperacionDetaProforma)
                v_FacOperacionDetaProforma2.Banco = If(Not DirectCast(Me._ventana.Banco, FacBanco).Id.Equals("NGN"), DirectCast(Me._ventana.Banco, FacBanco), Nothing)
                'Dim v_FacOperacionDetaProforma2 As List(Of FacOperacionDetaProforma) = DirectCast(_ventana.Resultados, List(Of FacOperacionDetaProforma))

                For i As Integer = 0 To v_FacOperacionDetaProforma.Count - 1
                    If (v_FacOperacionDetaProforma(i).Seleccion = True) Then
                        If (Me._ventana.Banco IsNot Nothing) AndAlso (DirectCast(Me._ventana.Banco, FacBanco).Id <> Integer.MinValue) Then
                            v_FacOperacionDetaProforma(i).Deposito = "2"
                            v_FacOperacionDetaProforma(i).FechaDeposito = v_FacOperacionDetaProforma2.FechaDeposito
                            v_FacOperacionDetaProforma(i).NDeposito = v_FacOperacionDetaProforma2.NDeposito
                            v_FacOperacionDetaProforma(i).Banco = v_FacOperacionDetaProforma2.Banco
                            Dim a As New FacOperacionDetaProforma
                            a = convertir_FacOperacionDetaProforma(v_FacOperacionDetaProforma(i))
                            Dim exitoso As Boolean = _FacOperacionDetaProformaServicios.InsertarOModificar(a, UsuarioLogeado.Hash)
                        End If
                    End If
                Next

                Dim FacOperacionDetaProformaAuxiliar As New FacOperacionDetaProforma()
                FacOperacionDetaProformaAuxiliar.Deposito = "1"
                'Me._FacOperacionDetaProformas = Me._FacOperacionDetaProformaServicios.ConsultarTodos()
                Me._FacOperacionDetaProformas = Me._FacOperacionDetaProformaServicios.ObtenerFacOperacionDetaProformasFiltro(FacOperacionDetaProformaAuxiliar)
                FacOperacionDetaProformaselect = convertir_FacOperacionDetaProformaSelec(Me._FacOperacionDetaProformas)
                Me._ventana.Resultados = FacOperacionDetaProformaselect

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
        ''' Método que invoca una nueva página "ConsultarFacOperacionDetaProforma" y la instancia con el objeto seleccionado
        ''' </summary>
        Public Sub IrConsultarFacOperacionDetaProforma()
            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region

            'Me.Navegar(New ConsultarFacOperacionDetaProforma(Me._ventana.FacOperacionDetaProformaSeleccionado))
            'Me.Navegar(New ConsultarFacOperacionDetaProforma())
            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region
        End Sub

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
            '    newDir = ListSortDirection.Descending
            'End If

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

        'para tildar  o destildar 
        Public Sub seleccionar(ByVal value As Boolean)

            Try
                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                End If


                'Dim FacOperacionDetaProformaAuxiliar As New FacOperacionDetaProforma()
                'FacOperacionDetaProformaAuxiliar.Deposito = "1"

                ''Me._FacOperacionDetaProformas = Me._FacOperacionDetaProformaServicios.ConsultarTodos()
                'Me._FacOperacionDetaProformas = Me._FacOperacionDetaProformaServicios.ObtenerFacOperacionDetaProformasFiltro(FacOperacionDetaProformaAuxiliar)

                'FacOperacionDetaProformaselect = convertir_FacOperacionDetaProformaSelec(Me._FacOperacionDetaProformas)

                For i As Integer = 0 To FacOperacionDetaProformaselect.Count - 1
                    FacOperacionDetaProformaselect.Item(i).Seleccion = value
                Next
                Me._ventana.Resultados = Nothing
                Me._ventana.Resultados = FacOperacionDetaProformaselect

                Me._ventana.FocoPredeterminado()

                '#Region "trace"
                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                End If
                '#End Region
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


        'para covertir una clase FacOperacionDetaProforma a FacOperacionDetaProformaSelec
        Public Function convertir_FacOperacionDetaProformaSelec(ByVal v_FacOperacionDetaProforma As IList(Of FacOperacionDetaProforma)) As IList(Of FacOperacionDetaProformaSelec)
            Dim FacOperacionDetaProformaSelec As New List(Of FacOperacionDetaProformaSelec)
            Dim monto As Double = 0
            Me._ventana.NReg = v_FacOperacionDetaProforma.Count
            For i As Integer = 0 To v_FacOperacionDetaProforma.Count - 1
                FacOperacionDetaProformaSelec.Add(New FacOperacionDetaProformaSelec)
                FacOperacionDetaProformaSelec.Item(i).Seleccion = False
                FacOperacionDetaProformaSelec.Item(i).Banco = v_FacOperacionDetaProforma.Item(i).Banco
                FacOperacionDetaProformaSelec.Item(i).BancoG = v_FacOperacionDetaProforma.Item(i).BancoG
                FacOperacionDetaProformaSelec.Item(i).Id = v_FacOperacionDetaProforma.Item(i).Id
                FacOperacionDetaProformaSelec.Item(i).NDeposito = v_FacOperacionDetaProforma.Item(i).NDeposito
                FacOperacionDetaProformaSelec.Item(i).Deposito = v_FacOperacionDetaProforma.Item(i).Deposito
                FacOperacionDetaProformaSelec.Item(i).NCheque = v_FacOperacionDetaProforma.Item(i).NCheque
                FacOperacionDetaProformaSelec.Item(i).Monto = v_FacOperacionDetaProforma.Item(i).Monto
                monto = v_FacOperacionDetaProforma.Item(i).Monto + monto
                FacOperacionDetaProformaSelec.Item(i).Fecha = v_FacOperacionDetaProforma.Item(i).Fecha
                FacOperacionDetaProformaSelec.Item(i).FechaReg = v_FacOperacionDetaProforma.Item(i).FechaReg
                FacOperacionDetaProformaSelec.Item(i).FechaDeposito = v_FacOperacionDetaProforma.Item(i).FechaDeposito
            Next
            Me._ventana.Tmonto = monto
            Return FacOperacionDetaProformaSelec
        End Function

        'para covertir una clase FacOperacionDetaProformaSelec a FacOperacionDetaProforma
        Public Function convertir_FacOperacionDetaProforma(ByVal v_FacOperacionDetaProformaSelec2 As FacOperacionDetaProformaSelec) As FacOperacionDetaProforma
            Dim FacOperacionDetaProforma2 As New FacOperacionDetaProforma
            FacOperacionDetaProforma2.Banco = v_FacOperacionDetaProformaSelec2.Banco
            FacOperacionDetaProforma2.BancoG = v_FacOperacionDetaProformaSelec2.BancoG
            FacOperacionDetaProforma2.Id = v_FacOperacionDetaProformaSelec2.Id
            FacOperacionDetaProforma2.NDeposito = v_FacOperacionDetaProformaSelec2.NDeposito
            FacOperacionDetaProforma2.Deposito = v_FacOperacionDetaProformaSelec2.Deposito
            FacOperacionDetaProforma2.NCheque = v_FacOperacionDetaProformaSelec2.NCheque
            FacOperacionDetaProforma2.Monto = v_FacOperacionDetaProformaSelec2.Monto
            FacOperacionDetaProforma2.Fecha = v_FacOperacionDetaProformaSelec2.Fecha
            FacOperacionDetaProforma2.FechaReg = v_FacOperacionDetaProformaSelec2.FechaReg
            FacOperacionDetaProforma2.FechaDeposito = v_FacOperacionDetaProformaSelec2.FechaDeposito

            Return FacOperacionDetaProforma2
        End Function

    End Class

    'este es para agregar el check para seleccionar las que se van a depositar
    Public Class FacOperacionDetaProformaSelec
        Inherits FacOperacionDetaProforma

        Private p_seleccion As Boolean

        Public Property Seleccion() As Boolean
            Get
                Return Me.p_seleccion
            End Get
            Set(ByVal Value As Boolean)
                Me.p_seleccion = Value
            End Set
        End Property

    End Class

End Namespace