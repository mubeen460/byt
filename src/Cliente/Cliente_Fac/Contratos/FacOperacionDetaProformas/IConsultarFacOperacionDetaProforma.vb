﻿Imports System.Windows.Controls
Imports Diginsoft.Bolet.Cliente.Fac.Contratos
Namespace Contratos.FacOperacionDetaProformas
    Interface IConsultarFacOperacionDetaProforma
        Inherits IPaginaBaseFac
        Property FacOperacionDetaProforma() As Object
        WriteOnly Property HabilitarCampos() As Boolean

        Property Asociado() As Object

        Property Asociados() As Object

        Property Banco() As Object

        Property Bancos() As Object

        Property idAsociadoFiltrar() As String

        Property NombreAsociado() As String

        Property NombreAsociadoFiltrar() As String
        'Property Region() As String 

        Property TextoBotonModificar() As String
    End Interface
End Namespace