﻿Imports System.Configuration
Imports System.Net.Sockets
Imports System.Runtime.Remoting
Imports System.Windows.Input
Imports NLog
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.FacReportes
Imports Diginsoft.Bolet.ObjetosComunes.ContratosServicios
Imports Diginsoft.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.ObjetosComunes.ContratosServicios
Imports Trascend.Bolet.Cliente.Presentadores
Imports Trascend.Bolet.Cliente.Ventanas.Principales
Imports Diginsoft.Bolet.Cliente.Fac.Ventanas.FacReportes

Imports Trascend.Bolet.Cliente.FacReportes
Imports System.Windows
Imports System.Windows.Controls
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports DataTable = System.Data.DataTable
Imports System.Data
Namespace Presentadores.FacReportes
    Class PresentadorFacFacturacionLoteVieja
        Inherits PresentadorBase

        Private _ventana As IFacFacturacionLoteVieja
        Private _FacFactura As FacFactura
        Dim _FacFacturaDetalle As List(Of FacFactuDetalle)
        Private _FacFacturaServicios As IFacFacturaServicios
        'Private _FacFacturaProformaServicios As IFacFacturaProformaServicios
        Private Shared _paginaPrincipal As PaginaPrincipal = PaginaPrincipal.ObtenerInstancia
        Private Shared logger As Logger = LogManager.GetCurrentClassLogger()
        'Private _asociadosServicios As IAsociadoServicios
        'Private _InteresadosServicios As IInteresadoServicios
        'Private _contactoServicios As IContactoServicios
        ''Private _facoperacionesServicios As IFacOperacionServicios
        'Private _idiomasServicios As IIdiomaServicios
        'Private _monedasServicios As IMonedaServicios
        'Private _asociados As IList(Of Asociado)
        'Private _Interesados As IList(Of Interesado)
        'Private _asociadosimp As IList(Of Asociado)
        'Private _Marcas As IList(Of Marca)
        'Private _detalleenvios As IList(Of FacDetalleEnvio)
        'Private _Cartas As IList(Of Carta)
        ''Private _FacOperaciones As IList(Of FacOperacion)
        'Private _tasasServicios As ITasaServicios
        ''Private _FacCreditoServicios As IFacCreditoServicios
        'Private _FacContadorProServicios As IFacContadorProServicios
        'Private _bancosServicios As IFacBancoServicios
        Private _detalleenviosServicios As IFacDetalleEnvioServicios
        Private _etiquetaServicios As IEtiquetaServicios
        'Private _cartasServicios As ICartaServicios
        'Private _paisesServicios As IPaisServicios
        'Private _desgloseserviciosServicios As IFacDesgloseServicioServicios
        'Private _DepartamentoserviciosServicios As IFacDepartamentoServicioServicios
        Private _FacFactuDetaServicios As IFacFactuDetalleServicios
        'Private _FacFactuDetaProformasServicios As IFacFactuDetaProformaServicios
        'Private _TarifaServiciosServicios As ITarifaServicioServicios
        'Private _DocumentosMarcasServicios As IDocumentosMarcaServicios
        'Private _DocumentosPatentesServicios As IDocumentosPatenteServicios
        'Private _DocumentosTraduccionesServicios As IDocumentosTraduccionServicios
        'Private _FacRecursosServicios As IFacRecursoServicios
        'Private _MaterialesServicios As IMaterialServicios
        'Private _MarcasServicios As IMarcaServicios
        'Private _PatentesServicios As IPatenteServicios
        'Private _FacAnualidadesServicios As IFacAnualidadServicios
        'Private _TipoMarcasServicios As ITipoMarcaServicios
        'Private _TipoPatentesServicios As ITipoPatenteServicios
        'Private _TipoClasesServicios As ITipoClaseServicios
        'Private _FacOperacionDetalleTmsServicios As IFacOperacionDetalleTmServicios
        'Private _FacImpuestosServicios As IFacImpuestoServicios
        'Private _FacOperacionServicios As IFacOperacionServicios
        'Private _FacOperacionProformasServicios As IFacOperacionProformaServicios
        'Private _FacOperacionDetaServicios As IFacOperacionDetalleServicios
        'Private _FacOperacionDetaProformasServicios As IFacOperacionDetaProformaServicios
        'Private _FacOperacionDetaTmServicios As IFacOperacionDetalleTmServicios
        'Private _FacOperacionDetaTmProformasServicios As IFacOperacionDetaTmProformaServicios
        'Private _FacContadorServicios As IContadorFacServicios
        'Private _FacInternacionalesServicios As IFacInternacionalServicios
        'FacOperacionDetaProforma
        ''Private _FacFormaServicios As IFacFormaServicios
        'Dim xoperacion As String
        ''' <summary>
        ''' Constructor predeterminado
        ''' </summary>
        ''' <param name="ventana">Página que satisface el contrato</param>

        ''' <summary>
        ''' Constructor predeterminado
        ''' </summary>
        ''' <param name="ventana">Página que satisface el contrato</param>
        ''' <param name="FacFacturaProforma">FacFacturaProforma a mostrar</param>
        Public Sub New(ByVal ventana As IFacFacturacionLoteVieja)
            Try
                Me._ventana = ventana                

                'Me._ventana.FacFacturaProforma = New FacFacturaProforma()
                Me._FacFacturaServicios = DirectCast(Activator.GetObject(GetType(IFacFacturaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacFacturaServicios")), IFacFacturaServicios)
                'Me._FacFacturaProformaServicios = DirectCast(Activator.GetObject(GetType(IFacFacturaProformaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacFacturaProformaServicios")), IFacFacturaProformaServicios)
                'Me._asociadosServicios = DirectCast(Activator.GetObject(GetType(IAsociadoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("AsociadoServicios")), IAsociadoServicios)
                'Me._InteresadosServicios = DirectCast(Activator.GetObject(GetType(IInteresadoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("InteresadoServicios")), IInteresadoServicios)
                ''Me._facoperacionesServicios = DirectCast(Activator.GetObject(GetType(IFacOperacionServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacOperacionServicios")), IFacOperacionServicios)
                'Me._idiomasServicios = DirectCast(Activator.GetObject(GetType(IIdiomaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("IdiomaServicios")), IIdiomaServicios)
                'Me._monedasServicios = DirectCast(Activator.GetObject(GetType(IMonedaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("MonedaServicios")), IMonedaServicios)
                'Me._tasasServicios = DirectCast(Activator.GetObject(GetType(ITasaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("TasaServicios")), ITasaServicios)
                ''Me._FacCreditoServicios = DirectCast(Activator.GetObject(GetType(IFacCreditoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacCreditoServicios")), IFacCreditoServicios)
                'Me._FacContadorProServicios = DirectCast(Activator.GetObject(GetType(IFacContadorProServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacContadorProServicios")), IFacContadorProServicios)
                'Me._bancosServicios = DirectCast(Activator.GetObject(GetType(IFacBancoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacBancoServicios")), IFacBancoServicios)
                'Me._detalleenviosServicios = DirectCast(Activator.GetObject(GetType(IFacDetalleEnvioServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("DetalleEnvioServicios")), IFacDetalleEnvioServicios)
                'Me._cartasServicios = DirectCast(Activator.GetObject(GetType(ICartaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("CartaServicios")), ICartaServicios)
                'Me._paisesServicios = DirectCast(Activator.GetObject(GetType(IPaisServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("PaisServicios")), IPaisServicios)
                'Me._desgloseserviciosServicios = DirectCast(Activator.GetObject(GetType(IFacDesgloseServicioServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("DesgloseservicioServicios")), IFacDesgloseServicioServicios)
                'Me._DepartamentoserviciosServicios = DirectCast(Activator.GetObject(GetType(IFacDepartamentoServicioServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("DepartamentoServicioServicios")), IFacDepartamentoServicioServicios)
                Me._FacFactuDetaServicios = DirectCast(Activator.GetObject(GetType(IFacFactuDetalleServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacFactuDetalleServicios")), IFacFactuDetalleServicios)
                Me._etiquetaServicios = DirectCast(Activator.GetObject(GetType(IEtiquetaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("EtiquetaServicios")), IEtiquetaServicios)
                'Me._FacFactuDetaProformasServicios = DirectCast(Activator.GetObject(GetType(IFacFactuDetaProformaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacFactuDetaProformaServicios")), IFacFactuDetaProformaServicios)
                'Me._TarifaServiciosServicios = DirectCast(Activator.GetObject(GetType(ITarifaServicioServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("TarifaServicioServicios")), ITarifaServicioServicios)

                'Me._DocumentosMarcasServicios = DirectCast(Activator.GetObject(GetType(IDocumentosMarcaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("DocumentosMarcaServicios")), IDocumentosMarcaServicios)
                'Me._DocumentosPatentesServicios = DirectCast(Activator.GetObject(GetType(IDocumentosPatenteServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("DocumentosPatenteServicios")), IDocumentosPatenteServicios)
                'Me._DocumentosTraduccionesServicios = DirectCast(Activator.GetObject(GetType(IDocumentosTraduccionServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("DocumentosTraduccionServicios")), IDocumentosTraduccionServicios)
                'Me._FacRecursosServicios = DirectCast(Activator.GetObject(GetType(IFacRecursoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacRecursoServicios")), IFacRecursoServicios)
                'Me._MaterialesServicios = DirectCast(Activator.GetObject(GetType(IMaterialServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("MaterialServicios")), IMaterialServicios)
                ''Me._FacFormaServicios = DirectCast(Activator.GetObject(GetType(IFacFormaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacFormaServicios")), IFacFormaServicios)
                'Me._MarcasServicios = DirectCast(Activator.GetObject(GetType(IMarcaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("MarcaServicios")), IMarcaServicios)
                'Me._PatentesServicios = DirectCast(Activator.GetObject(GetType(IPatenteServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("PatenteServicios")), IPatenteServicios)
                'Me._FacAnualidadesServicios = DirectCast(Activator.GetObject(GetType(IFacAnualidadServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacAnualidadServicios")), IFacAnualidadServicios)
                'Me._TipoMarcasServicios = DirectCast(Activator.GetObject(GetType(ITipoMarcaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("TipoMarcaServicios")), ITipoMarcaServicios)
                'Me._TipoPatentesServicios = DirectCast(Activator.GetObject(GetType(ITipoPatenteServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("TipoPatenteServicios")), ITipoPatenteServicios)
                'Me._TipoClasesServicios = DirectCast(Activator.GetObject(GetType(ITipoClaseServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("TipoClaseServicios")), ITipoClaseServicios)
                'Me._FacOperacionDetalleTmsServicios = DirectCast(Activator.GetObject(GetType(IFacOperacionDetalleTmServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacOperacionDetalleTmServicios")), IFacOperacionDetalleTmServicios)
                'Me._FacImpuestosServicios = DirectCast(Activator.GetObject(GetType(IFacImpuestoServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacImpuestoServicios")), IFacImpuestoServicios)
                'Me._FacOperacionServicios = DirectCast(Activator.GetObject(GetType(IFacOperacionServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacOperacionServicios")), IFacOperacionServicios)
                'Me._FacOperacionProformasServicios = DirectCast(Activator.GetObject(GetType(IFacOperacionProformaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacOperacionProformaServicios")), IFacOperacionProformaServicios)
                'Me._FacOperacionDetaServicios = DirectCast(Activator.GetObject(GetType(IFacOperacionDetalleServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacOperacionDetalleServicios")), IFacOperacionDetalleServicios)
                'Me._FacOperacionDetaProformasServicios = DirectCast(Activator.GetObject(GetType(IFacOperacionDetaProformaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacOperacionDetaProformaServicios")), IFacOperacionDetaProformaServicios)
                'Me._FacOperacionDetaTmServicios = DirectCast(Activator.GetObject(GetType(IFacOperacionDetalleTmServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacOperacionDetalleTmServicios")), IFacOperacionDetalleTmServicios)
                'Me._FacOperacionDetaTmProformasServicios = DirectCast(Activator.GetObject(GetType(IFacOperacionDetaTmProformaServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacOperacionDetaTmProformaServicios")), IFacOperacionDetaTmProformaServicios)
                'Me._FacContadorServicios = DirectCast(Activator.GetObject(GetType(IContadorFacServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("ContadorFacServicios")), IContadorFacServicios)
                'Me._FacInternacionalesServicios = DirectCast(Activator.GetObject(GetType(IFacInternacionalServicios), ConfigurationManager.AppSettings("RutaServidor") + ConfigurationManager.AppSettings("FacInternacionalServicios")), IFacInternacionalServicios)

                'eliminar_operacion_detalle_tm_usuario() ' para eliminar los operacion tmp de operacion_detalle_tm

                'Dim proforma As FacFacturaProforma = FacFacturaOProforma
                'If proforma.Status = 1 Then
                '    pasar_profora_a_factura(FacFacturaOProforma)
                'Else
                '    'Me._ventana.FacFactura = FacFacturaOProforma
                'End If

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

            Me.Navegar(New Diginsoft.Bolet.Cliente.Fac.Ventanas.FacReportes.FacFacturacionLoteVieja())
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
                '#Region "trace"
                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                End If

                ActualizarTitulo()


                Me._ventana.FocoPredeterminado()


                '#Region "trace"
                If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                    logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
                    '#End Region
                End If
                'Else
                'Me.Navegar(Recursos.MensajesConElUsuario.fac_error_tasa_dia, True)
                'End If
            Catch ex As Exception
                logger.[Error](ex.Message)
                Me.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, True)
            Finally
                Mouse.OverrideCursor = Nothing
            End Try
        End Sub

        Public Sub ActualizarTitulo()
            Me.ActualizarTituloVentanaPrincipal(Recursos.Etiquetas.fac_menuItemFacFacturacionLoteVieja, Recursos.Ids.fac_menuItemFacFacturacionLoteVieja)
        End Sub

        Public Sub Reporte()
            Mouse.OverrideCursor = Cursors.Wait
            Try

                Dim reporte As New ReportDocument()
                Dim estructuraDeDatosEnc As IList(Of StructReporteFActuraEnc) = New List(Of StructReporteFActuraEnc)()

                Dim estructuraDeDatosDeta As IList(Of StructReporteFActuraDeta) = New List(Of StructReporteFActuraDeta)()
                'reporte.Load();                
                Dim datosEnc As New DataTable("DataTable1")
                Dim datosDeta As New DataTable("DataTable2")
                datosEnc = datosenc_colum()

                datosDeta = datosdeta_colum()
                'Dim estructura As StructReporteCarta1 = ObtenerEstructuraCarta1()
                'estructuraDeDatos.Add(estructura)

                ObtenerEstructura(estructuraDeDatosEnc, estructuraDeDatosDeta)
                'estructuraDeDatosEnc = ObtenerEstructuraEnc()

                'estructuraDeDatosDeta = ObtenerEstructuraDeta()


                datosEnc = ArmarReporteEnc(datosEnc, estructuraDeDatosEnc)

                datosDeta = ArmarReporteDeta(datosDeta, estructuraDeDatosDeta)
                'reporte.PrintOptions.PrinterName = ConfigurationManager.AppSettings["ImpresoraReportes"];
                Dim ds As New DataSet()
                ds.Tables.Add(datosEnc)
                ds.Tables.Add(datosDeta)
                reporte.Load(GetRutaReporte())
                reporte.SetDataSource(ds)
                'reporte.SetDataSource(datosDeta)
                'Me._ventana.CrystalViewer = reporte
                'reporte.PrintToPrinter(1, False, 1, 0)                

                Mouse.OverrideCursor = Nothing
                IrConsultarReporte(reporte)
                '#End Region

            Catch ex As ApplicationException
                'logger.Error(ex.Message)
                'Me.Navegar(ex.Message, True)
                Mouse.OverrideCursor = Nothing
            Catch ex As Exception
                Mouse.OverrideCursor = Nothing
                '' Me.Navegar(Recursos.MensajesConElUsuario.ErrorInesperado, True)

            End Try
        End Sub

        Public Sub IrConsultarReporte(ByVal reporte As ReportDocument)
            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Entrando al metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region
            'Me._ventana.FacFacturaSeleccionado.Accion = 2 'no modificar
            Me.Navegar(New ReportesRpt(reporte))
            'Me.Navegar(New ConsultarFacFactura())
            '#Region "trace"
            If ConfigurationManager.AppSettings("ambiente").ToString().Equals("desarrollo") Then
                logger.Debug("Saliendo del metodo {0}", (New System.Diagnostics.StackFrame()).GetMethod().Name)
            End If
            '#End Region
        End Sub

        Private Function GetRutaReporte() As String
            Dim retorno As String
            retorno = Environment.CurrentDirectory & ConfigurationManager.AppSettings("rutaFacreportes") & "FacFacturacionLoteVieja.rpt"
            'retorno = "C:\DG_2012_09_11\DG\src\Cliente\Cliente_Fac\Reportes\Carta1CR.rpt"
            Return retorno

        End Function

        Public Function consultar_factura() As List(Of FacFactura)

            Dim FacFacturaAuxiliar As New FacFactura()
            Dim FacFacturas As List(Of FacFactura)
            FacFacturas = Nothing
            Try                
                FacFacturaAuxiliar.P_mip = Me._ventana.desde
                FacFacturaAuxiliar.Bst = Me._ventana.hasta
                If Me._ventana.Tipo = 1 Or Me._ventana.Tipo = 22 Then
                    FacFacturaAuxiliar.Status = 4
                Else
                    FacFacturaAuxiliar.Status = 3
                End If                

                FacFacturas = Me._FacFacturaServicios.ObtenerFacFacturasFiltro(FacFacturaAuxiliar)
                If FacFacturas IsNot Nothing Then
                    If FacFacturas.Count > 0 Then
                        Return (FacFacturas)
                    Else
                        Return (Nothing)
                    End If
                Else
                    Return (Nothing)
                End If

            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Public Function etiquetas_texto(ByVal codigo As String) As String()
            Dim valor(2) As String

            Dim etiquetas As List(Of Etiqueta) = _etiquetaServicios.ConsultarTodos()
            Dim EtiquetaFiltrados As IEnumerable(Of Etiqueta) = etiquetas
            EtiquetaFiltrados = From e In EtiquetaFiltrados Where e.Id IsNot Nothing AndAlso e.Id.ToLower().Contains(codigo.ToLower())
            valor(0) = EtiquetaFiltrados(0).Descripcion1
            valor(1) = EtiquetaFiltrados(0).Descripcion2
            Return (valor)

        End Function

        Public Sub lp_fecha_esc_n(ByVal dfecha As Date, ByRef cfecha As String, ByVal idioma As String)
            Dim w_dia, w_mes, w_ano As Integer
            w_mes = dfecha.Month
            w_dia = dfecha.Day
            w_ano = dfecha.Year
            cfecha = fecha(w_mes, w_dia, w_ano, idioma)
        End Sub

        Public Sub lp_fecha_esc_n_bf(ByVal dfecha As Date, ByRef cfecha As String)
            Dim w_dia, w_mes, w_ano As Integer
            w_mes = dfecha.Month
            w_dia = dfecha.Day
            w_ano = dfecha.Year
            If w_dia < 10 Then
                w_dia = "0" & w_dia
            End If
            If w_mes < 10 Then
                cfecha = "Caracas " & w_dia & "/0" & w_mes & "/" & w_ano
            Else
                cfecha = "Caracas " & w_dia & "/" & w_mes & "/" & w_ano
            End If
        End Sub

        Public Function fecha(ByVal mes As Integer, ByVal dia As Integer, ByVal anio As Integer, ByVal idioma As String) As String
            Dim retorna As String = ""
            Select Case mes
                Case 1
                    If idioma = "ES" Then
                        retorna = "Caracas, Enero " & dia & ", " & anio
                    Else
                        retorna = "Caracas, January " & dia & ", " & anio
                    End If
                Case 2
                    If idioma = "ES" Then
                        retorna = "Caracas, Febrero " & dia & ", " & anio
                    Else
                        retorna = "Caracas, February " & dia & ", " & anio
                    End If
                Case 3
                    If idioma = "ES" Then
                        retorna = "Caracas, Marzo " & dia & ", " & anio
                    Else
                        retorna = "Caracas, March " & dia & ", " & anio
                    End If
                Case 4
                    If idioma = "ES" Then
                        retorna = "Caracas, Abril " & dia & ", " & anio
                    Else
                        retorna = "Caracas, April " & dia & ", " & anio
                    End If
                Case 5
                    If idioma = "ES" Then
                        retorna = "Caracas, Mayo " & dia & ", " & anio
                    Else
                        retorna = "Caracas, May " & dia & ", " & anio
                    End If
                Case 6
                    If idioma = "ES" Then
                        retorna = "Caracas, Junio " & dia & ", " & anio
                    Else
                        retorna = "Caracas, June " & dia & ", " & anio
                    End If
                Case 7
                    If idioma = "ES" Then
                        retorna = "Caracas, Julio " & dia & ", " & anio
                    Else
                        retorna = "Caracas, July " & dia & ", " & anio
                    End If
                Case 8
                    If idioma = "ES" Then
                        retorna = "Caracas, Agosto " & dia & ", " & anio
                    Else
                        retorna = "Caracas, August " & dia & ", " & anio
                    End If
                Case 9
                    If idioma = "ES" Then
                        retorna = "Caracas, Septiembre " & dia & ", " & anio
                    Else
                        retorna = "Caracas, September " & dia & ", " & anio
                    End If
                Case 10
                    If idioma = "ES" Then
                        retorna = "Caracas, Octubre " & dia & ", " & anio
                    Else
                        retorna = "Caracas, October " & dia & ", " & anio
                    End If
                Case 11
                    If idioma = "ES" Then
                        retorna = "Caracas, Noviembre " & dia & ", " & anio
                    Else
                        retorna = "Caracas, November " & dia & ", " & anio
                    End If
                Case 12
                    If idioma = "ES" Then
                        retorna = "Caracas, Diciembre " & dia & ", " & anio
                    Else
                        retorna = "Caracas, December " & dia & ", " & anio
                    End If

            End Select

            Return (retorna)
        End Function

        Public Sub lp_compl(ByVal dfecha As DateTime, ByVal nfac As Integer, ByRef csalida As String)
            Dim w_par, w_camp, w_cero As String
            Dim w_fal As Integer
            w_cero = "0"
            w_par = dfecha.Year
            w_par = w_par.Substring(2, 2)
            w_camp = nfac
            If w_camp.Length < 7 Then
                w_fal = 7 - w_camp.Length
                While (w_fal > 0)
                    w_camp = w_cero & w_camp
                    w_fal = w_fal - 1
                End While
            End If
            csalida = w_par & "-" & w_camp
        End Sub

        Public Function BT_FACT00_N(ByVal factura As FacFactura,monto As double) As StructReporteFActuraEnc
            Dim structura As New StructReporteFActuraEnc()
            Dim w_s As String = ""
            structura = inicializar_enc()
            Try
                'w_lista = "cfactura=%%cfactura.fac_facturastipo=%%xterrero.fac_facturasimp=%%dst.trailermip=2bst=0ifpago=%%ifpago.fac_facturas"
                If factura.Caso IsNot Nothing Then
                    structura.Caso = factura.Caso
                Else
                    structura.Caso = ""
                End If
                'If factura.Status = 1 Then 'if ($ifp$ = 1)
                '    structura.TituloPago = "Condicion de Pago : Credito"
                'Else
                '    structura.TituloPago = "Condicion de Pago : Contado"
                'End If

                'If factura.AsociadoImp IsNot Nothing Then
                '    If factura.Asociado.BIsf = True Then
                '        structura.Invoice = "I N V O I C E   N°"
                '    End If
                '    'structura.Cliente = factura.Asociado.Nombre
                '    'structura. = factura.Asociado.Domicilio
                '    'structura.Rif = factura.Asociado.Rif
                '    'structura.Nit = factura.Asociado.Nit
                '    '---  'factura.Asociado.Pais.NombreEspanol 
                'Else
                '    If factura.Asociado.BIsf = True Then
                '        structura.Invoice = "I N V O I C E   N°"
                '    End If
                '    'structura.Cliente = factura.InteresadoImp.Nombre
                '    'structura. = factura.InteresadoImp.Domicilio
                '    'structura.Rif = factura.InteresadoImp.Rif
                '    'structura.Nit = factura.InteresadoImp.Nit
                '    '---  'factura.InteresadoImp.Pais.NombreEspanol 
                'End If
                If factura.XAsociado IsNot Nothing Then
                    structura.Cliente = factura.XAsociado
                Else
                    structura.Cliente = ""
                End If

                If factura.Rif IsNot Nothing Then
                    structura.Rif = factura.Rif
                Else
                    structura.Rif = ""
                End If
                If factura.XNit IsNot Nothing Then
                    structura.Nit = factura.XNit
                Else
                    structura.Nit = ""
                End If

                If structura.Cliente <> "" Then

                    If factura.P_mip IsNot Nothing Then
                        Select Case factura.P_mip
                            Case 1 ' Factura 
                                'call lp_compl(fseniat.fac_facturas,cseniat.fac_facturas,w_s) 
                                'xfactura.encabezado   = w_s                            
                                structura.Invoice = ""
                                If factura.FechaSeniat IsNot Nothing Then
                                    lp_compl(factura.FechaSeniat, factura.Seniat, w_s)
                                    structura.Xfactura = w_s

                                    lp_fecha_esc_n(factura.FechaSeniat, structura.Fecha, factura.Idioma.Id)
                                Else
                                    structura.Fecha = ""
                                End If
                            Case 2 'Caso Est
                                'Call lp_compl(ffactura.fac_facturas, cfactura.fac_facturas, w_s)
                                'xinvoice.encabezado = w_s
                                structura.Invoice = ""
                                If factura.FechaFactura IsNot Nothing Then
                                    lp_compl(factura.FechaFactura, factura.Id, w_s)
                                    structura.Invoice = w_s
                                    structura.Xfactura = ""

                                    lp_fecha_esc_n(factura.FechaFactura, structura.Fecha, factura.Idioma.Id)
                                Else
                                    structura.Fecha = ""
                                End If
                            Case 3 'Caso Factura
                                'call lp_compl(fseniat.fac_facturas,cseniat.fac_facturas,w_s) 
                                'xfactura.encabezado   = w_s                            
                                structura.Invoice = ""
                                If factura.FechaSeniat IsNot Nothing Then
                                    lp_compl(factura.FechaSeniat, factura.Seniat, w_s)
                                    structura.Xfactura = w_s
                                    structura.Invoice = ""
                                    lp_fecha_esc_n(factura.FechaSeniat, structura.Fecha, factura.Idioma.Id)
                                Else
                                    structura.Fecha = ""
                                End If
                                If factura.FechaFactura IsNot Nothing Then
                                    lp_compl(factura.FechaFactura, factura.Id, w_s)
                                End If
                            Case 4 ' Est
                                'Call lp_compl(ffactura.fac_facturas, cfactura.fac_facturas, w_s)
                                'xinvoice.encabezado = w_s
                                If factura.FechaFactura IsNot Nothing Then
                                    lp_compl(factura.FechaFactura, factura.Id, w_s)
                                    structura.Invoice = w_s
                                    structura.Xfactura = ""

                                    lp_fecha_esc_n(factura.FechaFactura, structura.Fecha, factura.Idioma.Id)
                                Else
                                    structura.Fecha = ""
                                End If
                        End Select
                        'skip(10)
                        'print_break("ENCABEZADO")
                    End If
                End If

                '''NOTA :Agregar campo a a la structura verificar donde se imprime
                'If factura.NumeroControl IsNot Nothing Then
                '    structura.Control = factura.NumeroControl
                'Else
                '    structura.Control = ""
                'End If
                'If factura.Codeti <> "" And factura.Codeti IsNot Nothing Then
                '    Dim textos(2) As String
                '    Select Case factura.P_mip
                '        Case 1 ' Factura 
                '            textos = etiquetas_texto(factura.Codeti)
                '        Case 2 'Caso Est
                '            textos = etiquetas_texto(factura.Codeti)
                '        Case 3 'Caso Factura
                '            textos = etiquetas_texto("9")
                '        Case 4 ' Est
                '            textos = etiquetas_texto(factura.Codeti)
                '    End Select
                '    structura.Texto1 = textos(0)
                '    structura.Texto2 = textos(1)
                'End If
                '''NOTA :Agregar campo a a la structura verificar donde se imprime
                'If factura.Idioma.Id = "ES" Then
                '    structura.Xour = "Nuestra Referencia"
                '    structura.Xourref = factura.Ourref
                'Else
                '    structura.Xour = "Our Reference"
                '    structura.Xourref = factura.Ourref
                'End If

                structura.Msubtimpo = monto
                structura.Mdescuento = (monto * factura.Descuento)
                structura.Piva = factura.Impuesto
                structura.Mtbimp = ((monto - (monto * factura.Descuento)) * factura.Impuesto) / 100
                structura.Msubtotal = factura.MSubtotal
                structura.Mttotal = monto - (monto * factura.Descuento) + (((monto - (monto * factura.Descuento)) * factura.Impuesto) / 100)
                structura.Mtimp = factura.Mtimp

                'Select Case factura.P_mip
                '    Case 1 ' Factura 
                '        structura.Seniat = w_s
                '    Case 2 'Caso Est
                '        structura.Seniat = w_s
                '    Case 3 'Caso Factura
                '        structura.Seniat = w_s
                '    Case 4 ' Est
                '        structura.Seniat = w_s
                'End Select

                'If factura.Moneda.Id <> "BF" Then
                structura.Moneda = factura.Moneda.Id
                'Else
                '    structura.Moneda = "BSF"
                'End If
                'If factura.Bst = 1 Then
                '    structura.Piva = factura.PSeniat
                'Else
                structura.Piva = factura.Impuesto
                'End If
                'structura.TituloNa = "tax"
                'structura.TituloCantidad = "Quanti"
                'structura.TituloPub = "Price/Unt"
                'structura.TituloNDesc = "Discount"
                'structura.TituloMMonto = "Amount"


            Catch ex As Exception
                'logger.Error(ex.Message)

            End Try
            Return (structura)
        End Function

        Private Sub ObtenerEstructura(ByRef enc As IList(Of StructReporteFActuraEnc), ByRef det As List(Of StructReporteFActuraDeta))
            Dim retorno As IList(Of StructReporteFActuraEnc) = New List(Of StructReporteFActuraEnc)
            Dim retornodet As IList(Of StructReporteFActuraDeta) = New List(Of StructReporteFActuraDeta)
            Dim structura As New StructReporteFActuraEnc()
            Dim structuradet As New StructReporteFActuraDeta()
            Dim factura As List(Of FacFactura) = consultar_factura()
            Dim monto As Double = 0
            Try
                If factura IsNot Nothing Then

                    For i As Integer = 0 To factura.Count - 1
                        structura = inicializar_enc()
                        If Me._ventana.Tipo = 22 Then
                            factura(i).P_mip = 3
                        Else
                            If Me._ventana.Tipo = 21 Then
                                factura(i).P_mip = 2
                            Else
                                If Me._ventana.Tipo = 3 Then
                                    factura(i).P_mip = 2
                                Else
                                    factura(i).P_mip = 1
                                End If

                            End If
                        End If
                        monto = 0
                        ObtenerEstructuraDeta(i, det, factura(i), monto)
                        structura = BT_FACT00_N(factura(i), monto)
                        structura.Id = i
                        retorno.Add(structura)
                    Next
                    enc = retorno
                End If
            Catch ex As Exception

            End Try
        End Sub

        Private Sub ObtenerEstructuraDeta(ByVal id As String, ByRef detalle As List(Of StructReporteFActuraDeta), ByVal factura As FacFactura, ByRef monto As Double)
            Dim retorno As IList(Of StructReporteFActuraDeta) = New List(Of StructReporteFActuraDeta)
            retorno = detalle
            Dim structura As New StructReporteFActuraDeta()
            structura = inicializar_det()
            Dim detalleaux As New FacFactuDetalle
            Try
                detalleaux.Factura = factura
                _FacFacturaDetalle = Me._FacFactuDetaServicios.ObtenerFacFactuDetallesFiltro(detalleaux)

                If _FacFacturaDetalle IsNot Nothing Then
                    For i As Integer = 0 To _FacFacturaDetalle.Count - 1
                        structura.Id = id
                        structura.Servicio = _FacFacturaDetalle(i).XDetalle
                        structura.MMonto = _FacFacturaDetalle(i).BDetalle
                        monto = monto + _FacFacturaDetalle(i).BDetalle
                        detalle.Add(structura)
                    Next
                End If
            Catch ex As Exception
                'logger.Error(ex.Message)

            End Try

            'detalle = retorno
        End Sub

        Public Function inicializar_det() As StructReporteFActuraDeta
            Dim structura As New StructReporteFActuraDeta()
            structura.Id = ""
            structura.Cantidad = ""
            structura.MMonto = ""
            structura.Na = ""
            structura.Ndesc = ""
            structura.Npub = ""
            structura.Servicio = ""
            Return (structura)
        End Function

        Public Function inicializar_enc() As StructReporteFActuraEnc
            Dim structura As New StructReporteFActuraEnc()
            structura.Id = ""
            structura.Fecha = ""
            structura.Cliente = ""
            structura.Invoice = ""
            structura.Rif = ""
            structura.Nit = ""
            structura.Caso = ""
            structura.TituloPago = ""
            structura.TipoPago = ""
            structura.Texto1 = ""
            structura.Texto2 = ""
            structura.Msubtimpo = ""
            structura.Mdescuento = ""
            structura.Mtbimp = ""
            structura.Mtbexc = ""
            structura.Msubtotal = ""
            structura.Mttotal = ""
            structura.Mtimp = ""
            structura.Moneda = ""
            structura.Piva = ""
            structura.TituloNa = ""
            structura.TituloCantidad = ""
            structura.TituloPub = ""
            structura.TituloNDesc = ""
            structura.TituloMMonto = ""
            structura.Xfactura = ""
            structura.Control = ""
            structura.Seniat = ""
            structura.Xour = ""
            structura.Xourref = ""
            Return (structura)
        End Function

        Public Function datosenc_colum() As DataTable
            Dim datosEnc2 As New DataTable("DataTable1")
            datosEnc2.Columns.Add("Id")
            datosEnc2.Columns.Add("Cliente")
            datosEnc2.Columns.Add("Invoice")
            datosEnc2.Columns.Add("Fecha")
            datosEnc2.Columns.Add("Rif")
            datosEnc2.Columns.Add("Nit")
            datosEnc2.Columns.Add("Caso")
            datosEnc2.Columns.Add("TituloPago")
            datosEnc2.Columns.Add("TipoPago")
            datosEnc2.Columns.Add("Texto1")
            datosEnc2.Columns.Add("Texto2")
            datosEnc2.Columns.Add("Msubtimpo")
            datosEnc2.Columns.Add("Mdescuento")
            datosEnc2.Columns.Add("Mtbimp")
            datosEnc2.Columns.Add("Mtbexc")
            datosEnc2.Columns.Add("Msubtotal")
            datosEnc2.Columns.Add("Mtimp")
            datosEnc2.Columns.Add("Moneda")
            datosEnc2.Columns.Add("Mttotal")
            datosEnc2.Columns.Add("Piva")
            datosEnc2.Columns.Add("TituloNa")
            datosEnc2.Columns.Add("TituloCantidad")
            datosEnc2.Columns.Add("TituloPub")
            datosEnc2.Columns.Add("TituloNDesc")
            datosEnc2.Columns.Add("TituloMMonto")
            datosEnc2.Columns.Add("Xfactura")
            datosEnc2.Columns.Add("Control")
            datosEnc2.Columns.Add("Xour")
            datosEnc2.Columns.Add("Xourref")
            datosEnc2.Columns.Add("Seniat")
            Return datosEnc2
        End Function

        Public Function datosdeta_colum() As DataTable
            Dim datosdeta2 As New DataTable("DataTable2")
            datosdeta2.Columns.Add("Id")
            datosdeta2.Columns.Add("Servicio")
            datosdeta2.Columns.Add("Na")
            datosdeta2.Columns.Add("Cantidad")
            datosdeta2.Columns.Add("Npub")
            datosdeta2.Columns.Add("Ndesc")
            datosdeta2.Columns.Add("MMonto")
            Return datosdeta2
        End Function

        Private Function ArmarReporteEnc(ByVal datos As DataTable, ByVal estructurasDeDatos As IList(Of StructReporteFActuraEnc)) As DataTable
            Try
                For Each structura As StructReporteFActuraEnc In estructurasDeDatos
                    Dim filaDatos As DataRow = datos.NewRow()
                    filaDatos("Id") = structura.Id
                    filaDatos("Fecha") = structura.Fecha
                    filaDatos("Cliente") = structura.Cliente
                    filaDatos("Invoice") = structura.Invoice
                    filaDatos("Rif") = structura.Rif
                    filaDatos("Nit") = structura.Nit
                    filaDatos("Caso") = structura.Caso
                    filaDatos("TituloPago") = structura.TituloPago
                    filaDatos("TipoPago") = structura.TipoPago
                    filaDatos("Texto1") = structura.Texto1
                    filaDatos("Texto2") = structura.Texto2
                    filaDatos("Msubtimpo") = structura.Msubtimpo
                    filaDatos("Mdescuento") = structura.Mdescuento
                    filaDatos("Mtbimp") = structura.Mtbimp
                    filaDatos("Mtbexc") = structura.Mtbexc
                    filaDatos("Msubtotal") = structura.Msubtotal
                    filaDatos("Mtimp") = structura.Mtimp
                    filaDatos("Moneda") = structura.Moneda
                    filaDatos("Piva") = structura.Piva
                    filaDatos("TituloNa") = structura.TituloNa
                    filaDatos("TituloCantidad") = structura.TituloCantidad
                    filaDatos("TituloPub") = structura.TituloPub
                    filaDatos("TituloNDesc") = structura.TituloNDesc
                    filaDatos("TituloMMonto") = structura.TituloMMonto
                    filaDatos("Mttotal") = structura.Mttotal
                    filaDatos("Xfactura") = structura.Xfactura
                    filaDatos("Control") = structura.Control
                    filaDatos("Xour") = structura.Xour
                    filaDatos("Xourref") = structura.Xourref
                    filaDatos("Seniat") = structura.Seniat
                    datos.Rows.Add(filaDatos)
                Next
            Catch ex As Exception
                'logger.Error(ex.Message)

            End Try
            Return datos
        End Function

        Private Function ArmarReporteDeta(ByVal datos As DataTable, ByVal estructurasDeDatos As IList(Of StructReporteFActuraDeta)) As DataTable
            Try
                For Each structura As StructReporteFActuraDeta In estructurasDeDatos
                    Dim filaDatos As DataRow = datos.NewRow()
                    filaDatos("Id") = structura.Id
                    filaDatos("Servicio") = structura.Servicio
                    filaDatos("Na") = structura.Na
                    filaDatos("Npub") = poner_decimal(structura.Npub)
                    filaDatos("Cantidad") = structura.Cantidad
                    filaDatos("MMonto") = poner_decimal(structura.MMonto)
                    filaDatos("Ndesc") = poner_decimal(structura.Ndesc)
                    datos.Rows.Add(filaDatos)
                Next
            Catch ex As Exception
                'logger.Error(ex.Message)

            End Try
            Return datos
        End Function

        Structure StructReporteFActuraEnc
            Private _Id As String
            Private _Cliente As String
            Private _Fecha As String
            Private _Invoice As String
            Private _Rif As String
            Private _Nit As String
            Private _Caso As String
            Private _TituloPago As String
            Private _TipoPago As String
            Private _Texto1 As String
            Private _Texto2 As String
            Private _Msubtimpo As String
            Private _Mdescuento As String
            Private _Mtbimp As String
            Private _Mtbexc As String
            Private _Msubtotal As String
            Private _Mtimp As String
            Private _Moneda As String
            Private _Mttotal As String
            Private _Piva As String
            Private _TituloNa As String
            Private _TituloCantidad As String
            Private _TituloPub As String
            Private _TituloNDesc As String
            Private _TituloMMonto As String
            Private _Xfactura As String
            Private _Control As String
            Private _Xour As String
            Private _Xourref As String
            Private _Seniat As String

            Public Property Id() As String
                Get
                    Return Me._Id
                End Get
                Set(ByVal value As String)
                    Me._Id = value
                End Set
            End Property

            Public Property Cliente() As String
                Get
                    Return Me._Cliente
                End Get
                Set(ByVal value As String)
                    Me._Cliente = value
                End Set
            End Property

            Public Property Invoice() As String
                Get
                    Return Me._Invoice
                End Get
                Set(ByVal value As String)
                    Me._Invoice = value
                End Set
            End Property

            Public Property Fecha() As String
                Get
                    Return Me._Fecha
                End Get
                Set(ByVal value As String)
                    Me._Fecha = value
                End Set
            End Property


            Public Property Rif() As String
                Get
                    Return Me._Rif
                End Get
                Set(ByVal value As String)
                    Me._Rif = value
                End Set
            End Property

            Public Property Nit() As String
                Get
                    Return Me._Nit
                End Get
                Set(ByVal value As String)
                    Me._Nit = value
                End Set
            End Property

            Public Property Caso() As String
                Get
                    Return Me._Caso
                End Get
                Set(ByVal value As String)
                    Me._Caso = value
                End Set
            End Property

            Public Property TituloPago() As String
                Get
                    Return Me._TituloPago
                End Get
                Set(ByVal value As String)
                    Me._TituloPago = value
                End Set
            End Property

            Public Property TipoPago() As String
                Get
                    Return Me._TipoPago
                End Get
                Set(ByVal value As String)
                    Me._TipoPago = value
                End Set
            End Property

            Public Property Texto1() As String
                Get
                    Return Me._Texto1
                End Get
                Set(ByVal value As String)
                    Me._Texto1 = value
                End Set
            End Property

            Public Property Texto2() As String
                Get
                    Return Me._Texto2
                End Get
                Set(ByVal value As String)
                    Me._Texto2 = value
                End Set
            End Property

            Public Property Msubtimpo() As String
                Get
                    Return Me._Msubtimpo
                End Get
                Set(ByVal value As String)
                    Me._Msubtimpo = value
                End Set
            End Property


            Public Property Mdescuento() As String
                Get
                    Return Me._Mdescuento
                End Get
                Set(ByVal value As String)
                    Me._Mdescuento = value
                End Set
            End Property
            Public Property Mtbimp() As String
                Get
                    Return Me._Mtbimp
                End Get
                Set(ByVal value As String)
                    Me._Mtbimp = value
                End Set
            End Property
            Public Property Mtbexc() As String
                Get
                    Return Me._Mtbexc
                End Get
                Set(ByVal value As String)
                    Me._Mtbexc = value
                End Set
            End Property
            Public Property Msubtotal() As String
                Get
                    Return Me._Msubtotal
                End Get
                Set(ByVal value As String)
                    Me._Msubtotal = value
                End Set
            End Property
            Public Property Mtimp() As String
                Get
                    Return Me._Mtimp
                End Get
                Set(ByVal value As String)
                    Me._Mtimp = value
                End Set
            End Property
            Public Property Moneda() As String
                Get
                    Return Me._Moneda
                End Get
                Set(ByVal value As String)
                    Me._Moneda = value
                End Set
            End Property
            Public Property Mttotal() As String
                Get
                    Return Me._Mttotal
                End Get
                Set(ByVal value As String)
                    Me._Mttotal = value
                End Set
            End Property
            Public Property Piva() As String
                Get
                    Return Me._Piva
                End Get
                Set(ByVal value As String)
                    Me._Piva = value
                End Set
            End Property
            Public Property TituloNa() As String
                Get
                    Return Me._TituloNa
                End Get
                Set(ByVal value As String)
                    Me._TituloNa = value
                End Set
            End Property
            Public Property TituloCantidad() As String
                Get
                    Return Me._TituloCantidad
                End Get
                Set(ByVal value As String)
                    Me._TituloCantidad = value
                End Set
            End Property
            Public Property TituloPub() As String
                Get
                    Return Me._TituloPub
                End Get
                Set(ByVal value As String)
                    Me._TituloPub = value
                End Set
            End Property
            Public Property TituloNDesc() As String
                Get
                    Return Me._TituloNDesc
                End Get
                Set(ByVal value As String)
                    Me._TituloNDesc = value
                End Set
            End Property

            Public Property TituloMMonto() As String
                Get
                    Return Me._TituloMMonto
                End Get
                Set(ByVal value As String)
                    Me._TituloMMonto = value
                End Set
            End Property

            Public Property Xfactura() As String
                Get
                    Return Me._Xfactura
                End Get
                Set(ByVal value As String)
                    Me._Xfactura = value
                End Set
            End Property
            Public Property Control() As String
                Get
                    Return Me._Control
                End Get
                Set(ByVal value As String)
                    Me._Control = value
                End Set
            End Property
            Public Property Xour() As String
                Get
                    Return Me._Xour
                End Get
                Set(ByVal value As String)
                    Me._Xour = value
                End Set
            End Property
            Public Property Xourref() As String
                Get
                    Return Me._Xourref
                End Get
                Set(ByVal value As String)
                    Me._Xourref = value
                End Set
            End Property
            Public Property Seniat() As String
                Get
                    Return Me._Seniat
                End Get
                Set(ByVal value As String)
                    Me._Seniat = value
                End Set
            End Property

        End Structure


        Structure StructReporteFActuraDeta
            Private _Id As String
            Private _Servicio As String
            Private _Na As String
            Private _Cantidad As String
            Private _Npub As String
            Private _Ndesc As String
            Private _MMonto As String

            Public Property Id() As String
                Get
                    Return Me._Id
                End Get
                Set(ByVal value As String)
                    Me._Id = value
                End Set
            End Property

            Public Property Servicio() As String
                Get
                    Return Me._Servicio
                End Get
                Set(ByVal value As String)
                    Me._Servicio = value
                End Set
            End Property

            Public Property Na() As String
                Get
                    Return Me._Na
                End Get
                Set(ByVal value As String)
                    Me._Na = value
                End Set
            End Property

            Public Property Cantidad() As String
                Get
                    Return Me._Cantidad
                End Get
                Set(ByVal value As String)
                    Me._Cantidad = value
                End Set
            End Property


            Public Property Npub() As String
                Get
                    Return Me._Npub
                End Get
                Set(ByVal value As String)
                    Me._Npub = value
                End Set
            End Property

            Public Property Ndesc() As String
                Get
                    Return Me._Ndesc
                End Get
                Set(ByVal value As String)
                    Me._Ndesc = value
                End Set
            End Property

            Public Property MMonto() As String
                Get
                    Return Me._MMonto
                End Get
                Set(ByVal value As String)
                    Me._MMonto = value
                End Set
            End Property

        End Structure
    End Class
End Namespace