﻿Imports System.Collections.Generic
'Imports Diginsoft.Bolet.ObjetosComunes.Entidades
Imports Trascend.Bolet.ObjetosComunes.Entidades
Namespace ContratosServicios
    Public Interface IFacCobroServicios
        Inherits IServicioBase(Of FacCobro)
        Function ObtenerFacCobrosFiltro(ByVal FacCobro As FacCobro) As IList(Of FacCobro)
    End Interface
End Namespace