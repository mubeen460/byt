﻿Imports System.Configuration
Imports System.Net.Sockets
Imports System.Runtime.Remoting
Imports System.Windows.Input
Imports NLog
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.FacOperacionDetalles
'Imports Diginsoft.Bolet.Cliente.Fac.Ventanas.Principales
Imports Diginsoft.Bolet.ObjetosComunes.ContratosServicios
Imports Diginsoft.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.ObjetosComunes.ContratosServicios
Imports Trascend.Bolet.Cliente.Presentadores
Imports Trascend.Bolet.Cliente.Ventanas.Principales
Namespace Presentadores.FacOperacionDetalles
    Class PresentadorConsultarFacOperacionDetalle
        Inherits PresentadorBase

        Private _ventana As IConsultarFacOperacionDetalle
        Private _FacOperacionDetalleServicios As IFacOperacionDetalleServicios
        Private _AsociadoServicios As IAsociadoServicios
        Private _BancoGServicios As IBancoGServicios
        Private Shared _paginaPrincipal As PaginaPrincipal = PaginaPrincipal.ObtenerInstancia
        Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

        ''' <summary>
        ''' Constructor predeterminado
        ''' </summary>
        ''' <param name="ventana">Página que satisface el contrato</param>
        ''' <param name="FacOperacionDetalle">FacOperacionDetalle a mostrar</param>
        Public Sub New(ByVal ventana As IConsultarFacOperacionDetalle, ByVal FacOperacionDetalle As Object)
            Try
                Me._ventana = ventana
                Me._ventana.FacOperacionDetalle = FacOperacionDetalle
                'Me._ventana.Region = DirectCast(Me._ventana.FacOperacionDetalle, FacOperacionDetalle).Region
                Me._FacOperacionDetalleServicios = DirectCast(Activator.GetObject(GetType(IFacOperacionDetalleServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacOperacionDetalleServicios")), IFacOperacionDetalleServicios)
                Me._AsociadoServicios = DirectCast(Activator.GetObject(GetType(IAsociadoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("AsociadoServicios")), IAsociadoServicios)
                Me._BancoGServicios = DirectCast(Activator.GetObject(GetType(IBancoGServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("BancoGServicios")), IBancoGServicios)
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
                '#Region "trace"
                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                End If
                '#End Region

                Me.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.fac_titleConsultarFacOperacionDetalle, Recursos.Ids.fac_ConsultarFacOperacionDetalle)

                Dim FacOperacionDetalle As FacOperacionDetalle = DirectCast(Me._ventana.FacOperacionDetalle, FacOperacionDetalle)

                Dim asociados As IList(Of Servicio) = Me._AsociadoServicios.ConsultarTodos()
                Me._ventana.Asociados = asociados
                Me._ventana.Asociado = Me.BuscarAsociado(asociados, FacOperacionDetalle.Id)

                Dim bancos As IList(Of BancoG) = Me._BancoGServicios.ConsultarTodos()
                Me._ventana.Bancos = Bancos
                'Me._ventana.Banco = Me.BuscarBancoG(bancos, FacOperacionDetalle.Banco)

                Me._ventana.FocoPredeterminado()

                '#Region "trace"
                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                    '#End Region
                End If
            Catch ex As Exception
                logger.[Error](ex.Message)
                Me.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, True)
            Finally
                Mouse.OverrideCursor = Nothing
            End Try
        End Sub

        ''' <summary>
        ''' Método que dependiendo del estado de la página, habilita los campos o 
        ''' modifica los datos del usuario
        ''' </summary>
        Public Sub Modificar()
            Try
                '#Region "trace"
                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                End If
                '#End Region

                'Habilitar campos
                If Me._ventana.TextoBotonModificar = Recursos.Etiquetas.btnModificar Then
                    Me._ventana.HabilitarCampos = True
                    Me._ventana.TextoBotonModificar = Recursos.Etiquetas.btnAceptar
                Else

                    'Modifica los datos del FacOperacionDetalle
                    Dim FacOperacionDetalle As FacOperacionDetalle = DirectCast(Me._ventana.FacOperacionDetalle, FacOperacionDetalle)
                    'FacOperacionDetalle.Region = If(Not Me._ventana.Region.Equals(""), Me._ventana.Region, Nothing)

                    If Me._FacOperacionDetalleServicios.InsertarOModificar(FacOperacionDetalle, UsuarioLogeado.Hash) Then
                        _paginaPrincipal.MensajeUsuario = Recursos.MensajesConElUsuario.fac_FacOperacionDetalleModificado
                        Me.Navegar(_paginaPrincipal)
                    End If
                End If

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
            End Try
        End Sub

        Public Sub Eliminar()
            Try
                '#Region "trace"
                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                End If
                '#End Region

                If Me._FacOperacionDetalleServicios.Eliminar(DirectCast(Me._ventana.FacOperacionDetalle, FacOperacionDetalle), UsuarioLogeado.Hash) Then
                    _paginaPrincipal.MensajeUsuario = Recursos.MensajesConElUsuario.fac_FacOperacionDetalleEliminado
                    Me.Navegar(_paginaPrincipal)
                End If

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
            End Try
        End Sub
    End Class
End Namespace
