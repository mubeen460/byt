﻿Imports System.Windows.Controls
Imports Diginsoft.Bolet.Cliente.Fac.Contratos
Namespace Contratos.DocumentosMarcas
    Interface IConsultarDocumentosMarca
        Inherits IPaginaBaseFac
        Property DocumentosMarca() As Object
        WriteOnly Property HabilitarCampos() As Boolean

        'Property Region() As String 

        Property TextoBotonModificar() As String
    End Interface
End Namespace