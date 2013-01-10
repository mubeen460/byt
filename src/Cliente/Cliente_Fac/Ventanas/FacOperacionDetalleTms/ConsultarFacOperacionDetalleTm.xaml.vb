﻿Imports System.Windows
Imports System.Windows.Controls
Imports Diginsoft.Bolet.Cliente.Fac.Contratos.FacOperacionDetalleTms
Imports Diginsoft.Bolet.Cliente.Fac.Presentadores.FacOperacionDetalleTms
Imports Trascend.Bolet.Cliente.Contratos
Namespace Ventanas.FacOperacionDetalleTms
    ''' <summary>
    ''' Interaction logic for ConsultarObjeto.xaml
    ''' </summary>
    Partial Public Class ConsultarFacOperacionDetalleTm
        Inherits Page
        Implements IConsultarFacOperacionDetalleTm



        Private _presentador As PresentadorConsultarFacOperacionDetalleTm
        Private _cargada As Boolean

#Region "IConsultarFacOperacionDetalleTm"

        Public Property EstaCargada() As Boolean Implements IPaginaBase.EstaCargada
            Get
                Return Me._cargada
            End Get
            Set(ByVal value As Boolean)
                Me._cargada = value
            End Set
        End Property

        Public Sub FocoPredeterminado() Implements IPaginaBase.FocoPredeterminado
            Me._txtId.Focus()
        End Sub


        Public WriteOnly Property HabilitarCampos() As Boolean Implements Contratos.FacOperacionDetalleTms.IConsultarFacOperacionDetalleTm.HabilitarCampos
            Set(ByVal value As Boolean)
                Me._txtDetalle.IsEnabled = value
                Me._txtId.IsEnabled = value
            End Set
        End Property


        Public Property TextoBotonModificar() As String Implements Contratos.FacOperacionDetalleTms.IConsultarFacOperacionDetalleTm.TextoBotonModificar
            Get
                Return Me._txbModificar.Text
            End Get
            Set(ByVal value As String)
                Me._txbModificar.Text = value
            End Set
        End Property

#End Region

        Public Sub New(ByVal FacOperacionDetalleTm As Object)
            InitializeComponent()
            Me._cargada = False
            Me._presentador = New PresentadorConsultarFacOperacionDetalleTm(Me, FacOperacionDetalleTm)
        End Sub
        Private Sub _btnModificar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.Modificar()
        End Sub

        Private Sub _btnRegresar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me._presentador.Regresar()
        End Sub

        Private Sub _btnEliminar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If MessageBoxResult.Yes = MessageBox.Show(Recursos.MensajesConElUsuario.fac_ConfirmacionEliminarFacOperacionDetalleTm, "Eliminar FacOperacionDetalleTm", MessageBoxButton.YesNo, MessageBoxImage.Question) Then
                Me._presentador.Eliminar()
            End If
        End Sub

        Private Sub Page_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If Not EstaCargada Then
                Me._presentador.CargarPagina()
                EstaCargada = True
            End If
        End Sub

        Public Property FacOperacionDetalleTm As Object Implements Contratos.FacOperacionDetalleTms.IConsultarFacOperacionDetalleTm.FacOperacionDetalleTm
            Get
                Return Me._gridDatos.DataContext
            End Get
            Set(ByVal value As Object)
                Me._gridDatos.DataContext = value
            End Set
        End Property
    End Class
End Namespace